namespace TXT2XML
{
    public class AhInhalt
    {
        public string Inhalt { get; }

        public AhInhalt()
        {
            Inhalt = "";
        }

        public AhInhalt(string line)
        {
            Inhalt = line.Trim();
        }

        public override string ToString()
        {
            return Inhalt;
        }
    }
}
