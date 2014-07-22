namespace Fanex.FxCop
{
    using System;

    /// <summary>
    /// DirectoryHelper class.
    /// </summary>
    public static class DirectoryHelper
    {
        /// <summary>
        /// Correct directory path.
        /// </summary>
        /// <param name="path">Input path.</param>
        /// <returns>Return corrected path.</returns>
        public static string CorrectDirectoryPath(string path)
        {
            string result = path;

            // Solving parsing error when string end with \
            if (result.EndsWith("\"", StringComparison.CurrentCultureIgnoreCase))
            {
                result = path.Remove(result.Length - 1, 1);
            }

            if (!result.EndsWith("\\", StringComparison.CurrentCultureIgnoreCase))
            {
                result += "\\";
            }

            return result;
        }

        public static string GetProjectName(string path)
        {
            int start = path.LastIndexOf("\\", StringComparison.CurrentCultureIgnoreCase);
            int end = path.LastIndexOf(".csproj", StringComparison.CurrentCultureIgnoreCase);

            string name = path.Substring(start + 1, end - start - 1);
            return name;
        }

        /// <summary>
        /// Correct directory path.
        /// </summary>
        /// <param name="path">Input path.</param>
        /// <returns>Return corrected path.</returns>
        public static string GetDirectoryPath(string path)
        {
            int start = 0;
            int end = path.LastIndexOf("\\", StringComparison.CurrentCultureIgnoreCase);

            string name = path.Substring(start, end - start);
            return name;
        }
    }
}
