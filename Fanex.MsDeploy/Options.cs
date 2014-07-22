namespace Fanex.MSDeploy
{
    using CommandLine;
    using CommandLine.Text;

    /// <summary>
    /// Options class.
    /// </summary>
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
