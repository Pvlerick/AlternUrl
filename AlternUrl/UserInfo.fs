module UserInfo

open System.Text.RegularExpressions

type T = UserInfo of string

// wrap
let create (s:string) =
    if Regex.IsMatch(s, @"^([^@]*)$", RegexOptions.IgnoreCase)
        then Some (UserInfo s)
        else None
    
    // unwrap
let value (UserInfo s) = s