namespace AlternUrl

open System
open System.Text.RegularExpressions

type public UrlKind =
    | Relative = 0
    | Absolute = 1

type public Url(url:string) = 
    //do
        //if not(Regex.IsMatch(url, "/^([!#$&-;=?-[]_a-z~]|%[0-9a-fA-F]{2})+$/")) then raise(ArgumentException("url", "Invalid URL characters present"))
    let kind = if url.StartsWith("http") then UrlKind.Absolute else UrlKind.Relative
    let urlForUriBuilder = if kind = UrlKind.Absolute then url else String.Format("http://www.google.com/{0}", url.TrimStart('/'))

    member x.Kind = kind
    
    //Builder is always build using a "fake" host if relatice, so we can leverage Uri/UriBuilder members
    member x.UriBuilder = new UriBuilder(urlForUriBuilder)

    member x.ToUri = 
        if x.Kind = UrlKind.Absolute then x.UriBuilder.Uri
        else new Uri(url, UriKind.Relative)

    //Mostly delegated to UriBuilder
    member x.Scheme = if x.Kind = UrlKind.Absolute then x.UriBuilder.Scheme else raise(NotSupportedException("Not supported for a relative URL"))
    member x.UserName = if x.Kind = UrlKind.Absolute then x.UriBuilder.UserName else raise(NotSupportedException("Not supported for a relative URL"))
    member x.Password = if x.Kind = UrlKind.Absolute then x.UriBuilder.Password else raise(NotSupportedException("Not supported for a relative URL"))
    member x.Host = if x.Kind = UrlKind.Absolute then x.UriBuilder.Host else raise(NotSupportedException("Not supported for a relative URL"))
    member x.Port = if x.Kind = UrlKind.Absolute then x.UriBuilder.Port else raise(NotSupportedException("Not supported for a relative URL"))
    member x.Path = x.UriBuilder.Path
    member x.Query = x.UriBuilder.Query
    member x.Fragment = x.UriBuilder.Fragment

    member x.Extension = IO.Path.GetExtension(x.Path)
    
    member x.HasExtension = IO.Path.GetExtension(x.Path) <> ""

    member x.IsHttps =
        if x.Kind = UrlKind.Absolute
            then if url.StartsWith("https") then true else false
            else raise(NotSupportedException("Not supported for a relative URL"))