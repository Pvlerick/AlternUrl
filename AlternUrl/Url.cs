using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace AlternUrl
{
    [ImmutableObject(true)]
    public sealed class Url
    {
        int lastParameterIndex = 0;

        public Url(String url, bool encoded = true)
        {
            if (!encoded) url = HttpUtility.UrlEncode(url);

            this.Kind = Regex.IsMatch(url, "^https*://", RegexOptions.IgnoreCase) ? UrlKind.Absolute : UrlKind.Relative;

            //Use UriBuilder class for the heavy lifting (parsing, etc...) using a fake domain if the given url is relative
            var urlForBuilder = this.Kind == UrlKind.Absolute ? url : String.Format("http://a.com/{0}", url.TrimStart('/'));
            var uriBuilder = new UriBuilder(urlForBuilder);

            this._scheme = uriBuilder.Scheme;
            this._userName = uriBuilder.UserName;
            this._password = uriBuilder.Password;
            this._host = uriBuilder.Host;
            this._port = uriBuilder.Port;
            this._path = uriBuilder.Path;
            this._query = uriBuilder.Query.TrimStart('?');
            this._fragment = uriBuilder.Fragment.TrimStart('#');
        }

        public Url(Uri uri) : this(uri.ToString()) { }

        private Url(UrlKind kind, String scheme, String userName, String password, String host, int port, String path, String query, String fragment)
        {
            this.Kind = kind;
            this._scheme = scheme;
            this._userName = userName;
            this._password = password;
            this._host = host;
            this._port = port;
            this._path = path;
            this._query = query;
            this._fragment = fragment;
        }

        public override string ToString()
        {
            if (this.Kind == UrlKind.Absolute)
            {
                //Only include username and password if they are present
                String userNameAndPassword = String.Empty;

                if (!String.IsNullOrWhiteSpace(this.UserName) || !String.IsNullOrWhiteSpace(this.Password))
                {
                    userNameAndPassword = this.UserName + ":" + this.Password + "@";
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

                return String.Format("{0}://{1}{2}{3}{4}", this.Scheme, userNameAndPassword, this.Host, port, this.PathAndQueryAndFragment);
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
            else return new Url(this.Kind, scheme, this.UserName, this.Password, this.Host, this.Port, this.Path, this.Query, this.Fragment);
        }
        #endregion

        #region UserName
        private readonly String _userName;

        public String UserName
        {
            get
            {
                if (this.Kind == UrlKind.Relative) throw new NotSupportedException("Not supported for a relative URL");
                else return this._userName;
            }
        }

        public Url WithUserName(String userName)
        {
            if (this.Kind == UrlKind.Relative) throw new NotSupportedException("Not supported for a relative URL");
            else return new Url(this.Kind, this.Scheme, userName, this.Password, this.Host, this.Port, this.Path, this.Query, this.Fragment);
        }
        #endregion

        #region Password
        //TODO Use SecureString?
        private readonly String _password;

        public String Password
        {
            get
            {
                if (this.Kind == UrlKind.Relative) throw new NotSupportedException("Not supported for a relative URL");
                else return this._password;
            }
        }

        public Url WithPassword(String password)
        {
            if (this.Kind == UrlKind.Relative) throw new NotSupportedException("Not supported for a relative URL");
            else return new Url(this.Kind, this.Scheme, this.UserName, password, this.Host, this.Port, this.Path, this.Query, this.Fragment);
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
            else return new Url(this.Kind, this.Scheme, this.UserName, this.Password, host, this.Port, this.Path, this.Query, this.Fragment);
        }
        #endregion

        #region Port
        private readonly int _port;

        public int Port
        {
            get
            {
                if (this.Kind == UrlKind.Relative) throw new NotSupportedException("Not supported for a relative URL");
                else return this._port;
            }
        }

        public Url WithPort(int port)
        {
            if (this.Kind == UrlKind.Relative) throw new NotSupportedException("Not supported for a relative URL");
            else return new Url(this.Kind, this.Scheme, this.UserName, this.Password, this.Host, port, this.Path, this.Query, this.Fragment);
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
            return new Url(this.Kind, this.Scheme, this.UserName, this.Password, this.Host, this.Port, path, this.Query, this.Fragment);
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
            return new Url(this.Kind, this.Scheme, this.UserName, this.Password, this.Host, this.Port, this.Path, query, this.Fragment);
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
            return new Url(this.Kind, this.Scheme, this.UserName, this.Password, this.Host, this.Port, this.Path, this.Query, fragment);
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

        public String Extension { get { return System.IO.Path.GetExtension(this.Path); } }
        public bool HasExtension { get { return this.Extension != String.Empty; } }
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

            return new Url(this.Kind, this.Scheme, this.UserName, this.Password, this.Host, this.Port, this.Path.TrimEnd('/') + "/" + relativeUrl.Path.TrimStart('/'), this.Query, this.Fragment);
        }

        public Url Concat(String relativeUrl)
        {
            return this.Concat(new Url(relativeUrl));
        }

        private Url DoWithParametersDictionary(Action<Dictionary<String, Tuple<String, int>>> action)
        {
            var parameters = this.BuildParametersDictionary();

            action(parameters);

            return new Url(this.Kind, this._scheme, this._userName, this._password, this._host, this._port, this._path, this.GetQueryFromParametersDictionary(parameters), this._fragment);
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
