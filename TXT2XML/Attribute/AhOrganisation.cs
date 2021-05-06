namespace TXT2XML
{
    public class AhOrganisation
    {
        public AhOrganisationEnum AsEnum { get; private set; }
        public string AsString => StringRepresentation();

        public AhOrganisation()
        {
            AsEnum = AhOrganisationEnum.Unknown;
        }

        public AhOrganisation(string line) : this()
        {
            ParseString(line);
        }

        public override string ToString()
        {
            return AsString;
        }

        private string StringRepresentation()
        {
            switch (AsEnum)
            {
                case AhOrganisationEnum.Aew:
                    return "AEW";
                case AhOrganisationEnum.Bev:
                    return "BEV";
                case AhOrganisationEnum.Nek:
                    return "NEK";
                default:
                    return "";
            }
        }

        private void ParseString(string line)
        {
            if (line.Contains("NEK")) AsEnum = AhOrganisationEnum.Nek;
            if (line.Contains("BEV")) AsEnum = AhOrganisationEnum.Bev;
            if (line.Contains("AEW")) AsEnum = AhOrganisationEnum.Aew;
        }

    }


    public enum AhOrganisationEnum
    {
        Unknown,
        Nek,    // Normal-Eichungskommission
        Aew,    // Amt für Eichwesen
        Bev     // Bundesamt für Eich- und Vermessungswesen
    }
}
