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
            this.Kind = Regex.IsMatch(url, "^https*://", RegexOptions.IgnoreCase) ? UrlKind.Absolute : UrlKind.Relative;

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

        public String UserName
        {
            get
            {
                if (this.Kind == UrlKind.Absolute) return this.uriBuilder.UserName;
                else throw new NotSupportedException("Not supported for a relative URL");
            }
            set
            {
                if (this.Kind == UrlKind.Absolute) this.uriBuilder.UserName = value;
                else throw new NotSupportedException("Not supported for a relative URL");
            }
        }

        public String Password
        {
            get
            {
                if (this.Kind == UrlKind.Absolute) return this.uriBuilder.Password;
                else throw new NotSupportedException("Not supported for a relative URL");
            }
            set
            {
                if (this.Kind == UrlKind.Absolute) this.uriBuilder.Password = value;
                else throw new NotSupportedException("Not supported for a relative URL");
            }
        }

        public String Host
        {
            get
            {
                if (this.Kind == UrlKind.Absolute) return this.uriBuilder.Host;
                else throw new NotSupportedException("Not supported for a relative URL");
            }
            set
            {
                if (this.Kind == UrlKind.Absolute) this.uriBuilder.Host = value;
                else throw new NotSupportedException("Not supported for a relative URL");
            }
        }

        public int Port
        {
            get
            {
                if (this.Kind == UrlKind.Absolute) return this.uriBuilder.Port;
                else throw new NotSupportedException("Not supported for a relative URL");
            }
            set
            {
                if (this.Kind == UrlKind.Absolute) this.uriBuilder.Port = value;
                else throw new NotSupportedException("Not supported for a relative URL");
            }
        }

        public String Path
        {
            get { return this.uriBuilder.Path; }
            set { this.uriBuilder.Path = value; }
        }

        public String Query
        {
            get { return uriBuilder.Query; }
        }

        public bool HasQuery
        {
            get { return String.IsNullOrWhiteSpace(this.Query); }
        }

        public String Fragment
        {
            get { return this.uriBuilder.Fragment; }
            set { this.uriBuilder.Fragment = value; }
        }

        public bool HasFragment
        {
            get { return String.IsNullOrWhiteSpace(this.Fragment); }
        }

        public String PathAndQuery
        {
            get { return this.Path + this.Query; }
        }

        public String PathAndQueryAndFragment
        {
            get { return this.Path + this.Query + this.Fragment; }
        }

        public bool IsHttps
        {
            get
            {
                if (this.Kind == UrlKind.Absolute) return String.Equals("https", this.uriBuilder.Scheme);
                else throw new NotSupportedException("Not supported for a relative URL");
            }
        }

        #endregion

        public Uri ToUri()
        {
            if (this.Kind == UrlKind.Absolute) return this.uriBuilder.Uri;
            else return new Uri(url, UriKind.Relative);
        }

        //public bool HasParameter(String parameter)
        //{
        //    var parameters = new SortedDictionary<String, String>();

        //    foreach (var parameterAndValue in this.Query.TrimStart("?").Split('&'))
        //    {
        //        parameter
        //    }

        //    return false;
        //}
    }
}
