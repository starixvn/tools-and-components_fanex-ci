// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Nexcel Solutions">
// Qui@20131119.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.MSBuild.Website
{
    using System;
    using System.Collections;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using CommandLine;
    using CommandLine.Text;

    /// <summary>
    /// This program find all dependencies from output binaries and copy to \Bin\ folder.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Program entry point.
        /// </summary>
        /// <param name="args">Input arguments.</param>
        /// <returns>Return error code.</returns>
        private static int Main(string[] args)
        {
            var options = new Options();

            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                try
                {
                    Solution solution = new Solution(options.Solution);
                    ArrayList projectReferences = null;
                    string workspace = string.IsNullOrEmpty(options.Root) ? Environment.GetEnvironmentVariable("WORKSPACE") : options.Root;
                    SolutionProject website = null;

                    foreach (SolutionProject project in solution.Projects)
                    {
                        if (project.ProjectName == options.Website)
                        {
                            projectReferences = project.ProjectReferences;
                            website = project;
                        }
                    }

                    // Copy assemblies
                    foreach (SolutionProject project in solution.Projects)
                    {
                        if (projectReferences.Contains(project.ProjectGuid))
                        {
                            CopyBinaries(workspace + "\\" + project.RelativePath, workspace + "\\" + website.RelativePath + "Bin");
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return -1;
                }

                return 0;
            }

            return -1;
        }

        /// <summary>
        /// Copy binaries.
        /// </summary>
        /// <param name="projectPath">Project path.</param>
        /// <param name="targetPath">Target path.</param>
        private static void CopyBinaries(string projectPath, string targetPath)
        {
            try
            {
                string projectDir = projectPath.Substring(0, projectPath.LastIndexOf("\\", StringComparison.CurrentCultureIgnoreCase)) + "\\bin\\";

                string[] filePaths = Directory.GetFiles(projectDir, "*.dll", SearchOption.AllDirectories);
                if (!Directory.Exists(targetPath))
                {
                    Directory.CreateDirectory(targetPath);
                }

                foreach (string filePath in filePaths)
                {
                    string file = filePath.Substring(filePath.LastIndexOf("\\", StringComparison.CurrentCultureIgnoreCase) + 1);
                    File.Copy(filePath, targetPath + "\\" + file, true);
                }

                Console.WriteLine("Copied binaries from " + projectPath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
