// -----------------------------------------------------------------------
// <copyright file="XmlTransformCore.cs" company="Nexcel Solutions">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.MSBuild
{
    using System;
    using System.IO;
    using System.Xml;
    using System.Xml.Xsl;

    /// <summary>
    /// Transform xml to html.
    /// </summary>
    public class XmlTransformCore
    {
        public XmlTransformCore(string xmlFile, string xslFile)
        {
            this.XmlFile = xmlFile;
            this.XslFile = xslFile;
        }

        public string XmlFile
        {
            get;
            private set;
        }

        public string XslFile
        {
            get;
            private set;
        }

        /// <summary>
        /// Begin transform.
        /// </summary>
        /// <returns>Return transformed string.</returns>
        public string Transform()
        {
            XslCompiledTransform transform = new XslCompiledTransform();
            transform.Load(this.XslFile);

            XsltArgumentList args = new XsltArgumentList();
            args.AddExtensionObject("urn:XslTransformHelper", new XslTransformHelper());

            string outputFile = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".ci";
            Stream outputStream = File.Open(outputFile, FileMode.Create);
            XmlReader reader = XmlReader.Create(this.XmlFile);

            try
            {
                transform.Transform(reader, args, outputStream);
            }
            catch (Exception e)
            {
                var exception = e.InnerException == null ? e : e.InnerException;
                Console.WriteLine(exception.Message);
                Console.WriteLine(exception.StackTrace);
            }

            outputStream.Close();
            reader.Close();

            string output = System.IO.File.ReadAllText(outputFile);
            System.IO.File.Delete(outputFile);

            return output;
        }
    }
}
