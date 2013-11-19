// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Nexcel Solutions">
// Qui@20131119.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.MsDeploy
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using CommandLine;
    using CommandLine.Text;

    public class Program
    {
        private static int Main(string[] args)
        {
            var options = new Options();

            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                try
                {
                    IConfigureAppPoolIIS7 config = new MWA();
                    config.ConfigureAppPoolIIS7(options);
                    return 0;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return -1;
                }
            }
            else
            {
                return -1;
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

        [Option("root", Required = true, HelpText = "Root website.")]
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
