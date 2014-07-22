namespace Fanex.Atomiq
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
