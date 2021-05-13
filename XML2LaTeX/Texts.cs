namespace XML2LaTeX
{
    /// <summary>
    /// Sammlung einzufügender Fließtexte.
    /// </summary>
    static public class Texts
    {
        /// <summary>
        /// Im Abschnitt fehlender Hefte, gleich nach \section*{Hefte von allgemeinem Inhalt}
        /// Es sind spezielle Formatangaben notwendig.
        /// </summary>
        public static string AbsatzFehlendAllgemein => @"Diese Hefte konnten bei der Nachschau im Archiv nicht (mehr) aufgefunden werden. Die behandelten Sachthemen sind von allgemeiner Natur und entsprechen im Wesentlichen den von der heutigen Abteilung E2 betreuten Fachgebieten.";

        /// <summary>
        /// Im Abschnitt fehlender Hefte, gleich nach \section*{Elektrische Messungen betreffende Hefte}
        /// </summary>
        public static string AbsatzFehlendElektrisch => @"Die große Anzahl in dieser Kategorie lässt vermuten, dass diese Hefte im Zuge einer Organisationsänderung in ein entsprechendes Archiv einer anderen Abteilung (früher E3, jetzt E1) eingegliedert wurden und dort verloren gegangen sind. Relativ häufig werden Systemprüfungen (im heutigen Sprachgebrauch: Zulassungsprüfungen) an Elektrizitätszählern behandelt.";

        public static string AbsatzFehlendAeS => @"Diese Hefte konnten in den beiden Archivschachteln nicht mehr aufgefunden werden";

        public static string TitleSection1 => @"Einträge aus dem Haupt-Verzeichnis, 1. Heft";
        public static string TitleSection2 => @"Einträge aus dem Haupt-Verzeichnis, 2. Heft";
        public static string TitleSection3 => @"Einträge aus dem Haupt-Verzeichnis, 3. Heft";
        public static string TitleSection4 => @"Gemeinsame Einträge aus dem 3. Heft und aus dem grünen Heft";
        public static string TitleSection5 => @"Einträge aus dem grünen Heft";


    }
}
