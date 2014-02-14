// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Nexcel Solutions">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.MsBuild
{
    using System;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Text;
    using CommandLine;
    using CommandLine.Text;

    /// <summary>
    /// This program will generate html report for MSBuild and StyleCop.
    /// </summary>
    public class Program
    {
        private const string ReportName = "MSBUILD.REPORT";
        private const string JsHintReportName = "JSHINT.REPORT.htm";

        public static EmailSummaryConsole EmailConsole
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

        public static string Replace(string text, string search, string replace)
        {
            string output = text;

            output = output.Replace(search, replace);

            search = search.Substring(0, 1).ToLower() + search.Substring(1);
            output = output.Replace(search, replace);

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
            if (result.EndsWith("\""))
            {
                result = path.Remove(result.Length - 1, 1);
            }

            if (!result.EndsWith("\\"))
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
            
            // For Email JSHint report
            Environment.SetEnvironmentVariable("JSHINT_BUILD_FAILED_" + Options.Name, "0", EnvironmentVariableTarget.User);
            try
            {
                if (CommandLine.Parser.Default.ParseArguments(args, Options))
                {
                    Options.RootDirectory = CorrectDirectoryPath(Options.RootDirectory);

                    EmailConsole = new EmailSummaryConsole(Options.RootDirectory + Options.Name + "\\");

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
            int.TryParse(Environment.GetEnvironmentVariable("ERRORLEVEL", EnvironmentVariableTarget.Process), out error);

            if (error > 0)
            {
                Console.WriteLine("FAILED.");
            }
            else
            {
                Console.WriteLine("SUCCESS.");
            }

            return error;
        }
    }

    /// <summary>
    /// Command line option.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed.")]
    public class Options
    {
        [Option("root", Required = true, HelpText = "Root workspace.")]
        public string RootDirectory
        {
            get;
            set;
        }

        [Option("name", Required = true, HelpText = "Job name.")]
        public string Name
        {
            get;
            set;
        }

        [Option("buildid", Required = true, HelpText = "Build id.")]
        public string BuildId
        {
            get;
            set;
        }

        [Option("stylecopaserror", Required = false, HelpText = "Treat StyleCop warning as error with MSBuild.")]
        public string StyleCopAsError
        {
            get;
            set;
        }

        [Option("fxcopaserror", Required = false, HelpText = "Treat FxCop warning as error with MSBuild.")]
        public string FxCopAsError
        {
            get;
            set;
        }

        [Option("warningaserror", Required = false, HelpText = "Treat warning as error with MSBuild.")]
        public string WarningAsError
        {
            get;
            set;
        }

        [Option("jshintaserror", Required = false, HelpText = "Treat jshint warning as error with MSBuild.")]
        public string JsHintAsError
        {
            get;
            set;
        }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}