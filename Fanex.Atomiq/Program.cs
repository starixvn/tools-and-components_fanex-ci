// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Nexcel Solutions">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Atomiq
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using CommandLine;
    using CommandLine.Text;

    /// <summary>
    /// This program will truncate and transform html report from ATOMIQ.
    /// </summary>
    public class Program
    {
        private const string ReportName = "DRY.REPORT";

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

        public static string GetCurrentDateTime()
        {
            return Environment.GetEnvironmentVariable("BUILD_ID");
        }

        public static string GetBranchName()
        {
            return Environment.GetEnvironmentVariable("GIT_BRANCH");
        }

        private static string InjectAtomiqDownload(string content)
        {
            string start = "<h3>Project Options</h3>";
            string end = "<h3>Build Constraints</h3>";

            string header = "<h3>Build ID - " + GetCurrentDateTime() + "</h3>" +
                "<h3>Branch - " + GetBranchName() + "</h3>";
            string newSection = header + "For more visualization please download <a href=\"http://ci.nexcel.vn/userContent/Atomiq_1.1.3.94.exe\">Atomiq_1.1.3.94</a> or <a href=\"http://getatomiq.com/\">http://getatomiq.com/</a>";

            int from = content.IndexOf(start);
            int to = content.IndexOf(end, from);

            string oldSection = content.Substring(from, to - from);

            string newContent = content.Replace(oldSection, newSection);
            return newContent;
        }

        private static int Main(string[] args)
        {
            var options = new Options();

            try
            {
                if (CommandLine.Parser.Default.ParseArguments(args, options))
                {
                    options.RootDirectory = CorrectDirectoryPath(options.RootDirectory);
                    bool duplicateAsError = true;

                    if (!string.IsNullOrEmpty(options.DuplicateAsError))
                    {
                        bool.TryParse(options.DuplicateAsError, out duplicateAsError);
                    }

                    string outputDir = options.RootDirectory + options.Name + "\\cireport\\";

                    // Check exist
                    string file = outputDir + ReportName + ".htm";
                    if (File.Exists(file))
                    {
                        if (File.ReadAllText(file).StartsWith("System.InvalidOperationException: Sequence contains no elements"))
                        {
                            // Copy default file
                            StreamWriter writer = File.CreateText(file);
                            string text = File.ReadAllText(options.RootDirectory + ReportName + ".htm");
                            writer.BaseStream.Seek(0, SeekOrigin.Begin);
                            writer.Write(text);
                            writer.Close();
                        }
                        else
                        {
                            string text = File.ReadAllText(file);
                            text = InjectAtomiqDownload(text);
                            StreamWriter writer = File.CreateText(file);
                            writer.BaseStream.Seek(0, SeekOrigin.Begin);
                            writer.Write(text);
                            writer.Close();
                        }
                    }
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
    }

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

        [Option("duplicateaserror", Required = false, HelpText = "Treat duplicated code as error.")]
        public string DuplicateAsError
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
