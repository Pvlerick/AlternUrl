namespace AlternUrl

open System
open System.Runtime.InteropServices
open System.Text.RegularExpressions
open System.Web

//[<AbstractClass>]
//type Url() =
//    abstract member FileName : string with get
//    abstract member HasFileName : bool with get
//    abstract member Extension : string with get
//    abstract member HasExtension : bool with get

/// <summary>
/// A class that respresents an absolute url.
/// E.g: "http://www.github.com/"
/// </summary>
type AbsoluteUrl private (scheme: string, userInfo: string, host: string, port: int, path: string, query: string, fragment: string) = 
    //inherit Url()
    do 
        if scheme = null then raise (ArgumentNullException("scheme"))

    /// <summary>
    /// Creates a new instance of the AbsoluteUrl class, using the given <cref="System.String" /> that must be a valid URL
    /// </summary>
    /// <param name="url">The string representation of the URL to be parsed to create the object.</param>
    /// <param name="encoded">Indicates if the given string is encoded or not. Default is true.</param>
    /// <exception cref="System.ArgumentNullException">Thrown when the input string is null.</exception>
    /// <exception cref="System.ArgumentException">Thrown when the input string is not a valid URL.</exception>
    new(url: string, ?encoded: bool) =
        if url = null then raise (ArgumentNullException("url"))

        let url0 = defaultArg encoded true

        //Parsing with regex from RFC 3986, Appendix B. - http://www.ietf.org/rfc/rfc3986.txt
        let match0 = Regex.Match(url, "^(([^:/?#]+):)?(//([^/?#]*))?([^?#]*)(\?([^#]*))?(#(.*))?")
        let scheme = match0.Groups.[2].Value

        if not match0.Success || scheme = "" then raise (ArgumentException("url is not valid", "url"))

        if not (Regex.IsMatch(scheme, "^https?$", RegexOptions.IgnoreCase)) then raise (ArgumentException("Scheme has to be http or https for an absolute URL"))

        let authority = match0.Groups.[4].Value
        let path = "/" + match0.Groups.[5].Value.TrimStart('/')
        let query = match0.Groups.[7].Value
        let fragment = match0.Groups.[9].Value

        //Userinfo, host and port are found with further parsing of the authority
        let match1 = Regex.Match(authority, @"(([^@]+)@)?([^:]+)(:(\d+))?")
        let userInfo = match1.Groups.[2].Value
        let host = match1.Groups.[3].Value

        let port =
            if not (String.IsNullOrEmpty(match1.Groups.[5].Value)) then Convert.ToInt32(match1.Groups.[5].Value)
            else
                if scheme.ToLowerInvariant() = "http" then 80 else 443 //Default port for http(s) schemes

        AbsoluteUrl(scheme, userInfo, host, port, path, query, fragment)

    /// <summary>
    /// Creates a new instance of the AbsoluteUrl class, using the given <cref="System.Uri" />
    /// </summary>
    new(uri: Uri) =
        if uri = null then raise (ArgumentNullException("uri"))
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

    member this.IsDomainAnIPAddress = Regex.IsMatch(this.Host, @"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}")
    
    member this.TopLevelDomain =
        if this.IsDomainAnIPAddress then raise (NotSupportedException("This AbsoluteUrl's domain is an IP address so there is no top level domain. Use the IsDomainAnIpAddress property to check before calling TopLevelDomain."))
        else this.Host.Substring(this.Host.LastIndexOf(".") + 1);

    member this.SecondLevelDomain = 
        if this.IsDomainAnIPAddress then raise (NotSupportedException("This AbsoluteUrl's domain is an IP address so there is no second level domain. Use the IsDomainAnIpAddress property to check before calling SecondLevelDomain."))
        else
            let hostWithoutTopLevelDomain = this.Host.Substring(0, this.Host.LastIndexOf("."))
            this.Host.Substring(hostWithoutTopLevelDomain.LastIndexOf(".") + 1)