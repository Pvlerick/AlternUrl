module AlternUrl.Test.AbsoluteUrlTest

open System
open AlternUrl
open Xunit

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
    Assert.Equal("index", sut.FileName)
    Assert.Equal(".dtb", sut.Extension)
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
    Assert.Equal("index", sut.FileName)
    Assert.Equal(".dtb", sut.Extension)
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
    let sut = new AbsoluteUrl(urlText)
    // Exercise system & Verify outcome
    Assert.Equal(expectedFileName, sut.FileName)
    Assert.Equal(expectedHasFileName, sut.HasFileName)
    Assert.Equal(expectedExtension, sut.Extension)
    Assert.Equal(expectedHasExtension, sut.HasExtension)
    // Teardown