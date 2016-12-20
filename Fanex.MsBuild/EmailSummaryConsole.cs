// -----------------------------------------------------------------------
// <copyright file="EmailSummaryConsole.cs" company="Nexcel Solutions">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.MSBuild
{
    using System;
    using System.Configuration;
    using System.IO;
    using System.Text;

    /// <summary>
    /// Write email output.
    /// </summary>
    public class EmailSummaryConsole
    {
        // Fields        
        private string _filePath = string.Empty;
        private StringBuilder _content = new StringBuilder();

        // Methods
        public EmailSummaryConsole(string path)
        {
            if (!Directory.Exists(path))
            {
                throw new FieldAccessException("Could not access to directory " + path);
            }

            if (path.EndsWith(@"\", StringComparison.CurrentCultureIgnoreCase))
            {
                this._filePath = path;
            }
            else
            {
                this._filePath = path + @"\";
            }
        }

        /// <summary>
        /// Flush email content and close.
        /// </summary>
        public void Close()
        {
            string file = this._filePath + ConfigurationManager.AppSettings.Get("MsBuildSummaryEmail");
            try
            {
                StreamWriter writer = File.CreateText(file);
                writer.Write(this._content.ToString());
                writer.Close();
            }
            catch
            {
                throw new FieldAccessException("Unable to access " + file);
            }
        }

        public void Write(string message)
        {
            this._content.Append(message);
        }

        public void WriteLine(string message)
        {
            this._content.AppendLine(message);
        }
    }
}