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
        [Test]
        public void Constructor()
        {
            var url = new Url("http://username:password@example.com:8042/over/there/index.dtb?type=animal&name=narwhal#nose");

            Assert.AreEqual("http", url.Scheme);
            Assert.AreEqual("username", url.UserName);
            Assert.AreEqual("password", url.Password);
            Assert.AreEqual("example.com", url.Host);
            Assert.AreEqual(8042, url.Port);
            Assert.AreEqual("/over/there/index.dtb", url.Path);
            Assert.AreEqual("type=animal&name=narwhal", url.Query);
            Assert.AreEqual("nose", url.Fragment);
            Assert.AreEqual("index", url.FileName);
            Assert.AreEqual(".dtb", url.Extension);
        }

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

        [TestCase("http://www.google.com/", "", false, "", false)]
        [TestCase("http://www.google.com", "", false, "", false)]
        [TestCase("http://www.google.com/mail", "", false, "", false)]
        [TestCase("http://www.google.com/mail/", "", false, "", false)]
        [TestCase("http://www.google.com/hello.html", "hello", true, ".html", true)]
        [TestCase("http://www.google.com/mail/hello.htm", "hello", true, ".htm", true)]
        [TestCase("http://www.google.com/mail/hello.html", "hello", true, ".html", true)]
        [TestCase("http://www.google.com/mail/hello.html#", "hello", true, ".html", true)]
        [TestCase("http://www.google.com/mail/hello.html#anchor", "hello", true, ".html", true)]
        [TestCase("http://www.google.com/mail/hello.html?", "hello", true, ".html", true)]
        [TestCase("http://www.google.com/mail/hello.html?#", "hello", true, ".html", true)]
        [TestCase("http://www.google.com/mail/hello.html?foo", "hello", true, ".html", true)]
        [TestCase("http://www.google.com/mail/hello.html?foo=12", "hello", true, ".html", true)]
        [TestCase("http://www.google.com/mail/hello.html?foo=12&bar", "hello", true, ".html", true)]
        [TestCase("http://www.google.com/mail/hello.html?foo=12&bar=34", "hello", true, ".html", true)]
        [TestCase("http://www.google.com/mail/hello.html?foo", "hello", true, ".html", true)]
        [TestCase("http://www.google.com/mail/hello.html?foo#anchor", "hello", true, ".html", true)]
        [TestCase("http://www.google.com/mail/hello.html?foo=12", "hello", true, ".html", true)]
        [TestCase("http://www.google.com/mail/hello.html?foo=12#anchor", "hello", true, ".html", true)]
        [TestCase("http://www.google.com/mail/hello.html?foo=12&bar", "hello", true, ".html", true)]
        [TestCase("http://www.google.com/mail/hello.html?foo=12&bar=34", "hello", true, ".html", true)]
        [TestCase("http://www.google.com/mail/hello.html?foo=12&bar=34#anchor", "hello", true, ".html", true)]
        [TestCase("http://www.google.com/hello", "", false, "", false)]
        [TestCase("http://www.google.com/mail#", "", false, "", false)]
        [TestCase("http://www.google.com/mail#anchor", "", false, "", false)]
        [TestCase("http://www.google.com/mail?", "", false, "", false)]
        [TestCase("http://www.google.com/mail?#", "", false, "", false)]
        [TestCase("http://www.google.com/mail?foo", "", false, "", false)]
        [TestCase("http://www.google.com/mail?foo=12", "", false, "", false)]
        [TestCase("http://www.google.com/mail?foo=12&bar", "", false, "", false)]
        [TestCase("http://www.google.com/mail?foo=12&bar=34", "", false, "", false)]
        [TestCase("http://www.google.com/mail?foo", "", false, "", false)]
        [TestCase("http://www.google.com/mail?foo#anchor", "", false, "", false)]
        [TestCase("http://www.google.com/mail?foo=12", "", false, "", false)]
        [TestCase("http://www.google.com/mail?foo=12#anchor", "", false, "", false)]
        [TestCase("http://www.google.com/mail?foo=12&bar", "", false, "", false)]
        [TestCase("http://www.google.com/mail?foo=12&bar=34", "", false, "", false)]
        [TestCase("http://www.google.com/mail?foo=12&bar=34#anchor", "", false, "", false)]
        [TestCase("http://www.google.com/mail/", "", false, "", false)]
        [TestCase("http://www.google.com/mail/#", "", false, "", false)]
        [TestCase("http://www.google.com/mail/#anchor", "", false, "", false)]
        [TestCase("http://www.google.com/mail/?", "", false, "", false)]
        [TestCase("http://www.google.com/mail/?#", "", false, "", false)]
        [TestCase("http://www.google.com/mail/?foo", "", false, "", false)]
        [TestCase("http://www.google.com/mail/?foo=12", "", false, "", false)]
        [TestCase("http://www.google.com/mail/?foo=12&bar", "", false, "", false)]
        [TestCase("http://www.google.com/mail/?foo=12&bar=34", "", false, "", false)]
        [TestCase("http://www.google.com/mail/?foo", "", false, "", false)]
        [TestCase("http://www.google.com/mail/?foo#anchor", "", false, "", false)]
        [TestCase("http://www.google.com/mail/?foo=12", "", false, "", false)]
        [TestCase("http://www.google.com/mail/?foo=12#anchor", "", false, "", false)]
        [TestCase("http://www.google.com/mail/?foo=12&bar", "", false, "", false)]
        [TestCase("http://www.google.com/mail/?foo=12&bar=34", "", false, "", false)]
        [TestCase("http://www.google.com/mail/?foo=12&bar=34#anchor", "", false, "", false)]
        [TestCase("/", "", false, "", false)]
        [TestCase("/mail", "", false, "", false)]
        [TestCase("/mail/", "", false, "", false)]
        [TestCase("/hello.html", "hello", true, ".html", true)]
        [TestCase("/mail/hello.htm", "hello", true, ".htm", true)]
        [TestCase("/mail/hello.html", "hello", true, ".html", true)]
        [TestCase("/mail/hello.html#", "hello", true, ".html", true)]
        [TestCase("/mail/hello.html#anchor", "hello", true, ".html", true)]
        [TestCase("/mail/hello.html?", "hello", true, ".html", true)]
        [TestCase("/mail/hello.html?#", "hello", true, ".html", true)]
        [TestCase("/mail/hello.html?foo", "hello", true, ".html", true)]
        [TestCase("/mail/hello.html?foo=12", "hello", true, ".html", true)]
        [TestCase("/mail/hello.html?foo=12&bar", "hello", true, ".html", true)]
        [TestCase("/mail/hello.html?foo=12&bar=34", "hello", true, ".html", true)]
        [TestCase("/mail/hello.html?foo", "hello", true, ".html", true)]
        [TestCase("/mail/hello.html?foo#anchor", "hello", true, ".html", true)]
        [TestCase("/mail/hello.html?foo=12", "hello", true, ".html", true)]
        [TestCase("/mail/hello.html?foo=12#anchor", "hello", true, ".html", true)]
        [TestCase("/mail/hello.html?foo=12&bar", "hello", true, ".html", true)]
        [TestCase("/mail/hello.html?foo=12&bar=34", "hello", true, ".html", true)]
        [TestCase("/mail/hello.html?foo=12&bar=34#anchor", "hello", true, ".html", true)]
        [TestCase("/hello", "", false, "", false)]
        [TestCase("/mail#", "", false, "", false)]
        [TestCase("/mail#anchor", "", false, "", false)]
        [TestCase("/mail?", "", false, "", false)]
        [TestCase("/mail?#", "", false, "", false)]
        [TestCase("/mail?foo", "", false, "", false)]
        [TestCase("/mail?foo=12", "", false, "", false)]
        [TestCase("/mail?foo=12&bar", "", false, "", false)]
        [TestCase("/mail?foo=12&bar=34", "", false, "", false)]
        [TestCase("/mail?foo", "", false, "", false)]
        [TestCase("/mail?foo#anchor", "", false, "", false)]
        [TestCase("/mail?foo=12", "", false, "", false)]
        [TestCase("/mail?foo=12#anchor", "", false, "", false)]
        [TestCase("/mail?foo=12&bar", "", false, "", false)]
        [TestCase("/mail?foo=12&bar=34", "", false, "", false)]
        [TestCase("/mail?foo=12&bar=34#anchor", "", false, "", false)]
        [TestCase("/mail/", "", false, "", false)]
        [TestCase("/mail/#", "", false, "", false)]
        [TestCase("/mail/#anchor", "", false, "", false)]
        [TestCase("/mail/?", "", false, "", false)]
        [TestCase("/mail/?#", "", false, "", false)]
        [TestCase("/mail/?foo", "", false, "", false)]
        [TestCase("/mail/?foo=12", "", false, "", false)]
        [TestCase("/mail/?foo=12&bar", "", false, "", false)]
        [TestCase("/mail/?foo=12&bar=34", "", false, "", false)]
        [TestCase("/mail/?foo", "", false, "", false)]
        [TestCase("/mail/?foo#anchor", "", false, "", false)]
        [TestCase("/mail/?foo=12", "", false, "", false)]
        [TestCase("/mail/?foo=12#anchor", "", false, "", false)]
        [TestCase("/mail/?foo=12&bar", "", false, "", false)]
        [TestCase("/mail/?foo=12&bar=34", "", false, "", false)]
        [TestCase("/mail/?foo=12&bar=34#anchor", "", false, "", false)]
        public void FileName_HasFileName_And_Extension_HasExtension(String urlText, String expectedFileName, bool expectedHasFileName, String expectedExtension, bool expectedHasExtension)
        {
            var url = new Url(urlText);

            Assert.AreEqual(expectedFileName, url.FileName);
            Assert.AreEqual(expectedHasFileName, url.HasFileName);
            Assert.AreEqual(expectedExtension, url.Extension);
            Assert.AreEqual(expectedHasExtension, url.HasExtension);
        }

        [TestCase("http://root:mypass@www.google.com/mail/?foo=12&bar=34#anchor", "http", "root", "mypass", "www.google.com", 80, "/mail/", "foo=12&bar=34", "anchor")]
        [TestCase("http://root:mypass@www.google.com:90/mail/?foo=12&bar=34#anchor", "http", "root", "mypass", "www.google.com", 90, "/mail/", "foo=12&bar=34", "anchor")]
        [TestCase("https://root:mypass@www.google.com/mail/?foo=12&bar=34#anchor", "https", "root", "mypass", "www.google.com", 443, "/mail/", "foo=12&bar=34", "anchor")]
        [TestCase("https://root:mypass@www.google.com:444/mail/?foo=12&bar=34#anchor", "https", "root", "mypass", "www.google.com", 444, "/mail/", "foo=12&bar=34", "anchor")]
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

            Assert.AreEqual(urlText, url.ToString());
        }

        [TestCase("/mail/?foo=12&bar=34#anchor", "/mail/", "foo=12&bar=34", "anchor")]
        [TestCase("/mail/index.html?foo=12&bar=34#anchor", "/mail/index.html", "foo=12&bar=34", "anchor")]
        public void UriBuilderMembers_RelativeUrl(String urlText, String expectedPath, String expectedQuery, String expectedFragment)
        {
            var url = new Url(urlText);

            Assert.Throws<NotSupportedException>(() => { var x = url.Scheme; });
            Assert.Throws<NotSupportedException>(() => { var x = url.UserName; });
            Assert.Throws<NotSupportedException>(() => { var x = url.Password; });
            Assert.Throws<NotSupportedException>(() => { var x = url.Host; });
            Assert.Throws<NotSupportedException>(() => { var x = url.Port; });
            Assert.AreEqual(expectedPath, url.Path);
            Assert.AreEqual(expectedQuery, url.Query);
            Assert.AreEqual(expectedPath + "?" + expectedQuery, url.PathAndQuery);
            Assert.AreEqual(expectedFragment, url.Fragment);

            Assert.AreEqual(urlText, url.ToString());
        }

        //[TestCase("http://root:mypass@www.disney.com/mail/?foo=12&bar=34#anchor", "http://root:mypass@www.disney.com/mail/?foo=12&bar=34#anchor")]
        [TestCase("/mail/?foo=12&bar=34#anchor", "/mail/?foo=12&bar=34#anchor")]
        [TestCase("mail/?foo=12&bar=34#anchor", "/mail/?foo=12&bar=34#anchor")]
        public void ToString_And_ToUri(String urlText, String expectedUrlText)
        {
            var url = new Url(urlText);

            Assert.AreEqual(expectedUrlText, url.ToString());
            Assert.AreEqual(expectedUrlText, url.ToUri().ToString());
        }

        [TestCase("http://www.google.com/mail/?foo=12&bar=34#anchor", false)]
        [TestCase("https://www.google.com/mail/?foo=12&bar=34#anchor", true)]
        [TestCase("HTTP://www.google.com/mail/?foo=12&bar=34#anchor", false)]
        [TestCase("HTTPS://www.google.com/mail/?foo=12&bar=34#anchor", true)]
        public void IsHttps_AbsoluteUrl(String urlText, bool expectedResult)
        {
            var url = new Url(urlText);

            Assert.AreEqual(expectedResult, url.IsHttps);
        }

        [TestCase("/mail/?foo=12&bar=34#anchor", false)]
        [TestCase("mail/?foo=12&bar=34#anchor", false)]
        public void IsHttps_RelativeUrl(String urlText, bool expectedResult)
        {
            var url = new Url(urlText);

            Assert.Throws<NotSupportedException>(() => { var x = url.IsHttps; });
        }

        [TestCase("/mail/?foo", "foo", true)]
        [TestCase("/mail/?foo=12", "foo", true)]
        [TestCase("/mail/?foo&bar", "foo", true)]
        [TestCase("/mail/?foo&bar=13", "foo", true)]
        [TestCase("/mail/?foo=12&bar=13", "foo", true)]
        [TestCase("/mail/?foo#anchor", "foo", true)]
        [TestCase("/mail/?foo=12#anchor", "foo", true)]
        [TestCase("https://www.google.com/mail/?foo", "foo", true)]
        [TestCase("https://www.google.com/mail/?foo=12", "foo", true)]
        [TestCase("https://www.google.com/mail/?foo&bar", "foo", true)]
        [TestCase("https://www.google.com/mail/?foo&bar=13", "foo", true)]
        [TestCase("https://www.google.com/mail/?foo=12&bar=13", "foo", true)]
        [TestCase("https://www.google.com/mail/?foo#anchor", "foo", true)]
        [TestCase("https://www.google.com/mail/?foo=12#anchor", "foo", true)]
        [TestCase("/mail/?bar", "foo", false)]
        [TestCase("/mail/?bar=13", "foo", false)]
        [TestCase("/mail/?bar=13#anchor", "foo", false)]
        [TestCase("https://www.google.com/mail/?bar", "foo", false)]
        [TestCase("https://www.google.com/mail/?bar=13", "foo", false)]
        [TestCase("https://www.google.com/mail/?bar=13#anchor", "foo", false)]
        [TestCase("https://www.google.com/mail/hello.html?this+is+a+test", "this is a test", true)]
        [TestCase("https://www.google.com/mail/hello.html?this+is+a+test=how+did+it+go", "this is a test", true)]
        [TestCase("https://www.google.com/mail/hello.html?this+is+a+test#anchor", "this is a test", true)]
        [TestCase("https://www.google.com/mail/hello.html?this+is+a+test=how+did+it+go#anchor", "this is a test", true)]
        public void HasParameter(String urlText, String parameter, bool expectedResult)
        {
            var url = new Url(urlText);

            Assert.AreEqual(expectedResult, url.HasParameter(parameter));
        }

        [TestCase("/mail/?foo", "bar", "", "foo&bar", "/mail/?foo&bar", "/mail/?foo&bar")]
        [TestCase("/mail/?foo", "bar", "12", "foo&bar=12", "/mail/?foo&bar=12", "/mail/?foo&bar=12")]
        [TestCase("/mail/?foo=12", "bar", "13", "foo=12&bar=13", "/mail/?foo=12&bar=13", "/mail/?foo=12&bar=13")]
        [TestCase("/mail/?foo&bar", "foobar", "42", "foo&bar&foobar=42", "/mail/?foo&bar&foobar=42", "/mail/?foo&bar&foobar=42")]
        [TestCase("/mail/?foo&bar=13", "foobar", "42", "foo&bar=13&foobar=42", "/mail/?foo&bar=13&foobar=42", "/mail/?foo&bar=13&foobar=42")]
        [TestCase("/mail/?foo=12&bar=13", "foobar", "42", "foo=12&bar=13&foobar=42", "/mail/?foo=12&bar=13&foobar=42", "/mail/?foo=12&bar=13&foobar=42")]
        [TestCase("/mail/?foo#anchor", "bar", "", "foo&bar", "/mail/?foo&bar", "/mail/?foo&bar#anchor")]
        [TestCase("/mail/?foo#anchor", "bar", "12", "foo&bar=12", "/mail/?foo&bar=12", "/mail/?foo&bar=12#anchor")]
        [TestCase("/mail/?foo=12#anchor", "bar", "13", "foo=12&bar=13", "/mail/?foo=12&bar=13", "/mail/?foo=12&bar=13#anchor")]
        [TestCase("/mail/?foo=12&bar=13#anchor", "foobar", "42", "foo=12&bar=13&foobar=42", "/mail/?foo=12&bar=13&foobar=42", "/mail/?foo=12&bar=13&foobar=42#anchor")]
        [TestCase("/mail/?foo", "bar bar", "", "foo&bar+bar", "/mail/?foo&bar+bar", "/mail/?foo&bar+bar")]
        [TestCase("/mail/?foo", "bar bar", "42", "foo&bar+bar=42", "/mail/?foo&bar+bar=42", "/mail/?foo&bar+bar=42")]
        [TestCase("/mail/?foo", "bar bar", "4:2", "foo&bar+bar=4%3a2", "/mail/?foo&bar+bar=4%3a2", "/mail/?foo&bar+bar=4%3a2")]
        public void AddParameter(String urlText, String parameter, String argument, String expectedQuery, String expectedPathAndQuery, String expectedPathAndQueryAndFragment)
        {
            var url = new Url(urlText).AddParameter(parameter, argument);

            Assert.AreEqual(expectedQuery, url.Query);
            Assert.AreEqual(expectedPathAndQuery, url.PathAndQuery);
            Assert.AreEqual(expectedPathAndQueryAndFragment, url.PathAndQueryAndFragment);
        }

        [TestCase("/mail/?foo", "foo", "", "/mail/", "/mail/")]
        [TestCase("/mail/?foo=12", "foo", "", "/mail/", "/mail/")]
        [TestCase("/mail/?foo&bar", "foo", "bar", "/mail/?bar", "/mail/?bar")]
        [TestCase("/mail/?foo&bar=13", "foo", "bar=13", "/mail/?bar=13", "/mail/?bar=13")]
        [TestCase("/mail/?foo=12&bar=13", "foo", "bar=13", "/mail/?bar=13", "/mail/?bar=13")]
        [TestCase("/mail/?foo#anchor", "foo", "", "/mail/", "/mail/#anchor")]
        [TestCase("/mail/?foo=12#anchor", "foo", "", "/mail/", "/mail/#anchor")]
        [TestCase("https://www.google.com/mail/?foo", "foo", "", "/mail/", "/mail/")]
        [TestCase("https://www.google.com/mail/?foo=12", "foo", "", "/mail/", "/mail/")]
        [TestCase("https://www.google.com/mail/?foo&bar", "foo", "bar", "/mail/?bar", "/mail/?bar")]
        [TestCase("https://www.google.com/mail/?foo&bar=13", "foo", "bar=13", "/mail/?bar=13", "/mail/?bar=13")]
        [TestCase("https://www.google.com/mail/?foo=12&bar=13", "foo", "bar=13", "/mail/?bar=13", "/mail/?bar=13")]
        [TestCase("https://www.google.com/mail/?foo#anchor", "foo", "", "/mail/", "/mail/#anchor")]
        [TestCase("https://www.google.com/mail/?foo=12#anchor", "foo", "", "/mail/", "/mail/#anchor")]
        [TestCase("/mail/?foo=42&bar=18&fb=45&babar=78", "fb", "foo=42&bar=18&babar=78", "/mail/?foo=42&bar=18&babar=78", "/mail/?foo=42&bar=18&babar=78")] //To test parameters order
        [TestCase("/mail/?foo=42&bar=18&fb=45&babar=78", "bar", "foo=42&fb=45&babar=78", "/mail/?foo=42&fb=45&babar=78", "/mail/?foo=42&fb=45&babar=78")] //To test parameters order
        [TestCase("/mail/?foo foo", "foo foo", "", "/mail/", "/mail/")]
        [TestCase("/mail/?foo foo&bar bar", "foo foo", "bar+bar", "/mail/?bar+bar", "/mail/?bar+bar")]
        [TestCase("/mail/?foo foo&bar bar#anchor", "foo foo", "bar+bar", "/mail/?bar+bar", "/mail/?bar+bar#anchor")]
        [TestCase("/mail/?foo foo=42", "foo foo", "", "/mail/", "/mail/")]
        [TestCase("/mail/?foo foo=42&bar bar", "foo foo", "bar+bar", "/mail/?bar+bar", "/mail/?bar+bar")]
        [TestCase("/mail/?foo foo=4:2&bar bar#anchor", "foo foo", "bar+bar", "/mail/?bar+bar", "/mail/?bar+bar#anchor")]
        public void RemoveParameter(String urlText, String parameter, String expectedQuery, String expectedPathAndQuery, String expectedPathAndQueryAndFragment)
        {
            var url = new Url(urlText).RemoveParameter(parameter);

            Assert.AreEqual(expectedQuery, url.Query);
            Assert.AreEqual(expectedPathAndQuery, url.PathAndQuery);
            Assert.AreEqual(expectedPathAndQueryAndFragment, url.PathAndQueryAndFragment);
        }

        //[TestCase("http://www.google.com", "/mail/index.html", "http://www.google/com/mail/index.html")]
        //public void Concat(String urlText, String secondUrlText, String expectedUrlString)
        //{
        //    var url = new Url(urlText);
        //    var secondUrl = new Url(secondUrlText);

        //    Assert.AreEqual(expectedUrlString, url.Concat(secondUrl).ToString());
        //}
    }
}
