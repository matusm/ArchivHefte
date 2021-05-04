namespace TXT2XML
{
    public class AhBearbeiter : BaseAhMultiline
    {
        public override string ToString()
        {
            string returnValue = "";
            foreach (string p in paragraphs)
            {
                returnValue += $"Beobachter: {p}\n";
            }
            return returnValue;
        }
    }
}
