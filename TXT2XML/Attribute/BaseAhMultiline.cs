using System.Collections.Generic;

namespace TXT2XML
{
    public abstract class BaseAhMultiline
    {
        /// <summary>
        /// The collection of paragraphs (actually just strings).
        /// </summary>
        protected List<string> paragraphs = new List<string>();

        public int NumberOfParagraphs => paragraphs.Count;
        public List<string> Paragraphs => paragraphs;

        public virtual void AddParagraph(string par)
        {
            if (string.IsNullOrEmpty(par.Trim()))
                return;
            paragraphs.Add(par.Trim());
        }
    }
}
