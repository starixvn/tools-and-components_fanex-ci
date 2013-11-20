// -----------------------------------------------------------------------
// <copyright file="MWA.cs" company="Nexcel Solutions">
// Qui@20131119.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.MsDeploy
{
    using System;
    using System.Linq;
    using Microsoft.Web.Administration;

    public class MWA : IConfigureAppPoolIIS7
    {
        public void ConfigureAppPoolIIS7(Options options)
        {
            var mgr = ServerManager.OpenRemote(options.Host);

            // Create appPool if it does not exist
            if (!mgr.ApplicationPools.Any(pool => pool.Name == options.Pool))
            {
                ApplicationPool app = mgr.ApplicationPools.Add(options.Pool);
                app.AutoStart = true;
                app.ManagedPipelineMode = ManagedPipelineMode.Integrated;
                app.ManagedRuntimeVersion = "v4.0";
                app.ProcessModel.IdentityType = ProcessModelIdentityType.ApplicationPoolIdentity;

                Console.WriteLine("Create application pool " + options.Pool);

                mgr.CommitChanges();
            }

            // Set app pool to target application name
            foreach (Site site in mgr.Sites)
            {
                if (site.Name == options.RootWebsite)
                {
                    foreach (Application webApplication in site.Applications)
                    {
                        if (webApplication.Path == "/" + options.Application)
                        {
                            if (webApplication.ApplicationPoolName != options.Pool)
                            {
                                Console.WriteLine("Assign application pool " + options.Pool + " to " + options.Application);
                                webApplication.ApplicationPoolName = options.Pool;
                                mgr.CommitChanges();
                            }
                        }
                    }
                }
            }
        }
    }
}
