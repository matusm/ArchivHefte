using System.Collections.Generic;
using ArchivHefte;

namespace XML2LaTeX
{
    public class IndexSpezial
    {
        private int[] numberIndex;
        private Dictionary<char, int> codeIndex = new Dictionary<char, int>();

        public IndexSpezial()
        {
            int i = 0;
            for (char c = 'A'; c <= 'Z'; c++)
            {
                codeIndex[c] = i;
                i++;
            }
            for (char c = 'a'; c <= 'z'; c++)
            {
                codeIndex[c] = i;
                i++;
            }
            numberIndex = new int[i];
        }

        public int NumberOfSpez(char code)
        { 
            return numberIndex[codeIndex[code]]; 
        }
        
        public string FormattedString(char code)
        {
            return $"{Spezial.CodeToName(code)} --- {NumberOfSpez(code)} Hefte";
        }
        
        public void Add(Heft heft)
        {
            char[] codes = heft.KeysCode.ToCharArray();
            if (codes.Length == 0) return;
            foreach (var c in codes)
                Increment(c);
        }
        
        private void Increment(char code)
        {
            numberIndex[codeIndex[code]]++;
        }
    }
}
