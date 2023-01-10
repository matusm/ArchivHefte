using System.Security;
using System.Net;

namespace ArchivHefte
{
    public static class SaveEncoder
    {
        public static string Encode(string unsaveString, EncodeFor type)
        {
            if (string.IsNullOrEmpty(unsaveString))
                return "";
            switch (type)
            {
                case EncodeFor.Xml:
                    return SecurityElement.Escape(unsaveString); // save for XML use
                case EncodeFor.Html:
                    return WebUtility.HtmlEncode(unsaveString); // save for HTML use
                case EncodeFor.Tex:
                    return TexEncode(unsaveString); // save for TeX use
                default:
                    return unsaveString;
            }
        }

        private static string TexEncode(string raw)
        {
            string temp = raw.Replace(@"%", @"\%{}");
            //temp = temp.Replace(@"_", @"\_{}");
            //temp = temp.Replace(@"^", @"\^{}");
            temp = temp.Replace(@"&", @"\&{}");
            temp = temp.Replace(@"$", @"\${}");
            temp = temp.Replace(@"³", @"{$^3$}");
            temp = temp.Replace(@"µ", @"{$\mu$}");
            temp = temp.Replace(@"²", @"{$^2$}");
            temp = temp.Replace(@"°", @"{$^\circ$}");
            return temp;
        }
    }

    public enum EncodeFor 
    { 
        None, 
        Xml, 
        Html, 
        Tex 
    }
}

