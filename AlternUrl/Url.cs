using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlternUrl
{
    public class Url
    {
        UriBuilder builder;

        public Url(String url)
        {
            Uri uri;

            if (!Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out uri)) throw new ArgumentException("url");

            this.builder = new UriBuilder(uri);
        }

        public Url(Uri uri)
        {
            this.builder = new UriBuilder(uri);
        }

        public Uri ToUri()
        {
            return new UriBuilder().Uri;
        }

        public static Url Concat(Url url, String path)
        {
            throw new NotImplementedException();
        }
    }
}
