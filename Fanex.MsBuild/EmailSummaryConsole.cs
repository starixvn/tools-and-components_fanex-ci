// -----------------------------------------------------------------------
// <copyright file="EmailSummaryConsole.cs" company="Nexcel Solutions">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.MsBuild
{
    using System;
    using System.IO;
    using System.Text;

    public class EmailSummaryConsole
    {
        // Fields
        private const string FileName = "MSBUILD.SUMMARY.email";
        private string filePath = string.Empty;
        private StringBuilder content = new StringBuilder();

        // Methods
        public EmailSummaryConsole(string path)
        {
            if (!Directory.Exists(path))
            {
                throw new FieldAccessException("Could not access to directory " + path);
            }

            if (path.EndsWith(@"\"))
            {
                this.filePath = path;
            }
            else
            {
                this.filePath = path + @"\";
            }
        }

        public void Close()
        {
            string file = this.filePath + "MSBUILD.SUMMARY.email";
            try
            {
                StreamWriter writer = File.CreateText(file);
                writer.Write(this.content.ToString());
                writer.Close();
            }
            catch
            {
                throw new FieldAccessException("Unable to access " + file);
            }
        }

        public void Write(string message)
        {
            this.content.Append(message);
        }

        public void WriteLine(string message)
        {
            this.content.AppendLine(message);
        }
    }
}