// -----------------------------------------------------------------------
// <copyright file="IHtmlTransform.cs" company="Nexcel Solutions">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Atomiq
{
    /// <summary>
    /// IHtmlTransform interface.
    /// </summary>
    public interface IHtmlTransform
    {
        string HtmlFile
        {
            get;
        }

        string Transform(string htmlFile);
    }
}
