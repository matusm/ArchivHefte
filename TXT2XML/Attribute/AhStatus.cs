namespace TXT2XML
{
    public class AhStatus
    {

        public AhStatus()
        {
            AsEnum = AhStatusEnum.Unknown;
        }

        public AhStatus(string line) : this()
        {
            ParseString(line);
        }

        public AhStatusEnum AsEnum { get; private set; }
        public string AsString => StringRepresentation();

        public override string ToString() { return AsString; }

        private void ParseString(string line)
        {
            if (line.Contains("Archiv")) AsEnum = AhStatusEnum.ImArchiv;
            if (line.Contains("fehlt")) AsEnum = AhStatusEnum.Fehlt;
        }

        private string StringRepresentation()
        {
            switch (AsEnum)
            {
                case AhStatusEnum.Fehlt:
                    return "fehlt!";
                case AhStatusEnum.ImArchiv:
                    return "im Archiv.";
                case AhStatusEnum.Unknown:
                    return "unbekannt.";
                default:
                    return "";
            }
        }
    }

    public enum AhStatusEnum
    {
        Unknown,
        Fehlt,
        ImArchiv
    }

}
