namespace Fanex.FxCop
{
    using System.IO;
    using System.Xml;

    /// <summary>
    /// CodeAnalysisHelper class.
    /// </summary>
    public class CodeAnalysisHelper
    {
        public string RuleSetsDirectory { get; set; }

        public CodeAnalysisConfiguration GetConfiguration(string directory, CodeAnalysisConfiguration defaultConfiguration)
        {
            var configuration = new CodeAnalysisConfiguration();
            var dictionary = GetDictionary(directory);
            var ruleSet = GetRuleSet(directory);

            configuration.RuleSet = string.IsNullOrWhiteSpace(ruleSet) ? defaultConfiguration.RuleSet : ruleSet;
            configuration.Dictionary = string.IsNullOrWhiteSpace(dictionary) ? defaultConfiguration.Dictionary : dictionary;

            return configuration;
        }

        /// <summary>
        /// Gets the rule set.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <returns>Returns rule set file path.</returns>
        private string GetRuleSet(string directory)
        {
            var codeAnalysisDirectory = Path.Combine(directory, "CodeAnalysis");
            if (!Directory.Exists(codeAnalysisDirectory))
            {
                return string.Empty;
            }

            var configurationFiles = Directory.GetFiles(codeAnalysisDirectory, "CodeAnalysisConfiguration.xml");
            if (configurationFiles.Length != 1)
            {
                return string.Empty;
            }

            var document = new XmlDocument();
            using (var reader = new XmlTextReader(configurationFiles[0]))
            {
                document.Load(reader);
                reader.Close();
            }

            return GetRuleSet(document);
        }

        /// <summary>
        /// Gets the rule set.
        /// </summary>
        /// <param name="xmlDocument">The XML document.</param>
        /// <returns>Rule set path.</returns>
        private string GetRuleSet(XmlDocument xmlDocument)
        {
            var root = xmlDocument.DocumentElement;
            var ruleSetNode = root.SelectSingleNode("/CodeAnalysisConfiguration/RuleSet");
            var valueAttribute = ruleSetNode.Attributes["Value"];
            if (valueAttribute == null || string.IsNullOrWhiteSpace(valueAttribute.Value))
            {
                return string.Empty;
            }

            var value = valueAttribute.Value;

            var ruleSetFileName = value.Replace("Bundled;", string.Empty).Trim();

            return string.IsNullOrWhiteSpace(ruleSetFileName)
                ? string.Empty
                : Path.Combine(RuleSetsDirectory, ruleSetFileName);
        }

        private string GetDictionary(string directory)
        {
            var codeAnalysisDirectory = Path.Combine(directory, "CodeAnalysis");
            if (!Directory.Exists(codeAnalysisDirectory))
            {
                return string.Empty;
            }

            var dictionaryFiles = Directory.GetFiles(codeAnalysisDirectory, "FxCopDictionary.xml");
            return dictionaryFiles.Length == 1 ? dictionaryFiles[0] : string.Empty;
        }
    }
}
