using ArchivHefte;
using Bev.UI;
using System.IO;

namespace XML2LaTeX
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
            string outFilename = Path.ChangeExtension(baseOutFileName, ".tex");
            #endregion

            // read xml file and parse it
            ConsoleUI.ReadingFile(inFilename);
            var data = new XmlDeserializer(inFilename);
            ConsoleUI.Done();

            // nothing found -> exit
            if (data.NumberOfHefte == 0)
                ConsoleUI.ErrorExit("No data found", 1);

            // preparing translation
            ConsoleUI.StartOperation("Performing translation");
            LatexSource latexSource = new LatexSource(); // this writes the preamble
            foreach (Heft h in data.Hefte)
            {
                latexSource.AddHeft(h);
            }
            ConsoleUI.Done();

            // write tex file
            ConsoleUI.WritingFile(outFilename);
            using (StreamWriter outFile = new StreamWriter(outFilename))
            {
                    outFile.WriteLine(latexSource.Content());
            }
            ConsoleUI.Done();

        }
    }
}
