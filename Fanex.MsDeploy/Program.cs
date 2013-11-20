// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Nexcel Solutions">
// Qui@20131119.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.MsDeploy
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using CommandLine;
    using CommandLine.Text;

    public class Program
    {
        private const string _configPy = "/_AutomationTest/config.py";
        private const string _runTest = "/_AutomationTest/run.bat";

        private static int Main(string[] args)
        {
            var options = new Options();

            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                // Set up website
                try
                {
                    IConfigureAppPoolIIS7 config = new ADSI();
                    config.ConfigureAppPoolIIS7(options);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return -1;
                }

                // Config test url to /_AutomationTest/config.py -> Url=
                RunTest(options.Application);

                return 0;
            }
            else
            {
                return -1;
            }
        }

        private static void RunTest(string application)
        {
            string workspace = Environment.GetEnvironmentVariable("WORKSPACE");
            string testbase = Environment.GetEnvironmentVariable("CI_PUBLISH_URL");

            string file = workspace + _configPy;

            if (File.Exists(file))
            {
                string newConfig = string.Empty;
                string[] configs = File.ReadAllLines(file);
                foreach (string line in configs)
                {
                    if (line.StartsWith("Url=") || line.StartsWith("Url ="))
                    {
                        newConfig += "Url='" + testbase + application + "'\r\n";
                    }
                    else
                    {
                        newConfig += line + "\r\n";
                    }
                }

                File.WriteAllText(file, newConfig);
            }
        }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed.")]
    public class Options
    {
        [Option("host", Required = true, HelpText = "Host machine.")]
        public string Host
        {
            get;
            set;
        }

        [Option("root", Required = true, HelpText = "Root website or site id.")]
        public string RootWebsite
        {
            get;
            set;
        }

        [Option("app", Required = true, HelpText = "Application name.")]
        public string Application
        {
            get;
            set;
        }

        [Option("pool", Required = true, HelpText = "Application pool name.")]
        public string Pool
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
