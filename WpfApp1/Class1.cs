﻿using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Threading.Tasks;

namespace WpfApp1
{

        /// <summary>
        /// Contains functionality for executing PowerShell scripts.
        /// </summary>
        public class HostedRunspace1
        {

        private string nameValue;

        public string Name
        {
            get { return nameValue; }
            set { nameValue = value; }
        }

        /// <summary>
        /// Runs a PowerShell script with parameters and prints the resulting pipeline objects to the console output. 
        /// </summary>
        /// <param name="scriptContents">The script file contents.</param>
        /// <param name="scriptParameters">A dictionary of parameter names and parameter values.</param>
        public async Task RunScript(string scriptContents, Dictionary<string, object> scriptParameters)
            {
                // create a new hosted PowerShell instance using the default runspace.
                // wrap in a using statement to ensure resources are cleaned up.

                using (PowerShell ps = PowerShell.Create())
                {
                    // specify the script code to run.
                    ps.AddScript(scriptContents);

                    // specify the parameters to pass into the script.
                    ps.AddParameters(scriptParameters);

                    // execute the script and await the result.

                    var pipelineObjects = await ps.InvokeAsync().ConfigureAwait(false);

                    // print the resulting pipeline objects to the console.
                  //  foreach (var item in pipelineObjects)
                   // {
                    //    Console.WriteLine(item.BaseObject.ToString());
                    //}

                foreach (var item in pipelineObjects)
                {
                    this.Name += item.BaseObject.ToString();
                }
            }
            }
        }
    

}
