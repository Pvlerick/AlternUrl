namespace AlternUrl

open System
open System.Runtime.InteropServices
open System.Text.RegularExpressions
open System.Web

open Scheme

type UserInfo = UserInfo of string
type Host = Host of string
type Port = Port of int
type Path = Path of string
type Query = Query of string
type Fragment = Fragment of string

/// <summary>
/// A class that respresents an absolute url.
/// E.g: "http://www.github.com/"
/// </summary>
type AbsoluteUrl = {
    Scheme:Scheme.T
    UserInfo:string
    Host:string
    Port:int
    Path:string
    Query:string
    Fragment:string
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
            let query = match0.Groups.[7].Value
            let fragment = match0.Groups.[9].Value

            //Userinfo, host and port are found with further parsing of the authority
            let match1 = Regex.Match(authority, @"(([^@]+)@)?([^:]+)(:(\d+))?")
            let userinfo = match1.Groups.[2].Value
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
    
        member this.ToString =
            ""

        member this.Extension =
            let ext = System.IO.Path.GetExtension(this.Path)
            if ext <> "" then Some(ext) else None

        member this.HasExtension =
            Option.isSome this.Extension

        member this.FileName =
            if this.HasExtension then Some(System.IO.Path.GetFileNameWithoutExtension(this.Path)) else None
    
        member this.HasFileName =
            Option.isSome this.FileName

        member this.IsDomainAnIPAddress = Regex.IsMatch(this.Host, @"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}")
    
        member this.TopLevelDomain =
            if this.IsDomainAnIPAddress then None else Some(this.Host.Substring(this.Host.LastIndexOf(".") + 1))

        member this.SecondLevelDomain = 
            if this.IsDomainAnIPAddress then None
            else
                let hostWithoutTopLevelDomain = this.Host.Substring(0, this.Host.LastIndexOf("."))
                Some(this.Host.Substring(hostWithoutTopLevelDomain.LastIndexOf(".") + 1))