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
    public sealed class AbsoluteUrl
    {
        int _lastParameterIndex = 0;

        public string Scheme { get; }
        public string UserInfo { get; }
        public string Host { get; }
        public int Port { get; }
        public string Path { get; }
        public string Query { get; }
        public string Fragment { get; }

        private AbsoluteUrl(string scheme, string userInfo, string host, int port, string path, string query, string fragment)
        {
            Scheme = scheme;
            UserInfo = userInfo;
            Host = host;
            Port = port;
            Path = path;
            Query = query;
            Fragment = fragment;
        }

        public static AbsoluteUrl Create(string url, bool encoded = true)
        {
            string scheme, authority, userInfo = "", host = "", path, query, fragment;
            int port = 0;

            if (!encoded) url = HttpUtility.UrlEncode(url);

            //Parsing with regex from RFC 3986, Appendix B. - http://www.ietf.org/rfc/rfc3986.txt
            var match = Regex.Match(url, @"^(([^:/?#]+):)?(//([^/?#]*))?([^?#]*)(\?([^#]*))?(#(.*))?");
            authority = match.Groups[4].Value;

            scheme = match.Groups[2].Value.ToLowerInvariant(); //Scheme is case insensitive
            path = "/" + match.Groups[5].Value.TrimStart('/');
            query = match.Groups[7].Value;
            fragment = match.Groups[9].Value;

            //Check if the Scheme is "illegal"
            if (string.IsNullOrEmpty(scheme) && Regex.IsMatch(scheme, "https*", RegexOptions.IgnoreCase))
                throw new NotSupportedException("Scheme has to be http or https for an absolute URL");

            //Userinfo, host and port are found with further parsing of the authority
            match = Regex.Match(authority, @"(([^@]+)@)?([^:]+)(:(\d+))?");
            userInfo = match.Groups[2].Value;
            host = match.Groups[3].Value.ToLowerInvariant();

            //Port
            if (!string.IsNullOrEmpty(match.Groups[5].Value))
                port = Convert.ToInt32(match.Groups[5].Value);
            else
                port = scheme == "http" ? 80 : 443;

            return new AbsoluteUrl(scheme, userInfo, host, port, path, query, fragment);
        }

        public static AbsoluteUrl Create(Uri uri) => Create(uri.ToString());

        /// <summary>
        /// Returns the normalized string of the URL
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            //Only include username and password if they are present
            string userInfo = string.Empty;

            if (!string.IsNullOrWhiteSpace(UserInfo))
            {
                userInfo = UserInfo + "@";
            }
            else
            {
                //UserName and password not present, no need to include it in the URL
            }

            //Only include port is it is not the default one for the scheme
            string port = string.Empty;

            if ((Scheme == "http" && Port != 80) ||
                (Scheme == "https" && Port != 443))
            {
                port = ":" + Port.ToString();
            }
            else
            {
                //Default port for this scheme, no need to include it in the URL
            }

            return string.Format("{0}://{1}{2}{3}{4}", Scheme, userInfo, Host, port, PathAndQueryAndFragment);
        }

        public AbsoluteUrl WithScheme(string scheme) => new AbsoluteUrl(scheme, UserInfo, Host, Port, Path, Query, Fragment);

        public AbsoluteUrl WithUserInfo(string userInfo) => new AbsoluteUrl(Scheme, userInfo, Host, Port, Path, Query, Fragment);

        public AbsoluteUrl WithHost(string host) => new AbsoluteUrl(Scheme, UserInfo, host, Port, Path, Query, Fragment);

        public AbsoluteUrl WithPort(int port) => new AbsoluteUrl(Scheme, UserInfo, Host, port, Path, Query, Fragment);

        public AbsoluteUrl WithPath(string path) => new AbsoluteUrl(Scheme, UserInfo, Host, Port, path, Query, Fragment);

        public AbsoluteUrl WithQuery(string query) => new AbsoluteUrl(Scheme, UserInfo, Host, Port, Path, query, Fragment);

        public AbsoluteUrl WithFragment(string fragment) => new AbsoluteUrl(Scheme, UserInfo, Host, Port, Path, Query, fragment);

        public bool HasQuery => !string.IsNullOrWhiteSpace(Query);

        public bool HasFragment => !string.IsNullOrWhiteSpace(Fragment);

        public string PathAndQuery => Path + (HasQuery ? "?" + Query : string.Empty);

        public string PathAndQueryAndFragment => PathAndQuery + (HasFragment ? "#" + Fragment : string.Empty);

        /// <summary>
        /// Returns the extension of the file name, if it has one.
        /// E.g: "/path/index.html" will return ".html"
        /// </summary>
        /// <remarks>The dot "." is considered part of the extension</remarks>
        public string Extension => System.IO.Path.GetExtension(Path);

        /// <summary>
        /// Returns true if the url has a file with an extension, false otherwise.
        /// </summary>
        public bool HasExtension => Extension != string.Empty;

        /// <summary>
        /// Returns a new Url where the extension is replaced by the extension as argument.
        /// E.g: "/path/index.html" with "aspx" will return "/path/index.aspx"
        /// </summary>
        /// <remarks>Leading and trailing dots "." will be trimmed.</remarks>
        /// <param name="extension"></param>
        //public Url WithExtension(string extension)
        //{
        //    if (!HasExtension)
        //    {
        //        return this;
        //    }
        //    else {

        //    }
        //}

        /// <summary>
        /// Returns the file name if there is one, an empty string otherwise.
        /// </summary>
        public string FileName => HasExtension ? System.IO.Path.GetFileName(Path) : "";

        /// <summary>
        /// Returns true if the url has a file name, false otherwise.
        /// </summary>
        public bool HasFileName => FileName != string.Empty;

        //public bool HasFileNameAndExtension { get { return Extension != string.Empty; } }

        //public Url WithFileNameAndExtension(string fileNameAndExtension)
        //{
        //    if (HasFileNameAndExtension)
        //    { }
        //    else
        //    {
        //        return new Url(Kind, Scheme, UserName, Password, Host, Port, Path, Query, Fragment);
        //    }
        //}

        public bool IsHttps => string.Equals("https", Scheme);

        public bool IsDomainAnIPAddress => Regex.IsMatch(Host, @"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}");

        /// <summary>
        /// Returns the top level domain if there is one, an empty string otherwise.
        /// </summary>
        public string TopLevelDomain => !IsDomainAnIPAddress ? Host.Substring(Host.LastIndexOf(".") + 1) : "";

        public string SecondLevelDomain
        {
            get
            {
                if (IsDomainAnIPAddress)
                    return "";
                else
                {
                    var hostWithoutTopLevelDomain = Host.Substring(0, Host.LastIndexOf("."));
                    return Host.Substring(hostWithoutTopLevelDomain.LastIndexOf(".") + 1);
                }
            }
        }

        public Uri ToUri() => new Uri(ToString(), UriKind.Absolute);

        public bool HasParameter(string param)
        {
            var parameters = BuildParametersDictionary();
            return parameters.ContainsKey(param);
        }

        //TODO Encode the parameter and the value
        public AbsoluteUrl SetParameter(string param, string value) => DoWithParametersDictionary(p => p.Add(param, Tuple.Create(value, _lastParameterIndex++)));

        public AbsoluteUrl RemoveParameter(string param) => DoWithParametersDictionary(p => p.Remove(param));

        public AbsoluteUrl Concat(AbsoluteUrl relativeUrl) => new AbsoluteUrl(Scheme, UserInfo, Host, Port, Path.TrimEnd('/') + "/" + relativeUrl.Path.TrimStart('/'), Query, Fragment);

        public AbsoluteUrl Concat(string relativeUrl) => Concat(Create(relativeUrl));

        private AbsoluteUrl DoWithParametersDictionary(Action<Dictionary<string, Tuple<string, int>>> action)
        {
            var parameters = BuildParametersDictionary();

            action(parameters);

            return new AbsoluteUrl(Scheme, UserInfo, Host, Port, Path, GetQueryFromParametersDictionary(parameters), Fragment);
        }

        private Dictionary<string, Tuple<string, int>> BuildParametersDictionary()
        {
            var parameters = new Dictionary<string, Tuple<string, int>>();

            foreach (var keyValue in Query.Split('&').Select((s => GetKeyValueAsTuple(s))))
            {
                parameters.Add(keyValue.Item1, Tuple.Create(keyValue.Item2, _lastParameterIndex++));
            }

            return parameters;
        }

        private Tuple<string, string> GetKeyValueAsTuple(string keyValue)
        {
            var keyValueArray = keyValue.Split('=');

            switch (keyValueArray.Length)
            {
                case 1:
                    return Tuple.Create(HttpUtility.UrlDecode(keyValueArray[0]), string.Empty);
                case 2:
                    return Tuple.Create(HttpUtility.UrlDecode(keyValueArray[0]), keyValueArray[1]);
                default:
                    throw new InvalidOperationException("Query element contains more than one '=' character");
            }
        }

        private string GetQueryFromParametersDictionary(Dictionary<string, Tuple<string, int>> parameters) =>
            parameters.OrderBy(kv => kv.Value.Item2)
                .Select(kv => FormatKeyValue(kv.Key, kv.Value.Item1))
                .Aggregate(new StringBuilder(), (sb, s) => sb.AppendFormat("{0}&", s), sb => sb.ToString())
                .TrimEnd('&');

        private string FormatKeyValue(string key, string value) =>
            HttpUtility.UrlEncode(key) + (!string.IsNullOrWhiteSpace(value) ? "=" + HttpUtility.UrlEncode(value) : "");
    }
}
