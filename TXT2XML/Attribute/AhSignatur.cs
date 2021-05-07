using ArchivHefte;
using System;

namespace TXT2XML
{
    public class AhSignatur
    {
        private readonly string sigString;
        private readonly int doublet;
        private readonly HeftType heftType;

        public AhSignatur(HeftType heftType, string rawString)
        {
            this.heftType = heftType;
            // zuerst auf Dubletten prüfen
            doublet = ParseNumberOfDubs(rawString);
            rawString = StripDubs(rawString);
            sigString = ConvertFromLegacySyntax(rawString);
        }

        public string PrettyString => FormatSignature();

        public override string ToString() { return PrettyString; }

        private string FormatSignature()
        {
            if (doublet == 0)
                return sigString;
            else
                return $"{sigString}.{doublet}";
        }

        private int ParseNumberOfDubs(string rawString)
        {
            if (string.IsNullOrWhiteSpace(rawString))
                return 0;
            if (heftType == HeftType.AeS)
                return 0;
            char[] delimiterChars = { '.' };
            string[] token = rawString.Split(delimiterChars, StringSplitOptions.RemoveEmptyEntries);
            if (token.Length != 2)
                return 0;
            if (int.TryParse(token[1], out int dubs))
                return dubs;
            return 0;
        }

        private string StripDubs(string rawString)
        {
            if (string.IsNullOrWhiteSpace(rawString))
                return "";
            if (heftType == HeftType.AeS)
                return rawString.Trim();
            char[] delimiterChars = { '.' };
            string[] token = rawString.Split(delimiterChars, StringSplitOptions.RemoveEmptyEntries);
            if (token.Length >= 1)
                return token[0].Trim();
            return "";
        }

        private string ConvertFromLegacySyntax(string rawString)
        {
            if (string.IsNullOrWhiteSpace(rawString))
                return "";
            if (rawString.Contains("["))
            {
                return rawString;
            }
            else
            {
                // remove leading underscores
                rawString = rawString.Replace("__", " ");
                rawString = rawString.Replace("_", " ");
                // enclose with []
                return $"[{rawString.Trim()}]";
            }
        }

    }
}
