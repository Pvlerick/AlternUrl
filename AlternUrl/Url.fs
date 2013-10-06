namespace AlternUrl

open System
open System.Collections.Generic
open System.Text.RegularExpressions

type public UrlKind =
    | Relative = 0
    | Absolute = 1

type public Url(url:string) = 
    //do
        //if not(Regex.IsMatch(url, "/^([!#$&-;=?-[]_a-z~]|%[0-9a-fA-F]{2})+$/")) then raise(ArgumentException("url", "Invalid URL characters present"))
    let kind = if url.StartsWith("http") then UrlKind.Absolute else UrlKind.Relative
    let urlForUriBuilder = if kind = UrlKind.Absolute then url else String.Format("http://www.google.com/{0}", url.TrimStart('/')) //Builder is always build using a "fake" host if relatice, so we can leverage Uri/UriBuilder members
    let uriBuilder = new UriBuilder(urlForUriBuilder)
    let mutable parametersIndex = 0
    let mutable parameters =
        seq {
            for s in uriBuilder.Query.TrimStart('?').Split('&') do
                match s.Split('=') with
                    | [|param; arg|] -> yield (param, arg)
                    | [|param|] -> yield (param, "")
                    | _ -> yield ("", "") }
        |> Seq.filter (fun (param, arg) -> not(String.IsNullOrWhiteSpace(param) && String.IsNullOrWhiteSpace(arg)))
        |> Map.ofSeq

    member x.Kind = kind
    member private x.UriBuilder = uriBuilder

    //#region Mostly delegated to UriBuilder
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

    member x.Query =
        let q =
            seq {
                for p in parameters do
                    if p.Value = "" then yield p.Key
                    else yield String.Format("{0}={1}", p.Key, p.Value) }
            |> String.concat "&"
        x.UriBuilder.Query <- q
        x.UriBuilder.Query

    member x.PathAndQuery =
        x.Path + x.Query

    member x.PathAndQueryAndFragment =
        x.Path + x.Query + x.Fragment

    member x.Fragment
        with public get() = x.UriBuilder.Fragment
        and public set fragment = x.UriBuilder.Fragment <- fragment
    //#endregion

    member x.ToUri() = 
        if x.Kind = UrlKind.Absolute then x.UriBuilder.Uri
        else new Uri(url, UriKind.Relative)

    member x.Extension = IO.Path.GetExtension(x.Path)
    member x.HasExtension = x.Extension <> ""
    
    member x.HasQuery = x.Query <> ""
    member x.HasFragment = x.Fragment <> ""

    member x.IsHttps =
        if x.Kind = UrlKind.Absolute then if url.StartsWith("https") then true else false
        else raise(NotSupportedException("Not supported for a relative URL"))

    member x.HasParameter param =
        parameters.ContainsKey(param)

    member x.AddParameter (param, value) =
        if parameters.ContainsKey(param) then raise(ArgumentException("param", "Parameter is already present in the query string"))
        parameters <- parameters.Add(param, value)
        x

    member x.AddParameter param =
        x.AddParameter(param, "")

    member x.RemoveParameter param =
        if not(parameters.ContainsKey(param)) then raise(ArgumentException("param", "Parameter is not present in the query string"))
        parameters <- parameters.Remove(param)
        x

    /// <summary>Changes the value of a parameter, throws an exception if the parameter is not already present</summary>
    member x.SetParameter (param, value) =
        if not(x.HasParameter(param)) then raise(ArgumentException("param", "Parameter is not present in the query string"))
        parameters <- parameters.Add(param, value)
        x

    member x.AddOrSetParameter(param, value) =
        if parameters.ContainsKey(param) then x.SetParameter(param, value)
        else x.AddParameter(param, value)