using System;
using System.Text;
using ArchivHefte;

namespace XML2LaTeX
{
    public class LatexSource
    {
        private readonly StringBuilder sb = new StringBuilder(); // The place for the complete source
        private readonly StringBuilder sbFehlendA = new StringBuilder(); // Die im Archiv fehlenden Hefte allgemeiner Natur        
        private readonly StringBuilder sbFehlendE = new StringBuilder(); // Die im Archiv fehlenden Hefte elektrischer Natur ohne E-Zähler
        private readonly StringBuilder sbChronologie = new StringBuilder();
        private readonly StringBuilder sbSpezialIndex = new StringBuilder();
        private readonly IndexSpezial spezialIndex = new IndexSpezial();
        private readonly IndexChronologie chronoIndex = new IndexChronologie();
        private bool finalized = false;

        public LatexSource()
        {
            CreatePreamble();
        }

        public void AddHeft(Heft heft)
        {
            // wenn Datei bereits abgeschlossen, tue nichts.
            if (finalized == true)
                return;
            // Nehme Heft in Index fehlender Hefte auf
            AddToIndexFehlend(heft);
            // Nehme Heft ins Chronologisch Verzeichnis auf
            chronoIndex.Add(heft);
            // Nehme Heft ins Spezialverzeichnis auf
            spezialIndex.Add(heft);
            // Je nachdem aus welchen Verzeichnis entnommen, gibt es eine neue Überschrift
            InVerzeichnisHeft(heft.Signatur);
            // die Breiten der beiden minipages sollen der Signatur angepasst sein
            string leftMinipageFormat, rightMinipageFormat;
            switch (heft.Signatur.Length)
            {
                case 3:
                    leftMinipageFormat = @"\begin{minipage}[t]{0.1\textwidth}\vspace{0pt}";
                    rightMinipageFormat = @"\begin{minipage}[t]{0.9\textwidth}\vspace{0pt}";
                    break;
                case 4:
                    leftMinipageFormat = @"\begin{minipage}[t]{0.15\textwidth}\vspace{0pt}";
                    rightMinipageFormat = @"\begin{minipage}[t]{0.85\textwidth}\vspace{0pt}";
                    break;
                case 5:
                    leftMinipageFormat = @"\begin{minipage}[t]{0.2\textwidth}\vspace{0pt}";
                    rightMinipageFormat = @"\begin{minipage}[t]{0.8\textwidth}\vspace{0pt}";
                    break;
                default:
                    leftMinipageFormat = @"\begin{minipage}[t]{0.22\textwidth}\vspace{0pt}";
                    rightMinipageFormat = @"\begin{minipage}[t]{0.78\textwidth}\vspace{0pt}";
                    break;
            }
            sb.AppendLine(@"%%%%% " + heft.Signatur + @" %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%");
            sb.AppendLine(@"\parbox{\textwidth}{%");
            sb.AppendLine(@"\rule{\textwidth}{1pt}\vspace*{-3mm}\\");
            // Signatur
            sb.AppendLine(leftMinipageFormat);
            sb.AppendLine(@"\Huge\rule[-4mm]{0cm}{1cm}" + heft.Signatur);
            sb.AppendLine(@"\end{minipage}");
            // Titel + Beilagen
            sb.AppendLine(@"\hfill");
            sb.AppendLine(rightMinipageFormat);
            sb.AppendLine(@"\large " + AhEncode(heft.Inhalt) + @"\rule[-2mm]{0mm}{2mm}");
            if(heft.Beilagen.Count != 0)
            {
                sb.AppendLine(@"{\footnotesize \\{}");
                for (int i = 0; i < heft.Beilagen.Count; i++)
                {
                    sb.AppendLine(string.Format("Beilage\\,B{0}: {1}\\\\", i+1 , AhEncode(heft.Beilagen[i])));
                }
                sb.AppendLine(@"}");
            }
            sb.AppendLine(@"\end{minipage}");
            // Sachgebiete
            if (heft.Keys.Count != 0)
            {
                sb.AppendLine(@"{\footnotesize\flushright");
                for (int i = 0; i < heft.Keys.Count; i++)
                {
                    sb.AppendLine(string.Format("{0}\\\\", AhEncode(heft.Keys[i])));
                }
                sb.AppendLine(@"}");
            }
            // Jahr, Amt und Status
            sb.AppendLine(FormatJahr(heft.Jahr) + @"\quad---\quad " + AhEncode(heft.Organisation) + @"\quad---\quad Heft " + AhEncode(heft.Status) + @"\\");
            // Bemerkungen
            if (heft.Bemerkungen.Count != 0)
            {
                sb.AppendLine(@"\textcolor{blue}{Bemerkungen:\\{}");
                for (int i = 0; i < heft.Bemerkungen.Count; i++)
                {
                    sb.AppendLine(string.Format("{0}", AhEncode(heft.Bemerkungen[i])) + @"\\{}");
                }
                sb.AppendLine(@"}");
                sb.AppendLine(@"\\[-15pt]");
                
        }
            sb.AppendLine(@"\rule{\textwidth}{1pt}");
            sb.AppendLine(@"}");
            sb.AppendLine(@"\\");
            sb.AppendLine(@"\vspace*{-2.5pt}\\");
        }
        
        /// <summary>
        /// Delivers the LaTeX source as a single string and terminates the process of adding new items.
        /// </summary>
        /// <returns>The LaTeX source as a single string.</returns>
        public string Content()
        {
            FinalizeSource();
            return sb.ToString();
        }

        private void AddToIndexFehlend(Heft heft)
        {
            if (FehltImArchiv(heft))
            {
                if (Elektrisches(heft))
                {
                    sbFehlendE.Append(heft.Signatur + " ");
                    return;
                }
                sbFehlendA.Append(heft.Signatur + " ");
            }
        }

        /// <summary>
        /// Converts some tokens of the input string to LaTeX code. Takes care of mathematical mode and german quotation marks, too.
        /// </summary>
        /// <param name="inp">The input string.</param>
        /// <returns>The output string.</returns>
        private string AhEncode(string inp)
        {
            string tmp = SaveEncoder.Encode(inp, EncodeFor.Tex);
            // Umwandlung einiger Formel(zeichen)
            tmp = tmp.Replace(@" {$^\circ$}R", @"\,{$^\circ$}R");
            tmp = tmp.Replace(@" {$^\circ$}C", @"\,{$^\circ$}C");
            tmp = tmp.Replace(@"t_H=n+a-z", @"$t_{H}=n+a-z$");
            tmp = tmp.Replace(@"phi(n)-z", @"$\phi(n)-{z}$");
            tmp = tmp.Replace(@"t_Oe", @"$t_{Oe}$");
            tmp = tmp.Replace(@"t_c", @"$t_{c}$");
            tmp = tmp.Replace(@"t_l", @"$t_{l}$");
            tmp = tmp.Replace(@"t_h", @"$t_{h}$");
            tmp = tmp.Replace(@"T_h", @"$T_{h}$");
            tmp = tmp.Replace(@"t_H", @"$t_{H}$");
            tmp = tmp.Replace(@"T_H", @"$T_{H}$");
            tmp = tmp.Replace(@"t_N", @"$t_{N}$");
            tmp = tmp.Replace(@"t_C", @"$t_{C}$");
            tmp = tmp.Replace(@"II_b", @"$II_{b}$");
            tmp = tmp.Replace(@"R=s_t'-s_12", @"$R=s_{t}'-s_{12}$");
            tmp = tmp.Replace(@"D_t", @"$D_{t}$");
            tmp = tmp.Replace(@"D_T", @"$D_{T}$");
            tmp = tmp.Replace(@"v_a", @"$v_{a}$");
            tmp = tmp.Replace(@"v_c", @"$v_{c}$");
            tmp = tmp.Replace(@"D'_t", @"${D'}_{t}$");
            tmp = tmp.Replace(@"D'_T", @"${D'}_{T}$");
            tmp = tmp.Replace(@"gamma_1", @"$\gamma_{1}$");
            tmp = tmp.Replace(@"beta_e", @"$\beta_{e}$");
            tmp = tmp.Replace(@"beta_i", @"$\beta_{i}$");
            tmp = tmp.Replace(@"chi_u", @"$\chi_{u}$");
            tmp = tmp.Replace(@"hc'f_t", @"$hc'f_{t}$");
            tmp = tmp.Replace(@"v_12/P-1", @"$\frac{v_{12}}{P}-1$");
            tmp = tmp.Replace(@"1-P/v_12", @"$1-\frac{P}{v_{12}}$");
            // Umwandlung der Namen für Gewichtsstücke und Senkkörper
            // Reihenfolge ist kritisch!
            tmp = tmp.Replace(@"_ab", @"$\mathrm{_{ab}}$");
            tmp = tmp.Replace(@"Osigma_I", @"$\mathrm{O\sigma_{I}}$");
            tmp = tmp.Replace(@"Wsigma_I", @"$\mathrm{W\sigma_{I}}$");
            tmp = tmp.Replace(@"P_sigma_I", @"$\mathrm{P\sigma_{I}}$");
            tmp = tmp.Replace(@"U_sigma_I", @"$\mathrm{U\sigma_{I}}$");
            tmp = tmp.Replace(@"sigma_1", @"$\sigma_{1}$");
            tmp = tmp.Replace(@"Sigma_20", @"$\Sigma_{20}$");
            tmp = tmp.Replace(@"Sigma_1", @"$\Sigma_{1}$");
            tmp = tmp.Replace(@"sigma_20", @"$\sigma_{20}$");
            tmp = tmp.Replace(@"H_0.3", @"$\mathrm{H_{0.3}}$");
            tmp = tmp.Replace(@"W_0.1", @"$\mathrm{W_{0.1}}$");
            tmp = tmp.Replace(@"T_0.03", @"$\mathrm{T_{0.03}}$");
            tmp = tmp.Replace(@"W_0.01", @"$\mathrm{W_{0.01}}$");
            tmp = tmp.Replace(@"F_1712", @"$\mathrm{F_{1712}}$");
            tmp = tmp.Replace(@"Sonne^K", @"$\mathrm{\bigodot^K}$");
            tmp = tmp.Replace(@"S^K", @"$\mathrm{S^K}$");
            tmp = tmp.Replace(@"_10", @"$_\mathrm{10}$");
            tmp = tmp.Replace(@"_14", @"$_\mathrm{14}$");
            tmp = tmp.Replace(@"_33", @"$_\mathrm{33}$");
            tmp = tmp.Replace(@"_500", @"$_\mathrm{500}$");
            tmp = tmp.Replace(@"_0", @"$_\mathrm{0}$");
            tmp = tmp.Replace(@"_1", @"$_\mathrm{1}$");
            tmp = tmp.Replace(@"_2", @"$_\mathrm{2}$");
            tmp = tmp.Replace(@"_3", @"$_\mathrm{3}$");
            tmp = tmp.Replace(@"_4", @"$_\mathrm{4}$");
            tmp = tmp.Replace(@"_5", @"$_\mathrm{5}$");
            tmp = tmp.Replace(@"_6", @"$_\mathrm{6}$");
            tmp = tmp.Replace(@"_7", @"$_\mathrm{7}$");
            tmp = tmp.Replace(@"_8", @"$_\mathrm{8}$");
            tmp = tmp.Replace(@"_9", @"$_\mathrm{9}$");
            tmp = tmp.Replace(@"_(0)", @"$_\mathrm{(0)}$");
            tmp = tmp.Replace(@"_(1)", @"$_\mathrm{(1)}$");
            tmp = tmp.Replace(@"_(2)", @"$_\mathrm{(2)}$");
            tmp = tmp.Replace(@"_(3)", @"$_\mathrm{(3)}$");
            tmp = tmp.Replace(@"_(4)", @"$_\mathrm{(4)}$");
            tmp = tmp.Replace(@"_(5)", @"$_\mathrm{(5)}$");
            tmp = tmp.Replace(@"_(6)", @"$_\mathrm{(6)}$");
            tmp = tmp.Replace(@"_a", @"$_\mathrm{a}$");
            tmp = tmp.Replace(@"_b", @"$_\mathrm{b}$");
            tmp = tmp.Replace(@"_c", @"$_\mathrm{c}$");
            tmp = tmp.Replace(@"_d", @"$_\mathrm{d}$");
            tmp = tmp.Replace(@"_e", @"$_\mathrm{e}$");
            tmp = tmp.Replace(@"_f", @"$_\mathrm{f}$");
            tmp = tmp.Replace(@"_g", @"$_\mathrm{g}$");
            tmp = tmp.Replace(@"_h", @"$_\mathrm{h}$");
            tmp = tmp.Replace(@"_V", @"$_\mathrm{V}$");
            tmp = tmp.Replace(@"_s", @"$_\mathrm{s}$");
            tmp = tmp.Replace(@"_S", @"$_\mathrm{S}$");
            tmp = tmp.Replace(@"_B", @"$_\mathrm{B}$");
            tmp = tmp.Replace(@"_II", @"$_\mathrm{II}$");
            tmp = tmp.Replace(@"_I", @"$_\mathrm{I}$");
            tmp = tmp.Replace(@"_XX", @"$_\mathrm{XX}$");
            tmp = tmp.Replace(@"_X", @"$_\mathrm{X}$");
            tmp = tmp.Replace(@"_J", @"$_\mathrm{J}$");

            // Punkte im Satz
            tmp = tmp.Replace(@"k.k. ", @"k.k.\ ");
            tmp = tmp.Replace(@"Dr. ", @"Dr.~");
            tmp = tmp.Replace(@"Nr. ", @"Nr.~");
            tmp = tmp.Replace(@"Ph. ", @"Ph.\ ");

            // rot ausgezeichnete Worte
            tmp = tmp.Replace(@"???", @"\textcolor{red}{???}");
            tmp = tmp.Replace(@"fehlt!", @"\textcolor{red}{fehlt!}");

            // Specialitäten
            tmp = tmp.Replace(@"^ter", @"$^\mathrm{ter}$");

            // und jetzt die Anführungszeichen
            string formattedString = FormatGermanQQ(tmp); 
            return formattedString;
        }

        /// <summary>
        /// Converts simple " in german style \glqq and \grqq.
        /// </summary>
        /// <param name="tmp">The input string.</param>
        /// <returns>The formatted output string.</returns>
        private string FormatGermanQQ(string tmp)
        {
            string glqq = @"{\glqq}";
            string grqq = @"{\grqq}";
            string[] sep = { "\"" };
            string[] token = tmp.Split(sep, StringSplitOptions.None);
            if (token.Length <= 1)
                return tmp;
            string formattedString = token[0] + glqq;
            for (int i = 1; i < token.Length; i++)
            {
                formattedString += token[i];
                if (i == token.Length - 1) break;
                if(IsOdd(i)) formattedString += grqq;
                if(IsEven(i)) formattedString += glqq;
            }
            return formattedString;
        }
        
        /// <summary>
        /// Ersetzt den Bindestrich bei "1888 - 1897" zu "1888--1897" 
        /// </summary>
        /// <returns>Formatierte Jahresangabe.</returns>
        /// <param name="inp">Zu formattierende Jahresangabe.</param>
        private string FormatJahr(string inp)
        {
            return inp.Replace(@" - ", @"--");
        }

        private bool FehltImArchiv(Heft heft)
        {
            return !heft.Status.Contains("im Archiv.");
        }

        private bool Elektrisches(Heft heft)
        {
            if (heft.KeysCode.Contains("E") || !heft.KeysCode.Contains("e")) return true;
            return false;
        }

        private void InVerzeichnisHeft(string signatur)
        {
            switch (signatur)
            {
                case "[A]":
                    sb.AppendLine(@"\section{" + Texts.TitleSection1 + @"}");
                    return;
                case "[LL]":
                    sb.AppendLine(@"\section{" + Texts.TitleSection2 + @"}");
                    return;
                case "[AFA]":
                    sb.AppendLine(@"\section{" + Texts.TitleSection3 + @"}");
                    return;
                case "[BLA]":
                    sb.AppendLine(@"\section{" + Texts.TitleSection4 + @"}");
                    return;
                case "[BRS]":
                    sb.AppendLine(@"\section{" + Texts.TitleSection5 + @"}");
                    return;
                default:
                    break;
            }
        }

        private bool IsEven(int i)
        { return i % 2 == 0; }

        private bool IsOdd(int i)
        { return !IsEven(i); }

        private void CreatePreamble()
        {
            // Hauptverzeichnis
            sb.Clear();
            finalized = false;
            sb.AppendLine(@"%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%");
            sb.AppendLine(@"% Erzeugt von XML2LaTeX am " + DateTime.UtcNow + @" %");
            sb.AppendLine(@"%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%");
            sb.AppendLine();
            sb.AppendLine(@"% Diese Datei muss von einer gültigen LaTeX Datei umhüllt werden!");
            sb.AppendLine();
            sb.AppendLine(@"\chapter{Verzeichnis der Archiv-Hefte und Vormerkungen}");
            // Index fehlender Hefte, allgemein
            sbFehlendA.Clear();
            sb.AppendLine(@"%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%");
            sbFehlendA.AppendLine(@"\chapter{Liste der im Archiv fehlenden Hefte}\label{AHfehlend}");
            sbFehlendA.AppendLine(@"\section{Fehlende Hefte von allgemeinem oder unbekannten Inhalt}");
            sbFehlendA.AppendLine(Texts.AbsatzFehlendAllgemein + @"\\");
            sbFehlendA.AppendLine(@"\\{}");
            // Index fehlender Hefte, Elektrizität
            sbFehlendE.Clear();
            sbFehlendE.AppendLine(@"\section{Fehlende Hefte elektrische Messungen betreffend.}");
            sbFehlendE.AppendLine(Texts.AbsatzFehlendElektrisch+ @"\\");
            sbFehlendE.AppendLine(@"\\{}");
            // Spezialverzeichnis
            sbSpezialIndex.Clear();
            sbSpezialIndex.AppendLine(@"\chapter{Themen des Spezialverzeichnises}");
            // Chronologisches Verzeichnis
            sbChronologie.Clear();
            sbChronologie.AppendLine(@"\chapter{Chronologisches Verzeichnises}");
        }

        private void FinalizeSource()
        {
            if (finalized == true)
                return;
            CreateSpezial();
            sbChronologie.Append(chronoIndex.AsText());
            sb.AppendLine();
            sb.Append(sbSpezialIndex.ToString());
            sb.AppendLine();
            sb.Append(sbChronologie.ToString());
            sb.AppendLine();
            sb.Append(sbFehlendA.ToString());
            sb.AppendLine();
            sb.Append(sbFehlendE.ToString());
            sb.AppendLine();
            finalized = true;
        }

        private void CreateSpezial()
        {
            sbSpezialIndex.AppendLine(@"\begin{itemize}");
            sbSpezialIndex.AppendLine(@"\item " + spezialIndex.FormattedString('H'));

            sbSpezialIndex.AppendLine(@"\begin{itemize}");
            sbSpezialIndex.AppendLine(@"\item " + spezialIndex.FormattedString('h'));
            sbSpezialIndex.AppendLine(@"\end{itemize}");
            
            sbSpezialIndex.AppendLine(@"\item " + spezialIndex.FormattedString('L'));
 
            sbSpezialIndex.AppendLine(@"\begin{itemize}");
            sbSpezialIndex.AppendLine(@"\item " + spezialIndex.FormattedString('m'));
            sbSpezialIndex.AppendLine(@"\end{itemize}");
            
            sbSpezialIndex.AppendLine(@"\item " + spezialIndex.FormattedString('W'));

            sbSpezialIndex.AppendLine(@"\begin{itemize}");
            sbSpezialIndex.AppendLine(@"\item " + spezialIndex.FormattedString('w'));
            sbSpezialIndex.AppendLine(@"\item " + spezialIndex.FormattedString('p'));
            sbSpezialIndex.AppendLine(@"\item " + spezialIndex.FormattedString('g'));
            sbSpezialIndex.AppendLine(@"\item " + spezialIndex.FormattedString('q'));
            sbSpezialIndex.AppendLine(@"\item " + spezialIndex.FormattedString('r'));
            sbSpezialIndex.AppendLine(@"\item " + spezialIndex.FormattedString('b'));
            sbSpezialIndex.AppendLine(@"\item " + spezialIndex.FormattedString('c'));
            sbSpezialIndex.AppendLine(@"\end{itemize}");
            
            sbSpezialIndex.AppendLine(@"\item " + spezialIndex.FormattedString('a'));
            sbSpezialIndex.AppendLine(@"\item " + spezialIndex.FormattedString('Q'));
            sbSpezialIndex.AppendLine(@"\item " + spezialIndex.FormattedString('N'));

            sbSpezialIndex.AppendLine(@"\begin{itemize}");
            sbSpezialIndex.AppendLine(@"\item " + spezialIndex.FormattedString('f'));
            sbSpezialIndex.AppendLine(@"\item " + spezialIndex.FormattedString('R'));
            sbSpezialIndex.AppendLine(@"\item " + spezialIndex.FormattedString('v'));
            sbSpezialIndex.AppendLine(@"\item " + spezialIndex.FormattedString('n'));
            sbSpezialIndex.AppendLine(@"\item " + spezialIndex.FormattedString('o'));
            sbSpezialIndex.AppendLine(@"\item " + spezialIndex.FormattedString('s'));
            sbSpezialIndex.AppendLine(@"\end{itemize}");
            
            sbSpezialIndex.AppendLine(@"\item " + spezialIndex.FormattedString('O'));
            sbSpezialIndex.AppendLine(@"\item " + spezialIndex.FormattedString('G'));
            sbSpezialIndex.AppendLine(@"\item " + spezialIndex.FormattedString('D'));

            sbSpezialIndex.AppendLine(@"\begin{itemize}");
            sbSpezialIndex.AppendLine(@"\item " + spezialIndex.FormattedString('C'));
            sbSpezialIndex.AppendLine(@"\item " + spezialIndex.FormattedString('A'));
            sbSpezialIndex.AppendLine(@"\item " + spezialIndex.FormattedString('S'));
            sbSpezialIndex.AppendLine(@"\end{itemize}");
            
            sbSpezialIndex.AppendLine(@"\item " + spezialIndex.FormattedString('J'));
            sbSpezialIndex.AppendLine(@"\item " + spezialIndex.FormattedString('T'));
            sbSpezialIndex.AppendLine(@"\item " + spezialIndex.FormattedString('B'));
            sbSpezialIndex.AppendLine(@"\item " + spezialIndex.FormattedString('M'));
            sbSpezialIndex.AppendLine(@"\item " + spezialIndex.FormattedString('F'));
            sbSpezialIndex.AppendLine(@"\item " + spezialIndex.FormattedString('E'));
            sbSpezialIndex.AppendLine(@"\begin{itemize}");
            sbSpezialIndex.AppendLine(@"\item " + spezialIndex.FormattedString('e'));
            sbSpezialIndex.AppendLine(@"\end{itemize}");
 
            sbSpezialIndex.AppendLine(@"\item " + spezialIndex.FormattedString('V'));
            sbSpezialIndex.AppendLine(@"\item " + spezialIndex.FormattedString('X'));
            sbSpezialIndex.AppendLine(@"\item " + spezialIndex.FormattedString('Y'));
            sbSpezialIndex.AppendLine(@"\item " + spezialIndex.FormattedString('Z'));
            sbSpezialIndex.AppendLine(@"\item " + spezialIndex.FormattedString('K'));
            sbSpezialIndex.AppendLine(@"\item " + spezialIndex.FormattedString('I'));
            sbSpezialIndex.AppendLine(@"\item " + spezialIndex.FormattedString('U'));
            sbSpezialIndex.AppendLine(@"\item " + spezialIndex.FormattedString('z'));
            sbSpezialIndex.AppendLine(@"\end{itemize}");

        }

    }
}
