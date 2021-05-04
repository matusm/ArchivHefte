using ArchivHefte;

namespace TXT2XML
{
    public class AhEntry
    {
        #region Fields =: Attributes
        private readonly AhSignatur ahSignatur;
        private AhInhalt ahInhalt;
        private readonly AhBeilagen ahBeilagen;
        private readonly AhBemerkungen ahBemerkungen;
        private readonly AhBearbeiter ahBearbeiter;
        private AhYear ahYear;
        private AhStatus ahStatus;
        private AhOrganisation ahOrganisation;
        private AhSchlagworte ahKeys;
        private AhMarkierung ahMarkierung;
        #endregion

        public Heft ArchivHeft => Consolidate();

        public AhEntry(AhSignatur sig) : this()
        {
            ahSignatur = sig;
        }

        public AhEntry()
        {
            ahInhalt = new AhInhalt();
            ahBeilagen = new AhBeilagen();
            ahBemerkungen = new AhBemerkungen();
            ahBearbeiter = new AhBearbeiter();
            ahYear = new AhYear();
            ahStatus = new AhStatus();
            ahOrganisation = new AhOrganisation();
            ahMarkierung = new AhMarkierung();
        }

        public Heft Consolidate()
        {
            Heft heft = new Heft();
            heft.Signatur = ahSignatur.PrettyString;
            heft.Inhalt = ahInhalt.Inhalt;
            heft.Jahr = ahYear.AsString;
            heft.Status = ahStatus.AsString;
            heft.Organisation = ahOrganisation.AsString;
            heft.KeysCode = ahKeys.RawKeywordString;
            heft.Keys = ahKeys.Keywords;
            heft.Beilagen = ahBeilagen.Paragraphs;
            heft.Bearbeiter = ahBearbeiter.Paragraphs;
            heft.Bemerkungen = ahBemerkungen.Paragraphs;
            heft.Chronologie = ahYear.AsNumber;
            heft.HtmlFileName = ahSignatur.HtmlFileName;
            heft.ImageFileName = ahSignatur.ImageFileName;
            heft.Markierung = ahMarkierung.Markierung;
            return heft;
        }

        public void FillAttributes(string dataLine)
        {
            if (dataLine.Contains("#JAHR= "))
            {
                ahYear = new AhYear(StripKey(dataLine));
                return;
            }
            if (dataLine.Contains("#STAT= "))
            {
                ahStatus = new AhStatus(StripKey(dataLine));
                return;
            }
            if (dataLine.Contains("#ORGA= "))
            {
                ahOrganisation = new AhOrganisation(StripKey(dataLine));
                return;
            }
            if (dataLine.Contains("#INHA= "))
            {
                ahInhalt = new AhInhalt(StripKey(dataLine));
                return;
            }
            if (dataLine.Contains("#SPEZ= "))
            {
                ahKeys = new AhSchlagworte(StripKey(dataLine));
                return;
            }
            if (dataLine.Contains("#MARK= "))
            {
                ahMarkierung = new AhMarkierung(StripKey(dataLine));
                return;
            }
            if (dataLine.Contains("#BEIL= "))
            {
                ahBeilagen.AddParagraph(StripKey(dataLine));
                return;
            }
            if (dataLine.Contains("#BEME= "))
            {
                ahBemerkungen.AddParagraph(StripKey(dataLine));
                return;
            }
            if (dataLine.Contains("#BEOB= "))
            {
                ahBearbeiter.AddParagraph(StripKey(dataLine));
                return;
            }
        }

        public override string ToString()
        {
            return $"AhEntity {ahSignatur.PrettyString}";
        }

        private string StripKey(string line)
        {
            return line.Substring(7);
        }

    }
}
