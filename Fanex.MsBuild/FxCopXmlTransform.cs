// -----------------------------------------------------------------------
// <copyright file="FxCopXmlTransform.cs" company="Nexcel Solutions">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.MSBuild
{
    using System.Configuration;

    /// <summary>
    /// Transform FXCOP violations xml to html.
    /// </summary>
    public class FxCopXmlTransform : IXmlTransform
    {
        public string XslFile
        {
            get
            {
                return ConfigurationManager.AppSettings["FxCopXsl"];
            }
        }

        public string Transform(string xmlFile)
        {
            string result = string.Empty;

            // Program.ReplaceInFile(xmlFile, Program.Options.RootDirectory + Program.Options.Name + "\\", string.Empty);
            XmlTransformCore core = new XmlTransformCore(xmlFile, this.XslFile);
            result = core.Transform();

            return result;
        }
    }
}
