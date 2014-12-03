namespace Fanex.MSBuild.SummaryDisplay
{
    using System;
    using System.Xml.Serialization;

    /// <summary>
    /// ProjectBuildSummary entities.
    /// </summary>
    [Serializable]
    [XmlRoot("Project")]
    public class ProjectSummary
    {
        public string Name { get; set; }

        public int Error { get; set; }

        public int Warning { get; set; }

        public int StyleCop { get; set; }

        public int FxCop { get; set; }

        public int ErrorCount
        {
            get
            {
                return Error + Warning + FxCop + StyleCop;
            }
        }

        public override string ToString()
        {
            return string.Format(
                "{0}.\t{1} errors.\t{2} warnings.\t{3} stylecop errors.\t{4} fxcopy errors",
                Name,
                Error,
                Warning,
                StyleCop,
                FxCop);
        }
    }
}
