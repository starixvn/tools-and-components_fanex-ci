// -----------------------------------------------------------------------
// <copyright file="StyleCopXmlTransform.cs" company="Nexcel Solutions">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.MsBuild
{
    using System.Configuration;

    /// <summary>
    /// Transform stylecop violations xml to html.
    /// </summary>
    public class StyleCopXmlTransform : IXmlTransform
    {
        private const string OmitStyleCopString = " xmlns:fn=\"http://www.w3.org/2005/xpath-functions\" xmlns:msdata=\"urn:schemas-microsoft-com:xml-msdata\" xmlns:msprop=\"urn:schemas-microsoft-com:xml-msprop\" xmlns:mstns=\"http://stylecopcmd.sourceforge.net/StyleCopReport.xsd\" xmlns:n1=\"urn:schemas-microsoft-com:xml-msdatasource\" xmlns:xdt=\"http://www.w3.org/2005/xpath-datatypes\" xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:altova=\"http://www.altova.com\"";

        public string XslFile
        {
            get
            {
                return ConfigurationManager.AppSettings["StyleCopXsl"];
            }
        }

        public string Transform(string xmlFile)
        {
            string result = string.Empty;

            // Program.ReplaceInFile(xmlFile.Replace(".StyleCop.xml", ".StyleCop.violations.xml"), Program.Options.RootDirectory + Program.Options.Name + "\\", string.Empty);
            XmlTransformCore core = new XmlTransformCore(xmlFile, this.XslFile);
            result = core.Transform().Replace(OmitStyleCopString, string.Empty);

            return result;
        }
    }
}
