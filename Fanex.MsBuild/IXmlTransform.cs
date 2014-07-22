// -----------------------------------------------------------------------
// <copyright file="IXmlTransform.cs" company="Nexcel Solutions">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.MSBuild
{
    /// <summary>
    /// XmlTransform interface.
    /// </summary>
    public interface IXmlTransform
    {
        string XslFile
        {
            get;
        }

        string Transform(string xmlFile);
    }
}
