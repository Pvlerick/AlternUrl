using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlternUrl.Test
{
    [TestFixture]
    public class UrlTest
    {
        [TestCase("http://www.google.com/", UrlKind.Absolute)]
        [TestCase("http://www.google.com", UrlKind.Absolute)]
        [TestCase("https://www.google.com/", UrlKind.Absolute)]
        [TestCase("https://www.google.com", UrlKind.Absolute)]
        [TestCase("/mail/?foo=12&bar=34#anchor", UrlKind.Relative)]
        [TestCase("mail/?foo=12&bar=34#anchor", UrlKind.Relative)]
        [TestCase("http://www.google.com/mail/?foo=12&bar=34#anchor", UrlKind.Absolute)]
        [TestCase("https://www.google.com/mail/?foo=12&bar=34#anchor", UrlKind.Absolute)]
        [TestCase("http/foo/", UrlKind.Relative)]
        [TestCase("/http/foo/", UrlKind.Relative)]
        [TestCase("/http/foo/index.html", UrlKind.Relative)]
        [TestCase("/http/foo/index.html?foo=12&bar=34#anchor", UrlKind.Relative)]
        [TestCase("/http/foo/?foo=12&bar=34#anchor", UrlKind.Relative)]
        [TestCase("/http/?foo=12&bar=34#anchor", UrlKind.Relative)]
        public void Kind(String urlText, UrlKind expectedResult)
        {
            var url = new Url(urlText);

            Assert.AreEqual(expectedResult, url.Kind);
        }

        [TestCase("http://www.google.com/", "", false)]
        [TestCase("http://www.google.com", "", false)]
        [TestCase("http://www.google.com/mail", "", false)]
        [TestCase("http://www.google.com/mail/", "", false)]
        [TestCase("http://www.google.com/hello.html", ".html", true)]
        [TestCase("http://www.google.com/mail/hello.htm", ".htm", true)]
        [TestCase("http://www.google.com/mail/hello.html", ".html", true)]
        [TestCase("http://www.google.com/mail/hello.html#", ".html", true)]
        [TestCase("http://www.google.com/mail/hello.html#anchor", ".html", true)]
        [TestCase("http://www.google.com/mail/hello.html?", ".html", true)]
        [TestCase("http://www.google.com/mail/hello.html?#", ".html", true)]
        [TestCase("http://www.google.com/mail/hello.html?foo", ".html", true)]
        [TestCase("http://www.google.com/mail/hello.html?foo=12", ".html", true)]
        [TestCase("http://www.google.com/mail/hello.html?foo=12&bar", ".html", true)]
        [TestCase("http://www.google.com/mail/hello.html?foo=12&bar=34", ".html", true)]
        [TestCase("http://www.google.com/mail/hello.html?foo", ".html", true)]
        [TestCase("http://www.google.com/mail/hello.html?foo#anchor", ".html", true)]
        [TestCase("http://www.google.com/mail/hello.html?foo=12", ".html", true)]
        [TestCase("http://www.google.com/mail/hello.html?foo=12#anchor", ".html", true)]
        [TestCase("http://www.google.com/mail/hello.html?foo=12&bar", ".html", true)]
        [TestCase("http://www.google.com/mail/hello.html?foo=12&bar=34", ".html", true)]
        [TestCase("http://www.google.com/mail/hello.html?foo=12&bar=34#anchor", ".html", true)]
        [TestCase("http://www.google.com/hello", "", false)]
        [TestCase("http://www.google.com/mail#", "", false)]
        [TestCase("http://www.google.com/mail#anchor", "", false)]
        [TestCase("http://www.google.com/mail?", "", false)]
        [TestCase("http://www.google.com/mail?#", "", false)]
        [TestCase("http://www.google.com/mail?foo", "", false)]
        [TestCase("http://www.google.com/mail?foo=12", "", false)]
        [TestCase("http://www.google.com/mail?foo=12&bar", "", false)]
        [TestCase("http://www.google.com/mail?foo=12&bar=34", "", false)]
        [TestCase("http://www.google.com/mail?foo", "", false)]
        [TestCase("http://www.google.com/mail?foo#anchor", "", false)]
        [TestCase("http://www.google.com/mail?foo=12", "", false)]
        [TestCase("http://www.google.com/mail?foo=12#anchor", "", false)]
        [TestCase("http://www.google.com/mail?foo=12&bar", "", false)]
        [TestCase("http://www.google.com/mail?foo=12&bar=34", "", false)]
        [TestCase("http://www.google.com/mail?foo=12&bar=34#anchor", "", false)]
        [TestCase("http://www.google.com/mail/", "", false)]
        [TestCase("http://www.google.com/mail/#", "", false)]
        [TestCase("http://www.google.com/mail/#anchor", "", false)]
        [TestCase("http://www.google.com/mail/?", "", false)]
        [TestCase("http://www.google.com/mail/?#", "", false)]
        [TestCase("http://www.google.com/mail/?foo", "", false)]
        [TestCase("http://www.google.com/mail/?foo=12", "", false)]
        [TestCase("http://www.google.com/mail/?foo=12&bar", "", false)]
        [TestCase("http://www.google.com/mail/?foo=12&bar=34", "", false)]
        [TestCase("http://www.google.com/mail/?foo", "", false)]
        [TestCase("http://www.google.com/mail/?foo#anchor", "", false)]
        [TestCase("http://www.google.com/mail/?foo=12", "", false)]
        [TestCase("http://www.google.com/mail/?foo=12#anchor", "", false)]
        [TestCase("http://www.google.com/mail/?foo=12&bar", "", false)]
        [TestCase("http://www.google.com/mail/?foo=12&bar=34", "", false)]
        [TestCase("http://www.google.com/mail/?foo=12&bar=34#anchor", "", false)]
        [TestCase("/", "", false)]
        [TestCase("/mail", "", false)]
        [TestCase("/mail/", "", false)]
        [TestCase("/hello.html", ".html", true)]
        [TestCase("/mail/hello.htm", ".htm", true)]
        [TestCase("/mail/hello.html", ".html", true)]
        [TestCase("/mail/hello.html#", ".html", true)]
        [TestCase("/mail/hello.html#anchor", ".html", true)]
        [TestCase("/mail/hello.html?", ".html", true)]
        [TestCase("/mail/hello.html?#", ".html", true)]
        [TestCase("/mail/hello.html?foo", ".html", true)]
        [TestCase("/mail/hello.html?foo=12", ".html", true)]
        [TestCase("/mail/hello.html?foo=12&bar", ".html", true)]
        [TestCase("/mail/hello.html?foo=12&bar=34", ".html", true)]
        [TestCase("/mail/hello.html?foo", ".html", true)]
        [TestCase("/mail/hello.html?foo#anchor", ".html", true)]
        [TestCase("/mail/hello.html?foo=12", ".html", true)]
        [TestCase("/mail/hello.html?foo=12#anchor", ".html", true)]
        [TestCase("/mail/hello.html?foo=12&bar", ".html", true)]
        [TestCase("/mail/hello.html?foo=12&bar=34", ".html", true)]
        [TestCase("/mail/hello.html?foo=12&bar=34#anchor", ".html", true)]
        [TestCase("/hello", "", false)]
        [TestCase("/mail#", "", false)]
        [TestCase("/mail#anchor", "", false)]
        [TestCase("/mail?", "", false)]
        [TestCase("/mail?#", "", false)]
        [TestCase("/mail?foo", "", false)]
        [TestCase("/mail?foo=12", "", false)]
        [TestCase("/mail?foo=12&bar", "", false)]
        [TestCase("/mail?foo=12&bar=34", "", false)]
        [TestCase("/mail?foo", "", false)]
        [TestCase("/mail?foo#anchor", "", false)]
        [TestCase("/mail?foo=12", "", false)]
        [TestCase("/mail?foo=12#anchor", "", false)]
        [TestCase("/mail?foo=12&bar", "", false)]
        [TestCase("/mail?foo=12&bar=34", "", false)]
        [TestCase("/mail?foo=12&bar=34#anchor", "", false)]
        [TestCase("/mail/", "", false)]
        [TestCase("/mail/#", "", false)]
        [TestCase("/mail/#anchor", "", false)]
        [TestCase("/mail/?", "", false)]
        [TestCase("/mail/?#", "", false)]
        [TestCase("/mail/?foo", "", false)]
        [TestCase("/mail/?foo=12", "", false)]
        [TestCase("/mail/?foo=12&bar", "", false)]
        [TestCase("/mail/?foo=12&bar=34", "", false)]
        [TestCase("/mail/?foo", "", false)]
        [TestCase("/mail/?foo#anchor", "", false)]
        [TestCase("/mail/?foo=12", "", false)]
        [TestCase("/mail/?foo=12#anchor", "", false)]
        [TestCase("/mail/?foo=12&bar", "", false)]
        [TestCase("/mail/?foo=12&bar=34", "", false)]
        [TestCase("/mail/?foo=12&bar=34#anchor", "", false)]
        public void Extension_And_HasExtension(String urlText, String expectedResult1, bool expectedResult2)
        {
            var url = new Url(urlText);

            Assert.AreEqual(expectedResult1, url.Extension);
            Assert.AreEqual(expectedResult2, url.HasExtension);
        }

        [TestCase("http://root:mypass@www.google.com/mail/?foo=12&bar=34#anchor", "http", "root", "mypass", "www.google.com", 80, "/mail/", "?foo=12&bar=34", "#anchor")]
        [TestCase("http://root:mypass@www.google.com:90/mail/?foo=12&bar=34#anchor", "http", "root", "mypass", "www.google.com", 90, "/mail/", "?foo=12&bar=34", "#anchor")]
        [TestCase("https://root:mypass@www.google.com/mail/?foo=12&bar=34#anchor", "https", "root", "mypass", "www.google.com", 443, "/mail/", "?foo=12&bar=34", "#anchor")]
        [TestCase("https://root:mypass@www.google.com:444/mail/?foo=12&bar=34#anchor", "https", "root", "mypass", "www.google.com", 444, "/mail/", "?foo=12&bar=34", "#anchor")]
        public void UriBuilderMembers_AbsoluteUrl(String urlText, String expectedScheme, String expectedUserName, String expectedPassword, String expectedHost, int expectedPort, String expectedPath, String expectedQuery, String expectedFragment)
        {
            var url = new Url(urlText);

            Assert.AreEqual(expectedScheme, url.Scheme);
            //Assert.AreEqual(expectedUserName, url.UserName);
            //Assert.AreEqual(expectedPassword, url.Password);
            //Assert.AreEqual(expectedHost, url.Host);
            //Assert.AreEqual(expectedPort, url.Port);
            //Assert.AreEqual(expectedPath, url.Path);
            //Assert.AreEqual(expectedQuery, url.Query);
            //Assert.AreEqual(expectedFragment, url.Fragment);

            Assert.AreEqual(urlText, url.ToUri().ToString());
        }

        //[TestCase("/mail/?foo=12&bar=34#anchor",  "/mail/", "?foo=12&bar=34", "#anchor")]
        //[TestCase("/mail/index.html?foo=12&bar=34#anchor", "/mail/index.html", "?foo=12&bar=34", "#anchor")]
        //public void UriBuilderMembers_RelativeUrl(String urlText, String expectedPath, String expectedQuery, String expectedFragment)
        //{
        //    var url = new Url(urlText);

        //    Assert.Throws<NotSupportedException>(() => { var x = url.Scheme; });
        //    Assert.Throws<NotSupportedException>(() => { var x = url.UserName; });
        //    Assert.Throws<NotSupportedException>(() => { var x = url.Password; });
        //    Assert.Throws<NotSupportedException>(() => { var x = url.Host; });
        //    Assert.Throws<NotSupportedException>(() => { var x = url.Port; });
        //    Assert.AreEqual(expectedPath, url.Path);
        //    Assert.AreEqual(expectedQuery, url.Query);
        //    Assert.AreEqual(expectedPath + expectedQuery, url.PathAndQuery);
        //    Assert.AreEqual(expectedFragment, url.Fragment);

        //    Assert.AreEqual(urlText, url.ToUri().ToString());
        //}

        //[TestCase("http://root:mypass@www.google.com/mail/?foo=12&bar=34#anchor", UriKind.Absolute)]
        //[TestCase("/mail/?foo=12&bar=34#anchor", UriKind.Relative)]
        //[TestCase("mail/?foo=12&bar=34#anchor", UriKind.Relative)]
        //public void ToUri(String urlText, UrlKind expectedKind)
        //{
        //    var uri = new Url(urlText).ToUri();

        //    Assert.AreEqual(urlText, uri.ToString());
        //}

        //[TestCase("http://www.google.com/mail/?foo=12&bar=34#anchor", false)]
        //[TestCase("https://www.google.com/mail/?foo=12&bar=34#anchor", true)]
        //public void IsHttps_AbsoluteUrl(String urlText, bool expectedResult)
        //{
        //    var url = new Url(urlText);

        //    Assert.AreEqual(expectedResult, url.IsHttps);
        //}

        //[TestCase("/mail/?foo=12&bar=34#anchor", false)]
        //[TestCase("mail/?foo=12&bar=34#anchor", false)]
        //public void IsHttps_RelativeUrl(String urlText, bool expectedResult)
        //{
        //    var url = new Url(urlText);

        //    Assert.Throws<NotSupportedException>(() => { var x = url.IsHttps; });
        //}

        //[TestCase("/mail/?foo", "foo", true)]
        //[TestCase("/mail/?foo=12", "foo", true)]
        //[TestCase("/mail/?foo&bar", "foo", true)]
        //[TestCase("/mail/?foo&bar=13", "foo", true)]
        //[TestCase("/mail/?foo=12&bar=13", "foo", true)]
        //[TestCase("/mail/?foo#anchor", "foo", true)]
        //[TestCase("/mail/?foo=12#anchor", "foo", true)]
        //[TestCase("https://www.google.com/mail/?foo", "foo", true)]
        //[TestCase("https://www.google.com/mail/?foo=12", "foo", true)]
        //[TestCase("https://www.google.com/mail/?foo&bar", "foo", true)]
        //[TestCase("https://www.google.com/mail/?foo&bar=13", "foo", true)]
        //[TestCase("https://www.google.com/mail/?foo=12&bar=13", "foo", true)]
        //[TestCase("https://www.google.com/mail/?foo#anchor", "foo", true)]
        //[TestCase("https://www.google.com/mail/?foo=12#anchor", "foo", true)]
        //[TestCase("/mail/?bar", "foo", false)]
        //[TestCase("/mail/?bar=13", "foo", false)]
        //[TestCase("/mail/?bar=13#anchor", "foo", false)]
        //[TestCase("https://www.google.com/mail/?bar", "foo", false)]
        //[TestCase("https://www.google.com/mail/?bar=13", "foo", false)]
        //[TestCase("https://www.google.com/mail/?bar=13#anchor", "foo", false)]
        //public void HasParameter(String urlText, String parameter, bool expectedResult)
        //{
        //    var url = new Url(urlText);
            
        //    Assert.AreEqual(expectedResult, url.HasParameter(parameter));
        //}

        //[TestCase("/mail/?foo", "bar", "", "?foo&bar", "/mail/?foo&bar", "/mail/?foo&bar")]
        //[TestCase("/mail/?foo", "bar", "12", "?foo&bar=12", "/mail/?foo&bar=12", "/mail/?foo&bar=12")]
        //[TestCase("/mail/?foo=12", "bar", "13", "?foo=12&bar=13", "/mail/?foo=12&bar=13", "/mail/?foo=12&bar=13")]
        //[TestCase("/mail/?foo&bar", "foobar", "42", "?foo&bar&foobar=42", "/mail/?foo&bar&foobar=42", "/mail/?foo&bar&foobar=42")]
        //[TestCase("/mail/?foo&bar=13", "foobar", "42", "?foo&bar=13&foobar=42", "/mail/?foo&bar=13&foobar=42", "/mail/?foo&bar=13&foobar=42")]
        //[TestCase("/mail/?foo=12&bar=13", "foobar", "42", "?foo=12&bar=13&foobar=42", "/mail/?foo=12&bar=13&foobar=42", "/mail/?foo=12&bar=13&foobar=42")]
        //[TestCase("/mail/?foo#anchor", "bar", "", "?foo&bar", "/mail/?foo&bar", "/mail/?foo&bar#anchor")]
        //[TestCase("/mail/?foo#anchor", "bar", "12", "?foo&bar=12", "/mail/?foo&bar=12", "/mail/?foo&bar=12#anchor")]
        //[TestCase("/mail/?foo=12#anchor", "bar", "13", "?foo=12&bar=13", "/mail/?foo=12&bar=13", "/mail/?foo=12&bar=13#anchor")]
        //[TestCase("/mail/?foo=12&bar=13#anchor", "foobar", "42", "?foo=12&bar=13&foobar=42", "/mail/?foo=12&bar=13&foobar=42", "/mail/?foo=12&bar=13&foobar=42#anchor")]
        //public void AddParameter(String urlText, String parameter, String argument, String expectedQuery, String expectedPathAndQuery, String expectedPathAndQueryAndFragment)
        //{
        //    var url = new Url(urlText).AddParameter(parameter, argument);

        //    Assert.AreEqual(expectedQuery, url.Query);
        //    Assert.AreEqual(expectedPathAndQuery, url.PathAndQuery);
        //    Assert.AreEqual(expectedPathAndQueryAndFragment, url.PathAndQueryAndFragment);
        //}

        //[TestCase("/mail/?foo", "foo", "", "/mail/", "/mail/")]
        //[TestCase("/mail/?foo=12", "foo", "", "/mail/", "/mail/")]
        //[TestCase("/mail/?foo&bar", "foo", "?bar", "/mail/?bar", "/mail/?bar")]
        //[TestCase("/mail/?foo&bar=13", "foo", "?bar=13", "/mail/?bar=13", "/mail/?bar=13")]
        //[TestCase("/mail/?foo=12&bar=13", "foo", "?bar=13", "/mail/?bar=13", "/mail/?bar=13")]
        //[TestCase("/mail/?foo#anchor", "foo", "", "/mail/", "/mail/#anchor")]
        //[TestCase("/mail/?foo=12#anchor", "foo", "", "/mail/", "/mail/#anchor")]
        //[TestCase("https://www.google.com/mail/?foo", "foo", "", "/mail/", "/mail/")]
        //[TestCase("https://www.google.com/mail/?foo=12", "foo", "", "/mail/", "/mail/")]
        //[TestCase("https://www.google.com/mail/?foo&bar", "foo", "?bar", "/mail/?bar", "/mail/?bar")]
        //[TestCase("https://www.google.com/mail/?foo&bar=13", "foo", "?bar=13", "/mail/?bar=13", "/mail/?bar=13")]
        //[TestCase("https://www.google.com/mail/?foo=12&bar=13", "foo", "?bar=13", "/mail/?bar=13", "/mail/?bar=13")]
        //[TestCase("https://www.google.com/mail/?foo#anchor", "foo", "", "/mail/", "/mail/#anchor")]
        //[TestCase("https://www.google.com/mail/?foo=12#anchor", "foo", "", "/mail/", "/mail/#anchor")]
        //[TestCase("/mail/?foo=42&bar=18&fb=45&babar=78", "fb", "?foo=42&bar=18&babar=78", "/mail/?foo=42&bar=18&babar=78", "/mail/?foo=42&bar=18&babar=78")] //To test parameters order
        //[TestCase("/mail/?foo=42&bar=18&fb=45&babar=78", "bar", "?foo=42&fb=45&babar=78", "/mail/?foo=42&fb=45&babar=78", "/mail/?foo=42&fb=45&babar=78")] //To test parameters order
        //public void RemoveParameter(String urlText, String parameter, String expectedQuery, String expectedPathAndQuery, String expectedPathAndQueryAndFragment)
        //{
        //    var url = new Url(urlText).RemoveParameter(parameter);

        //    Assert.AreEqual(expectedQuery, url.Query);
        //    Assert.AreEqual(expectedPathAndQuery, url.PathAndQuery);
        //    Assert.AreEqual(expectedPathAndQueryAndFragment, url.PathAndQueryAndFragment);
        //}
    }
}
