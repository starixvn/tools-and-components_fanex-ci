// -----------------------------------------------------------------------
// <copyright file="ADSI.cs" company="Nexcel Solutions">
// Qui@20131119.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.MSDeploy
{
    using System;
    using System.DirectoryServices;

    /// <summary>
    /// Active Directory Service Interfaces.
    /// </summary>
    public class ADSI : IConfigureAppPoolIIS7
    {
        /// <summary>
        /// Config application pool.
        /// </summary>
        /// <param name="options">Input option.</param>
        public void ConfigureAppPoolIIS7(Options options)
        {
            // Create appPool if it does not exist
            string metabasePath = "IIS://" + options.Host + "/W3SVC/AppPools";
            DirectoryEntry newpool = new DirectoryEntry(metabasePath + "/" + options.Pool);
            DirectoryEntry apppools = new DirectoryEntry(metabasePath);

            string pool = null;
            try 
            {
                pool = newpool.Name;
            }
            catch
            {
                pool = null;
            }

            if (pool == null)
            {
                newpool = apppools.Children.Add(options.Pool, "IIsApplicationPool");
                this.SetBinaryProperty(newpool, "ManagedPipelineMode", "Integrated");
                newpool.CommitChanges();
                Console.WriteLine("Create application pool " + options.Pool);
            }

            // Set app pool to target application name
            metabasePath = "IIS://" + options.Host + "/W3SVC/" + options.RootWebsite + "/Root/" + options.Application;
            DirectoryEntry dir = new DirectoryEntry(metabasePath);
            string className = dir.SchemaClassName.ToString();
            if (className.EndsWith("VirtualDir", StringComparison.CurrentCultureIgnoreCase))
            {
                object[] param = { 0, options.Pool, true };
                dir.Invoke("AppCreate3", param);
                dir.Properties["AppIsolated"][0] = "2";
                Console.WriteLine("Assign application pool " + options.Pool + " to " + options.Application);
            }
        }

        /// <summary>
        /// Set properties.
        /// </summary>
        /// <param name="entry">Entry name.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="newValue">Property value.</param>
        private void SetBinaryProperty(DirectoryEntry entry, string propertyName, string newValue)
        {
            try
            {
                DirectoryEntry path = entry;
                PropertyValueCollection propValues = path.Properties[propertyName];

                newValue = newValue.Trim();
                string[] newValueSplit = newValue.Split(new char[] { ',' });
                object[] newObj = new object[newValueSplit.Length];
                newValueSplit.CopyTo(newObj, 0);

                propValues.Clear();
                propValues.Add(newObj);
                path.CommitChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed in SetBinaryProperty with the following exception: \n{0}", ex.Message);
            }
        }
    }
}
