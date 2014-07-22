namespace Fanex.MSBuild.Website
{
    using CommandLine;
    using CommandLine.Text;

    /// <summary>
    /// Options class.
    /// </summary>
    public class Options
    {
        [Option("solution", Required = true, HelpText = "Solution that contains website need to build.")]
        public string Solution
        {
            get;
            set;
        }

        [Option("website", Required = true, HelpText = "Website need to build.")]
        public string Website
        {
            get;
            set;
        }

        [Option("refs", HelpText = "Path of reference assemblies which were seperated by semi-colons.")]
        public string References
        {
            get;
            set;
        }

        [Option("root", HelpText = "Root path of solution.")]
        public string Root
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
