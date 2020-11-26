using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{

    public class HostedRunspace : INotifyPropertyChanged
    {
        /// <summary>
        /// The PowerShell runspace pool.
        /// </summary>
        private RunspacePool RsPool { get; set; }


        public string psWarning;
        public string psError;
        public string psInfo;

        public TextBox progBox;
        private string usersValue;


        private double _progressValue;


        public event PropertyChangedEventHandler PropertyChanged;

        public string Users
        {
            get { return usersValue; }
            set
            {
                {
                    if (value != usersValue)
                    {
                        usersValue = value;
                        OnPropertyChanged("Users");
                    }
                }
            }
        }

        public double ProgressValue
        {
            get
            {
                return _progressValue;
            }
            set
            {
                _progressValue = value;
                OnPropertyChanged("ProgressValue");
            }
        }



        public string Name { get; private set; }



        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        /// <summary>
        /// Initialize the runspace pool.
        /// </summary>
        /// <param name="minRunspaces"></param>
        /// <param name="maxRunspaces"></param>
        ///  public void InitializeRunspaces(int minRunspaces, int maxRunspaces, string[] modulesToLoad)
        public void InitializeRunspaces(int minRunspaces, int maxRunspaces)
        {
            // create the default session state.
            // session state can be used to set things like execution policy, language constraints, etc.
            // optionally load any modules (by name) that were supplied.

            var defaultSessionState = InitialSessionState.CreateDefault();
            defaultSessionState.ExecutionPolicy = Microsoft.PowerShell.ExecutionPolicy.Unrestricted;

           /* foreach (var moduleName in modulesToLoad)
            {
                defaultSessionState.ImportPSModule(moduleName);
            }*/
            // use the runspace factory to create a pool of runspaces
            // with a minimum and maximum number of runspaces to maintain.

            RsPool = RunspaceFactory.CreateRunspacePool(defaultSessionState);
            RsPool.SetMinRunspaces(minRunspaces);
            RsPool.SetMaxRunspaces(maxRunspaces);

            // set the pool options for thread use.
            // we can throw away or re-use the threads depending on the usage scenario.

            RsPool.ThreadOptions = PSThreadOptions.UseNewThread;

            // open the pool. 
            // this will start by initializing the minimum number of runspaces.

            RsPool.Open();
        }

        /// <summary>
        /// Runs a PowerShell script with parameters and prints the resulting pipeline objects to the console output. 
        /// </summary>
        /// <param name="scriptContents">The script file contents.</param>
        /// <param name="scriptParameters">A dictionary of parameter names and parameter values.</param>
        public async Task RunScript(string scriptContents, Dictionary<string, object> scriptParameters, TextBox box)
        {

            progBox = box;

            if (RsPool == null)
            {
                throw new ApplicationException("Runspace Pool must be initialized before calling RunScript().");
            }

            // create a new hosted PowerShell instance using a custom runspace.
            // wrap in a using statement to ensure resources are cleaned up.

            using (PowerShell ps = PowerShell.Create())
            {
                // use the runspace pool.
                ps.RunspacePool = RsPool;

                // specify the script code to run.
                ps.AddScript(scriptContents);

                // specify the parameters to pass into the script.
                ps.AddParameters(scriptParameters);

/*                PROPERTIES
Debug
Gets or sets the debug buffer. Powershell invocation writes the debug data into this buffer.Can be null.

Error
Gets or sets the error buffer.Powershell invocation writes the error data into this buffer.

Information
Gets or sets the information buffer.Powershell invocation writes the warning data into this buffer.Can be null.

Progress
Gets or sets the progress buffer.Powershell invocation writes the progress data into this buffer.Can be null.

Verbose
Gets or sets the verbose buffer.Powershell invocation writes the verbose data into this buffer.Can be null.

Warning
Gets or sets the warning buffer.Powershell invocation writes the warning data into this buffer.Can be null.

Me*/

                // subscribe to events from some of the streams
                ps.Streams.Error.DataAdded += Error_DataAdded;
                ps.Streams.Warning.DataAdded += Warning_DataAdded;
                ps.Streams.Information.DataAdded += Information_DataAdded;
                ps.Streams.Progress.DataAdded += Progress_DataAdded;

                // execute the script and await the result.
                var pipelineObjects = await ps.InvokeAsync().ConfigureAwait(false);

                // print the resulting pipeline objects to the console.
                Console.WriteLine("----- Pipeline Output below this point -----");
                foreach (var item in pipelineObjects)
                {
                    Console.WriteLine(item.BaseObject.ToString());
                    this.Name += item.BaseObject.ToString();
                    this.Name += "\n";
                }
            }
        }

        /// <summary>
        /// Handles data-added events for the information stream.
        /// </summary>
        /// <remarks>
        /// Note: Write-Host and Write-Information messages will end up in the information stream.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Information_DataAdded(object sender, DataAddedEventArgs e)
        {
            var streamObjectsReceived = sender as PSDataCollection<InformationRecord>;
            var currentStreamRecord = streamObjectsReceived[e.Index];

            Console.WriteLine($"InfoStreamEvent: {currentStreamRecord.MessageData}");
            this.psInfo += $"InfoStreamEvent: {currentStreamRecord.MessageData}";
            this.psInfo += "\n";
            this.Users = $"InfoStreamEvent: {currentStreamRecord.MessageData}";

        }

        private void Progress_DataAdded(object sender, DataAddedEventArgs e)
        {
            var streamObjectsReceived = sender as PSDataCollection<ProgressRecord>;
            var currentStreamRecord = streamObjectsReceived[e.Index];

            this.ProgressValue = currentStreamRecord.PercentComplete;
            
         


        }

        /// <summary>
        /// Handles data-added events for the warning stream.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Warning_DataAdded(object sender, DataAddedEventArgs e)
        {
            var streamObjectsReceived = sender as PSDataCollection<WarningRecord>;
            var currentStreamRecord = streamObjectsReceived[e.Index];

            Console.WriteLine($"WarningStreamEvent: {currentStreamRecord.Message}");
            this.psWarning += $"WarningStreamEvent: {currentStreamRecord.Message}";
            this.psWarning += "\n";
        }

        /// <summary>
        /// Handles data-added events for the error stream.
        /// </summary>
        /// <remarks>
        /// Note: Uncaught terminating errors will stop the pipeline completely.
        /// Non-terminating errors will be written to this stream and execution will continue.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Error_DataAdded(object sender, DataAddedEventArgs e)
        {
            var streamObjectsReceived = sender as PSDataCollection<ErrorRecord>;
            var currentStreamRecord = streamObjectsReceived[e.Index];

            Console.WriteLine($"ErrorStreamEvent: {currentStreamRecord.Exception}");
            this.psError += $"ErrorStreamEvent: {currentStreamRecord.Exception}";
            this.psError += "\n";
        }


        public void LongWork(IProgress<string> progress)
        {
            // Perform a long running work...
            for (var i = 0; i < 10; i++)
            {
                Task.Delay(500).Wait();
                progress.Report(this.ProgressValue.ToString());
            }
        }
    }



}
