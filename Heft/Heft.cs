using System.Collections.Generic;
using System.Text;

namespace ArchivHefte
{
    public class Heft
    {
        public string Signatur { get; set; }
        public HeftType Serie { get; set; }
        public string Inhalt { get; set; }
        public string Jahr { get; set; }
        public string Status { get; set; }
        public string Organisation { get; set; }
        public string KeysCode { get; set; }
        public List<string> Beilagen { get; set; }
        public List<string> Bearbeiter { get; set; }
        public List<string> Bemerkungen { get; set; }
        public string Markierung { get; set; } // ?
        // The following properties are redundant but handy
        public List<string> Keys { get; set; }
        public int Chronologie { get; set; }

        public override string ToString() { return $"Heft: {Signatur}"; }

        public string ToLegacyFormat() { return ToLegacyFormat(Serie); }

        public string ToLegacyFormat(HeftType heftType)
        {
            if (heftType == HeftType.Empty)
                return separator;
            return ToPlainFormat(ConvertToLegacySignatur(Signatur));
        }

        public string ToTextFormat(HeftType heftType)
        {
            if (heftType == HeftType.Empty)
                return separator;
            return ToPlainFormat(Signatur);
        }

        public string ToTextFormat() { return ToTextFormat(Serie); }

        private string ToPlainFormat(string signatureText)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(separator);
            sb.AppendLine($"#HEFT= {signatureText}");
            sb.AppendLine($"#INHA= {Inhalt}");
            foreach (var s in Beilagen)
                sb.AppendLine($"#BEIL= {s}");
            sb.AppendLine($"#JAHR= {Jahr}");
            sb.AppendLine($"#SPEZ= {KeysCode}");
            foreach (var s in Bemerkungen)
                sb.AppendLine($"#BEME= {s}");
            foreach (var s in Bearbeiter)
                sb.AppendLine($"#BEOB= {s}");
            sb.AppendLine($"#STAT= {Status}");
            sb.AppendLine($"#ORGA= {Organisation}");
            sb.Append($"#MARK= {Markierung}");
            return sb.ToString();
        }

        /// <summary>
        /// Transforms a string like [ZA] to the legacy syntax _ZA.
        /// For HeftType.NS only
        /// </summary>
        /// <param name="signatur">The nice signature string.</param>
        /// <returns>The legacy format of the signature string.</returns>
        private string ConvertToLegacySignatur(string signatur)
        {
            if (Serie == HeftType.NS)
            {
                string retString = signatur;
                retString = retString.Replace("[", "");
                retString = retString.Replace("]", "");
                retString = retString.Trim();
                if (retString.Length == 1) return "__" + retString;
                if (retString.Length == 2) return "_" + retString;
                return retString;
            }
            return signatur.Trim();
        }

        private const string separator = "****************************************";

    }

    public enum HeftType
    {
        Unknown,
        Empty,  // nichts drinnen -> erzeugt Trennlinie
        NS,     // Archivheft der "Neuen Serie"
        AeS     // Archivheft der "Älteren Serie"
    }
}
