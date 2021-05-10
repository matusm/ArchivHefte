using ArchivHefte;
using Bev.UI;
using System.IO;
using System;
using System.Reflection;

namespace XML2LaTeX
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"This is {Assembly.GetExecutingAssembly().GetName().Name} version {Assembly.GetExecutingAssembly().GetName().Version}");

            #region File name logic
            string defaultFileName = "/Users/michaelmatus/Projects/ArchivHefte/Daten/AH_AeS";
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

            var data = new XmlDeserializer(inFilename);

            Console.WriteLine($"{inFilename} deserialized.");

            // nothing found -> exit
            if (data.NumberOfHefte == 0)
                ConsoleUI.ErrorExit("No data found", 1);

            LatexSource latexSource = new LatexSource(); // this writes the preamble

            foreach (Heft h in data.Hefte)
            {
                latexSource.AddHeft(h);
            }

            using (StreamWriter outFile = new StreamWriter(outFilename))
            {
                    outFile.WriteLine(latexSource.Content());
            }

            Console.WriteLine($"{outFilename} written.");

        }
    }
}
