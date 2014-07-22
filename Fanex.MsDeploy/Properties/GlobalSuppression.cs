using System.Diagnostics.CodeAnalysis;

[module: SuppressMessage(
    "Microsoft.Security",
    "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands",
    Scope = "member",
    Target = "Fanex.MSDeploy.ADSI.#SetBinaryProperty(System.DirectoryServices.DirectoryEntry,System.String,System.String)",
    Justification = "We don't care about security code here")]

[module: SuppressMessage(
    "Microsoft.Security",
    "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands",
    Scope = "member",
    Target = "Fanex.MSDeploy.ADSI.#ConfigureAppPoolIIS7(Fanex.MSDeploy.Options)",
    Justification = "We don't care about security code here")]

[module: SuppressMessage(
    "Microsoft.Naming",
    "CA1704:IdentifiersShouldBeSpelledCorrectly",
    MessageId = "ci-msdeploy",
    Justification = "We would like to set the name of execute file as lower case.")]

[module: SuppressMessage(
    "Microsoft.Design",
    "CA1031:DoNotCatchGeneralExceptionTypes",
    Scope = "member",
    Target = "Fanex.MSDeploy.ADSI.#SetBinaryProperty(System.DirectoryServices.DirectoryEntry,System.String,System.String)",
    Justification = "We would like catch all exception and return exit code = -1.")]

[module: SuppressMessage(
    "Microsoft.Design",
    "CA1031:DoNotCatchGeneralExceptionTypes",
    Scope = "member",
    Target = "Fanex.MSDeploy.ADSI.#ConfigureAppPoolIIS7(Fanex.MSDeploy.Options)",
    Justification = "We would like catch all exception and return exit code = -1.")]

[module: SuppressMessage(
    "Microsoft.Design",
    "CA1024:UsePropertiesWhereAppropriate",
    Scope = "member",
    Target = "Fanex.MSDeploy.Options.#GetUsage()",
    Justification = "We would like to use HelpOptions.")]

[module: SuppressMessage(
    "Microsoft.Naming",
    "CA1709:IdentifiersShouldBeCasedCorrectly",
    MessageId = "ci-msdeploy",
    Justification = "We would like to set the name of execute file as lower case.")]
