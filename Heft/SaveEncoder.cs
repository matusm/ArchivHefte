using System.Security;
using System.Net;

namespace ArchivHefte
{

    public static class SaveEncoder
    {
        /// <summary>
        /// Encode the specified string for save use within XML or HTML use.
        /// </summary>
        /// <returns>The save string.</returns>
        /// <param name="unsaveString">The string to be encoded.</param>
        /// <param name="type">Type.</param>
        public static string Encode(string unsaveString, EncodeFor type)
        {
            if (string.IsNullOrEmpty(unsaveString))
                return "";
            switch (type)
            {
                case EncodeFor.Xml:
                    return SecurityElement.Escape(unsaveString);
                case EncodeFor.Html:
                    return WebUtility.HtmlEncode(unsaveString);
                case EncodeFor.Tex:
                    return TexEncode(unsaveString);
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

