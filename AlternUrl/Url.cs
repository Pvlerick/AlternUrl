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

        int lastParameterIndex = 0;

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

        public UrlKind Kind { get; private set; }
        public String Extension { get { return System.IO.Path.GetExtension(this.Path); } }
        public bool HasExtension { get { return this.Extension != String.Empty; } }

        #region Properties and Setters

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
        #endregion

        public bool HasQuery
        {
            get { return String.IsNullOrWhiteSpace(this.Query); }
        }

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
        #endregion

        public bool HasFragment
        {
            get { return String.IsNullOrWhiteSpace(this.Fragment); }
        }

        public String PathAndQuery
        {
            get { return this.Path + "?" + this.Query; }
        }

        public String PathAndQueryAndFragment
        {
            get { return this.Path + "?" + this.Query + "#" + this.Fragment; }
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
            return new Uri(this.ToString(), this.Kind.ToUriKind());
        }

        public bool HasParameter(String param)
        {
            var parameters = this.BuildParametersDictionary();

            return parameters.ContainsKey(param);
        }

        public Url AddParameter(String param, String value)
        {
            //TODO Encode the parameter and the value
            this.DoWithParametersDictionary(p => p.Add(param, Tuple.Create(value, this.lastParameterIndex++)));

            return this;
        }

        public Url AddParameter(String param)
        {
            return this.AddParameter(param, String.Empty);
        }

        public Url RemoveParameter(String param)
        {
            this.DoWithParametersDictionary(p => p.Remove(param));

            return this;
        }

        public Url SetParameter(String param, String value)
        {
            if (!this.HasParameter(param)) throw new ArgumentException("param", "Parameter is not present in the query string");

            this.DoWithParametersDictionary(p => p[param] = Tuple.Create(value, p[param].Item2));

            return this;
        }

        public Url AddOrSetParameter(String param, String value)
        {
            if (this.HasParameter(param)) return this.SetParameter(param, value);
            else return this.AddParameter(param, value);
        }

        private void DoWithParametersDictionary(Action<Dictionary<String, Tuple<String, int>>> action)
        {
            var parameters = this.BuildParametersDictionary();

            action(parameters);

            this.Query = this.GetQueryFromParametersDictionary(parameters);
        }

        private Dictionary<String, Tuple<String, int>> BuildParametersDictionary()
        {
            var parameters = new Dictionary<String, Tuple<String, int>>();

            foreach (var keyValue in this.Query.TrimStart('?').Split('&').Select((s => this.GetKeyValueAsTuple(s))))
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
                    return Tuple.Create(keyValueArray[0], String.Empty);
                case 2:
                    return Tuple.Create(keyValueArray[0], keyValueArray[1]);
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
            if (String.IsNullOrWhiteSpace(value)) return key;
            else return String.Format("{0}={1}", key, value);
        }
    }
}
