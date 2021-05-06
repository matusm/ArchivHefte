using ArchivHefte;

namespace TXT2XML
{
    public class AhSignatur
    {
        private readonly string sigString;
        private readonly int doublet;
        private readonly HeftType heftType;

        public AhSignatur(HeftType heftType, string token)
        {
            this.heftType = heftType;
            doublet = 0;
            if (heftType == HeftType.NS)
            {
                // remove leading underscores
                token = token.Replace("__", " ");
                token = token.Replace("_", " ");
                sigString = token.Trim().ToUpper();
                if (sigString.Length == 5)
                {
                    string index = sigString.Substring(4);
                    doublet = int.Parse(index); // TODO this may raise an exception
                    sigString = sigString.Substring(0, 3);
                }
            }
            if(heftType==HeftType.AeS)
            {
                sigString = token.Trim();
            }
        }

        public string ImageFileName => CreateFilename("_V.JPG");
        public string HtmlFileName => CreateFilename("_V.HTM");
        public string PrettyString => FormatSignature();

        public override string ToString() { return PrettyString; }

        private string PadWithUnderScore()
        {
            if (sigString.Length == 1)
                return $"__{sigString}";
            if (sigString.Length == 2)
                return $"_{sigString}";
            return sigString;
        }

        private string CreateFilename(string postfix)
        {
            if (doublet == 0)
                return PadWithUnderScore() + postfix;
            else
                return PadWithUnderScore() + doublet.ToString() + postfix.Substring(1);
        }

        private string FormatSignature()
        {
            if (heftType == HeftType.NS)
            {
                if (doublet == 0)
                    return $"[{sigString}]";
                else
                    return $"[{sigString}].{doublet}";
            }
            return sigString;
        }
    }
}
