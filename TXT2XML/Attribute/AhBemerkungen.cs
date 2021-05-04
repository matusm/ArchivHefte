namespace TXT2XML
{
    public class AhBemerkungen : BaseAhMultiline
    {
        public override string ToString()
        {
            string returnValue = "";
            foreach (string p in paragraphs)
            {
                returnValue += $"Bemerkung: {p}\n";
            }
            return returnValue;
        }
    }
}
