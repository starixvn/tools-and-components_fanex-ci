namespace Fanex.FxCop
{
    using System.Collections.Generic;

    /// <summary>
    /// CodeAnalysisConfiguration class.
    /// </summary>
    public class CodeAnalysisConfiguration
    {
        public List<string> RuleSet { get; set; }

        public List<string> Dictionary { get; set; }
    }
}
