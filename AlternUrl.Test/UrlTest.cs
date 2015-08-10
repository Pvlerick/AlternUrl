using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AlternUrl.Test
{
    public class UrlTest
    {
        [Fact]
        public void Create()
        {
            var url = Url.Create("http://username:password@example.com:8042/over/there/index.dtb?type=animal&name=narwhal#nose");

            Assert.Equal("http", url.Scheme);
            Assert.Equal("username:password", url.UserInfo);
            Assert.Equal("example.com", url.Host);
            Assert.Equal(8042, url.Port);
            Assert.Equal("/over/there/index.dtb", url.Path);
            Assert.Equal("type=animal&name=narwhal", url.Query);
            Assert.Equal("nose", url.Fragment);
            Assert.Equal("index", url.FileName);
            Assert.Equal(".dtb", url.Extension);
        }

        [Theory]
        [InlineData("http://www.google.com/", UrlKind.Absolute)]
        [InlineData("http://www.google.com", UrlKind.Absolute)]
        [InlineData("https://www.google.com/", UrlKind.Absolute)]
        [InlineData("https://www.google.com", UrlKind.Absolute)]
        [InlineData("/mail/?foo=12&bar=34#anchor", UrlKind.Relative)]
        [InlineData("mail/?foo=12&bar=34#anchor", UrlKind.Relative)]
        [InlineData("http://www.google.com/mail/?foo=12&bar=34#anchor", UrlKind.Absolute)]
        [InlineData("https://www.google.com/mail/?foo=12&bar=34#anchor", UrlKind.Absolute)]
        [InlineData("http/foo/", UrlKind.Relative)]
        [InlineData("/http/foo/", UrlKind.Relative)]
        [InlineData("/http/foo/index.html", UrlKind.Relative)]
        [InlineData("/http/foo/index.html?foo=12&bar=34#anchor", UrlKind.Relative)]
        [InlineData("/http/foo/?foo=12&bar=34#anchor", UrlKind.Relative)]
        [InlineData("/http/?foo=12&bar=34#anchor", UrlKind.Relative)]
        public void Kind(String urlText, UrlKind expectedResult)
        {
            var url = Url.Create(urlText);

            Assert.Equal(expectedResult, url.Kind);
        }

        [Theory]
        [InlineData("http://www.google.com/", "", false, "", false)]
        [InlineData("http://www.google.com", "", false, "", false)]
        [InlineData("http://www.google.com/mail", "", false, "", false)]
        [InlineData("http://www.google.com/mail/", "", false, "", false)]
        [InlineData("http://www.google.com/hello.html", "hello", true, ".html", true)]
        [InlineData("http://www.google.com/mail/hello.htm", "hello", true, ".htm", true)]
        [InlineData("http://www.google.com/mail/hello.html", "hello", true, ".html", true)]
        [InlineData("http://www.google.com/mail/hello.html#", "hello", true, ".html", true)]
        [InlineData("http://www.google.com/mail/hello.html#anchor", "hello", true, ".html", true)]
        [InlineData("http://www.google.com/mail/hello.html?", "hello", true, ".html", true)]
        [InlineData("http://www.google.com/mail/hello.html?#", "hello", true, ".html", true)]
        [InlineData("http://www.google.com/mail/hello.html?foo", "hello", true, ".html", true)]
        [InlineData("http://www.google.com/mail/hello.html?foo=12", "hello", true, ".html", true)]
        [InlineData("http://www.google.com/mail/hello.html?foo=12&bar", "hello", true, ".html", true)]
        [InlineData("http://www.google.com/mail/hello.html?foo=12&bar=34", "hello", true, ".html", true)]
        [InlineData("http://www.google.com/mail/hello.html?foo", "hello", true, ".html", true)]
        [InlineData("http://www.google.com/mail/hello.html?foo#anchor", "hello", true, ".html", true)]
        [InlineData("http://www.google.com/mail/hello.html?foo=12", "hello", true, ".html", true)]
        [InlineData("http://www.google.com/mail/hello.html?foo=12#anchor", "hello", true, ".html", true)]
        [InlineData("http://www.google.com/mail/hello.html?foo=12&bar", "hello", true, ".html", true)]
        [InlineData("http://www.google.com/mail/hello.html?foo=12&bar=34", "hello", true, ".html", true)]
        [InlineData("http://www.google.com/mail/hello.html?foo=12&bar=34#anchor", "hello", true, ".html", true)]
        [InlineData("http://www.google.com/hello", "", false, "", false)]
        [InlineData("http://www.google.com/mail#", "", false, "", false)]
        [InlineData("http://www.google.com/mail#anchor", "", false, "", false)]
        [InlineData("http://www.google.com/mail?", "", false, "", false)]
        [InlineData("http://www.google.com/mail?#", "", false, "", false)]
        [InlineData("http://www.google.com/mail?foo", "", false, "", false)]
        [InlineData("http://www.google.com/mail?foo=12", "", false, "", false)]
        [InlineData("http://www.google.com/mail?foo=12&bar", "", false, "", false)]
        [InlineData("http://www.google.com/mail?foo=12&bar=34", "", false, "", false)]
        [InlineData("http://www.google.com/mail?foo", "", false, "", false)]
        [InlineData("http://www.google.com/mail?foo#anchor", "", false, "", false)]
        [InlineData("http://www.google.com/mail?foo=12", "", false, "", false)]
        [InlineData("http://www.google.com/mail?foo=12#anchor", "", false, "", false)]
        [InlineData("http://www.google.com/mail?foo=12&bar", "", false, "", false)]
        [InlineData("http://www.google.com/mail?foo=12&bar=34", "", false, "", false)]
        [InlineData("http://www.google.com/mail?foo=12&bar=34#anchor", "", false, "", false)]
        [InlineData("http://www.google.com/mail/", "", false, "", false)]
        [InlineData("http://www.google.com/mail/#", "", false, "", false)]
        [InlineData("http://www.google.com/mail/#anchor", "", false, "", false)]
        [InlineData("http://www.google.com/mail/?", "", false, "", false)]
        [InlineData("http://www.google.com/mail/?#", "", false, "", false)]
        [InlineData("http://www.google.com/mail/?foo", "", false, "", false)]
        [InlineData("http://www.google.com/mail/?foo=12", "", false, "", false)]
        [InlineData("http://www.google.com/mail/?foo=12&bar", "", false, "", false)]
        [InlineData("http://www.google.com/mail/?foo=12&bar=34", "", false, "", false)]
        [InlineData("http://www.google.com/mail/?foo", "", false, "", false)]
        [InlineData("http://www.google.com/mail/?foo#anchor", "", false, "", false)]
        [InlineData("http://www.google.com/mail/?foo=12", "", false, "", false)]
        [InlineData("http://www.google.com/mail/?foo=12#anchor", "", false, "", false)]
        [InlineData("http://www.google.com/mail/?foo=12&bar", "", false, "", false)]
        [InlineData("http://www.google.com/mail/?foo=12&bar=34", "", false, "", false)]
        [InlineData("http://www.google.com/mail/?foo=12&bar=34#anchor", "", false, "", false)]
        [InlineData("/", "", false, "", false)]
        [InlineData("/mail", "", false, "", false)]
        [InlineData("/mail/", "", false, "", false)]
        [InlineData("/hello.html", "hello", true, ".html", true)]
        [InlineData("/mail/hello.htm", "hello", true, ".htm", true)]
        [InlineData("/mail/hello.html", "hello", true, ".html", true)]
        [InlineData("/mail/hello.html#", "hello", true, ".html", true)]
        [InlineData("/mail/hello.html#anchor", "hello", true, ".html", true)]
        [InlineData("/mail/hello.html?", "hello", true, ".html", true)]
        [InlineData("/mail/hello.html?#", "hello", true, ".html", true)]
        [InlineData("/mail/hello.html?foo", "hello", true, ".html", true)]
        [InlineData("/mail/hello.html?foo=12", "hello", true, ".html", true)]
        [InlineData("/mail/hello.html?foo=12&bar", "hello", true, ".html", true)]
        [InlineData("/mail/hello.html?foo=12&bar=34", "hello", true, ".html", true)]
        [InlineData("/mail/hello.html?foo", "hello", true, ".html", true)]
        [InlineData("/mail/hello.html?foo#anchor", "hello", true, ".html", true)]
        [InlineData("/mail/hello.html?foo=12", "hello", true, ".html", true)]
        [InlineData("/mail/hello.html?foo=12#anchor", "hello", true, ".html", true)]
        [InlineData("/mail/hello.html?foo=12&bar", "hello", true, ".html", true)]
        [InlineData("/mail/hello.html?foo=12&bar=34", "hello", true, ".html", true)]
        [InlineData("/mail/hello.html?foo=12&bar=34#anchor", "hello", true, ".html", true)]
        [InlineData("/hello", "", false, "", false)]
        [InlineData("/mail#", "", false, "", false)]
        [InlineData("/mail#anchor", "", false, "", false)]
        [InlineData("/mail?", "", false, "", false)]
        [InlineData("/mail?#", "", false, "", false)]
        [InlineData("/mail?foo", "", false, "", false)]
        [InlineData("/mail?foo=12", "", false, "", false)]
        [InlineData("/mail?foo=12&bar", "", false, "", false)]
        [InlineData("/mail?foo=12&bar=34", "", false, "", false)]
        [InlineData("/mail?foo", "", false, "", false)]
        [InlineData("/mail?foo#anchor", "", false, "", false)]
        [InlineData("/mail?foo=12", "", false, "", false)]
        [InlineData("/mail?foo=12#anchor", "", false, "", false)]
        [InlineData("/mail?foo=12&bar", "", false, "", false)]
        [InlineData("/mail?foo=12&bar=34", "", false, "", false)]
        [InlineData("/mail?foo=12&bar=34#anchor", "", false, "", false)]
        [InlineData("/mail/", "", false, "", false)]
        [InlineData("/mail/#", "", false, "", false)]
        [InlineData("/mail/#anchor", "", false, "", false)]
        [InlineData("/mail/?", "", false, "", false)]
        [InlineData("/mail/?#", "", false, "", false)]
        [InlineData("/mail/?foo", "", false, "", false)]
        [InlineData("/mail/?foo=12", "", false, "", false)]
        [InlineData("/mail/?foo=12&bar", "", false, "", false)]
        [InlineData("/mail/?foo=12&bar=34", "", false, "", false)]
        [InlineData("/mail/?foo", "", false, "", false)]
        [InlineData("/mail/?foo#anchor", "", false, "", false)]
        [InlineData("/mail/?foo=12", "", false, "", false)]
        [InlineData("/mail/?foo=12#anchor", "", false, "", false)]
        [InlineData("/mail/?foo=12&bar", "", false, "", false)]
        [InlineData("/mail/?foo=12&bar=34", "", false, "", false)]
        [InlineData("/mail/?foo=12&bar=34#anchor", "", false, "", false)]
        public void FileName_HasFileName_And_Extension_HasExtension(String urlText, String expectedFileName, bool expectedHasFileName, String expectedExtension, bool expectedHasExtension)
        {
            var url = Url.Create(urlText);

            Assert.Equal(expectedFileName, url.FileName);
            Assert.Equal(expectedHasFileName, url.HasFileName);
            Assert.Equal(expectedExtension, url.Extension);
            Assert.Equal(expectedHasExtension, url.HasExtension);
        }

        [Theory]
        [MemberData("TestData")]
        public void Scheme(UrlTestData urlData)
        {
            var url = Url.Create(urlData.Url);

            Assert.Equal(urlData.Scheme, url.Scheme);
        }

        [Theory]
        [MemberData("TestData")]
        public void UserInfo(UrlTestData urlData)
        {
            var url = Url.Create(urlData.Url);

            Assert.Equal(urlData.UserInfo, url.UserInfo);
        }

        [Theory]
        [MemberData("TestData")]
        public void Host(UrlTestData urlData)
        {
            var url = Url.Create(urlData.Url);

            Assert.Equal(urlData.Host, url.Host);
        }

        [Theory]
        [MemberData("TestData")]
        public void Port(UrlTestData urlData)
        {
            var url = Url.Create(urlData.Url);

            Assert.Equal(urlData.Port, url.Port);
        }

        [Theory]
        [MemberData("TestData")]
        public void Path(UrlTestData urlData)
        {
            var url = Url.Create(urlData.Url);

            Assert.Equal(urlData.Path, url.Path);
        }

        [Theory]
        [MemberData("TestData")]
        public void Query(UrlTestData urlData)
        {
            var url = Url.Create(urlData.Url);

            Assert.Equal(urlData.Query, url.Query);
        }

        [Theory]
        [MemberData("TestData")]
        public void Fragment(UrlTestData urlData)
        {
            var url = Url.Create(urlData.Url);

            Assert.Equal(urlData.Fragment, url.Fragment);
        }

        [Theory]
        [InlineData("http://www.example.com/", "com")]
        [InlineData("http://www.anotherexample.net/", "net")]
        [InlineData("http://www.againanexample.com:8080/", "com")]
        [InlineData("http://www.thisisasillyexample.net:194/", "net")]
        [InlineData("http://example.com/", "com")]
        [InlineData("http://anotherexample.net/", "net")]
        [InlineData("http://againanexample.com:8080/", "com")]
        [InlineData("http://thisisasillyexample.net:194/", "net")]
        public void TopLevelDomain(String urlText, String expectedTopLevelDomain)
        {
            var url = Url.Create(urlText);

            Assert.Equal(expectedTopLevelDomain, url.TopLevelDomain);
        }

        [Theory]
        [InlineData("http://www.example.com/", "example.com")]
        [InlineData("http://www.anotherexample.net/", "anotherexample.net")]
        [InlineData("http://www.againanexample.com:8080/", "againanexample.com")]
        [InlineData("http://www.thisisasillyexample.net:194/", "thisisasillyexample.net")]
        [InlineData("http://example.com/", "example.com")]
        [InlineData("http://anotherexample.net/", "anotherexample.net")]
        [InlineData("http://againanexample.com:8080/", "againanexample.com")]
        [InlineData("http://thisisasillyexample.net:194/", "thisisasillyexample.net")]
        [InlineData("http://this.is.even.a.sillier.example.net:194/", "example.net")]
        public void SecondLevelDomain(String urlText, String expectedSecondLevelDomain)
        {
            var url = Url.Create(urlText);

            Assert.Equal(expectedSecondLevelDomain, url.SecondLevelDomain);
        }

        [InlineData("http://www.example.com/", false)]
        [InlineData("http://www.anotherexample.net/", false)]
        //[InlineData("http://192.168.1.1/", true)]
        public void IsDomainIPAddress(String urlText, bool expectedIsIPAddress)
        {
            var url = Url.Create(urlText);

            Assert.Equal(expectedIsIPAddress, url.IsDomainAnIPAddress);
        }

        [Theory]
        [InlineData("http://192.168.1.1/")]
        public void TopLevelDomain_And_SecondLevelDomain_ThrowWithIPDomain(String urlText)
        {
            var url = Url.Create(urlText);

            Assert.Throws<NotSupportedException>(() => { var x = url.TopLevelDomain; });
            Assert.Throws<NotSupportedException>(() => { var x = url.SecondLevelDomain; });
        }

        [Theory]
        [MemberData("TestData")]
        public void Normalized(UrlTestData urlData)
        {
            var url = Url.Create(urlData.Url);

            Assert.Equal(urlData.Normalized, url.ToString());
        }

        //[Theory]
        ////https://en.wikipedia.org/wiki/URL_normalization - only normalization that preserves semantics is tested
        //[InlineData("HTTP://www.Example.com/", "http://www.example.com/")]
        ////[InlineData("http://www.example.com/a%c2%b1b", "http://www.example.com/a%C2%B1b")]
        ////[InlineData("http://www.example.com/%7Eusername/", "http://www.example.com/~username/")]
        //[InlineData("http://www.example.com:80/bar.html", "http://www.example.com/bar.html")]
        //public void Normalized(String urlText, String expectedUrlText)
        //{
        //    var url = Url.Create(urlText);

        //    Assert.Equal(expectedUrlText, url.ToString());
        //}

        [Theory]
        [InlineData("/mail/?foo=12&bar=34#anchor", "/mail/", "foo=12&bar=34", "anchor")]
        [InlineData("/mail/index.html?foo=12&bar=34#anchor", "/mail/index.html", "foo=12&bar=34", "anchor")]
        public void UriBuilderMembers_RelativeUrl(String urlText, String expectedPath, String expectedQuery, String expectedFragment)
        {
            var url = Url.Create(urlText);

            Assert.Throws<NotSupportedException>(() => { var x = url.Scheme; });
            Assert.Throws<NotSupportedException>(() => { var x = url.UserInfo; });
            Assert.Throws<NotSupportedException>(() => { var x = url.Host; });
            Assert.Throws<NotSupportedException>(() => { var x = url.Port; });
            Assert.Equal(expectedPath, url.Path);
            Assert.Equal(expectedQuery, url.Query);
            Assert.Equal(expectedPath + "?" + expectedQuery, url.PathAndQuery);
            Assert.Equal(expectedFragment, url.Fragment);

            Assert.Equal(urlText, url.ToString());
        }

        [Theory]
        //[InlineData("http://root:mypass@www.disney.com/mail/?foo=12&bar=34#anchor", "http://root:mypass@www.disney.com/mail/?foo=12&bar=34#anchor")]
        [InlineData("/mail/?foo=12&bar=34#anchor", "/mail/?foo=12&bar=34#anchor")]
        [InlineData("mail/?foo=12&bar=34#anchor", "/mail/?foo=12&bar=34#anchor")]
        public void ToString(String urlText, String expectedUrlText)
        {
            var url = Url.Create(urlText);

            Assert.Equal(expectedUrlText, url.ToString());
        }

        [Theory]
        [InlineData("http://www.google.com/mail/?foo=12&bar=34#anchor", false)]
        [InlineData("https://www.google.com/mail/?foo=12&bar=34#anchor", true)]
        [InlineData("HTTP://www.google.com/mail/?foo=12&bar=34#anchor", false)]
        [InlineData("HTTPS://www.google.com/mail/?foo=12&bar=34#anchor", true)]
        public void IsHttps_AbsoluteUrl(String urlText, bool expectedResult)
        {
            var url = Url.Create(urlText);

            Assert.Equal(expectedResult, url.IsHttps);
        }

        [Theory]
        [InlineData("/mail/?foo=12&bar=34#anchor", false)]
        [InlineData("mail/?foo=12&bar=34#anchor", false)]
        public void IsHttps_RelativeUrl(String urlText, bool expectedResult)
        {
            var url = Url.Create(urlText);

            Assert.Throws<NotSupportedException>(() => { var x = url.IsHttps; });
        }

        [Theory]
        [InlineData("/mail/?foo", "foo", true)]
        [InlineData("/mail/?foo=12", "foo", true)]
        [InlineData("/mail/?foo&bar", "foo", true)]
        [InlineData("/mail/?foo&bar=13", "foo", true)]
        [InlineData("/mail/?foo=12&bar=13", "foo", true)]
        [InlineData("/mail/?foo#anchor", "foo", true)]
        [InlineData("/mail/?foo=12#anchor", "foo", true)]
        [InlineData("https://www.google.com/mail/?foo", "foo", true)]
        [InlineData("https://www.google.com/mail/?foo=12", "foo", true)]
        [InlineData("https://www.google.com/mail/?foo&bar", "foo", true)]
        [InlineData("https://www.google.com/mail/?foo&bar=13", "foo", true)]
        [InlineData("https://www.google.com/mail/?foo=12&bar=13", "foo", true)]
        [InlineData("https://www.google.com/mail/?foo#anchor", "foo", true)]
        [InlineData("https://www.google.com/mail/?foo=12#anchor", "foo", true)]
        [InlineData("/mail/?bar", "foo", false)]
        [InlineData("/mail/?bar=13", "foo", false)]
        [InlineData("/mail/?bar=13#anchor", "foo", false)]
        [InlineData("https://www.google.com/mail/?bar", "foo", false)]
        [InlineData("https://www.google.com/mail/?bar=13", "foo", false)]
        [InlineData("https://www.google.com/mail/?bar=13#anchor", "foo", false)]
        [InlineData("https://www.google.com/mail/hello.html?this+is+a+test", "this is a test", true)]
        [InlineData("https://www.google.com/mail/hello.html?this+is+a+test=how+did+it+go", "this is a test", true)]
        [InlineData("https://www.google.com/mail/hello.html?this+is+a+test#anchor", "this is a test", true)]
        [InlineData("https://www.google.com/mail/hello.html?this+is+a+test=how+did+it+go#anchor", "this is a test", true)]
        public void HasParameter(String urlText, String parameter, bool expectedResult)
        {
            var url = Url.Create(urlText);

            Assert.Equal(expectedResult, url.HasParameter(parameter));
        }

        [Theory]
        [InlineData("/mail/?foo", "bar", "", "foo&bar", "/mail/?foo&bar", "/mail/?foo&bar")]
        [InlineData("/mail/?foo", "bar", "12", "foo&bar=12", "/mail/?foo&bar=12", "/mail/?foo&bar=12")]
        [InlineData("/mail/?foo=12", "bar", "13", "foo=12&bar=13", "/mail/?foo=12&bar=13", "/mail/?foo=12&bar=13")]
        [InlineData("/mail/?foo&bar", "foobar", "42", "foo&bar&foobar=42", "/mail/?foo&bar&foobar=42", "/mail/?foo&bar&foobar=42")]
        [InlineData("/mail/?foo&bar=13", "foobar", "42", "foo&bar=13&foobar=42", "/mail/?foo&bar=13&foobar=42", "/mail/?foo&bar=13&foobar=42")]
        [InlineData("/mail/?foo=12&bar=13", "foobar", "42", "foo=12&bar=13&foobar=42", "/mail/?foo=12&bar=13&foobar=42", "/mail/?foo=12&bar=13&foobar=42")]
        [InlineData("/mail/?foo#anchor", "bar", "", "foo&bar", "/mail/?foo&bar", "/mail/?foo&bar#anchor")]
        [InlineData("/mail/?foo#anchor", "bar", "12", "foo&bar=12", "/mail/?foo&bar=12", "/mail/?foo&bar=12#anchor")]
        [InlineData("/mail/?foo=12#anchor", "bar", "13", "foo=12&bar=13", "/mail/?foo=12&bar=13", "/mail/?foo=12&bar=13#anchor")]
        [InlineData("/mail/?foo=12&bar=13#anchor", "foobar", "42", "foo=12&bar=13&foobar=42", "/mail/?foo=12&bar=13&foobar=42", "/mail/?foo=12&bar=13&foobar=42#anchor")]
        [InlineData("/mail/?foo", "bar bar", "", "foo&bar+bar", "/mail/?foo&bar+bar", "/mail/?foo&bar+bar")]
        [InlineData("/mail/?foo", "bar bar", "42", "foo&bar+bar=42", "/mail/?foo&bar+bar=42", "/mail/?foo&bar+bar=42")]
        [InlineData("/mail/?foo", "bar bar", "4:2", "foo&bar+bar=4%3a2", "/mail/?foo&bar+bar=4%3a2", "/mail/?foo&bar+bar=4%3a2")]
        public void AddParameter(String urlText, String parameter, String argument, String expectedQuery, String expectedPathAndQuery, String expectedPathAndQueryAndFragment)
        {
            var url = Url.Create(urlText).AddParameter(parameter, argument);

            Assert.Equal(expectedQuery, url.Query);
            Assert.Equal(expectedPathAndQuery, url.PathAndQuery);
            Assert.Equal(expectedPathAndQueryAndFragment, url.PathAndQueryAndFragment);
        }

        [Theory]
        [InlineData("/mail/?foo", "foo", "", "/mail/", "/mail/")]
        [InlineData("/mail/?foo=12", "foo", "", "/mail/", "/mail/")]
        [InlineData("/mail/?foo&bar", "foo", "bar", "/mail/?bar", "/mail/?bar")]
        [InlineData("/mail/?foo&bar=13", "foo", "bar=13", "/mail/?bar=13", "/mail/?bar=13")]
        [InlineData("/mail/?foo=12&bar=13", "foo", "bar=13", "/mail/?bar=13", "/mail/?bar=13")]
        [InlineData("/mail/?foo#anchor", "foo", "", "/mail/", "/mail/#anchor")]
        [InlineData("/mail/?foo=12#anchor", "foo", "", "/mail/", "/mail/#anchor")]
        [InlineData("https://www.google.com/mail/?foo", "foo", "", "/mail/", "/mail/")]
        [InlineData("https://www.google.com/mail/?foo=12", "foo", "", "/mail/", "/mail/")]
        [InlineData("https://www.google.com/mail/?foo&bar", "foo", "bar", "/mail/?bar", "/mail/?bar")]
        [InlineData("https://www.google.com/mail/?foo&bar=13", "foo", "bar=13", "/mail/?bar=13", "/mail/?bar=13")]
        [InlineData("https://www.google.com/mail/?foo=12&bar=13", "foo", "bar=13", "/mail/?bar=13", "/mail/?bar=13")]
        [InlineData("https://www.google.com/mail/?foo#anchor", "foo", "", "/mail/", "/mail/#anchor")]
        [InlineData("https://www.google.com/mail/?foo=12#anchor", "foo", "", "/mail/", "/mail/#anchor")]
        [InlineData("/mail/?foo=42&bar=18&fb=45&babar=78", "fb", "foo=42&bar=18&babar=78", "/mail/?foo=42&bar=18&babar=78", "/mail/?foo=42&bar=18&babar=78")] //To test parameters order
        [InlineData("/mail/?foo=42&bar=18&fb=45&babar=78", "bar", "foo=42&fb=45&babar=78", "/mail/?foo=42&fb=45&babar=78", "/mail/?foo=42&fb=45&babar=78")] //To test parameters order
        [InlineData("/mail/?foo foo", "foo foo", "", "/mail/", "/mail/")]
        [InlineData("/mail/?foo foo&bar bar", "foo foo", "bar+bar", "/mail/?bar+bar", "/mail/?bar+bar")]
        [InlineData("/mail/?foo foo&bar bar#anchor", "foo foo", "bar+bar", "/mail/?bar+bar", "/mail/?bar+bar#anchor")]
        [InlineData("/mail/?foo foo=42", "foo foo", "", "/mail/", "/mail/")]
        [InlineData("/mail/?foo foo=42&bar bar", "foo foo", "bar+bar", "/mail/?bar+bar", "/mail/?bar+bar")]
        [InlineData("/mail/?foo foo=4:2&bar bar#anchor", "foo foo", "bar+bar", "/mail/?bar+bar", "/mail/?bar+bar#anchor")]
        public void RemoveParameter(String urlText, String parameter, String expectedQuery, String expectedPathAndQuery, String expectedPathAndQueryAndFragment)
        {
            var url = Url.Create(urlText).RemoveParameter(parameter);

            Assert.Equal(expectedQuery, url.Query);
            Assert.Equal(expectedPathAndQuery, url.PathAndQuery);
            Assert.Equal(expectedPathAndQueryAndFragment, url.PathAndQueryAndFragment);
        }

        //[InlineData("http://www.google.com", "/mail/index.html", "http://www.google/com/mail/index.html")]
        //public void Concat(String urlText, String secondUrlText, String expectedUrlString)
        //{
        //    var url = Url.Create(urlText);
        //    var secondUrl = new Url(secondUrlText);

        //    Assert.Equal(expectedUrlString, url.Concat(secondUrl).ToString());
        //}

        public class UrlTestData
        {
            public String Url { get; set; }
            public String Normalized { get; set; }
            public String Scheme { get; set; }
            public String UserInfo { get; set; }
            public String Host { get; set; }
            public int Port { get; set; }
            public String Path { get; set; }
            public String Query { get; set; }
            public String Fragment { get; set; }

            public UrlTestData(String url, String urlToString, String scheme, String userInfo, String host, int port, String path, String query, String fragment)
            {
                this.Url = url;
                this.Normalized = urlToString;
                this.Scheme = scheme;
                this.UserInfo = userInfo;
                this.Host = host;
                this.Port = port;
                this.Path = path;
                this.Query = query;
                this.Fragment = fragment;
            }
        }

        public static IEnumerable TestData
        {
            get
                {
                    yield return new object[] { new UrlTestData("http://www.google.com","http://www.google.com/","http","","www.google.com",80,"/","", "") };
                    yield return new object[] { new UrlTestData("HTTP://WWW.GOOGLE.COM","http://www.google.com/","http","","www.google.com",80,"/","", "") };
                    yield return new object[] { new UrlTestData("http://www.google.com/","http://www.google.com/","http","","www.google.com",80,"/","", "") };
                    yield return new object[] { new UrlTestData("http://www.google.com/mail","http://www.google.com/mail","http","","www.google.com",80,"/mail","", "") };
                    yield return new object[] { new UrlTestData("http://WWW.GOOGLE.COM/mail","http://www.google.com/mail","http","","www.google.com",80,"/mail","", "") };
                    yield return new object[] { new UrlTestData("http://WWW.GOOGLE.COM/MAIL","http://www.google.com/MAIL","http","","www.google.com",80,"/MAIL","", "") };
                    yield return new object[] { new UrlTestData("http://www.google.com/mail/","http://www.google.com/mail/","http","","www.google.com",80,"/mail/","", "") };
                    yield return new object[] { new UrlTestData("http://www.google.com/hello.html","http://www.google.com/hello.html","http","","www.google.com",80,"/hello.html","", "") };
                    yield return new object[] { new UrlTestData("http://www.google.com/mail/hello.htm","http://www.google.com/mail/hello.htm","http","","www.google.com",80,"/mail/hello.htm","", "") };
                    yield return new object[] { new UrlTestData("http://www.google.com/mail/hello.html","http://www.google.com/mail/hello.html","http","","www.google.com",80,"/mail/hello.html","", "") };
                    yield return new object[] { new UrlTestData("http://www.google.com/mail/hello.html#","http://www.google.com/mail/hello.html","http","","www.google.com",80,"/mail/hello.html","", "") };
                    yield return new object[] { new UrlTestData("http://www.google.com/mail/hello.html#anchor","http://www.google.com/mail/hello.html#anchor","http","","www.google.com",80,"/mail/hello.html","", "anchor") };
                    yield return new object[] { new UrlTestData("http://www.google.com/mail/hello.html?","http://www.google.com/mail/hello.html","http","","www.google.com",80,"/mail/hello.html","", "") };
                    yield return new object[] { new UrlTestData("http://www.google.com/mail/hello.html?#","http://www.google.com/mail/hello.html","http","","www.google.com",80,"/mail/hello.html","", "") };
                    yield return new object[] { new UrlTestData("http://www.google.com/mail/hello.html?foo","http://www.google.com/mail/hello.html?foo","http","","www.google.com",80,"/mail/hello.html","foo", "") };
                    yield return new object[] { new UrlTestData("http://www.google.com/mail/hello.html?foo=12","http://www.google.com/mail/hello.html?foo=12","http","","www.google.com",80,"/mail/hello.html","foo=12", "") };
                    yield return new object[] { new UrlTestData("http://www.google.com/mail/hello.html?foo=12&bar","http://www.google.com/mail/hello.html?foo=12&bar","http","","www.google.com",80,"/mail/hello.html","foo=12&bar", "") };
                    yield return new object[] { new UrlTestData("http://www.google.com/mail/hello.html?foo=12&bar=34","http://www.google.com/mail/hello.html?foo=12&bar=34","http","","www.google.com",80,"/mail/hello.html","foo=12&bar=34", "") };
                    yield return new object[] { new UrlTestData("http://www.google.com/mail/hello.html?foo","http://www.google.com/mail/hello.html?foo","http","","www.google.com",80,"/mail/hello.html","foo", "") };
                    yield return new object[] { new UrlTestData("http://www.google.com/mail/hello.html?foo#anchor","http://www.google.com/mail/hello.html?foo#anchor","http","","www.google.com",80,"/mail/hello.html","foo", "anchor") };
                    yield return new object[] { new UrlTestData("http://www.google.com/mail/hello.html?foo=12","http://www.google.com/mail/hello.html?foo=12","http","","www.google.com",80,"/mail/hello.html","foo=12", "") };
                    yield return new object[] { new UrlTestData("http://www.google.com/mail/hello.html?foo=12#anchor","http://www.google.com/mail/hello.html?foo=12#anchor","http","","www.google.com",80,"/mail/hello.html","foo=12", "anchor") };
                    yield return new object[] { new UrlTestData("http://www.google.com/mail/hello.html?foo=12&bar","http://www.google.com/mail/hello.html?foo=12&bar","http","","www.google.com",80,"/mail/hello.html","foo=12&bar", "") };
                    yield return new object[] { new UrlTestData("http://www.google.com/mail/hello.html?foo=12&bar=34","http://www.google.com/mail/hello.html?foo=12&bar=34","http","","www.google.com",80,"/mail/hello.html","foo=12&bar=34", "") };
                    yield return new object[] { new UrlTestData("http://www.google.com/mail/hello.html?foo=12&bar=34#anchor","http://www.google.com/mail/hello.html?foo=12&bar=34#anchor","http","","www.google.com",80,"/mail/hello.html","foo=12&bar=34", "anchor") };
                    yield return new object[] { new UrlTestData("http://www.google.com/hello","http://www.google.com/hello","http","","www.google.com",80,"/hello","", "") };
                    yield return new object[] { new UrlTestData("http://www.google.com/mail#","http://www.google.com/mail","http","","www.google.com",80,"/mail","", "") };
                    yield return new object[] { new UrlTestData("http://www.google.com/mail#anchor","http://www.google.com/mail#anchor","http","","www.google.com",80,"/mail","", "anchor") };
                    yield return new object[] { new UrlTestData("http://www.google.com/mail?","http://www.google.com/mail","http","","www.google.com",80,"/mail","", "") };
                    yield return new object[] { new UrlTestData("http://www.google.com/mail?#","http://www.google.com/mail","http","","www.google.com",80,"/mail","", "") };
                    yield return new object[] { new UrlTestData("http://www.google.com/mail?foo","http://www.google.com/mail?foo","http","","www.google.com",80,"/mail","foo", "") };
                    yield return new object[] { new UrlTestData("http://www.google.com/mail?foo=12","http://www.google.com/mail?foo=12","http","","www.google.com",80,"/mail","foo=12", "") };
                    yield return new object[] { new UrlTestData("http://www.google.com/mail?foo=12&bar","http://www.google.com/mail?foo=12&bar","http","","www.google.com",80,"/mail","foo=12&bar", "") };
                    yield return new object[] { new UrlTestData("http://www.google.com/mail?foo=12&bar=34","http://www.google.com/mail?foo=12&bar=34","http","","www.google.com",80,"/mail","foo=12&bar=34", "") };
                    yield return new object[] { new UrlTestData("http://www.google.com/mail?foo","http://www.google.com/mail?foo","http","","www.google.com",80,"/mail","foo", "") };
                    yield return new object[] { new UrlTestData("http://www.google.com/mail?foo#anchor","http://www.google.com/mail?foo#anchor","http","","www.google.com",80,"/mail","foo", "anchor") };
                    yield return new object[] { new UrlTestData("http://www.google.com/mail?foo=12","http://www.google.com/mail?foo=12","http","","www.google.com",80,"/mail","foo=12", "") };
                    yield return new object[] { new UrlTestData("http://www.google.com/mail?foo=12#anchor","http://www.google.com/mail?foo=12#anchor","http","","www.google.com",80,"/mail","foo=12", "anchor") };
                    yield return new object[] { new UrlTestData("http://www.google.com/mail?foo=12&bar","http://www.google.com/mail?foo=12&bar","http","","www.google.com",80,"/mail","foo=12&bar", "") };
                    yield return new object[] { new UrlTestData("http://www.google.com/mail?foo=12&bar=34","http://www.google.com/mail?foo=12&bar=34","http","","www.google.com",80,"/mail","foo=12&bar=34", "") };
                    yield return new object[] { new UrlTestData("http://www.google.com/mail?foo=12&bar=34#anchor","http://www.google.com/mail?foo=12&bar=34#anchor","http","","www.google.com",80,"/mail","foo=12&bar=34", "anchor") };
                    yield return new object[] { new UrlTestData("http://www.google.com/mail/","http://www.google.com/mail/","http","","www.google.com",80,"/mail/","", "") };
                    yield return new object[] { new UrlTestData("http://www.google.com/mail/#","http://www.google.com/mail/","http","","www.google.com",80,"/mail/","", "") };
                    yield return new object[] { new UrlTestData("http://www.google.com/mail/#anchor","http://www.google.com/mail/#anchor","http","","www.google.com",80,"/mail/","", "anchor") };
                    yield return new object[] { new UrlTestData("http://www.google.com/mail/?","http://www.google.com/mail/","http","","www.google.com",80,"/mail/","", "") };
                    yield return new object[] { new UrlTestData("http://www.google.com/mail/?#","http://www.google.com/mail/","http","","www.google.com",80,"/mail/","", "") };
                    yield return new object[] { new UrlTestData("http://www.google.com/mail/?foo","http://www.google.com/mail/?foo","http","","www.google.com",80,"/mail/","foo", "") };
                    yield return new object[] { new UrlTestData("http://www.google.com/mail/?foo=12","http://www.google.com/mail/?foo=12","http","","www.google.com",80,"/mail/","foo=12", "") };
                    yield return new object[] { new UrlTestData("http://www.google.com/mail/?foo=12&bar","http://www.google.com/mail/?foo=12&bar","http","","www.google.com",80,"/mail/","foo=12&bar", "") };
                    yield return new object[] { new UrlTestData("http://www.google.com/mail/?foo=12&bar=34","http://www.google.com/mail/?foo=12&bar=34","http","","www.google.com",80,"/mail/","foo=12&bar=34", "") };
                    yield return new object[] { new UrlTestData("http://www.google.com/mail/?foo","http://www.google.com/mail/?foo","http","","www.google.com",80,"/mail/","foo", "") };
                    yield return new object[] { new UrlTestData("http://www.google.com/mail/?foo#anchor","http://www.google.com/mail/?foo#anchor","http","","www.google.com",80,"/mail/","foo", "anchor") };
                    yield return new object[] { new UrlTestData("http://www.google.com/mail/?foo=12","http://www.google.com/mail/?foo=12","http","","www.google.com",80,"/mail/","foo=12", "") };
                    yield return new object[] { new UrlTestData("http://www.google.com/mail/?foo=12#anchor","http://www.google.com/mail/?foo=12#anchor","http","","www.google.com",80,"/mail/","foo=12", "anchor") };
                    yield return new object[] { new UrlTestData("http://www.google.com/mail/?foo=12&bar","http://www.google.com/mail/?foo=12&bar","http","","www.google.com",80,"/mail/","foo=12&bar", "") };
                    yield return new object[] { new UrlTestData("http://www.google.com/mail/?foo=12&bar=34","http://www.google.com/mail/?foo=12&bar=34","http","","www.google.com",80,"/mail/","foo=12&bar=34", "") };
                    yield return new object[] { new UrlTestData("http://www.google.com/mail/?foo=12&bar=34#anchor","http://www.google.com/mail/?foo=12&bar=34#anchor","http","","www.google.com",80,"/mail/","foo=12&bar=34", "anchor") };
                    yield return new object[] { new UrlTestData("http://root:mypass@www.google.com/mail/?foo=12&bar=34#anchor","http://root:mypass@www.google.com/mail/?foo=12&bar=34#anchor","http","root:mypass","www.google.com",80,"/mail/","foo=12&bar=34", "anchor") };
                    yield return new object[] { new UrlTestData("http://root:mypass@www.google.com:90/mail/?foo=12&bar=34#anchor","http://root:mypass@www.google.com:90/mail/?foo=12&bar=34#anchor","http","root:mypass","www.google.com",90,"/mail/","foo=12&bar=34", "anchor") };
                    yield return new object[] { new UrlTestData("https://root:mypass@www.google.com/mail/?foo=12&bar=34#anchor","https://root:mypass@www.google.com/mail/?foo=12&bar=34#anchor","https","root:mypass","www.google.com",443,"/mail/","foo=12&bar=34", "anchor") };
                    yield return new object[] { new UrlTestData("https://root:mypass@www.google.com:444/mail/?foo=12&bar=34#anchor","https://root:mypass@www.google.com:444/mail/?foo=12&bar=34#anchor","https","root:mypass","www.google.com",444,"/mail/","foo=12&bar=34", "anchor") };
                    yield return new object[] { new UrlTestData("http://root:mypass@www.google.com/mail/?foo=12&bar=34#anchor","http://root:mypass@www.google.com/mail/?foo=12&bar=34#anchor","http","root:mypass","www.google.com",80,"/mail/","foo=12&bar=34", "anchor") };
                    yield return new object[] { new UrlTestData("http://www.google.com/mail/?foo=12&bar=34#anchor","http://www.google.com/mail/?foo=12&bar=34#anchor","http","","www.google.com",80,"/mail/","foo=12&bar=34", "anchor") };
                    yield return new object[] { new UrlTestData("https://root:mypass@www.google.com/mail/?foo=12&bar=34#anchor","https://root:mypass@www.google.com/mail/?foo=12&bar=34#anchor","https","root:mypass","www.google.com",443,"/mail/","foo=12&bar=34", "anchor") };
                    yield return new object[] { new UrlTestData("https://www.google.com/mail/?foo=12&bar=34#anchor","https://www.google.com/mail/?foo=12&bar=34#anchor","https","","www.google.com",443,"/mail/","foo=12&bar=34", "anchor") };
            }
        }
    }
}
