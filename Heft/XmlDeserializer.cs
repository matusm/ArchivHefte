using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace ArchivHefte
{
    public class XmlDeserializer
    {
        public List<Heft> Hefte { get; } = new List<Heft>();
        public int NumberOfHefte => Hefte.Count;

        public XmlDeserializer(string fileName)
        {
            XmlSerializer xmls = new XmlSerializer(Hefte.GetType());
            FileStream fs = new FileStream(fileName, FileMode.Open);
            Hefte = (List<Heft>)xmls.Deserialize(fs);
        }
    }
}
