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
    using System.Collections.Generic;

    /// <summary>
    /// This program will pick all projects that have output type is "Class Library" then run FXCOP for these output assemblies.
    /// </summary>
    public static class Program
    {
        private static CodeAnalysisConfiguration _defaultCodeAnalysisConfiguration;

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

                InitializeDefaultConfiguration(_options.RootDirectory + _options.Name);

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
                        RunFxCop(project, assemblyPath + assemblyName, dir + "\\" + outputName);
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

        private static void RunFxCop(string projectFile, string assembly, string output)
        {
            var codeAnalysisConfiguration = _codeAnalysisHelper.GetConfiguration(projectFile, _defaultCodeAnalysisConfiguration);

            var path = DirectoryHelper.CorrectDirectoryPath(ConfigurationManager.AppSettings["FxCopDirectory"]);
            string command = "FxCopCmd.exe";

            var defaultArg = "/ignoregeneratedcode";
            var ruleArg = " /r:\"" + path + "Rules\"";

            var refArg = " /d:\"" + ConfigurationManager.AppSettings["GAC_NET40"] + "\"";
            refArg += " /d:\"" + ConfigurationManager.AppSettings["GAC_ASPNETMVC4"] + "\"";
            refArg += " /d:\"" + ConfigurationManager.AppSettings["GAC_ASPNETMVC3"] + "\"";
            refArg += " /d:\"" + ConfigurationManager.AppSettings["GAC_ASPNETMVC2"] + "\"";
            refArg += " /d:\"" + ConfigurationManager.AppSettings["GAC_ASPNET"] + "\"";
            refArg += " /d:\"" + ConfigurationManager.AppSettings["GAC_IDE"] + "\"";

            var fileArg = " /f:\"" + assembly + "\"";

            var rulesetArg = " /ruleset:-\"" + ConfigurationManager.AppSettings["AllRuleset"] + "\"";
            foreach (var rule in codeAnalysisConfiguration.RuleSet)
            {
                rulesetArg += " /ruleset:+\"" + rule + "\"";
            }

            var dictionaryArg = "";
            foreach (var dictionary in codeAnalysisConfiguration.Dictionary)
            {
                dictionaryArg += " /dictionary:\"" + dictionary + "\"";
            }

            var outputArg = " /o:\"" + output + "\"";

            var arguments = defaultArg + ruleArg + refArg + fileArg + rulesetArg + dictionaryArg + outputArg;
            Console.WriteLine("Execute FXCOP: FxCopCmd.exe " + arguments);
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
        private static void InitializeDefaultConfiguration(string rootDirectory)
        {
            _defaultCodeAnalysisConfiguration = new CodeAnalysisConfiguration
            {
                RuleSet = new List<string> { ConfigurationManager.AppSettings["RecommendedRuleset"] },
                Dictionary = new List<string> { ConfigurationManager.AppSettings["RecommendedDictionary"] }
            };
        }
    }
}
