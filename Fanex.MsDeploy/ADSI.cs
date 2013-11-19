// -----------------------------------------------------------------------
// <copyright file="ADSI.cs" company="Nexcel Solutions">
// Qui@20131119.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.MsDeploy
{
    using System;

    /// <summary>
    /// Active Directory Service Interfaces.
    /// </summary>
    public class ADSI : IConfigureAppPoolIIS7
    {
        public void ConfigureAppPoolIIS7(Options options)
        {
            // Create appPool if it does not exist


            // Set app pool to target application name
        }
    }
}
