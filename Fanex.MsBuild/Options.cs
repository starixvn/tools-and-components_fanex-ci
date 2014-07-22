namespace Fanex.MSBuild
{
    using CommandLine;
    using CommandLine.Text;

    /// <summary>
    /// Options class.
    /// </summary>
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
