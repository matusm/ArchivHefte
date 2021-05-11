using System.Collections.Generic;
using ArchivHefte;
using System.IO;
using System.Xml.Serialization;
using System;
using System.Reflection;

namespace TXT2XML
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"This is {Assembly.GetExecutingAssembly().GetName().Name} version {Assembly.GetExecutingAssembly().GetName().Version}");


            //string defaultFileName = @"/Users/michaelmatus/Projects/ArchivHefte/Daten/AH_AeS"; // mac
            // string defaultFileName = @"/Users/michaelmatus/Projects/ArchivHefte/Daten/AH_NS"; // mac            
            //string defaultFileName = @"C:\Users\Administrator\source\repos\ArchivHefte\Daten\AH_AeS"; // windows
             string defaultFileName = @"C:\Users\Administrator\source\repos\ArchivHefte\Daten\AH_NS"; // windows

            // Ältere oder Neue Serie?
            HeftType serie = HeftType.NS;


            #region File name logic
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
            if(args.Length >= 2)
            {
                baseInFileName = args[0];
                baseOutFileName = args[1];
            }
            string inFilename = Path.ChangeExtension(baseInFileName, ".txt");
            string outFilename = Path.ChangeExtension(baseOutFileName, ".xml");
            #endregion

            #region Reading and parsing the TXT-file

            // Einlesen und Decodierung der Text-Datei
            List<Heft> hefte = new List<Heft>();
            AhSignatur currentSignatur;
            AhEntry currentHeft = new AhEntry();
            currentHeft.Serie = serie;

            int numberTextLines = 0;
            string textLine;
            bool isFirstHeft = true;

            StreamReader hTxtFile = File.OpenText(inFilename);
            while ((textLine = hTxtFile.ReadLine()) != null)
            {
                if (!string.IsNullOrWhiteSpace(textLine))
                {
                    numberTextLines++;
                    // start a new database entity
                    if (textLine.Contains("#HEFT= "))
                    {
                        if (!isFirstHeft)
                            hefte.Add(currentHeft.ArchivHeft);
                        isFirstHeft = false;
                        currentSignatur = new AhSignatur(serie, textLine.Substring(7));
                        currentHeft = new AhEntry(currentSignatur);
                        currentHeft.Serie = serie;
                    }
                    // parse attributes for the entity
                    currentHeft.ParseAttributes(textLine);
                }
            }
            // the last entity must be added to the collection
            hefte.Add(currentHeft.ArchivHeft);
            hTxtFile.Close();

            Console.WriteLine($"{numberTextLines} Zeilen in Datei {inFilename} -> {hefte.Count} Hefte");
            
            #endregion

            #region Writing XML-file

            //ConsoleUI.WritingFile(outFilename);
            StreamWriter hXmlFile = new StreamWriter(outFilename);
            XmlSerializer x = new XmlSerializer(hefte.GetType());
            x.Serialize(hXmlFile, hefte);
            hXmlFile.Close();
            //ConsoleUI.Done();

            Console.WriteLine($"{outFilename} written.");

            # endregion

        }
    }
}
