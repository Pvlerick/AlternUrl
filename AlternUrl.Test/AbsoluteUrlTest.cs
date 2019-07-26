using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace AlternUrl.Test
{
    public class AbsoluteUrlTest
    {
        [Fact]
        public void Create()
        {
            // Fixture setup
            // Exercise system
            var url = AbsoluteUrl.Create("http://username:password@example.com:8042/over/there/index.dtb?type=animal&name=narwhal#nose");
            // Verify outcome
            Assert.Equal("http", url.Scheme);
            Assert.Equal("username:password", url.UserInfo);
            Assert.Equal("example.com", url.Host);
            Assert.Equal(8042, url.Port);
            Assert.Equal("/over/there/index.dtb", url.Path);
            Assert.Equal("type=animal&name=narwhal", url.Query);
            Assert.Equal("nose", url.Fragment);
            Assert.Equal("index.dtb", url.FileName);
            Assert.Equal(".dtb", url.Extension);
            // Teardown  
        }

        [Theory]
        [InlineData("http://www.google.com/", "", false, "", false)]
        [InlineData("http://www.google.com", "", false, "", false)]
        [InlineData("http://www.google.com/mail", "", false, "", false)]
        [InlineData("http://www.google.com/mail/", "", false, "", false)]
        [InlineData("http://www.google.com/hello.html", "hello.html", true, ".html", true)]
        [InlineData("http://www.google.com/mail/hello.htm", "hello.htm", true, ".htm", true)]
        [InlineData("http://www.google.com/mail/hello.html", "hello.html", true, ".html", true)]
        [InlineData("http://www.google.com/mail/hello.html#", "hello.html", true, ".html", true)]
        [InlineData("http://www.google.com/mail/hello.html#anchor", "hello.html", true, ".html", true)]
        [InlineData("http://www.google.com/mail/hello.html?", "hello.html", true, ".html", true)]
        [InlineData("http://www.google.com/mail/hello.html?#", "hello.html", true, ".html", true)]
        [InlineData("http://www.google.com/mail/hello.html?foo", "hello.html", true, ".html", true)]
        [InlineData("http://www.google.com/mail/hello.html?foo=12", "hello.html", true, ".html", true)]
        [InlineData("http://www.google.com/mail/hello.html?foo=12&bar", "hello.html", true, ".html", true)]
        [InlineData("http://www.google.com/mail/hello.html?foo=12&bar=34", "hello.html", true, ".html", true)]
        [InlineData("http://www.google.com/mail/hello.html?foo#anchor", "hello.html", true, ".html", true)]
        [InlineData("http://www.google.com/mail/hello.html?foo=12#anchor", "hello.html", true, ".html", true)]
        [InlineData("http://www.google.com/mail/hello.html?foo=12&bar=34#anchor", "hello.html", true, ".html", true)]
        [InlineData("http://www.google.com/hello", "", false, "", false)]
        [InlineData("http://www.google.com/mail#", "", false, "", false)]
        [InlineData("http://www.google.com/mail#anchor", "", false, "", false)]
        [InlineData("http://www.google.com/mail?", "", false, "", false)]
        [InlineData("http://www.google.com/mail?#", "", false, "", false)]
        [InlineData("http://www.google.com/mail?foo", "", false, "", false)]
        [InlineData("http://www.google.com/mail?foo=12", "", false, "", false)]
        [InlineData("http://www.google.com/mail?foo=12&bar", "", false, "", false)]
        [InlineData("http://www.google.com/mail?foo=12&bar=34", "", false, "", false)]
        [InlineData("http://www.google.com/mail?foo#anchor", "", false, "", false)]
        [InlineData("http://www.google.com/mail?foo=12#anchor", "", false, "", false)]
        [InlineData("http://www.google.com/mail?foo=12&bar=34#anchor", "", false, "", false)]
        [InlineData("http://www.google.com/mail/#", "", false, "", false)]
        [InlineData("http://www.google.com/mail/#anchor", "", false, "", false)]
        [InlineData("http://www.google.com/mail/?", "", false, "", false)]
        [InlineData("http://www.google.com/mail/?#", "", false, "", false)]
        [InlineData("http://www.google.com/mail/?foo", "", false, "", false)]
        [InlineData("http://www.google.com/mail/?foo=12", "", false, "", false)]
        [InlineData("http://www.google.com/mail/?foo=12&bar", "", false, "", false)]
        [InlineData("http://www.google.com/mail/?foo=12&bar=34", "", false, "", false)]
        [InlineData("http://www.google.com/mail/?foo#anchor", "", false, "", false)]
        [InlineData("http://www.google.com/mail/?foo=12#anchor", "", false, "", false)]
        [InlineData("http://www.google.com/mail/?foo=12&bar=34#anchor", "", false, "", false)]
        public void FileName_HasFileName_And_Extension_HasExtension(string urlText, string expectedFileName, bool expectedHasFileName, string expectedExtension, bool expectedHasExtension)
        {
            // Fixture setup
            // Exercise system
            var url = AbsoluteUrl.Create(urlText);
            // Verify outcome
            Assert.Equal(expectedFileName, url.FileName);
            Assert.Equal(expectedHasFileName, url.HasFileName);
            Assert.Equal(expectedExtension, url.Extension);
            Assert.Equal(expectedHasExtension, url.HasExtension);
            // Teardown  
        }

        [Theory]
        [MemberData(nameof(TestData))]
        public void Scheme(UrlTestData urlData)
        {
            // Fixture setup
            // Exercise system
            var url = AbsoluteUrl.Create(urlData.Url);
            // Verify outcome
            Assert.Equal(urlData.Scheme, url.Scheme);
            // Teardown  
        }

        [Theory]
        [MemberData(nameof(TestData))]
        public void UserInfo(UrlTestData urlData)
        {
            // Fixture setup
            // Exercise system
            var url = AbsoluteUrl.Create(urlData.Url);
            // Verify outcome
            Assert.Equal(urlData.UserInfo, url.UserInfo);
            // Teardown
        }

        [Theory]
        [MemberData(nameof(TestData))]
        public void Host(UrlTestData urlData)
        {
            // Fixture setup
            // Exercise system
            var url = AbsoluteUrl.Create(urlData.Url);
            // Verify outcome
            Assert.Equal(urlData.Host, url.Host);
            // Teardown  
        }

        [Theory]
        [MemberData(nameof(TestData))]
        public void Port(UrlTestData urlData)
        {
            // Fixture setup
            // Exercise system
            var url = AbsoluteUrl.Create(urlData.Url);
            // Verify outcome
            Assert.Equal(urlData.Port, url.Port);
            // Teardown  
        }

        [Theory]
        [MemberData(nameof(TestData))]
        public void Path(UrlTestData urlData)
        {
            // Fixture setup
            // Exercise system
            var url = AbsoluteUrl.Create(urlData.Url);
            // Verify outcome
            Assert.Equal(urlData.Path, url.Path);
            // Teardown
        }

        [Theory]
        [MemberData(nameof(TestData))]
        public void Query(UrlTestData urlData)
        {
            // Fixture setup
            // Exercise system
            var url = AbsoluteUrl.Create(urlData.Url);
            // Verify outcome
            Assert.Equal(urlData.Query, url.Query);
            // Teardown  
        }

        [Theory]
        [MemberData(nameof(TestData))]
        public void Fragment(UrlTestData urlData)
        {
            // Fixture setup
            // Exercise system
            var url = AbsoluteUrl.Create(urlData.Url);
            // Verify outcome
            Assert.Equal(urlData.Fragment, url.Fragment);
            // Teardown
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
        [InlineData("http://192.168.1.1/", "")]
        public void TopLevelDomain(string urlText, string expectedTopLevelDomain)
        {
            // Fixture setup
            // Exercise system
            var url = AbsoluteUrl.Create(urlText);
            // Verify outcome
            Assert.Equal(expectedTopLevelDomain, url.TopLevelDomain);
            // Teardown  
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
        [InlineData("http://192.168.1.1/", "")]
        public void SecondLevelDomain(string urlText, string expectedSecondLevelDomain)
        {
            // Fixture setup
            // Exercise system
            var url = AbsoluteUrl.Create(urlText);
            // Verify outcome
            Assert.Equal(expectedSecondLevelDomain, url.SecondLevelDomain);
            // Teardown  
        }

        [Theory]
        [InlineData("http://www.example.com/", false)]
        [InlineData("http://www.anotherexample.net/", false)]
        [InlineData("http://192.168.1.1/", true)]
        public void IsDomainIPAddress(string urlText, bool expectedIsIPAddress)
        {
            // Fixture setup
            // Exercise system
            var url = AbsoluteUrl.Create(urlText);
            // Verify outcome
            Assert.Equal(expectedIsIPAddress, url.IsDomainAnIPAddress);
            // Teardown
        }

        [Theory]
        [MemberData(nameof(TestData))]
        public void Normalized(UrlTestData urlData)
        {
            // Fixture setup
            // Exercise system
            var url = AbsoluteUrl.Create(urlData.Url);
            // Verify outcome
            Assert.Equal(urlData.Normalized, url.ToString());
            // Teardown
        }

        [Theory]
        [InlineData("http://www.google.com/mail/?foo=12&bar=34#anchor", false)]
        [InlineData("https://www.google.com/mail/?foo=12&bar=34#anchor", true)]
        [InlineData("HTTP://www.google.com/mail/?foo=12&bar=34#anchor", false)]
        [InlineData("HTTPS://www.google.com/mail/?foo=12&bar=34#anchor", true)]
        public void IsHttps_AbsoluteUrl(string urlText, bool expectedResult)
        {
            // Fixture setup
            // Exercise system
            var url = AbsoluteUrl.Create(urlText);
            // Verify outcome
            Assert.Equal(expectedResult, url.IsHttps);
            // Teardown
        }

        [Theory]
        [InlineData("https://www.google.com/mail/?foo", "foo", true)]
        [InlineData("https://www.google.com/mail/?foo=12", "foo", true)]
        [InlineData("https://www.google.com/mail/?foo&bar", "foo", true)]
        [InlineData("https://www.google.com/mail/?foo&bar=13", "foo", true)]
        [InlineData("https://www.google.com/mail/?foo=12&bar=13", "foo", true)]
        [InlineData("https://www.google.com/mail/?foo#anchor", "foo", true)]
        [InlineData("https://www.google.com/mail/?foo=12#anchor", "foo", true)]
        [InlineData("https://www.google.com/mail/?bar", "foo", false)]
        [InlineData("https://www.google.com/mail/?bar=13", "foo", false)]
        [InlineData("https://www.google.com/mail/?bar=13#anchor", "foo", false)]
        [InlineData("https://www.google.com/mail/hello.html?this+is+a+test", "this is a test", true)]
        [InlineData("https://www.google.com/mail/hello.html?this+is+a+test=how+did+it+go", "this is a test", true)]
        [InlineData("https://www.google.com/mail/hello.html?this+is+a+test#anchor", "this is a test", true)]
        [InlineData("https://www.google.com/mail/hello.html?this+is+a+test=how+did+it+go#anchor", "this is a test", true)]
        public void HasParameter(string urlText, string parameter, bool expectedResult)
        {
            // Fixture setup
            // Exercise system
            var url = AbsoluteUrl.Create(urlText);
            // Verify outcome
            Assert.Equal(expectedResult, url.HasParameter(parameter));
            // Teardown  
        }

        [Theory]
        [InlineData("https://www.google.com/mail/?foo", "foo", "", "/mail/", "/mail/")]
        [InlineData("https://www.google.com/mail/?foo=12", "foo", "", "/mail/", "/mail/")]
        [InlineData("https://www.google.com/mail/?foo&bar", "foo", "bar", "/mail/?bar", "/mail/?bar")]
        [InlineData("https://www.google.com/mail/?foo&bar=13", "foo", "bar=13", "/mail/?bar=13", "/mail/?bar=13")]
        [InlineData("https://www.google.com/mail/?foo=12&bar=13", "foo", "bar=13", "/mail/?bar=13", "/mail/?bar=13")]
        [InlineData("https://www.google.com/mail/?foo#anchor", "foo", "", "/mail/", "/mail/#anchor")]
        [InlineData("https://www.google.com/mail/?foo=12#anchor", "foo", "", "/mail/", "/mail/#anchor")]
        public void RemoveParameter(string urlText, string parameter, string expectedQuery, string expectedPathAndQuery, string expectedPathAndQueryAndFragment)
        {
            // Fixture setup
            // Exercise system
            var url = AbsoluteUrl.Create(urlText).RemoveParameter(parameter);
            // Verify outcome
            Assert.Equal(expectedQuery, url.Query);
            Assert.Equal(expectedPathAndQuery, url.PathAndQuery);
            Assert.Equal(expectedPathAndQueryAndFragment, url.PathAndQueryAndFragment);
            // Teardown
        }

        public class UrlTestData
        {
            public string Url { get; set; }
            public string Normalized { get; set; }
            public string Scheme { get; set; }
            public string UserInfo { get; set; }
            public string Host { get; set; }
            public int Port { get; set; }
            public string Path { get; set; }
            public string Query { get; set; }
            public string Fragment { get; set; }

            public UrlTestData(string url, string urlToString, string scheme, string userInfo, string host, int port, string path, string query, string fragment)
            {
                Url = url;
                Normalized = urlToString;
                Scheme = scheme;
                UserInfo = userInfo;
                Host = host;
                Port = port;
                Path = path;
                Query = query;
                Fragment = fragment;
            }
        }

        public static IEnumerable<object[]> TestData
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
