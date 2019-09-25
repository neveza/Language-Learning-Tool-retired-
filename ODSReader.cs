using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;

namespace ODSReader
{
    class ODSReader
    {
        private ZipArchiveEntry ContentXmlFile;
        private XDocument Document;

        public List<List<XElement>> CellRows;
        private List<XElement> Cells;


        void UnzipODS(System.IO.Stream stream)
        {
            //ZipArchive archive = ZipFile.OpenRead(path);

            ZipArchive archive = new ZipArchive(stream);

            ContentXmlFile = archive.GetEntry("content.xml");


        }

        void LoadXMLFile()
        {
            Document = XDocument.Load(ContentXmlFile.Open());
        }

        public void ReadXmlFile()
        {
            var rows = Document.Descendants("{urn:oasis:names:tc:opendocument:xmlns:table:1.0}table-row").Skip(1);

            foreach (var row in rows)
            {
                Cells = (from c in row.Descendants()
                         where c.Name == "{urn:oasis:names:tc:opendocument:xmlns:table:1.0}table-cell"
                         select c).ToList();

                CellRows.Add(Cells);

            }
        }

        public ODSReader(System.IO.Stream stream)
        {
            CellRows = new List<List<XElement>>();
            UnzipODS(stream);
            LoadXMLFile();
            ReadXmlFile();
        }
    }
}
