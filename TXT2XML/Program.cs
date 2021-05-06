using System;
using System.Collections.Generic;
using ArchivHefte;
using Bev.UI;
using System.IO;
using System.Xml.Serialization;

namespace TXT2XML
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
            if(args.Length >= 2)
            {
                baseInFileName = args[0];
                baseOutFileName = args[1];
            }
            string inFilename = Path.ChangeExtension(baseInFileName, ".txt");
            string outFilename = Path.ChangeExtension(baseOutFileName, ".xml");
            #endregion

            #region Reading and parsing the TXT-file

            ConsoleUI.ReadingFile(inFilename);
            // Ältere oder Neue Serie?
            HeftType heftType = HeftType.NS;
            // Einlesen und Decodierung der Text-Datei
            List<Heft> hefte = new List<Heft>();
            AhSignatur currentSignatur;
            AhEntry currentHeft = new AhEntry();
            currentHeft.Type = heftType;

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
                        currentSignatur = new AhSignatur(heftType, textLine.Substring(7));
                        currentHeft = new AhEntry(currentSignatur);
                        currentHeft.Type = heftType;

                    }
                    // parse attributes for the entity
                    currentHeft.FillAttributes(textLine);
                }
            }
            // the last entity must be added to the collection
            hefte.Add(currentHeft.ArchivHeft);
            hTxtFile.Close();
            ConsoleUI.Done();

            #endregion

            ConsoleUI.WriteLine();
            ConsoleUI.WriteLine($"{numberTextLines} Zeilen in Datei {inFilename} -> {hefte.Count} Hefte");
            ConsoleUI.WriteLine();

            #region Writing XML-file

            ConsoleUI.WritingFile(outFilename);
            StreamWriter hXmlFile = new StreamWriter(outFilename);
            XmlSerializer x = new XmlSerializer(hefte.GetType());
            x.Serialize(hXmlFile, hefte);
            hXmlFile.Close();
            ConsoleUI.Done();

            # endregion

        }
    }
}
