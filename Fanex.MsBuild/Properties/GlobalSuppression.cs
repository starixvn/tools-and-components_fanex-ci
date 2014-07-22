using System.Diagnostics.CodeAnalysis;

[module: SuppressMessage(
    "Microsoft.Naming", 
    "CA1709:IdentifiersShouldBeCasedCorrectly", 
    MessageId = "ci-msbuild-report",
    Justification = "We would like to set the name of execute file as lower case.")]

[module: SuppressMessage(
    "Microsoft.Design", 
    "CA1024:UsePropertiesWhereAppropriate", 
    Scope = "member", 
    Target = "Fanex.MSBuild.Options.#GetUsage()",
    Justification = "We would like to use HelpOptions.")]

[module: SuppressMessage(
    "Microsoft.Design", 
    "CA1031:DoNotCatchGeneralExceptionTypes",
    Scope = "member", 
    Target = "Fanex.MSBuild.JsHintResultHandler.#ReadResultFromJSHintReport()",
    Justification = "We would like catch all exception and return exit code = -1.")]

[module: SuppressMessage(
    "Microsoft.Design", 
    "CA1031:DoNotCatchGeneralExceptionTypes", 
    Scope = "member",
    Target = "Fanex.MSBuild.XmlTransformCore.#Transform()",
    Justification = "We would like catch all exception and return exit code = -1.")]
