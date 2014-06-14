namespace Fanex.MsBuild.SummaryDisplay
{
    using System.Configuration;
    using System.IO;
    using System.Xml;
    using System.Xml.Serialization;
    using System.Xml.Xsl;

    /// <summary>
    /// Write email output.
    /// </summary>
    public class SummaryDisplayConsole
    {
        // Fields
        private const string FileName = "SUMMARY.DISPLAY.xml";
        private ProjectSummaryList _projectSummaryList = new ProjectSummaryList();

        public string XslFile
        {
            get
            {
                return ConfigurationManager.AppSettings["SummaryDisplayXsl"];
            }
        }

        /// <summary>
        /// Flush summary content and close.
        /// </summary>
       /// <param name="directory">Output directory.</param>
        public void FlushXml(string directory)
        {
            directory = directory.EndsWith(@"\") ? directory : directory + @"\";
            var file = directory + FileName;
            try
            {
                // Get xml
                var xml = GetProjectSumarryXml();

                // Xslt
                var xslt = new XslCompiledTransform();
                xslt.Load(XslFile);

                // Transform
                using (var stringReader = new StringReader(xml))
                {
                    using (var xmlReader = XmlReader.Create(stringReader))
                    {
                        using (var streamWriter = new StreamWriter(file, false))
                        {
                            using (var xmlWriter = XmlWriter.Create(streamWriter, xslt.OutputSettings))
                            {
                                xslt.Transform(xmlReader, xmlWriter);
                            }
                        }
                    }
                }

                _projectSummaryList.Clear();
            }
            catch
            {
                throw;
            }
        }

        public void AddProjectSummary(ProjectSummary projectSummary)
        {
            _projectSummaryList.Add(projectSummary);
        }

        /// <summary>
        /// Get all project summaries by xml.
        /// </summary>
        /// <returns>String xml.</returns>
        public string GetProjectSumarryXml()
        {
            var xmlSerializerNamespaces = new XmlSerializerNamespaces();
            xmlSerializerNamespaces.Add(string.Empty, string.Empty);

            var xmlSerializer = new XmlSerializer(typeof(ProjectSummaryList));

            var stringWriter = new StringWriter();
            var xmlWriter = XmlWriter.Create(stringWriter);
            xmlSerializer.Serialize(xmlWriter, _projectSummaryList, xmlSerializerNamespaces);
            return stringWriter.ToString();
        }
    }
}