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
    member private x.UriBuilder = new UriBuilder(urlForUriBuilder)

    member x.ToUri() = 
        if x.Kind = UrlKind.Absolute then x.UriBuilder.Uri
        else new Uri(url, UriKind.Relative)

    //Mostly delegated to UriBuilder
    member x.Scheme
        with public get() =
            if x.Kind = UrlKind.Absolute then x.UriBuilder.Scheme else raise(NotSupportedException("Not supported for a relative URL"))
        and public set scheme =
            if x.Kind = UrlKind.Absolute then x.UriBuilder.Scheme <- scheme else raise(NotSupportedException("Not supported for a relative URL"))

    member x.UserName
        with public get() =
            if x.Kind = UrlKind.Absolute then x.UriBuilder.UserName else raise(NotSupportedException("Not supported for a relative URL"))
        and public set userName =
            if x.Kind = UrlKind.Absolute then x.UriBuilder.UserName <- userName else raise(NotSupportedException("Not supported for a relative URL"))

    member x.Password
        with public get() =
            if x.Kind = UrlKind.Absolute then x.UriBuilder.Password else raise(NotSupportedException("Not supported for a relative URL"))
        and public set password =
            if x.Kind = UrlKind.Absolute then x.UriBuilder.Password <- password else raise(NotSupportedException("Not supported for a relative URL"))

    member x.Host
        with public get() =
            if x.Kind = UrlKind.Absolute then x.UriBuilder.Host else raise(NotSupportedException("Not supported for a relative URL"))
        and public set host =
            if x.Kind = UrlKind.Absolute then x.UriBuilder.Host <- host else raise(NotSupportedException("Not supported for a relative URL"))

    member x.Port
        with public get() =
            if x.Kind = UrlKind.Absolute then x.UriBuilder.Port else raise(NotSupportedException("Not supported for a relative URL"))
        and public set port =
            if x.Kind = UrlKind.Absolute then x.UriBuilder.Port <- port else raise(NotSupportedException("Not supported for a relative URL"))

    member x.Path
        with public get() = x.UriBuilder.Path
        and public set path = x.UriBuilder.Path <- path

    member x.Query
        with public get() = x.UriBuilder.Query
        and public set query = x.UriBuilder.Query <- query

    member x.PathAndQuery
        with public get() = x.UriBuilder.Uri.PathAndQuery
        and public set(pathAndQuery:string) =
            let split = pathAndQuery.Split('?')
            match split.Length with
                | 1 -> x.Path <- pathAndQuery
                       x.Query <- ""
                | 2 -> x.Path <- split.[0]
                       x.Query <- split.[1]
                | _ -> raise(ArgumentException("PathAndQuery is not valid :" + split.Length.ToString()))

    member x.Fragment
        with public get() = x.UriBuilder.Fragment
        and public set fragment = x.UriBuilder.Fragment <- fragment

    member x.Extension = IO.Path.GetExtension(x.Path)
    member x.HasExtension = x.Extension <> ""
    
    member x.HasQuery = x.Query <> ""
    member x.HasFragment = x.Fragment <> ""

    member x.IsHttps =
        if x.Kind = UrlKind.Absolute
            then if url.StartsWith("https") then true else false
            else raise(NotSupportedException("Not supported for a relative URL"))