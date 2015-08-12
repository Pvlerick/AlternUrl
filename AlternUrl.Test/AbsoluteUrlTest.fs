module AlternUrl.Test.AbsoluteUrlTest

type AbsoluteUrlTestData =
    {
        Url:string
        Scheme:Scheme.T
        UserInfo:string
        Host:string
        Port:int
        Path:string
        Query:string
        Fragment:string
    }

let AbsoluteUrlTestData = 
    [
        [| "http://www.google.com"; "http"; ""; "www.google.com"; "80"; "/"; ""; "" |]
        [| "HTTP://WWW.GOOGLE.COM"; "HTTP"; ""; "WWW.GOOGLE.COM"; "80"; "/"; ""; "" |]
        [| "http://www.google.com/"; "http"; ""; "www.google.com"; "80"; "/"; ""; "" |]
        [| "http://www.google.com/mail"; "http"; ""; "www.google.com"; "80"; "/mail"; ""; "" |]
        [| "http://WWW.GOOGLE.COM/mail"; "http"; ""; "WWW.GOOGLE.COM"; "80"; "/mail"; ""; "" |]
        [| "http://WWW.GOOGLE.COM/MAIL"; "http"; ""; "WWW.GOOGLE.COM"; "80"; "/MAIL"; ""; "" |]
        [| "http://www.google.com/mail/"; "http"; ""; "www.google.com"; "80"; "/mail/"; ""; "" |]
        [| "http://www.google.com/hello.html"; "http"; ""; "www.google.com"; "80"; "/hello.html"; ""; "" |]
        [| "http://www.google.com/mail/hello.htm"; "http"; ""; "www.google.com"; "80"; "/mail/hello.htm"; ""; "" |]
        [| "http://www.google.com/mail/hello.html"; "http"; ""; "www.google.com"; "80"; "/mail/hello.html"; ""; "" |]
        [| "http://www.google.com/mail/hello.html#"; "http"; ""; "www.google.com"; "80"; "/mail/hello.html"; ""; "" |]
        [| "http://www.google.com/mail/hello.html#anchor"; "http"; ""; "www.google.com"; "80"; "/mail/hello.html"; ""; "anchor" |]
        [| "http://www.google.com/mail/hello.html?"; "http"; ""; "www.google.com"; "80"; "/mail/hello.html"; ""; "" |]
        [| "http://www.google.com/mail/hello.html?#"; "http"; ""; "www.google.com"; "80"; "/mail/hello.html"; ""; "" |]
        [| "http://www.google.com/mail/hello.html?foo"; "http"; ""; "www.google.com"; "80"; "/mail/hello.html"; "foo"; "" |]
        [| "http://www.google.com/mail/hello.html?foo=12"; "http"; ""; "www.google.com"; "80"; "/mail/hello.html"; "foo=12"; "" |]
        [| "http://www.google.com/mail/hello.html?foo=12&bar"; "http"; ""; "www.google.com"; "80"; "/mail/hello.html"; "foo=12&bar"; "" |]
        [| "http://www.google.com/mail/hello.html?foo=12&bar=34"; "http"; ""; "www.google.com"; "80"; "/mail/hello.html"; "foo=12&bar=34"; "" |]
        [| "http://www.google.com/mail/hello.html?foo"; "http"; ""; "www.google.com"; "80"; "/mail/hello.html"; "foo"; "" |]
        [| "http://www.google.com/mail/hello.html?foo#anchor"; "http"; ""; "www.google.com"; "80"; "/mail/hello.html"; "foo"; "anchor" |]
        [| "http://www.google.com/mail/hello.html?foo=12"; "http"; ""; "www.google.com"; "80"; "/mail/hello.html"; "foo=12"; "" |]
        [| "http://www.google.com/mail/hello.html?foo=12#anchor"; "http"; ""; "www.google.com"; "80"; "/mail/hello.html"; "foo=12"; "anchor" |]
        [| "http://www.google.com/mail/hello.html?foo=12&bar"; "http"; ""; "www.google.com"; "80"; "/mail/hello.html"; "foo=12&bar"; "" |]
        [| "http://www.google.com/mail/hello.html?foo=12&bar=34"; "http"; ""; "www.google.com"; "80"; "/mail/hello.html"; "foo=12&bar=34"; "" |]
        [| "http://www.google.com/mail/hello.html?foo=12&bar=34#anchor"; "http"; ""; "www.google.com"; "80"; "/mail/hello.html"; "foo=12&bar=34"; "anchor" |]
        [| "http://www.google.com/hello"; "http"; ""; "www.google.com"; "80"; "/hello"; ""; "" |]
        [| "http://www.google.com/mail#"; "http"; ""; "www.google.com"; "80"; "/mail"; ""; "" |]
        [| "http://www.google.com/mail#anchor"; "http"; ""; "www.google.com"; "80"; "/mail"; ""; "anchor" |]
        [| "http://www.google.com/mail?"; "http"; ""; "www.google.com"; "80"; "/mail"; ""; "" |]
        [| "http://www.google.com/mail?#"; "http"; ""; "www.google.com"; "80"; "/mail"; ""; "" |]
        [| "http://www.google.com/mail?foo"; "http"; ""; "www.google.com"; "80"; "/mail"; "foo"; "" |]
        [| "http://www.google.com/mail?foo=12"; "http"; ""; "www.google.com"; "80"; "/mail"; "foo=12"; "" |]
        [| "http://www.google.com/mail?foo=12&bar"; "http"; ""; "www.google.com"; "80"; "/mail"; "foo=12&bar"; "" |]
        [| "http://www.google.com/mail?foo=12&bar=34"; "http"; ""; "www.google.com"; "80"; "/mail"; "foo=12&bar=34"; "" |]
        [| "http://www.google.com/mail?foo"; "http"; ""; "www.google.com"; "80"; "/mail"; "foo"; "" |]
        [| "http://www.google.com/mail?foo#anchor"; "http"; ""; "www.google.com"; "80"; "/mail"; "foo"; "anchor" |]
        [| "http://www.google.com/mail?foo=12"; "http"; ""; "www.google.com"; "80"; "/mail"; "foo=12"; "" |]
        [| "http://www.google.com/mail?foo=12#anchor"; "http"; ""; "www.google.com"; "80"; "/mail"; "foo=12"; "anchor" |]
        [| "http://www.google.com/mail?foo=12&bar"; "http"; ""; "www.google.com"; "80"; "/mail"; "foo=12&bar"; "" |]
        [| "http://www.google.com/mail?foo=12&bar=34"; "http"; ""; "www.google.com"; "80"; "/mail"; "foo=12&bar=34"; "" |]
        [| "http://www.google.com/mail?foo=12&bar=34#anchor"; "http"; ""; "www.google.com"; "80"; "/mail"; "foo=12&bar=34"; "anchor" |]
        [| "http://www.google.com/mail/"; "http"; ""; "www.google.com"; "80"; "/mail/"; ""; "" |]
        [| "http://www.google.com/mail/#"; "http"; ""; "www.google.com"; "80"; "/mail/"; ""; "" |]
        [| "http://www.google.com/mail/#anchor"; "http"; ""; "www.google.com"; "80"; "/mail/"; ""; "anchor" |]
        [| "http://www.google.com/mail/?"; "http"; ""; "www.google.com"; "80"; "/mail/"; ""; "" |]
        [| "http://www.google.com/mail/?#"; "http"; ""; "www.google.com"; "80"; "/mail/"; ""; "" |]
        [| "http://www.google.com/mail/?foo"; "http"; ""; "www.google.com"; "80"; "/mail/"; "foo"; "" |]
        [| "http://www.google.com/mail/?foo=12"; "http"; ""; "www.google.com"; "80"; "/mail/"; "foo=12"; "" |]
        [| "http://www.google.com/mail/?foo=12&bar"; "http"; ""; "www.google.com"; "80"; "/mail/"; "foo=12&bar"; "" |]
        [| "http://www.google.com/mail/?foo=12&bar=34"; "http"; ""; "www.google.com"; "80"; "/mail/"; "foo=12&bar=34"; "" |]
        [| "http://www.google.com/mail/?foo"; "http"; ""; "www.google.com"; "80"; "/mail/"; "foo"; "" |]
        [| "http://www.google.com/mail/?foo#anchor"; "http"; ""; "www.google.com"; "80"; "/mail/"; "foo"; "anchor" |]
        [| "http://www.google.com/mail/?foo=12"; "http"; ""; "www.google.com"; "80"; "/mail/"; "foo=12"; "" |]
        [| "http://www.google.com/mail/?foo=12#anchor"; "http"; ""; "www.google.com"; "80"; "/mail/"; "foo=12"; "anchor" |]
        [| "http://www.google.com/mail/?foo=12&bar"; "http"; ""; "www.google.com"; "80"; "/mail/"; "foo=12&bar"; "" |]
        [| "http://www.google.com/mail/?foo=12&bar=34"; "http"; ""; "www.google.com"; "80"; "/mail/"; "foo=12&bar=34"; "" |]
        [| "http://www.google.com/mail/?foo=12&bar=34#anchor"; "http"; ""; "www.google.com"; "80"; "/mail/"; "foo=12&bar=34"; "anchor" |]
        [| "http://root:mypass@www.google.com/mail/?foo=12&bar=34#anchor"; "http"; "root:mypass"; "www.google.com"; "80"; "/mail/"; "foo=12&bar=34"; "anchor" |]
        [| "http://root:mypass@www.google.com:90/mail/?foo=12&bar=34#anchor"; "http"; "root:mypass"; "www.google.com"; "90"; "/mail/"; "foo=12&bar=34"; "anchor" |]
        [| "https://root:mypass@www.google.com/mail/?foo=12&bar=34#anchor"; "https"; "root:mypass"; "www.google.com"; "443"; "/mail/"; "foo=12&bar=34"; "anchor" |]
        [| "https://root:mypass@www.google.com:444/mail/?foo=12&bar=34#anchor"; "https"; "root:mypass"; "www.google.com"; "444"; "/mail/"; "foo=12&bar=34"; "anchor" |]
        [| "http://root:mypass@www.google.com/mail/?foo=12&bar=34#anchor"; "http"; "root:mypass"; "www.google.com"; "80"; "/mail/"; "foo=12&bar=34"; "anchor" |]
        [| "http://www.google.com/mail/?foo=12&bar=34#anchor"; "http"; ""; "www.google.com"; "80"; "/mail/"; "foo=12&bar=34"; "anchor" |]
        [| "https://root:mypass@www.google.com/mail/?foo=12&bar=34#anchor"; "https"; "root:mypass"; "www.google.com"; "443"; "/mail/"; "foo=12&bar=34"; "anchor" |]
        [| "https://www.google.com/mail/?foo=12&bar=34#anchor"; "https"; ""; "www.google.com"; "443"; "/mail/"; "foo=12&bar=34"; "anchor" |]
    ]
    |> Seq.map (fun a -> [|
                            {
                                Url = a.[0]
                                Scheme = Option.get (Scheme.create a.[1])
                                UserInfo = a.[2]
                                Host = a.[3]
                                Port = System.Int32.Parse a.[4]
                                Path = a.[5]
                                Query = a.[6]
                                Fragment = a.[7]
                            } |])

open System
open Xunit
open AlternUrl 

[<Fact>]
let ``constructor with string`` () =
    // Fixture setup
    let sut = AbsoluteUrl.Create("http://username:password@example.com:8042/over/there/index.dtb?type=animal&name=narwhal#nose")
    // Exercise system & Verify outcome
    Assert.Equal("http", Scheme.value sut.Scheme)
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
    let sut = AbsoluteUrl.Create(uri)
    // Exercise system & Verify outcome
    Assert.Equal("http", Scheme.value sut.Scheme)
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
    Assert.Throws<ArgumentNullException>(fun () -> AbsoluteUrl.Create(null) |> ignore)
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
    Assert.Throws<ArgumentException>(fun () -> AbsoluteUrl.Create(urlText) |> ignore)
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
    let sut = AbsoluteUrl.Create(urlText)
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
    let sut = AbsoluteUrl.Create(urlData.Url)
    // Exercise system & Verify outcome
    Assert.Equal(urlData.Scheme, sut.Scheme)
    // Teardown

[<Theory>]
[<MemberData("AbsoluteUrlTestData")>]
let ``user info`` (urlData:AbsoluteUrlTestData) =
    // Fixture setup
    let sut = AbsoluteUrl.Create(urlData.Url)
    // Exercise system & Verify outcome
    Assert.Equal(urlData.UserInfo, sut.UserInfo)
    // Teardown

[<Theory>]
[<MemberData("AbsoluteUrlTestData")>]
let ``host`` (urlData:AbsoluteUrlTestData) =
    // Fixture setup
    let sut = AbsoluteUrl.Create(urlData.Url)
    // Exercise system & Verify outcome
    Assert.Equal(urlData.Host, sut.Host)
    // Teardown

[<Theory>]
[<MemberData("AbsoluteUrlTestData")>]
let ``port`` (urlData:AbsoluteUrlTestData) =
    // Fixture setup
    let sut = AbsoluteUrl.Create(urlData.Url)
    // Exercise system & Verify outcome
    Assert.Equal(urlData.Port, sut.Port)
    // Teardown

[<Theory>]
[<MemberData("AbsoluteUrlTestData")>]
let ``path`` (urlData:AbsoluteUrlTestData) =
    // Fixture setup
    let sut = AbsoluteUrl.Create(urlData.Url)
    // Exercise system & Verify outcome
    Assert.Equal(urlData.Path, sut.Path)
    // Teardown

[<Theory>]
[<MemberData("AbsoluteUrlTestData")>]
let ``query`` (urlData:AbsoluteUrlTestData) =
    // Fixture setup
    let sut = AbsoluteUrl.Create(urlData.Url)
    // Exercise system & Verify outcome
    Assert.Equal(urlData.Query, sut.Query)
    // Teardown

[<Theory>]
[<MemberData("AbsoluteUrlTestData")>]
let ``fragment`` (urlData:AbsoluteUrlTestData) =
    // Fixture setup
    let sut = AbsoluteUrl.Create(urlData.Url)
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
    let sut = AbsoluteUrl.Create(urlText)
    // Exercise system & Verify outcome
    Assert.Equal(tld, sut.TopLevelDomain)
    // Teardown

[<Theory>]
[<InlineData("http://192.168.0.1:194/")>]
[<InlineData("http://10.0.1.1/")>]
[<InlineData("https://127.0.0.1/")>]
let ``top level domain return non when domain is an ip address`` (urlText:string) =
    // Fixture setup
    let sut = AbsoluteUrl.Create(urlText);
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
    let sut = AbsoluteUrl.Create(urlText);
    // Exercise system & Verify outcome
    Assert.Equal(sld, sut.SecondLevelDomain);
    // Teardown

[<Theory>]
[<InlineData("http://192.168.0.1:194/")>]
[<InlineData("http://10.0.1.1/")>]
[<InlineData("https://127.0.0.1/")>]
let ``SecondLevelDomainThrowsWhenDomainIsAnIpAddress`` (urlText:string) =
    // Fixture setup
    let sut = AbsoluteUrl.Create(urlText)
    // Exercise system & Verify outcome
    Assert.True(Option.isNone sut.SecondLevelDomain)
    // Teardown

[<Theory>]
[<InlineData("http://www.example.com/", false)>]
[<InlineData("http://www.anotherexample.net/", false)>]
[<InlineData("http://192.168.1.1/", true)>]
[<InlineData("http://192.168.0.1:194/", true)>]
[<InlineData("http://10.0.1.1/", true)>]
[<InlineData("https://127.0.0.1/", true)>]
let ``is domain an IP address`` (urlText:string, expectedIsIPAddress:bool) =
    // Fixture setup
    let sut = AbsoluteUrl.Create(urlText)
    // Exercise system & Verify outcome
    Assert.Equal(expectedIsIPAddress, sut.IsDomainAnIPAddress)
    // Teardown

[<Theory>]
[<InlineData("http://www.example.com/", "https", "https://www.example.com/")>]
[<InlineData("https://www.example.com/", "http", "http://www.example.com/")>]
[<InlineData("http://www.example.com/", "http", "http://www.example.com/")>]
let ``to string - scheme`` (urlText:string, schemeText:string, expectedUrlText:string) =
    // Fixture setup
    let sut = AbsoluteUrl.Create(urlText)
    let scheme = Option.get (Scheme.create schemeText)
    // Exercise system
    let url = { sut with Scheme = scheme }
    // Verify outcome
    Assert.Equal(expectedUrlText, url.ToString)
    // Teardown  

[<Theory>]
[<InlineData("http://user@www.example.com/", "otheruser", "http://otheruser@www.example.com/")>]
[<InlineData("https://user:pass@www.example.com/", "otheruser:otherpass", "https://otheruser:otherpass@www.example.com/")>]
[<InlineData("http://user@www.example.com/", "", "http://www.example.com/")>]
let ``to string - userinfo`` (urlText:string, userinfo:string, expectedUrlText:string) =
    // Fixture setup
    let sut = AbsoluteUrl.Create(urlText)
    // Exercise system
    let url = { sut with UserInfo = userinfo }
    // Verify outcome
    Assert.Equal(expectedUrlText, url.ToString)
    // Teardown  

[<Theory>]
[<InlineData("http://www.example.com/", "www2.google.co.uk", "http://www2.google.co.uk/")>]
let ``to string - host`` (urlText:string, host:string, expectedUrlText:string) =
    // Fixture setup
    let sut = AbsoluteUrl.Create(urlText)
    // Exercise system
    let url = { sut with Host = host }
    // Verify outcome
    Assert.Equal(expectedUrlText, url.ToString)
    // Teardown

[<Theory>]
[<InlineData("http://www.example.com/", "45", "http://www.example.com:45/")>]
[<InlineData("http://www.example.com:8080/", "8584", "http://www.example.com:8584/")>]
[<InlineData("http://www.example.com:1234/", "80", "http://www.example.com/")>]
[<InlineData("https://www.example.com:9090/", "9292", "https://www.example.com:9292/")>]
[<InlineData("https://www.example.com/", "8080", "https://www.example.com:8080/")>]
[<InlineData("https://www.example.com:12345/", "442", "https://www.example.com/")>]
let ``to string - port`` (urlText:string, port:int, expectedUrlText:string) =
    // Fixture setup
    let sut = AbsoluteUrl.Create(urlText)
    // Exercise system
    let url = { sut with Port = port }
    // Verify outcome
    Assert.Equal(expectedUrlText, url.ToString)
    // Teardown

[<Theory>]
[<InlineData("http://www.example.com/", "index.html", "http://www.example.com/index.html")>]
[<InlineData("http://www.example.com", "index.html", "http://www.example.com/index.html")>]
[<InlineData("http://www.example.com/index.html", "", "http://www.example.com/")>]
let ``to string - path`` (urlText:string, path:string, expectedUrlText:string) =
    // Fixture setup
    let sut = AbsoluteUrl.Create(urlText)
    // Exercise system
    let url = { sut with Path = path }
    // Verify outcome
    Assert.Equal(expectedUrlText, url.ToString)
    // Teardown