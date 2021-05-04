namespace TXT2XML
{
    public class AhBeilagen : BaseAhMultiline
    {
        public override string ToString()
        {
            string returnValue = "";
            for (int i = 0; i < paragraphs.Count; i++)
            {
                returnValue += $"Beilage B{i + 1}: {paragraphs[i]}\n";
            }
            return returnValue;
        }
    }
}
