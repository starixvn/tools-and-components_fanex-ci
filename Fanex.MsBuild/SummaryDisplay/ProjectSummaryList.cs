namespace Fanex.MsBuild.SummaryDisplay
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    /// <summary>
    /// ProjectBuildSummaryList entities.
    /// </summary>
    [XmlRoot("BuildSummary")]
    [XmlInclude(typeof(ProjectSummary))]
    public class ProjectSummaryList
    {
        public ProjectSummaryList()
        {
            Projects = new List<ProjectSummary>();
        }

        [XmlArray("Projects")]
        [XmlArrayItem("Project", typeof(ProjectSummary))]
        public List<ProjectSummary> Projects { get; set; }

        public void Add(ProjectSummary projectSummary)
        {
            Projects.Add(projectSummary);
        }

        public void Clear()
        {
            Projects.Clear();
        }
    }
}
