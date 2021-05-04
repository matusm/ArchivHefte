namespace ArchivHefte
{
    public static class Spezial
    {
        public static string CodeToName(char c)
        {
            switch (c)
            {
                case 'A':
                    return "Alkoholometrie";
                case 'B':
                    return "Barometrie (Luftdruck, Luftdichte)";
                case 'C':
                    return "Aräometer (excl. Alkoholometer und Saccharometer)";
                case 'D':
                    return "Dichte von Flüssigkeiten";
                case 'E':
                    return "Elektrische Messungen (excl. Elektrizitätszähler)";
                case 'F':
                    return "Feuchtemessung (Hygrometer)";
                case 'G':
                    return "Gasmesser, Gaskubizierer";
                case 'H':
                    return "Historische Metrologie (Alte Maßeinheiten, Einführung des metrischen Systems)";
                case 'I':
                    return "Theoretische Arbeiten";
                case 'J':
                    return "Dichte von Festkörpern";
                case 'K':
                    return "Arbeiten über Kapillarität";
                case 'L':
                    return "Längenmessungen";
                case 'M':
                    return "Druckmessung (Manometer)";
                case 'N':
                    return "Statisches Volumen (Eichkolben, Flüssigkeitsmaße, Trockenmaße)";
                case 'O':
                    return "Durchfluss (Wassermesser)";
                case 'P':
                    return "Protokolle";
                case 'Q':
                    return "Flächenmessmaschinen und Planimeter";
                case 'R':
                    return "Spirituskontrollmessapparate";
                case 'S':
                    return "Saccharometrie";
                case 'T':
                    return "Thermometrie";
                case 'U':
                    return "Versuche und Untersuchungen";
                case 'V':
                    return "Volumsbestimmungen";
                case 'W':
                    return "Masse (Gewichtsstücke, Wägungen)";
                case 'X':
                    return "Photometrie";
                case 'Y':
                    return "Flammpunktsprüfer, Abelprober";
                case 'Z':
                    return "Getreideprober";
                case 'a':
                    return "Winkelmessungen";
                case 'b':
                    return "Münzgewichte";
                case 'c':
                    return "Garngewichte";
                case 'd':
                    return "";
                case 'e':
                    return "Elektrizitätszähler";
                case 'f':
                    return "Fass-Kubizierapparate";
                case 'g':
                    return "Gewichtsstücke aus Gold (und vergoldete)";
                case 'h':
                    return "Eichstempel";
                case 'i':
                    return "";
                case 'j':
                    return "";
                case 'k':
                    return "";
                case 'l':
                    return "";
                case 'm':
                    return "Meterprototyp aus Platin-Iridium";
                case 'n':
                    return "Pyknometer";
                case 'o':
                    return "Petroleum-Messapparate";
                case 'p':
                    return "Gewichtsstücke aus Platin oder Platin-Iridium (auch Kilogramm-Prototyp)";
                case 'q':
                    return "Gewichtsstücke aus Bergkristall";
                case 'r':
                    return "Gewichtsstücke aus Glas";
                case 's':
                    return "Bierwürze-Messapparate";
                case 't':
                    return "";
                case 'u':
                    return "";
                case 'v':
                    return "Visierstäbe";
                case 'w':
                    return "Waagen";
                case 'x':
                    return "";
                case 'y':
                    return "";
                case 'z':
                    return "Verschiedenes";
                default:
                    return "";
            }
        }
    }
}
