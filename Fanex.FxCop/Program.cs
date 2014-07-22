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

    /// <summary>
    /// This program will pick all projects that have output type is "Class Library" then run FXCOP for these output assemblies.
    /// </summary>
    public static class Program
    {
        private static CodeAnalysisConfiguration _solutionCodeAnalysisConfiguration;

        private static CodeAnalysisHelper _codeAnalysisHelper;

        private static Options _options;

        /// <summary>
        /// Program entry point.
        /// </summary>
        /// <param name="args">Input argument.</param>
        /// <returns>Return error code.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed.")]
        private static int Main(string[] args)
        {
            _codeAnalysisHelper = new CodeAnalysisHelper
            {
                RuleSetsDirectory = DirectoryHelper.CorrectDirectoryPath(ConfigurationManager.AppSettings["RuleSetsDirectory"])
            };
            _options = new Options();

            try
            {
                if (!CommandLine.Parser.Default.ParseArguments(args, _options))
                {
                    return 0;
                }

                _options.RootDirectory = DirectoryHelper.CorrectDirectoryPath(_options.RootDirectory);

                DetectMasterCodeAnalysis(_options.RootDirectory + _options.Name);

                // Find all csproj whose output type is library
                var projects = Directory.GetFiles(_options.RootDirectory + _options.Name, "*.csproj", SearchOption.AllDirectories);
                foreach (string project in projects)
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(project);

                    XmlNode outputTypeNode = doc.SelectSingleNode("*/*[local-name()='PropertyGroup']/*[local-name()='OutputType']");
                    if (outputTypeNode == null)
                    {
                        continue;
                    }

                    var outputType = outputTypeNode.InnerText;

                    XmlNode outputPathNode = doc.SelectSingleNode("*/*[local-name()='PropertyGroup']/*[local-name()='OutputPath']");
                    if (outputPathNode == null || string.IsNullOrEmpty(outputPathNode.InnerText))
                    {
                        continue;
                    }

                    string dir = DirectoryHelper.GetDirectoryPath(project);
                    string output = outputPathNode.InnerText;
                    string projectName = DirectoryHelper.GetProjectName(project);
                    string assemblyPath = dir + "\\" + output;
                    string assemblyName = doc.SelectSingleNode("*/*[local-name()='PropertyGroup']/*[local-name()='AssemblyName']").InnerText
                        + (outputType == "Exe" ? ".exe" : ".dll");
                    string outputName = projectName + ".FxCop.xml";

                    // Run FxCop
                    if (File.Exists(assemblyPath + assemblyName))
                    {
                        RunFxCop(dir, assemblyPath + assemblyName, dir + "\\" + outputName);
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

        /// <summary>
        /// Runs the FX cop.
        /// </summary>
        /// <param name="projectDirectory">The project directory.</param>
        /// <param name="assembly">The assembly.</param>
        /// <param name="output">The output.</param>
        private static void RunFxCop(string projectDirectory, string assembly, string output)
        {
            var codeAnalysisConfiguration = _codeAnalysisHelper.GetConfiguration(projectDirectory, _solutionCodeAnalysisConfiguration);

            var path = DirectoryHelper.CorrectDirectoryPath(ConfigurationManager.AppSettings["FxCopDirectory"]);
            string command = "FxCopCmd.exe";

            var defaultArg = "/ignoregeneratedcode";
            var ruleArg = " /r:\"" + path + "Rules\"";

            var refArg = " /d:\"" + DirectoryHelper.CorrectDirectoryPath(ConfigurationManager.AppSettings["GAC_NET40"]) + "\\\"";
            refArg += " /d:\"" + DirectoryHelper.CorrectDirectoryPath(ConfigurationManager.AppSettings["GAC_ASPNETMVC4"]) + "\\\"";
            refArg += " /d:\"" + DirectoryHelper.CorrectDirectoryPath(ConfigurationManager.AppSettings["GAC_ASPNETMVC3"]) + "\\\"";
            refArg += " /d:\"" + DirectoryHelper.CorrectDirectoryPath(ConfigurationManager.AppSettings["GAC_ASPNETMVC2"]) + "\\\"";
            refArg += " /d:\"" + DirectoryHelper.CorrectDirectoryPath(ConfigurationManager.AppSettings["GAC_ASPNET"]) + "\\\"";
            refArg += " /d:\"" + DirectoryHelper.CorrectDirectoryPath(ConfigurationManager.AppSettings["GAC_IDE"]) + "\\\"";

            var fileArg = " /f:\"" + assembly + "\"";

            var rulesetArg = " /ruleset:-\"" + ConfigurationManager.AppSettings["AllRuleset"] + "\"";
            rulesetArg += " /ruleset:+\"" + codeAnalysisConfiguration.RuleSet + "\"";

            var dictionaryArg = " /dictionary:\"" + codeAnalysisConfiguration.Dictionary + "\"";
            var outputArg = " /o:\"" + output + "\"";

            var arguments = defaultArg + ruleArg + refArg + fileArg + rulesetArg + dictionaryArg + outputArg;

            try
            {
                Process process = new Process();
                process.StartInfo.FileName = path + command;
                process.StartInfo.Arguments = arguments;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.UseShellExecute = false;
                process.Start();

                var standardOutput = process.StandardOutput.ReadToEnd();
                Console.WriteLine(standardOutput);
                process.WaitForExit();
            }
            catch
            {
            }
        }

        /// <summary>
        /// Detects the master code analysis.
        /// </summary>
        /// <param name="rootDirectory">The root directory.</param>
        private static void DetectMasterCodeAnalysis(string rootDirectory)
        {
            _solutionCodeAnalysisConfiguration = new CodeAnalysisConfiguration
            {
                RuleSet = ConfigurationManager.AppSettings["RecommendedRuleset"],
                Dictionary = ConfigurationManager.AppSettings["RecommendedDictionary"]
            };

            _solutionCodeAnalysisConfiguration = _codeAnalysisHelper.GetConfiguration(
                rootDirectory,
                _solutionCodeAnalysisConfiguration);
        }
    }
}
