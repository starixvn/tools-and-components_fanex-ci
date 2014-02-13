// --------------------------------------------------------------------------
// <copyright file="JsHintResultHandler.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.2.13">Created</change>
// </history>

namespace Fanex.MsBuild
{
    using System;
    using System.IO;

    /// <summary>
    /// JSHint handler for check result.
    /// </summary>
    public class JsHintResultHandler
    {
        private string _jscriptHintFilePath = string.Empty;

        public JsHintResultHandler(string jscripHintFilePath)
        {
            _jscriptHintFilePath = jscripHintFilePath;
            this.ReadResultFromJSHintReport();
        }

        /// <summary>
        /// Read result from JS Hint Report, value is 0 for success, 1 for fail.
        /// </summary>
        /// <returns>0 for success, 1 for fail.</returns>
        public int ReadResultFromJSHintReport()
        {
            var result = 0;
            try
            {
                if (!string.IsNullOrEmpty(_jscriptHintFilePath)
                    && File.Exists(_jscriptHintFilePath))
                {
                    var matchString = "!!!@@@###$$$%%%";
                    string allText = File.ReadAllText(_jscriptHintFilePath);
                    var indexMatch = allText.LastIndexOf(matchString);
                    string resultCharacter = allText[indexMatch + matchString.Length].ToString();
                    int.TryParse(resultCharacter, out result);

                    if (result > 0)
                    {
                        Environment.SetEnvironmentVariable("ERRORLEVEL", result.ToString(), EnvironmentVariableTarget.Process);
                        Environment.SetEnvironmentVariable("JSHINT_BUILD_FAILED", result.ToString(), EnvironmentVariableTarget.Process);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return result;
        }
    }
}