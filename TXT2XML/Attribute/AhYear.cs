using System;

namespace TXT2XML
{
    public class AhYear
    {
        public AhYear()
        {
            FromYear = null;
            ToYear = null;
            IsQuestionable = false;
        }

        public AhYear(string dataLine) : this()
        {
            ParseString(dataLine);
        }

        public string AsString => StringRepresentation();
        public int AsNumber => FromYear.HasValue ? FromYear.Value : -1;
        public bool IsQuestionable { get; private set; }
        public int? FromYear { get; private set; }
        public int? ToYear { get; private set; }

        public override string ToString() { return AsString; }

        private string StringRepresentation()
        {
            string retString = "";
            if (!FromYear.HasValue && !ToYear.HasValue)
            {
                return retString;
            }
            if (FromYear.HasValue)
            {
                retString = $"{FromYear.Value}";
                if (ToYear.HasValue)
                {
                    retString += $" - {ToYear.Value}";
                }
            }
            if (IsQuestionable)
            {
                retString += " (?)";
            }
            return retString;
        }

        private void ParseString(string str)
        {
            if (string.IsNullOrEmpty(str)) return;
            // remove leading and trailing spaces (if any)
            str = str.Trim();
            string[] token;
            char[] separators = { ' ' };
            token = str.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            switch (token.Length)
            {
                // a single token must be the fromYear
                case 1:
                    FromYear = ParseYear(token[0]);
                    return;
                // two tokens: fromYear and dubious
                case 2:
                    FromYear = ParseYear(token[0]);
                    IsQuestionable = true;
                    return;
                // three tokens: fromYear and toYear
                case 3:
                    FromYear = ParseYear(token[0]);
                    ToYear = ParseYear(token[2]);
                    return;
                // three tokens: fromYear and toYear and dubios
                case 4:
                    FromYear = ParseYear(token[0]);
                    ToYear = ParseYear(token[2]);
                    IsQuestionable = true;
                    return;
                default:
                    return;
            }
        }

        private int? ParseYear(string token)
        {
            if (int.TryParse(token, out int temp))
                return temp;
            return null;
        }
    }
}
