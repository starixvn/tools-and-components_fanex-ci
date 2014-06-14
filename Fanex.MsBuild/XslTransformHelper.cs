// -----------------------------------------------------------------------
// <copyright file="XslTransformHelper.cs" company="Nexcel Solutions">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.MsBuild
{
    using System;
    using System.Configuration;
    using System.IO;
    using System.Text.RegularExpressions;
    using Fanex.MsBuild.SummaryDisplay;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class XslTransformHelper
    {
        private static int numberOfFxCopViolations = 0;
        private static int numberOfStyleCopViolations = 0;
        private static int numberOfWarnings = 0;
        private static int numberOfErrors = 0;
        private static string projectName = string.Empty;

        public static string GetCurrentDateTime()
        {
            return Environment.GetEnvironmentVariable("BUILD_ID");
        }

        public static string GetBranchName()
        {
            return Environment.GetEnvironmentVariable("GIT_BRANCH");
        }

        public static string GetSolutionName(string path)
        {
            int start = path.LastIndexOf("\\");
            int end = path.LastIndexOf(".sln");

            string name = path.Substring(start + 1, end - start - 1);
            return name;
        }

        public string GetProjectName(string path)
        {
            int start = path.LastIndexOf("\\");
            int end = path.LastIndexOf(".csproj");

            string name = path.Substring(start + 1, end - start - 1);
            return name;
        }

        public string GetDirectoryPath(string path)
        {
            int start = 0;
            int end = path.LastIndexOf("\\");

            string name = path.Substring(start, end - start);
            return name;
        }

        public string ResolveSourceFileLink(string path, string file)
        {
            string search = Program.Options.RootDirectory + Program.Options.Name;
            string replace = "/job/" + Program.Options.Name + "/ws";
            string output = path;

            output = Program.Replace(path, search, replace);
            output = output.Replace("\\", "/");

            return output + "/" + file;
        }

        public string GenerateGuid()
        {
            return Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Get style cop report.
        /// </summary>
        /// <param name="path">Input path.</param>
        /// <returns>Return transformed style cop report.</returns>
        public string GetStyleCopReport(string path)
        {
            string reportPath = path.Replace(".csproj", ConfigurationManager.AppSettings["StyleCopOutPutFile"]);

            if (!File.Exists(reportPath))
            {
                return string.Empty;
            }

            StyleCopXmlTransform styleCopTransform = new StyleCopXmlTransform();
            return styleCopTransform.Transform(reportPath);
        }

        /// <summary>
        /// Get FX cop report.
        /// </summary>
        /// <param name="path">Input path.</param>
        /// <returns>Return transformed FX cop report.</returns>
        public string GetFxCopReport(string path)
        {
            string reportPath = path.Replace(".csproj", ConfigurationManager.AppSettings["FxCopOutPutFile"]);

            if (!File.Exists(reportPath))
            {
                return string.Empty;
            }

            FxCopXmlTransform transform = new FxCopXmlTransform();
            return transform.Transform(reportPath);
        }

        public string EmailConsoleProjectName(string path)
        {
            projectName = this.GetProjectName(path);
            return string.Empty;
        }

        public string IncreaseError(int number)
        {
            numberOfErrors += number;
            return string.Empty;
        }

        public string IncreaseWarning(int number)
        {
            numberOfWarnings += number;
            return string.Empty;
        }

        public string IncreaseStyleCopViolation(int number)
        {
            numberOfStyleCopViolations += number;
            return string.Empty;
        }

        public string IncreaseFxCopViolation(int number)
        {
            numberOfFxCopViolations += number;
            return string.Empty;
        }

        /// <summary>
        /// Prepare email content.
        /// </summary>
        /// <returns>Return email content.</returns>
        public string EmailConsoleSummary()
        {
            var message = "<TR><TD>" + projectName + "</TD>";
            message += "<TD>" + this.FormatColor(numberOfErrors, string.Empty) + "</TD>";
            message += "<TD>" + this.FormatColor(numberOfWarnings, string.Empty) + "</TD>";
            message += "<TD>" + this.FormatColor(numberOfStyleCopViolations, string.Empty) + "</TD>";
            message += "<TD>" + this.FormatColor(numberOfFxCopViolations, string.Empty) + "</TD>";

            Program.EmailConsole.WriteLine(message);
            Program.EmailConsole.WriteLine("</TR>");

            var exitCode = numberOfErrors;
            if (Program.Options.StyleCopAsError != "false")
            {
                exitCode += numberOfStyleCopViolations;
            }

            if (Program.Options.FxCopAsError != "false")
            {
                exitCode += numberOfFxCopViolations;
            }

            if (Program.Options.WarningAsError != "false")
            {
                exitCode += numberOfWarnings;
            }

            if (exitCode > 0)
            {
                Environment.SetEnvironmentVariable("ERRORLEVEL", exitCode.ToString(), EnvironmentVariableTarget.Process);
            }

            Program.EmailConsole.WriteLine("</TR>");

            var projectSummary = new ProjectSummary
            {
                Name = projectName,
                Error = numberOfErrors,
                Warning = numberOfWarnings,
                FxCop = numberOfFxCopViolations,
                StyleCop = numberOfStyleCopViolations
            };
            Program.SummaryDisplayConsole.AddProjectSummary(projectSummary);

            numberOfErrors = 0;
            numberOfWarnings = 0;
            numberOfStyleCopViolations = 0;
            numberOfFxCopViolations = 0;

            return string.Empty;
        }

        private string FormatColor(int number, string suffix)
        {
            if (number > 0)
            {
                return "<font color='red'>" + number + " " + suffix + "</font>";
            }
            else
            {
                return "<font color='blue'>0 " + suffix + "</font>";
            }
        }
    }
}
