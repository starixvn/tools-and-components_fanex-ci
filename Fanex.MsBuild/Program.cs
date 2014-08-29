// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Nexcel Solutions">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.MSBuild
{
    using System;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Text;
    using CommandLine;
    using CommandLine.Text;
    using Fanex.MSBuild.SummaryDisplay;

    /// <summary>
    /// This program will generate html report for MSBuild and StyleCop.
    /// </summary>
    public static class Program
    {
        private const string ReportName = "MSBUILD.REPORT";
        private const string JsHintReportName = "JSHINT.REPORT.htm";

        public static EmailSummaryConsole EmailConsole
        {
            get;
            private set;
        }

        public static SummaryDisplayConsole SummaryDisplayConsole
        {
            get;
            private set;
        }

        public static Options Options
        {
            get;
            private set;
        }

        public static string OutputDir
        {
            get;
            private set;
        }

        public static string Replace(string text, string oldText, string newText)
        {
            string output = text;

            output = output.Replace(oldText, newText);

            oldText = oldText.Substring(0, 1).ToLower() + oldText.Substring(1);
            output = output.Replace(oldText, newText);

            return output;
        }

        public static void ReplaceInFile(string file, string search, string replace)
        {
            string output = File.ReadAllText(file);
            string newContent = Replace(output, search, replace);

            StreamWriter writer = File.CreateText(file);
            writer.Write(newContent);
            writer.Close();
        }

        /// <summary>
        /// Correct directory path.
        /// </summary>
        /// <param name="path">Input path.</param>
        /// <returns>Return corrected path.</returns>
        private static string CorrectDirectoryPath(string path)
        {
            string result = path;

            // Solving parsing error when string end with \
            if (result.EndsWith("\"", StringComparison.CurrentCultureIgnoreCase))
            {
                result = path.Remove(result.Length - 1, 1);
            }

            if (!result.EndsWith("\\", StringComparison.CurrentCultureIgnoreCase))
            {
                result += "\\";
            }

            return result;
        }

        /// <summary>
        /// Program entry point.
        /// </summary>
        /// <param name="args">Input arguments.</param>
        /// <returns>Return error code.</returns>
        private static int Main(string[] args)
        {
            Options = new Options();
            Environment.SetEnvironmentVariable("ERRORLEVEL", "0", EnvironmentVariableTarget.Process);

            try
            {
                if (CommandLine.Parser.Default.ParseArguments(args, Options))
                {
                    Options.RootDirectory = CorrectDirectoryPath(Options.RootDirectory);

                    EmailConsole = new EmailSummaryConsole(Options.RootDirectory + Options.Name + "\\");
                    SummaryDisplayConsole = new SummaryDisplayConsole();
                    SummaryDisplayConsole.RemoveXml(Options.RootDirectory + Options.Name + "\\");

                    MSBuildXmlTransform buildTransform = new MSBuildXmlTransform();
                    string output = buildTransform.Transform(Options.RootDirectory + Options.Name + "\\" + ConfigurationManager.AppSettings["MsBuildXml"]);

                    // Remove root directory C:\Program Files\Jenkins\workspace\
                    output = Replace(output, Options.RootDirectory, string.Empty);
                    OutputDir = Options.RootDirectory + Options.Name + "\\cireport\\";
                    
                    // Check JSHint Result
                    if (Program.Options.JsHintAsError == "true")
                    {
                        JsHintResultHandler jscriptHintHandler = new JsHintResultHandler(Program.OutputDir + "\\" + JsHintReportName);
                    }

                    // Latest report
                    File.WriteAllText(OutputDir + ReportName + ".htm", output, Encoding.UTF8);

                    EmailConsole.Close();
                    SummaryDisplayConsole.FlushXml(Options.RootDirectory + Options.Name + "\\");
                    return ShowResult();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR.");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                return -1;
            }

            return 0;
        }

        /// <summary>
        /// Show result information.
        /// </summary>
        /// <returns>Error Information.</returns>
        private static int ShowResult()
        {
            int error = 0;
            error = int.TryParse(Environment.GetEnvironmentVariable("ERRORLEVEL", EnvironmentVariableTarget.Process), out error)
                ? error
                : 0;

            if (error > 0)
            {
                Console.WriteLine("FAILED. Error Code:" + error);
            }
            else
            {
                Console.WriteLine("SUCCESS.");
            }

            return error;
        }
    }
}