namespace AlternUrl

open System
open System.Runtime.InteropServices
open System.Text.RegularExpressions
open System.Web
//
//[<AbstractClass>]
//type Url() =
//    abstract member FileName : string with get
//    abstract member HasFileName : bool with get
//    abstract member Extension : string with get
//    abstract member HasExtension : bool with get

type AbsoluteUrl private (scheme: string, userInfo: string, host: string, port: int, path: string, query: string, fragment: string) = 
    //inherit Url()
    do 
        if scheme = null then raise (ArgumentNullException("relay"))

    new(url: string, [<Optional;DefaultParameterValue(true)>]encoded: bool) =
        if url = null then raise (ArgumentNullException("url"))

        let url0 = if encoded then HttpUtility.UrlEncode(url) else url

        //Parsing with regex from RFC 3986, Appendix B. - http://www.ietf.org/rfc/rfc3986.txt
        let match0 = Regex.Match(url, "^(([^:/?#]+):)?(//([^/?#]*))?([^?#]*)(\?([^#]*))?(#(.*))?")

        if not match0.Success then raise (ArgumentException("url is not valid", "url"))

        let scheme = match0.Groups.[2].Value

        if not (Regex.IsMatch(scheme, "https*", RegexOptions.IgnoreCase)) then raise (NotSupportedException("Scheme has to be http or https for an absolute URL"))

        let authority = match0.Groups.[4].Value
        let path = "/" + match0.Groups.[5].Value.TrimStart('/')
        let query = match0.Groups.[7].Value
        let fragment = match0.Groups.[9].Value

        //Userinfo, host and port are found with further parsing of the authority
        let match1 = Regex.Match(authority, @"(([^@]+)@)?([^:]+)(:(\d+))?")
        let userInfo = match1.Groups.[2].Value
        let host = match1.Groups.[3].Value.ToLowerInvariant()

        let port =
            if not (String.IsNullOrEmpty(match1.Groups.[5].Value)) then Convert.ToInt32(match1.Groups.[5].Value)
            else
                if scheme = "http" then 80 else 443 //Default port for http(s) scheme

        AbsoluteUrl(scheme, userInfo, host, port, path, query, fragment)

    new(uri: Uri) =
        AbsoluteUrl(uri.ToString(), true)

    member this.Scheme = scheme
    member this.UserInfo = userInfo
    member this.Host = host
    member this.Port = port
    member this.Path = path
    member this.Query = query
    member this.Fragment = fragment
    member this.FileName = if this.HasExtension then System.IO.Path.GetFileNameWithoutExtension(this.Path) else ""
    member this.HasFileName = this.FileName <> ""
    member this.Extension = System.IO.Path.GetExtension(this.Path)
    member this.HasExtension = this.Extension <> ""