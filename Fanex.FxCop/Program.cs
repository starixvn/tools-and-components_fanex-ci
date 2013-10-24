// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Nexcel Solutions">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.FxCop
{
    using System;
    using System.Configuration;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Xml;
    using CommandLine;
    using CommandLine.Text;

    /// <summary>
    /// This program will pick all projects that have output type is "Class Library" then run FXCOP for these output assemblies.
    /// </summary>
    public class Program
    {
        public static Options Options
        {
            get;
            private set;
        }

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

        private static string GetProjectName(string path)
        {
            int start = path.LastIndexOf("\\");
            int end = path.LastIndexOf(".csproj");

            string name = path.Substring(start + 1, end - start - 1);
            return name;
        }

        private static string GetDirectoryPath(string path)
        {
            int start = 0;
            int end = path.LastIndexOf("\\");

            string name = path.Substring(start, end - start);
            return name;
        }

        private static int Main(string[] args)
        {
            Options = new Options();

            try
            {
                if (CommandLine.Parser.Default.ParseArguments(args, Options))
                {
                    Options.RootDirectory = CorrectDirectoryPath(Options.RootDirectory);

                    // Find all csproj whose output type is library
                    string[] projects = Directory.GetFiles(Options.RootDirectory + Options.Name, "*.csproj", SearchOption.AllDirectories);
                    foreach (string project in projects)
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.Load(project);
                        XmlNode root = doc.DocumentElement;

                        XmlNode outputTypeNode = doc.SelectSingleNode("*/*[local-name()='PropertyGroup']/*[local-name()='OutputType']");
                        if (outputTypeNode != null && outputTypeNode.InnerText != "Library")
                        {
                            continue;
                        }

                        XmlNode outputPathNode = doc.SelectSingleNode("*/*[local-name()='PropertyGroup']/*[local-name()='OutputPath']");
                        if (outputPathNode == null || string.IsNullOrEmpty(outputPathNode.InnerText))
                        {
                            continue;
                        }

                        string dir = GetDirectoryPath(project);
                        string output = outputPathNode.InnerText;
                        string projectName = GetProjectName(project);
                        string assemblyPath = dir + "\\" + output;
                        string assemblyName = doc.SelectSingleNode("*/*[local-name()='PropertyGroup']/*[local-name()='AssemblyName']").InnerText + ".dll";
                        string outputName = projectName + ".FxCop.xml";

                        // Run FxCop
                        if (File.Exists(assemblyPath + assemblyName))
                        {
                            RunFxCop(assemblyPath + assemblyName, dir + "\\" + outputName);
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

        private static void RunFxCop(string assembly, string output)
        {
            string path = CorrectDirectoryPath(ConfigurationManager.AppSettings["FxCopPath"]);
            string command = "FxCopCmd.exe";

            string ruleArg = "/r:\"" + path + "Rules\"";

            string refArg = " /d:\"" + CorrectDirectoryPath(ConfigurationManager.AppSettings["GAC_NET40"]) + "\\\"";
            refArg += " /d:\"" + CorrectDirectoryPath(ConfigurationManager.AppSettings["GAC_ASPNETMVC4"]) + "\\\"";
            refArg += " /d:\"" + CorrectDirectoryPath(ConfigurationManager.AppSettings["GAC_ASPNETMVC3"]) + "\\\"";
            refArg += " /d:\"" + CorrectDirectoryPath(ConfigurationManager.AppSettings["GAC_ASPNETMVC2"]) + "\\\"";
            refArg += " /d:\"" + CorrectDirectoryPath(ConfigurationManager.AppSettings["GAC_ASPNET"]) + "\\\"";
            refArg += " /d:\"" + CorrectDirectoryPath(ConfigurationManager.AppSettings["GAC_IDE"]) + "\\\"";

            string fileArg = " /f:\"" + assembly + "\"";

            string rulesetArg = " /ruleset:-\"" + ConfigurationManager.AppSettings["AllRuleset"] + "\"";
            rulesetArg += " /ruleset:+\"" + ConfigurationManager.AppSettings["RecommendedRuleset"] + "\"";

            string outputArg = " /o:\"" + output + "\"";

            string arguments = ruleArg + refArg + fileArg + rulesetArg + outputArg;

            try
            {
                Process process = new Process();
                process.StartInfo.FileName = path + command;
                process.StartInfo.Arguments = arguments;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.UseShellExecute = false;
                process.Start();

                Console.WriteLine(process.StandardOutput.ReadToEnd());
                process.WaitForExit();
            }
            catch
            {
            }
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

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
