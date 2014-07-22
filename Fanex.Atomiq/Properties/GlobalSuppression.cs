using System.Diagnostics.CodeAnalysis;

[module: SuppressMessage(
    "Microsoft.Naming",
    "CA1709:IdentifiersShouldBeCasedCorrectly",
    MessageId = "ci-dry-report",
    Justification = "We would like to set the name of execute file as lower case.")]

[module: SuppressMessage(
    "Microsoft.Design",
    "CA1024:UsePropertiesWhereAppropriate",
    Scope = "member",
    Target = "Fanex.Atomiq.Options.#GetUsage()",
    Justification = "We would like to use HelpOptions.")]