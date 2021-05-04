using ArchivHefte;
using Bev.UI;
using System;
using System.IO;

namespace XML2TXT
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleUI.Welcome();
            
            #region File name logic
            string defaultFileName = "AH_NS";
            string baseInFileName = "";
            string baseOutFileName = "";
            if(args==null || args.Length==0)
            {
                baseInFileName = defaultFileName;
                baseOutFileName = defaultFileName;
            }
            if(args.Length == 1)
            {
                baseInFileName = args[0];
                baseOutFileName = baseInFileName;
            }
            if(args.Length>=2)
            {
                baseInFileName = args[0];
                baseOutFileName = args[1];
            }
            string inFilename = Path.ChangeExtension(baseInFileName, ".xml");
            string outFilename = Path.ChangeExtension(baseOutFileName, ".txt");
            #endregion

            // read xml file and parse it
            ConsoleUI.ReadingFile(inFilename);
            var hefte = new XmlDeserializer(inFilename);
            ConsoleUI.Done();

            // nothing found -> exit
            if (hefte.NumberOfHefte == 0)
                ConsoleUI.ErrorExit("No data found", 1);

            // write consolidated TXT file
            ConsoleUI.WritingFile(outFilename);
            using (StreamWriter outFile = new StreamWriter(outFilename))
            {
                // add some documentation at top of file
                outFile.WriteLine(hefte.Hefte[0].ToLegacyFormat(true));
                outFile.WriteLine($"XML Datei: {inFilename}");
                outFile.WriteLine($"Erzeugt am {DateTime.Now} durch {ConsoleUI.Title}");
                outFile.WriteLine($"{hefte.NumberOfHefte} Archivhefte");
                foreach (var h in hefte.Hefte)
                {
                    outFile.WriteLine(h.ToLegacyFormat());
                }
                outFile.WriteLine(hefte.Hefte[0].ToLegacyFormat(true));
            }
            ConsoleUI.Done();

        }
    }
}
