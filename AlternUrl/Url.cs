using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AlternUrl
{
    public class Url
    {
        String url;
        UriBuilder uriBuilder;

        public Url(String url)
        {
            this.url = url;
            this.Kind = Url.GetKind(url);

            //Use UriBuilder class for the heavy lifting (parsing, etc...) using a fake domain if the given url is relative
            var urlForBuilder = this.Kind == UrlKind.Absolute ? url : String.Format("http://a.com/{0}", url.TrimStart('/'));
            this.uriBuilder = new UriBuilder(urlForBuilder);
        }

        public Url(Uri uri)
        {
            //TODO
        }

        public UrlKind Kind { get; private set; }
        public String Extension { get { return System.IO.Path.GetExtension(this.Path); } }
        public bool HasExtension { get { return this.Extension != String.Empty; } }

        #region Mostly delegated to UriBuilder...
        public String Path
        {
            get { return this.uriBuilder.Path; }
            set { this.uriBuilder.Path = value; }
        }

        public String Scheme
        {
            get
            {
                if (this.Kind == UrlKind.Absolute) return this.uriBuilder.Scheme;
                else throw new NotSupportedException("Not supported for a relative URL");
            }
            set
            {
                if (this.Kind == UrlKind.Absolute) this.uriBuilder.Scheme = value;
                else throw new NotSupportedException("Not supported for a relative URL");
            }
        }

        #endregion

        public Uri ToUri()
        {
            if (this.Kind == UrlKind.Absolute) return this.uriBuilder.Uri;
            else return new Uri(url, UriKind.Relative);
        }

        public static UrlKind GetKind(String url)
        {
            return Regex.IsMatch(url, "^https*://") ? UrlKind.Absolute : UrlKind.Relative;
        }
    }
}
