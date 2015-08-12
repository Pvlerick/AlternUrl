module Scheme

open System.Text.RegularExpressions

type T = Scheme of string

// wrap
let create (s:string) =
    if Regex.IsMatch(s, @"^https?$", RegexOptions.IgnoreCase)
        then Some (Scheme s)
        else None
    
    // unwrap
let value (Scheme s) = s