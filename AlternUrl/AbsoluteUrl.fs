namespace AlternUrl

open System
open System.Runtime.InteropServices
open System.Text.RegularExpressions
open System.Web

open Scheme
open UserInfo

/// <summary>
/// A class that respresents an absolute url.
/// E.g: "http://www.github.com/"
/// </summary>
type AbsoluteUrl = {
    Scheme:Scheme.T
    UserInfo:UserInfo.T option
    Host:string
    Port:int
    Path:string
    Query:string option
    Fragment:string option
    }
    with
        /// <summary>
        /// Creates a new instance of the AbsoluteUrl class, using the given <cref="System.String" /> that must be a valid URL
        /// </summary>
        /// <param name="url">The string representation of the URL to be parsed to create the object.</param>
        /// <param name="encoded">Indicates if the given string is encoded or not. Default is true.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the input string is null.</exception>
        /// <exception cref="System.ArgumentException">Thrown when the input string is not a valid URL.</exception>
        static member Create(url: string, ?encoded: bool) =
            if url = null then raise (ArgumentNullException("url"))

            let keepIfNotEmpty a = if a <> "" then Some(a) else None

            let url0 = defaultArg encoded true

            //Parsing with regex from RFC 3986, Appendix B. - http://www.ietf.org/rfc/rfc3986.txt
            let match0 = Regex.Match(url, "^(([^:/?#]+):)?(//([^/?#]*))?([^?#]*)(\?([^#]*))?(#(.*))?")
            if not match0.Success then raise (ArgumentException("url is not valid", "url"))
            
            let scheme =
                match Scheme.create match0.Groups.[2].Value with
                | Some(s) -> s
                | None -> raise (ArgumentException("scheme of the url is not valid", "url"))
            
            let authority = match0.Groups.[4].Value
            let path = "/" + match0.Groups.[5].Value.TrimStart('/')
            let query = keepIfNotEmpty match0.Groups.[7].Value
            let fragment = keepIfNotEmpty match0.Groups.[9].Value

            //Userinfo, host and port are found with further parsing of the authority
            let match1 = Regex.Match(authority, @"(([^@]+)@)?([^:]+)(:(\d+))?")

            let userinfo =
                match UserInfo.create match1.Groups.[2].Value with
                | Some(s) -> if (value s) <> "" then Some(s) else None
                | None -> raise (ArgumentException("user info of the url is not valid", "url"))

            let host = match1.Groups.[3].Value

            let port =
                if not (String.IsNullOrEmpty(match1.Groups.[5].Value)) then Convert.ToInt32(match1.Groups.[5].Value)
                else
                    if (Scheme.value scheme).ToLowerInvariant() = "http" then 80 else 443 //Default port for http(s) schemes

            { Scheme = scheme; UserInfo = userinfo; Host = host; Port = port; Path = path; Query = query; Fragment = fragment }

        /// <summary>
        /// Creates a new instance of the AbsoluteUrl class, using the given <cref="System.Uri" />
        /// </summary>
        static member Create(uri: Uri) =
            if uri = null then raise (ArgumentNullException("uri"))
            AbsoluteUrl.Create(uri.ToString(), true)

        static member private valueWithPrefixOrEmpty so prefix =
            match so with
            | Some(s) -> prefix + s
            | None -> ""

        static member private valueWithSuffixOrEmpty so prefix =
            match so with
            | Some(s) -> prefix + s
            | None -> ""

        member this.PathAndQuery =
            this.Path + (AbsoluteUrl.valueWithPrefixOrEmpty this.Query "?")

        member this.PathAndQueryAndFragment =
            this.PathAndQuery + (AbsoluteUrl.valueWithPrefixOrEmpty this.Fragment "#")

        member this.ToString =
            let scheme = Scheme.value this.Scheme

            let userInfo =
                match this.UserInfo with
                | Some(u) ->
                    let v = value u
                    if v <> "" then v + "@" else ""
                | None -> ""

            let port =
                if (Scheme.value this.Scheme = "http" && this.Port <> 80) || (Scheme.value this.Scheme = "https" && this.Port <> 443)
                    then ":" + this.Port.ToString()
                    else ""

            sprintf "%s://%s%s%s%s" scheme userInfo this.Host port this.PathAndQueryAndFragment

        member this.Extension =
            match this.Path with
            | "/" -> None
            | _ -> 
                let e = System.IO.Path.GetExtension(this.Path)
                if e <> "" then Some(e) else None

        member this.FileName =
            match this.Extension with
            | Some(_) -> Some(System.IO.Path.GetFileNameWithoutExtension(this.Path))
            | None -> None

        member this.IsDomainAnIPAddress = Regex.IsMatch(this.Host, @"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}")
    
        member this.TopLevelDomain =
            if this.IsDomainAnIPAddress then None else Some(this.Host.Substring(this.Host.LastIndexOf(".") + 1))

        member this.SecondLevelDomain = 
            if this.IsDomainAnIPAddress then None
            else
                let hostWithoutTopLevelDomain = this.Host.Substring(0, this.Host.LastIndexOf("."))
                Some(this.Host.Substring(hostWithoutTopLevelDomain.LastIndexOf(".") + 1))