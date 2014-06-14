namespace Fanex.MsBuild.SummaryDisplay
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
    }
}
