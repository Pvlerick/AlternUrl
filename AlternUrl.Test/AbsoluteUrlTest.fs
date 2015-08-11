module AlternUrl.Test.AbsoluteUrlTest

type AbsoluteUrlTestData =
    {
        Url:string
        Scheme:string
        UserInfo:string
        Host:string
        Port:int
        Path:string
        Query:string
        Fragment:string
    }

let AbsoluteUrlTestData = 
    [
        { Url = "http://www.google.com"; Scheme = "http"; UserInfo = ""; Host = "www.google.com"; Port = 80; Path = "/"; Query = ""; Fragment = "" }
        { Url = "HTTP://WWW.GOOGLE.COM"; Scheme = "HTTP"; UserInfo = ""; Host = "WWW.GOOGLE.COM"; Port = 80; Path = "/"; Query = ""; Fragment = "" }
        { Url = "http://www.google.com/"; Scheme = "http"; UserInfo = ""; Host = "www.google.com"; Port = 80; Path = "/"; Query = ""; Fragment = "" }
        { Url = "http://www.google.com/mail"; Scheme = "http"; UserInfo = ""; Host = "www.google.com"; Port = 80; Path = "/mail"; Query = ""; Fragment = "" }
        { Url = "http://WWW.GOOGLE.COM/mail"; Scheme = "http"; UserInfo = ""; Host = "WWW.GOOGLE.COM"; Port = 80; Path = "/mail"; Query = ""; Fragment = "" }
        { Url = "http://WWW.GOOGLE.COM/MAIL"; Scheme = "http"; UserInfo = ""; Host = "WWW.GOOGLE.COM"; Port = 80; Path = "/MAIL"; Query = ""; Fragment = "" }
        { Url = "http://www.google.com/mail/"; Scheme = "http"; UserInfo = ""; Host = "www.google.com"; Port = 80; Path = "/mail/"; Query = ""; Fragment = "" }
        { Url = "http://www.google.com/hello.html"; Scheme = "http"; UserInfo = ""; Host = "www.google.com"; Port = 80; Path = "/hello.html"; Query = ""; Fragment = "" }
        { Url = "http://www.google.com/mail/hello.htm"; Scheme = "http"; UserInfo = ""; Host = "www.google.com"; Port = 80; Path = "/mail/hello.htm"; Query = ""; Fragment = "" }
        { Url = "http://www.google.com/mail/hello.html"; Scheme = "http"; UserInfo = ""; Host = "www.google.com"; Port = 80; Path = "/mail/hello.html"; Query = ""; Fragment = "" }
        { Url = "http://www.google.com/mail/hello.html#"; Scheme = "http"; UserInfo = ""; Host = "www.google.com"; Port = 80; Path = "/mail/hello.html"; Query = ""; Fragment = "" }
        { Url = "http://www.google.com/mail/hello.html#anchor"; Scheme = "http"; UserInfo = ""; Host = "www.google.com"; Port = 80; Path = "/mail/hello.html"; Query = ""; Fragment = "anchor" }
        { Url = "http://www.google.com/mail/hello.html?"; Scheme = "http"; UserInfo = ""; Host = "www.google.com"; Port = 80; Path = "/mail/hello.html"; Query = ""; Fragment = "" }
        { Url = "http://www.google.com/mail/hello.html?#"; Scheme = "http"; UserInfo = ""; Host = "www.google.com"; Port = 80; Path = "/mail/hello.html"; Query = ""; Fragment = "" }
        { Url = "http://www.google.com/mail/hello.html?foo"; Scheme = "http"; UserInfo = ""; Host = "www.google.com"; Port = 80; Path = "/mail/hello.html"; Query = "foo"; Fragment = "" }
        { Url = "http://www.google.com/mail/hello.html?foo=12"; Scheme = "http"; UserInfo = ""; Host = "www.google.com"; Port = 80; Path = "/mail/hello.html"; Query = "foo=12"; Fragment = "" }
        { Url = "http://www.google.com/mail/hello.html?foo=12&bar"; Scheme = "http"; UserInfo = ""; Host = "www.google.com"; Port = 80; Path = "/mail/hello.html"; Query = "foo=12&bar"; Fragment = "" }
        { Url = "http://www.google.com/mail/hello.html?foo=12&bar=34"; Scheme = "http"; UserInfo = ""; Host = "www.google.com"; Port = 80; Path = "/mail/hello.html"; Query = "foo=12&bar=34"; Fragment = "" }
        { Url = "http://www.google.com/mail/hello.html?foo"; Scheme = "http"; UserInfo = ""; Host = "www.google.com"; Port = 80; Path = "/mail/hello.html"; Query = "foo"; Fragment = "" }
        { Url = "http://www.google.com/mail/hello.html?foo#anchor"; Scheme = "http"; UserInfo = ""; Host = "www.google.com"; Port = 80; Path = "/mail/hello.html"; Query = "foo"; Fragment = "anchor" }
        { Url = "http://www.google.com/mail/hello.html?foo=12"; Scheme = "http"; UserInfo = ""; Host = "www.google.com"; Port = 80; Path = "/mail/hello.html"; Query = "foo=12"; Fragment = "" }
        { Url = "http://www.google.com/mail/hello.html?foo=12#anchor"; Scheme = "http"; UserInfo = ""; Host = "www.google.com"; Port = 80; Path = "/mail/hello.html"; Query = "foo=12"; Fragment = "anchor" }
        { Url = "http://www.google.com/mail/hello.html?foo=12&bar"; Scheme = "http"; UserInfo = ""; Host = "www.google.com"; Port = 80; Path = "/mail/hello.html"; Query = "foo=12&bar"; Fragment = "" }
        { Url = "http://www.google.com/mail/hello.html?foo=12&bar=34"; Scheme = "http"; UserInfo = ""; Host = "www.google.com"; Port = 80; Path = "/mail/hello.html"; Query = "foo=12&bar=34"; Fragment = "" }
        { Url = "http://www.google.com/mail/hello.html?foo=12&bar=34#anchor"; Scheme = "http"; UserInfo = ""; Host = "www.google.com"; Port = 80; Path = "/mail/hello.html"; Query = "foo=12&bar=34"; Fragment = "anchor" }
        { Url = "http://www.google.com/hello"; Scheme = "http"; UserInfo = ""; Host = "www.google.com"; Port = 80; Path = "/hello"; Query = ""; Fragment = "" }
        { Url = "http://www.google.com/mail#"; Scheme = "http"; UserInfo = ""; Host = "www.google.com"; Port = 80; Path = "/mail"; Query = ""; Fragment = "" }
        { Url = "http://www.google.com/mail#anchor"; Scheme = "http"; UserInfo = ""; Host = "www.google.com"; Port = 80; Path = "/mail"; Query = ""; Fragment = "anchor" }
        { Url = "http://www.google.com/mail?"; Scheme = "http"; UserInfo = ""; Host = "www.google.com"; Port = 80; Path = "/mail"; Query = ""; Fragment = "" }
        { Url = "http://www.google.com/mail?#"; Scheme = "http"; UserInfo = ""; Host = "www.google.com"; Port = 80; Path = "/mail"; Query = ""; Fragment = "" }
        { Url = "http://www.google.com/mail?foo"; Scheme = "http"; UserInfo = ""; Host = "www.google.com"; Port = 80; Path = "/mail"; Query = "foo"; Fragment = "" }
        { Url = "http://www.google.com/mail?foo=12"; Scheme = "http"; UserInfo = ""; Host = "www.google.com"; Port = 80; Path = "/mail"; Query = "foo=12"; Fragment = "" }
        { Url = "http://www.google.com/mail?foo=12&bar"; Scheme = "http"; UserInfo = ""; Host = "www.google.com"; Port = 80; Path = "/mail"; Query = "foo=12&bar"; Fragment = "" }
        { Url = "http://www.google.com/mail?foo=12&bar=34"; Scheme = "http"; UserInfo = ""; Host = "www.google.com"; Port = 80; Path = "/mail"; Query = "foo=12&bar=34"; Fragment = "" }
        { Url = "http://www.google.com/mail?foo"; Scheme = "http"; UserInfo = ""; Host = "www.google.com"; Port = 80; Path = "/mail"; Query = "foo"; Fragment = "" }
        { Url = "http://www.google.com/mail?foo#anchor"; Scheme = "http"; UserInfo = ""; Host = "www.google.com"; Port = 80; Path = "/mail"; Query = "foo"; Fragment = "anchor" }
        { Url = "http://www.google.com/mail?foo=12"; Scheme = "http"; UserInfo = ""; Host = "www.google.com"; Port = 80; Path = "/mail"; Query = "foo=12"; Fragment = "" }
        { Url = "http://www.google.com/mail?foo=12#anchor"; Scheme = "http"; UserInfo = ""; Host = "www.google.com"; Port = 80; Path = "/mail"; Query = "foo=12"; Fragment = "anchor" }
        { Url = "http://www.google.com/mail?foo=12&bar"; Scheme = "http"; UserInfo = ""; Host = "www.google.com"; Port = 80; Path = "/mail"; Query = "foo=12&bar"; Fragment = "" }
        { Url = "http://www.google.com/mail?foo=12&bar=34"; Scheme = "http"; UserInfo = ""; Host = "www.google.com"; Port = 80; Path = "/mail"; Query = "foo=12&bar=34"; Fragment = "" }
        { Url = "http://www.google.com/mail?foo=12&bar=34#anchor"; Scheme = "http"; UserInfo = ""; Host = "www.google.com"; Port = 80; Path = "/mail"; Query = "foo=12&bar=34"; Fragment = "anchor" }
        { Url = "http://www.google.com/mail/"; Scheme = "http"; UserInfo = ""; Host = "www.google.com"; Port = 80; Path = "/mail/"; Query = ""; Fragment = "" }
        { Url = "http://www.google.com/mail/#"; Scheme = "http"; UserInfo = ""; Host = "www.google.com"; Port = 80; Path = "/mail/"; Query = ""; Fragment = "" }
        { Url = "http://www.google.com/mail/#anchor"; Scheme = "http"; UserInfo = ""; Host = "www.google.com"; Port = 80; Path = "/mail/"; Query = ""; Fragment = "anchor" }
        { Url = "http://www.google.com/mail/?"; Scheme = "http"; UserInfo = ""; Host = "www.google.com"; Port = 80; Path = "/mail/"; Query = ""; Fragment = "" }
        { Url = "http://www.google.com/mail/?#"; Scheme = "http"; UserInfo = ""; Host = "www.google.com"; Port = 80; Path = "/mail/"; Query = ""; Fragment = "" }
        { Url = "http://www.google.com/mail/?foo"; Scheme = "http"; UserInfo = ""; Host = "www.google.com"; Port = 80; Path = "/mail/"; Query = "foo"; Fragment = "" }
        { Url = "http://www.google.com/mail/?foo=12"; Scheme = "http"; UserInfo = ""; Host = "www.google.com"; Port = 80; Path = "/mail/"; Query = "foo=12"; Fragment = "" }
        { Url = "http://www.google.com/mail/?foo=12&bar"; Scheme = "http"; UserInfo = ""; Host = "www.google.com"; Port = 80; Path = "/mail/"; Query = "foo=12&bar"; Fragment = "" }
        { Url = "http://www.google.com/mail/?foo=12&bar=34"; Scheme = "http"; UserInfo = ""; Host = "www.google.com"; Port = 80; Path = "/mail/"; Query = "foo=12&bar=34"; Fragment = "" }
        { Url = "http://www.google.com/mail/?foo"; Scheme = "http"; UserInfo = ""; Host = "www.google.com"; Port = 80; Path = "/mail/"; Query = "foo"; Fragment = "" }
        { Url = "http://www.google.com/mail/?foo#anchor"; Scheme = "http"; UserInfo = ""; Host = "www.google.com"; Port = 80; Path = "/mail/"; Query = "foo"; Fragment = "anchor" }
        { Url = "http://www.google.com/mail/?foo=12"; Scheme = "http"; UserInfo = ""; Host = "www.google.com"; Port = 80; Path = "/mail/"; Query = "foo=12"; Fragment = "" }
        { Url = "http://www.google.com/mail/?foo=12#anchor"; Scheme = "http"; UserInfo = ""; Host = "www.google.com"; Port = 80; Path = "/mail/"; Query = "foo=12"; Fragment = "anchor" }
        { Url = "http://www.google.com/mail/?foo=12&bar"; Scheme = "http"; UserInfo = ""; Host = "www.google.com"; Port = 80; Path = "/mail/"; Query = "foo=12&bar"; Fragment = "" }
        { Url = "http://www.google.com/mail/?foo=12&bar=34"; Scheme = "http"; UserInfo = ""; Host = "www.google.com"; Port = 80; Path = "/mail/"; Query = "foo=12&bar=34"; Fragment = "" }
        { Url = "http://www.google.com/mail/?foo=12&bar=34#anchor"; Scheme = "http"; UserInfo = ""; Host = "www.google.com"; Port = 80; Path = "/mail/"; Query = "foo=12&bar=34"; Fragment = "anchor" }
        { Url = "http://root:mypass@www.google.com/mail/?foo=12&bar=34#anchor"; Scheme = "http"; UserInfo = "root:mypass"; Host = "www.google.com"; Port = 80; Path = "/mail/"; Query = "foo=12&bar=34"; Fragment = "anchor" }
        { Url = "http://root:mypass@www.google.com:90/mail/?foo=12&bar=34#anchor"; Scheme = "http"; UserInfo = "root:mypass"; Host = "www.google.com"; Port = 90; Path = "/mail/"; Query = "foo=12&bar=34"; Fragment = "anchor" }
        { Url = "https://root:mypass@www.google.com/mail/?foo=12&bar=34#anchor"; Scheme = "https"; UserInfo = "root:mypass"; Host = "www.google.com"; Port = 443; Path = "/mail/"; Query = "foo=12&bar=34"; Fragment = "anchor" }
        { Url = "https://root:mypass@www.google.com:444/mail/?foo=12&bar=34#anchor"; Scheme = "https"; UserInfo = "root:mypass"; Host = "www.google.com"; Port = 444; Path = "/mail/"; Query = "foo=12&bar=34"; Fragment = "anchor" }
        { Url = "http://root:mypass@www.google.com/mail/?foo=12&bar=34#anchor"; Scheme = "http"; UserInfo = "root:mypass"; Host = "www.google.com"; Port = 80; Path = "/mail/"; Query = "foo=12&bar=34"; Fragment = "anchor" }
        { Url = "http://www.google.com/mail/?foo=12&bar=34#anchor"; Scheme = "http"; UserInfo = ""; Host = "www.google.com"; Port = 80; Path = "/mail/"; Query = "foo=12&bar=34"; Fragment = "anchor" }
        { Url = "https://root:mypass@www.google.com/mail/?foo=12&bar=34#anchor"; Scheme = "https"; UserInfo = "root:mypass"; Host = "www.google.com"; Port = 443; Path = "/mail/"; Query = "foo=12&bar=34"; Fragment = "anchor" }
        { Url = "https://www.google.com/mail/?foo=12&bar=34#anchor"; Scheme = "https"; UserInfo = ""; Host = "www.google.com"; Port = 443; Path = "/mail/"; Query = "foo=12&bar=34"; Fragment = "anchor" }
    ] |> Seq.map (fun a -> [| a |])

open System
open Xunit
open AlternUrl

[<Fact>]
let ``constructor with string`` () =
    // Fixture setup
    let sut = new AbsoluteUrl("http://username:password@example.com:8042/over/there/index.dtb?type=animal&name=narwhal#nose")
    // Exercise system & Verify outcome
    Assert.Equal("http", sut.Scheme)
    Assert.Equal("username:password", sut.UserInfo)
    Assert.Equal("example.com", sut.Host)
    Assert.Equal(8042, sut.Port)
    Assert.Equal("/over/there/index.dtb", sut.Path)
    Assert.Equal("type=animal&name=narwhal", sut.Query)
    Assert.Equal("nose", sut.Fragment)
    Assert.True(Option.isSome sut.FileName)
    Assert.Equal("index", Option.get sut.FileName)
    Assert.True(Option.isSome sut.Extension)
    Assert.Equal(".dtb", Option.get sut.Extension)
    // Teardown

[<Fact>]
let ``constructor with uri`` () =
    // Fixture setup
    let uri = new Uri("http://username:password@example.com:8042/over/there/index.dtb?type=animal&name=narwhal#nose")
    let sut = new AbsoluteUrl(uri)
    // Exercise system & Verify outcome
    Assert.Equal("http", sut.Scheme)
    Assert.Equal("username:password", sut.UserInfo)
    Assert.Equal("example.com", sut.Host)
    Assert.Equal(8042, sut.Port)
    Assert.Equal("/over/there/index.dtb", sut.Path)
    Assert.Equal("type=animal&name=narwhal", sut.Query)
    Assert.Equal("nose", sut.Fragment)
    Assert.True(Option.isSome sut.FileName)
    Assert.Equal("index", Option.get sut.FileName)
    Assert.True(Option.isSome sut.Extension)
    Assert.Equal(".dtb", Option.get sut.Extension)
    // Teardown

[<Fact>]
let ``constructor with null throws``() =
    // Fixture setup
    // Exercise system & Verify outcome
    Assert.Throws<ArgumentNullException>(fun () -> new AbsoluteUrl(null) |> ignore)
    // Teardown

[<Theory>]
[<InlineData("")>]
[<InlineData("foo")>]
[<InlineData("ftp://hello")>] //Invalid scheme
[<InlineData("htttp://hello")>]
[<InlineData("httpss://www.google.com")>]
let ``constructor with invalid url throws``(urlText:string) =
    // Fixture setup
    // Exercise system & Verify outcome
    Assert.Throws<ArgumentException>(fun () -> new AbsoluteUrl(urlText) |> ignore)
    // Teardown

[<Theory>]
[<InlineData("http://www.google.com/", "", false, "", false)>]
[<InlineData("http://www.google.com", "", false, "", false)>]
[<InlineData("http://www.google.com/mail", "", false, "", false)>]
[<InlineData("http://www.google.com/mail/", "", false, "", false)>]
[<InlineData("http://www.google.com/hello.html", "hello", true, ".html", true)>]
[<InlineData("http://www.google.com/mail/hello.htm", "hello", true, ".htm", true)>]
[<InlineData("http://www.google.com/mail/hello.html", "hello", true, ".html", true)>]
[<InlineData("http://www.google.com/mail/hello.html#", "hello", true, ".html", true)>]
[<InlineData("http://www.google.com/mail/hello.html#anchor", "hello", true, ".html", true)>]
[<InlineData("http://www.google.com/mail/hello.html?", "hello", true, ".html", true)>]
[<InlineData("http://www.google.com/mail/hello.html?#", "hello", true, ".html", true)>]
[<InlineData("http://www.google.com/mail/hello.html?foo", "hello", true, ".html", true)>]
[<InlineData("http://www.google.com/mail/hello.html?foo=12", "hello", true, ".html", true)>]
[<InlineData("http://www.google.com/mail/hello.html?foo=12&bar", "hello", true, ".html", true)>]
[<InlineData("http://www.google.com/mail/hello.html?foo=12&bar=34", "hello", true, ".html", true)>]
[<InlineData("http://www.google.com/mail/hello.html?foo", "hello", true, ".html", true)>]
[<InlineData("http://www.google.com/mail/hello.html?foo#anchor", "hello", true, ".html", true)>]
[<InlineData("http://www.google.com/mail/hello.html?foo=12", "hello", true, ".html", true)>]
[<InlineData("http://www.google.com/mail/hello.html?foo=12#anchor", "hello", true, ".html", true)>]
[<InlineData("http://www.google.com/mail/hello.html?foo=12&bar", "hello", true, ".html", true)>]
[<InlineData("http://www.google.com/mail/hello.html?foo=12&bar=34", "hello", true, ".html", true)>]
[<InlineData("http://www.google.com/mail/hello.html?foo=12&bar=34#anchor", "hello", true, ".html", true)>]
[<InlineData("http://www.google.com/hello", "", false, "", false)>]
[<InlineData("http://www.google.com/mail#", "", false, "", false)>]
[<InlineData("http://www.google.com/mail#anchor", "", false, "", false)>]
[<InlineData("http://www.google.com/mail?", "", false, "", false)>]
[<InlineData("http://www.google.com/mail?#", "", false, "", false)>]
[<InlineData("http://www.google.com/mail?foo", "", false, "", false)>]
[<InlineData("http://www.google.com/mail?foo=12", "", false, "", false)>]
[<InlineData("http://www.google.com/mail?foo=12&bar", "", false, "", false)>]
[<InlineData("http://www.google.com/mail?foo=12&bar=34", "", false, "", false)>]
[<InlineData("http://www.google.com/mail?foo", "", false, "", false)>]
[<InlineData("http://www.google.com/mail?foo#anchor", "", false, "", false)>]
[<InlineData("http://www.google.com/mail?foo=12", "", false, "", false)>]
[<InlineData("http://www.google.com/mail?foo=12#anchor", "", false, "", false)>]
[<InlineData("http://www.google.com/mail?foo=12&bar", "", false, "", false)>]
[<InlineData("http://www.google.com/mail?foo=12&bar=34", "", false, "", false)>]
[<InlineData("http://www.google.com/mail?foo=12&bar=34#anchor", "", false, "", false)>]
[<InlineData("http://www.google.com/mail/", "", false, "", false)>]
[<InlineData("http://www.google.com/mail/#", "", false, "", false)>]
[<InlineData("http://www.google.com/mail/#anchor", "", false, "", false)>]
[<InlineData("http://www.google.com/mail/?", "", false, "", false)>]
[<InlineData("http://www.google.com/mail/?#", "", false, "", false)>]
[<InlineData("http://www.google.com/mail/?foo", "", false, "", false)>]
[<InlineData("http://www.google.com/mail/?foo=12", "", false, "", false)>]
[<InlineData("http://www.google.com/mail/?foo=12&bar", "", false, "", false)>]
[<InlineData("http://www.google.com/mail/?foo=12&bar=34", "", false, "", false)>]
[<InlineData("http://www.google.com/mail/?foo", "", false, "", false)>]
[<InlineData("http://www.google.com/mail/?foo#anchor", "", false, "", false)>]
[<InlineData("http://www.google.com/mail/?foo=12", "", false, "", false)>]
[<InlineData("http://www.google.com/mail/?foo=12#anchor", "", false, "", false)>]
[<InlineData("http://www.google.com/mail/?foo=12&bar", "", false, "", false)>]
[<InlineData("http://www.google.com/mail/?foo=12&bar=34", "", false, "", false)>]
[<InlineData("http://www.google.com/mail/?foo=12&bar=34#anchor", "", false, "", false)>]
let ``filename and extension`` (urlText:string, expectedFileName:string, expectedHasFileName:bool, expectedExtension:string, expectedHasExtension:bool) =
    // Fixture setup
    let fileName = if expectedFileName <> "" then Some(expectedFileName) else None
    let extension = if expectedExtension <> "" then Some(expectedExtension) else None
    let sut = new AbsoluteUrl(urlText)
    // Exercise system & Verify outcome
    Assert.Equal(fileName, sut.FileName)
    Assert.Equal(expectedHasFileName, sut.HasFileName)
    Assert.Equal(extension, sut.Extension)
    Assert.Equal(expectedHasExtension, sut.HasExtension)
    // Teardown

[<Theory>]
[<MemberData("AbsoluteUrlTestData")>]
let ``scheme`` (urlData:AbsoluteUrlTestData) =
    // Fixture setup
    let sut = new AbsoluteUrl(urlData.Url)
    // Exercise system & Verify outcome
    Assert.Equal(urlData.Scheme, sut.Scheme)
    // Teardown

[<Theory>]
[<MemberData("AbsoluteUrlTestData")>]
let ``user info`` (urlData:AbsoluteUrlTestData) =
    // Fixture setup
    let sut = new AbsoluteUrl(urlData.Url)
    // Exercise system & Verify outcome
    Assert.Equal(urlData.UserInfo, sut.UserInfo)
    // Teardown

[<Theory>]
[<MemberData("AbsoluteUrlTestData")>]
let ``host`` (urlData:AbsoluteUrlTestData) =
    // Fixture setup
    let sut = new AbsoluteUrl(urlData.Url)
    // Exercise system & Verify outcome
    Assert.Equal(urlData.Host, sut.Host)
    // Teardown

[<Theory>]
[<MemberData("AbsoluteUrlTestData")>]
let ``port`` (urlData:AbsoluteUrlTestData) =
    // Fixture setup
    let sut = new AbsoluteUrl(urlData.Url)
    // Exercise system & Verify outcome
    Assert.Equal(urlData.Port, sut.Port)
    // Teardown

[<Theory>]
[<MemberData("AbsoluteUrlTestData")>]
let ``path`` (urlData:AbsoluteUrlTestData) =
    // Fixture setup
    let sut = new AbsoluteUrl(urlData.Url)
    // Exercise system & Verify outcome
    Assert.Equal(urlData.Path, sut.Path)
    // Teardown

[<Theory>]
[<MemberData("AbsoluteUrlTestData")>]
let ``query`` (urlData:AbsoluteUrlTestData) =
    // Fixture setup
    let sut = new AbsoluteUrl(urlData.Url)
    // Exercise system & Verify outcome
    Assert.Equal(urlData.Query, sut.Query)
    // Teardown

[<Theory>]
[<MemberData("AbsoluteUrlTestData")>]
let ``fragment`` (urlData:AbsoluteUrlTestData) =
    // Fixture setup
    let sut = new AbsoluteUrl(urlData.Url)
    // Exercise system & Verify outcome
    Assert.Equal(urlData.Fragment, sut.Fragment)
    // Teardown

[<Theory>]
[<InlineData("http://www.example.com/", "com")>]
[<InlineData("http://www.anotherexample.net/", "net")>]
[<InlineData("http://www.againanexample.com:8080/", "com")>]
[<InlineData("http://www.thisisasillyexample.net:194/", "net")>]
[<InlineData("http://example.com/", "com")>]
[<InlineData("http://anotherexample.net/", "net")>]
[<InlineData("http://againanexample.com:8080/", "com")>]
[<InlineData("http://thisisasillyexample.net:194/", "net")>]
let ``top level domain`` (urlText:string, expectedTopLevelDomain:string) =
    // Fixture setup
    let tld = if expectedTopLevelDomain <> "" then Some(expectedTopLevelDomain) else None
    let sut = new AbsoluteUrl(urlText)
    // Exercise system & Verify outcome
    Assert.Equal(tld, sut.TopLevelDomain)
    // Teardown

[<Theory>]
[<InlineData("http://192.168.0.1:194/")>]
[<InlineData("http://10.0.1.1/")>]
[<InlineData("https://127.0.0.1/")>]
let ``top level domain return non when domain is an ip address`` (urlText:string) =
    // Fixture setup
    let sut = new AbsoluteUrl(urlText);
    // Exercise system & Verify outcome
    Assert.True(Option.isNone sut.TopLevelDomain)
    // Teardown

[<Theory>]
[<InlineData("http://www.example.com/", "example.com")>]
[<InlineData("http://www.anotherexample.net/", "anotherexample.net")>]
[<InlineData("http://www.againanexample.com:8080/", "againanexample.com")>]
[<InlineData("http://www.thisisasillyexample.net:194/", "thisisasillyexample.net")>]
[<InlineData("http://example.com/", "example.com")>]
[<InlineData("http://anotherexample.net/", "anotherexample.net")>]
[<InlineData("http://againanexample.com:8080/", "againanexample.com")>]
[<InlineData("http://thisisasillyexample.net:194/", "thisisasillyexample.net")>]
[<InlineData("http://this.is.even.a.sillier.example.net:194/", "example.net")>]
let ``second level domain`` (urlText:string, expectedSecondLevelDomain:string) =
    // Fixture setup
    let sld = if expectedSecondLevelDomain <> "" then Some(expectedSecondLevelDomain) else None
    let sut = new AbsoluteUrl(urlText);
    // Exercise system & Verify outcome
    Assert.Equal(sld, sut.SecondLevelDomain);
    // Teardown

[<Theory>]
[<InlineData("http://192.168.0.1:194/")>]
[<InlineData("http://10.0.1.1/")>]
[<InlineData("https://127.0.0.1/")>]
let ``SecondLevelDomainThrowsWhenDomainIsAnIpAddress`` (urlText:string) =
    // Fixture setup
    let sut = new AbsoluteUrl(urlText)
    // Exercise system & Verify outcome
    Assert.True(Option.isNone sut.SecondLevelDomain)
    // Teardown

[<InlineData("http://www.example.com/", false)>]
[<InlineData("http://www.anotherexample.net/", false)>]
[<InlineData("http://192.168.1.1/", true)>]
[<InlineData("http://192.168.0.1:194/", true)>]
[<InlineData("http://10.0.1.1/", true)>]
[<InlineData("https://127.0.0.1/", true)>]
let ``is domain an IP address`` (urlText:string, expectedIsIPAddress:bool) =
    // Fixture setup
    let sut = new AbsoluteUrl(urlText)
    // Exercise system & Verify outcome
    Assert.Equal(expectedIsIPAddress, sut.IsDomainAnIPAddress)
    // Teardown