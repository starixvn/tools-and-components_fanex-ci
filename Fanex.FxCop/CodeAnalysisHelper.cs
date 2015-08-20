namespace Fanex.FxCop
{
    using System.Linq;
    using System.IO;
    using System.Xml;
    using System.Collections.Generic;

    /// <summary>
    /// CodeAnalysisHelper class.
    /// </summary>
    public class CodeAnalysisHelper
    {
        public string RuleSetsDirectory { get; set; }

        public CodeAnalysisConfiguration GetConfiguration(string projectFile, CodeAnalysisConfiguration defaultConfiguration)
        {
            var configuration = new CodeAnalysisConfiguration();

            var document = new XmlDocument();
            using (var reader = new XmlTextReader(projectFile))
            {
                document.Load(reader);
                reader.Close();
            }

            var dictionary = GetDictionary(document);
            var ruleSet = GetRuleSet(document);

            var projectPath = Path.GetDirectoryName(projectFile);
            dictionary = dictionary.Select(d => Path.Combine(projectPath, d)).Select(d => Path.GetFullPath(d)).ToList();
            ruleSet = ruleSet.Select(r => Path.Combine(projectPath, r)).Select(d => Path.GetFullPath(d)).ToList();

            dictionary.AddRange(defaultConfiguration.Dictionary);
            ruleSet.AddRange(defaultConfiguration.RuleSet);

            configuration.RuleSet = ruleSet.Distinct().ToList();
            configuration.Dictionary = dictionary.Distinct().ToList();

            return configuration;
        }

        /// <summary>
        /// Gets the rule set.
        /// </summary>
        /// <param name="xmlDocument">The XML document.</param>
        /// <returns>Rule set path.</returns>
        private List<string> GetRuleSet(XmlDocument xmlDocument)
        {
            var nodeList = xmlDocument.GetElementsByTagName("CodeAnalysisRuleSet");
            return nodeList.Cast<XmlNode>().Select(n => n.InnerText).ToList();
        }

        private List<string> GetDictionary(XmlDocument xmlDocument)
        {
            var nodeList = xmlDocument.GetElementsByTagName("CodeAnalysisDictionary");

            return nodeList.Cast<XmlNode>().Select(n => n.Attributes["Include"].Value).ToList();
        }
    }
}
