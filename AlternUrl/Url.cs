using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace AlternUrl
{
    [ImmutableObject(true)]
    public sealed class Url
    {
        int lastParameterIndex = 0;

        private Url(UrlKind kind, String scheme, String userInfo, String host, int port, String path, String query, String fragment)
        {
            this.Kind = kind;
            this._scheme = scheme;
            this._userInfo = userInfo;
            this._host = host;
            this._port = port;
            this._path = path;
            this._query = query;
            this._fragment = fragment;
        }

        public static Url Create(String url, bool encoded = true)
        {
            String scheme, authority, userInfo = "", host = "", path, query, fragment;
            int port = 0;
            UrlKind kind;

            if (!encoded) url = HttpUtility.UrlEncode(url);

            //Parsing with regex from RFC 3986, Appendix B. - http://www.ietf.org/rfc/rfc3986.txt
            var match = Regex.Match(url, @"^(([^:/?#]+):)?(//([^/?#]*))?([^?#]*)(\?([^#]*))?(#(.*))?");
            authority = match.Groups[4].Value;

            scheme = match.Groups[2].Value.ToLowerInvariant(); //Scheme is case insensitive
            path = "/" + match.Groups[5].Value.TrimStart('/');
            query = match.Groups[7].Value;
            fragment = match.Groups[9].Value;

            //Kind of Url
            if (!String.IsNullOrEmpty(scheme) || !String.IsNullOrEmpty(authority))
            {
                //...Absolute URL
                kind = UrlKind.Absolute;

                //Check if the Scheme is "illegal"
                if (String.IsNullOrEmpty(scheme) && Regex.IsMatch(scheme, "https*", RegexOptions.IgnoreCase))
                {
                    throw new NotSupportedException("Scheme has to be http or https for an absolute URL");
                }
                else
                {
                    //Scheme is http or https
                }

                //Userinfo, host and port are found with further parsing of the authority
                match = Regex.Match(authority, @"(([^@]+)@)?([^:]+)(:(\d+))?");
                userInfo = match.Groups[2].Value;
                host = match.Groups[3].Value.ToLowerInvariant();

                //Port
                if (!String.IsNullOrEmpty(match.Groups[5].Value))
                {
                    port = Convert.ToInt32(match.Groups[5].Value);
                }
                else
                {
                    //Default port according to the scheme
                }
            }
            else
            {
                //...Relative URL, nothing else to do
                kind = UrlKind.Relative;
            }

            return new Url(kind, scheme, userInfo, host, port, path, query, fragment);
        }

        public static Url Create(Uri uri)
        {
            return Url.Create(uri.ToString());
        }

        /// <summary>
        /// Returns the normalized string of the URL
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (this.Kind == UrlKind.Absolute)
            {
                //Only include username and password if they are present
                String userInfo = String.Empty;

                if (!String.IsNullOrWhiteSpace(this.UserInfo))
                {
                    userInfo = this.UserInfo + "@";
                }
                else
                {
                    //UserName and password not present, no need to include it in the URL
                }

                //Only include port is it is not the default one for the scheme
                String port = String.Empty;

                if ((this.Scheme == "http" && this.Port != 80) ||
                    (this.Scheme == "https" && this.Port != 443))
                {
                    port = ":" + this.Port.ToString();
                }
                else
                {
                    //Default port for this scheme, no need to include it in the URL
                }

                return String.Format("{0}://{1}{2}{3}{4}", this.Scheme, userInfo, this.Host, port, this.PathAndQueryAndFragment);
            }
            else
            {
                return this.PathAndQueryAndFragment;
            }
        }

        #region Properties and With() methods

        public UrlKind Kind { get; private set; }

        #region Scheme
        private readonly String _scheme;

        public String Scheme
        {
            get
            {
                if (this.Kind == UrlKind.Relative) throw new NotSupportedException("Not supported for a relative URL");
                else return this._scheme;
            }
        }

        public Url WithScheme(String scheme)
        {
            if (this.Kind == UrlKind.Relative) throw new NotSupportedException("Not supported for a relative URL");
            else return new Url(this.Kind, scheme, this.UserInfo, this.Host, this.Port, this.Path, this.Query, this.Fragment);
        }
        #endregion

        #region UserInfo
        private readonly String _userInfo;

        public String UserInfo
        {
            get
            {
                if (this.Kind == UrlKind.Relative) throw new NotSupportedException("Not supported for a relative URL");
                else return this._userInfo;
            }
        }

        public Url WithUserInfo(String userInfo)
        {
            if (this.Kind == UrlKind.Relative) throw new NotSupportedException("Not supported for a relative URL");
            else return new Url(this.Kind, this.Scheme, userInfo, this.Host, this.Port, this.Path, this.Query, this.Fragment);
        }
        #endregion

        #region Host
        private readonly String _host;

        public String Host
        {
            get
            {
                if (this.Kind == UrlKind.Relative) throw new NotSupportedException("Not supported for a relative URL");
                else return this._host;
            }
        }

        public Url WithHost(String host)
        {
            if (this.Kind == UrlKind.Relative) throw new NotSupportedException("Not supported for a relative URL");
            else return new Url(this.Kind, this.Scheme, this.UserInfo, host, this.Port, this.Path, this.Query, this.Fragment);
        }
        #endregion

        #region Port
        private readonly int _port;

        public int Port
        {
            get
            {
                if (this.Kind == UrlKind.Relative) throw new NotSupportedException("Not supported for a relative URL");
                else
                {
                    if (this._port != 0)
                    {
                        return this._port;
                    }
                    else
                    {
                        return this.IsHttps ? 443 : 80;
                    }
                }
            }
        }

        public Url WithPort(int port)
        {
            if (this.Kind == UrlKind.Relative) throw new NotSupportedException("Not supported for a relative URL");
            else return new Url(this.Kind, this.Scheme, this.UserInfo, this.Host, port, this.Path, this.Query, this.Fragment);
        }
        #endregion

        #region Path
        private readonly String _path;

        public String Path
        {
            get
            {
                return this._path;
            }
        }

        public Url WithPath(String path)
        {
            return new Url(this.Kind, this.Scheme, this.UserInfo, this.Host, this.Port, path, this.Query, this.Fragment);
        }
        #endregion

        #region Query
        private readonly String _query;

        public String Query
        {
            get
            {
                return this._query;
            }
        }

        public Url WithQuery(String query)
        {
            return new Url(this.Kind, this.Scheme, this.UserInfo, this.Host, this.Port, this.Path, query, this.Fragment);
        }

        public bool HasQuery
        {
            get { return !String.IsNullOrWhiteSpace(this.Query); }
        }
        #endregion

        #region Fragment
        private readonly String _fragment;

        public String Fragment
        {
            get
            {
                return this._fragment;
            }
        }

        public Url WithFragment(String fragment)
        {
            return new Url(this.Kind, this.Scheme, this.UserInfo, this.Host, this.Port, this.Path, this.Query, fragment);
        }

        public bool HasFragment
        {
            get { return !String.IsNullOrWhiteSpace(this._fragment); }
        }
        #endregion

        public String PathAndQuery
        {
            get { return this.Path + (this.HasQuery ? "?" + this.Query : String.Empty); }
        }

        public String PathAndQueryAndFragment
        {
            get { return this.PathAndQuery + (this.HasFragment ? "#" + this.Fragment : String.Empty); }
        }

        /// <summary>
        /// Returns the extension of the file name, if it has one.
        /// E.g: "/path/index.html" will return ".html"
        /// </summary>
        /// <remarks>The dot "." is considered part of the extension</remarks>
        public String Extension { get { return System.IO.Path.GetExtension(this.Path); } }
        public bool HasExtension { get { return this.Extension != String.Empty; } }

        /// <summary>
        /// Returns a new Url where the extension is replaced by the extension as argument.
        /// E.g: "/path/index.html" with "aspx" will return "/path/index.aspx"
        /// </summary>
        /// <remarks>Leading and trailing dots "." will be trimmed.</remarks>
        /// <param name="extension"></param>
        //public Url WithExtension(String extension)
        //{
        //    if (!this.HasExtension)
        //    {
        //        return this;
        //    }
        //    else {

        //    }
        //}

        public String FileName { get { return this.HasExtension ? System.IO.Path.GetFileNameWithoutExtension(this.Path) : String.Empty; } }
        public bool HasFileName { get { return this.FileName != String.Empty; } }

        //public bool HasFileNameAndExtension { get { return this.Extension != String.Empty; } }

        //public Url WithFileNameAndExtension(String fileNameAndExtension)
        //{
        //    if (this.HasFileNameAndExtension)
        //    { }
        //    else
        //    {
        //        return new Url(this.Kind, this.Scheme, this.UserName, this.Password, this.Host, this.Port, this.Path, this.Query, this.Fragment);
        //    }
        //}

        public bool IsHttps
        {
            get
            {
                if (this.Kind == UrlKind.Absolute) return String.Equals("https", this.Scheme);
                else throw new NotSupportedException("Not supported for a relative URL");
            }
        }

        public bool IsDomainAnIPAddress
        {
            get
            {
                return Regex.IsMatch(this.Host, @"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}");
            }
        }

        public String TopLevelDomain
        {
            get
            {
                if (this.Kind == UrlKind.Relative) throw new NotSupportedException("Not supported for an relative url");
                if (this.IsDomainAnIPAddress) throw new NotSupportedException("Not supported for an url who's domain is a numerical IP address");

                return this.Host.Substring(this.Host.LastIndexOf(".") + 1);
            }
        }

        public String SecondLevelDomain
        {
            get
            {
                if (this.Kind == UrlKind.Relative) throw new NotSupportedException("Not supported for an relative url");
                if (this.IsDomainAnIPAddress) throw new NotSupportedException("Not supported for an url who's domain is a numerical IP address");

                var hostWithoutTopLevelDomain = this.Host.Substring(0, this.Host.LastIndexOf("."));

                return this.Host.Substring(hostWithoutTopLevelDomain.LastIndexOf(".") + 1);
            }
        }

        #endregion

        public Uri ToUri()
        {
            return new Uri(this.ToString(), this.Kind.ToUriKind());
        }

        public bool HasParameter(String param)
        {
            var parameters = this.BuildParametersDictionary();

            Console.WriteLine("param: {0}", param);
            Console.WriteLine("parameters: {0}", parameters.Keys.Aggregate(new StringBuilder(), (sb, s) => sb.AppendFormat("{0}, ", s), sb => sb.ToString()));

            return parameters.ContainsKey(param);
        }

        public Url AddParameter(String param, String value)
        {
            //TODO Encode the parameter and the value
            return this.DoWithParametersDictionary(p => p.Add(param, Tuple.Create(value, this.lastParameterIndex++)));
        }

        public Url AddParameter(String param)
        {
            return this.AddParameter(param, String.Empty);
        }

        public Url RemoveParameter(String param)
        {
            return this.DoWithParametersDictionary(p => p.Remove(param));
        }

        public Url SetParameter(String param, String value)
        {
            if (!this.HasParameter(param)) throw new ArgumentException("param", "Parameter is not present in the query string");

            return this.DoWithParametersDictionary(p => p[param] = Tuple.Create(value, p[param].Item2));
        }

        public Url AddOrSetParameter(String param, String value)
        {
            if (this.HasParameter(param)) return this.SetParameter(param, value);
            else return this.AddParameter(param, value);
        }

        public Url Concat(Url relativeUrl)
        {
            if (relativeUrl.Kind == UrlKind.Absolute) throw new NotSupportedException("Not supported for an absolute url");

            return new Url(this.Kind, this.Scheme, this.UserInfo, this.Host, this.Port, this.Path.TrimEnd('/') + "/" + relativeUrl.Path.TrimStart('/'), this.Query, this.Fragment);
        }

        public Url Concat(String relativeUrl)
        {
            return this.Concat(Url.Create(relativeUrl));
        }

        private Url DoWithParametersDictionary(Action<Dictionary<String, Tuple<String, int>>> action)
        {
            var parameters = this.BuildParametersDictionary();

            action(parameters);

            return new Url(this.Kind, this._scheme, this._userInfo, this._host, this._port, this._path, this.GetQueryFromParametersDictionary(parameters), this._fragment);
        }

        private Dictionary<String, Tuple<String, int>> BuildParametersDictionary()
        {
            var parameters = new Dictionary<String, Tuple<String, int>>();

            foreach (var keyValue in this.Query.Split('&').Select((s => this.GetKeyValueAsTuple(s))))
            {
                parameters.Add(keyValue.Item1, Tuple.Create(keyValue.Item2, this.lastParameterIndex++));
            }

            return parameters;
        }

        private Tuple<String, String> GetKeyValueAsTuple(String keyValue)
        {
            var keyValueArray = keyValue.Split('=');

            switch (keyValueArray.Length)
            {
                case 1:
                    return Tuple.Create(HttpUtility.UrlDecode(keyValueArray[0]), String.Empty);
                case 2:
                    return Tuple.Create(HttpUtility.UrlDecode(keyValueArray[0]), keyValueArray[1]);
                default:
                    throw new InvalidOperationException("Query seems to contain strange characters..."); //Yeah I know... :-)
            }
        }

        private String GetQueryFromParametersDictionary(Dictionary<String, Tuple<String, int>> parameters)
        {
            return parameters
                .OrderBy(kv => kv.Value.Item2)
                .Select(kv => this.FormatKeyValue(kv.Key, kv.Value.Item1))
                .Aggregate(new StringBuilder(), (sb, s) => sb.AppendFormat("{0}&", s), sb => sb.ToString())
                .TrimEnd('&');

        }

        private String FormatKeyValue(String key, String value)
        {
            if (String.IsNullOrWhiteSpace(value)) return HttpUtility.UrlEncode(key);
            else return String.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(value));
        }
    }
}
