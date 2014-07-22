// -----------------------------------------------------------------------
// <copyright file="MSBuildXmlTransform.cs" company="Nexcel Solutions">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.MSBuild
{
    using System.Configuration;

    /// <summary>
    /// Transform MS-BUILD output xml to html text.
    /// </summary>
    public class MSBuildXmlTransform : IXmlTransform
    {
        public string XslFile
        {
            get
            {
                return ConfigurationManager.AppSettings["MsBuildXsl"];
            }
        }

        public string Transform(string xmlFile)
        {
            XmlTransformCore core = new XmlTransformCore(xmlFile, this.XslFile);
            return core.Transform();
        }
    }
}
