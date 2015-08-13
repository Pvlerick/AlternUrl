module UserInfo

// encapsulated type
type T

// wrap
val create : string -> T option
    
// unwrap
val value : T -> string
