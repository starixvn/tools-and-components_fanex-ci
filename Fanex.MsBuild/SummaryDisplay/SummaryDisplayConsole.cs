namespace Fanex.MSBuild.SummaryDisplay
{
    using System;
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
        public const string FileName = "SUMMARY.DISPLAY.xml";
        private ProjectSummaryList _projectSummaryList = new ProjectSummaryList();

        public string XslFile
        {
            get
            {
                return ConfigurationManager.AppSettings["SummaryDisplayXsl"];
            }
        }

        /// <summary>
        /// Gets all project summaries by xml.
        /// </summary>
        /// <value>String xml.</value>
        public string ProjectSumarryXml
        {
            get
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

        /// <summary>
        /// Flush summary content and close.
        /// </summary>
       /// <param name="directory">Output directory.</param>
        public void FlushXml(string directory)
        {
            directory = directory.EndsWith(@"\", StringComparison.CurrentCultureIgnoreCase) ? directory : directory + @"\";
            var filePath = directory + FileName;
            try
            {
                // Get xml
                var xml = ProjectSumarryXml;

                // Xslt
                var xslt = new XslCompiledTransform();
                xslt.Load(XslFile);

                // Transform
                using (var stringReader = new StringReader(xml))
                {
                    using (var xmlReader = XmlReader.Create(stringReader))
                    {
                        using (var streamWriter = new StreamWriter(filePath, false))
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

        public void RemoveXml(string directory)
        {
            directory = directory.EndsWith(@"\", StringComparison.CurrentCultureIgnoreCase) ? directory : directory + @"\";
            var filePath = directory + FileName;

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public void AddProjectSummary(ProjectSummary projectSummary)
        {
            _projectSummaryList.Add(projectSummary);
        }
    }
}