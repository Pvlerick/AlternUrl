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