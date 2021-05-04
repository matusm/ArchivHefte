namespace TXT2XML
{
    public class AhMarkierung
    {
        public string Markierung { get; }

        public AhMarkierung()
        {
            Markierung = "";
        }

        public AhMarkierung(string line)
        {
            Markierung = line.Trim();
        }

        public override string ToString()
        {
            return Markierung;
        }
    }
}
