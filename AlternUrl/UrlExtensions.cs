using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlternUrl
{
    public static class UrlExtensions
    {
        public static UriKind ToUriKind(this UrlKind kind)
        {
            switch (kind)
            {
                case UrlKind.Relative:
                    return UriKind.Relative;
                case UrlKind.Absolute:
                    return UriKind.Absolute;
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
