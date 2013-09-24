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
            Assert.AreEqual(expectedUserName, url.UserName);
            Assert.AreEqual(expectedPassword, url.Password);
            Assert.AreEqual(expectedHost, url.Host);
            Assert.AreEqual(expectedPort, url.Port);
            Assert.AreEqual(expectedPath, url.Path);
            Assert.AreEqual(expectedQuery, url.Query);
            Assert.AreEqual(expectedFragment, url.Fragment);

            Assert.AreEqual(urlText, url.ToUri.ToString());
        }

        [TestCase("/mail/?foo=12&bar=34#anchor", "http", "root", "mypass", "google.com", 80, "/mail/", "?foo=12&bar=34", "#anchor")]
        public void UriBuilderMembers_RelativeUrl(String urlText, String expectedScheme, String expectedUserName, String expectedPassword, String expectedHost, int expectedPort, String expectedPath, String expectedQuery, String expectedFragment)
        {
            var url = new Url(urlText);

            Assert.Throws<NotSupportedException>(() => { var x = url.Scheme; });
            Assert.Throws<NotSupportedException>(() => { var x = url.UserName; });
            Assert.Throws<NotSupportedException>(() => { var x = url.Password; });
            Assert.Throws<NotSupportedException>(() => { var x = url.Host; });
            Assert.Throws<NotSupportedException>(() => { var x = url.Port; });
            Assert.AreEqual(expectedPath, url.Path);
            Assert.AreEqual(expectedQuery, url.Query);
            Assert.AreEqual(expectedFragment, url.Fragment);

            Assert.AreEqual(urlText, url.ToUri.ToString());
        }
    }
}
