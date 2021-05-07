using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArchivHefte;

namespace XML2LaTeX
{
    public class IndexChronologie
    {

        private Dictionary<int, string> chronoDict = new Dictionary<int, string>();
        private StringBuilder sb = new StringBuilder();

        public void Add(Heft heft)
        {
            int year = heft.Chronologie;
            if (year <= 0)
                return;
            if(chronoDict.ContainsKey(year))
            {
                string newValue = chronoDict[year] + " " + heft.Signatur;
                chronoDict[year] = newValue;
            }
            else
            {
                chronoDict[year] = heft.Signatur;
            }
        }

        public string AsText()
        {
            GenerateSB();
            return sb.ToString();
        }

        private void GenerateSB()
        {
            sb.Clear();
            var keyList = chronoDict.Keys.ToList();
            keyList.Sort();
            foreach (var k in keyList)
            {
                sb.AppendLine($"{k} : {chronoDict[k]}");
                sb.AppendLine();
            }
        }


    }
}
