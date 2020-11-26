using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public HostedRunspace hosted = new HostedRunspace();


  
        public MainWindow()
        {
          

            InitializeComponent();

       
            MyOtherBar.Value = 75;

            Startja().ContinueWith(task => { /* do some other stuff */ },
        TaskScheduler.FromCurrentSynchronizationContext());


            DataContext = hosted;

        


        }

        public TextBox getBox()
        {

            return MyBox;
        }


        private void UpdateTextBox(string text)
        {
            ProgressBox.AppendText(text + "\r\n");
        }

        public  async Task Startja()
        {
            var scriptContents = new StringBuilder();
            scriptContents.AppendLine("Param($StrParam, $IntParam)");
            scriptContents.AppendLine("");
            scriptContents.AppendLine("Write-Progress -Activity Updating -Status \"Progress->\" PercentComplete 0 -CurrentOperation OuterLoop");
            scriptContents.AppendLine("Write-Progress -Id 1 -Activity Updating -Status \"Progress\" -PercentComplete 5 -CurrentOperation InnerLoop");
            scriptContents.AppendLine("");
            scriptContents.AppendLine("Write-Output \"Message from inside the running script\"");
            scriptContents.AppendLine("Write-Output \"This is the value from the first param: $StrParam\"");
            scriptContents.AppendLine("Write-Output \"This is the value from the second param: $IntParam\"");
            scriptContents.AppendLine("");
            scriptContents.AppendLine("Write-Output \"Here are the loaded modules in the script:\"");
            scriptContents.AppendLine("Get-Module");
            scriptContents.AppendLine("");
            scriptContents.AppendLine("# write some data to the info/warning streams");
            scriptContents.AppendLine("Start-Sleep 2");
            scriptContents.AppendLine("");
            scriptContents.AppendLine("Write-Host \"A message from write-host\"");
            scriptContents.AppendLine("Write-Information \"A message from write-information\"");
            scriptContents.AppendLine("");
            scriptContents.AppendLine("Write-Warning \"A message from write-warning\"");
            scriptContents.AppendLine("");
            scriptContents.AppendLine("# write a message to the error stream by throwing a non-terminating error");
            scriptContents.AppendLine("# note: terminating errors will stop the pipeline.");
            scriptContents.AppendLine("Get-ChildItem -Directory \"folder-doesnt-exist\"");
            scriptContents.AppendLine("Write-Progress -Id 1 -Activity Updating -Status \"Progress\" -PercentComplete 10 -CurrentOperation InnerLoop");
            scriptContents.AppendLine("Start-Sleep 2");
            scriptContents.AppendLine("Write-Progress -Id 1 -Activity Updating -Status \"Progress\" -PercentComplete 50 -CurrentOperation InnerLoop");
            scriptContents.AppendLine("");

            var scriptParameters = new Dictionary<string, object>()
            {
                { "StrParam", "Hello from script" },
                { "IntParam", 7 }
            };



            Console.WriteLine("Initializing runspace pool.");

            // The 'Az' module (bundle) is the Windows Azure PowerShell module that works on both PS 5.1 and PS Core.
            // For this example to work, the Az module should already be installed.

            //var modulesToLoad = new string[] { "Az.Accounts", "Az.Compute" };

            //var hosted = new HostedRunspace();
            hosted.InitializeRunspaces(2, 10);

            Console.WriteLine("Calling RunScript()");
            await hosted.RunScript(scriptContents.ToString(), scriptParameters, ProgressBox);

            Console.WriteLine("Script execution completed. Press enter key to exit:");
            Console.Read();

            MyBox.Text = hosted.Name;

            WarningBox.Text = hosted.psWarning;
            WarningBox.Background = new SolidColorBrush(Colors.Orange);

            InfoBox.Text = hosted.psInfo;
            InfoBox.Background = new SolidColorBrush(Colors.AliceBlue);

            ErrorBox.Text = hosted.psError;
            ErrorBox.Background = new SolidColorBrush(Colors.Red);


// ProgressBox.Text = hosted.psProgress.ToString();
            //  ErrorBox.Background = new SolidColorBrush(Colors.Red);


        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            
            var progress = new Progress<string>(s => SlepperBox.Text = s);
            await Task.Factory.StartNew(() => hosted.LongWork(progress),
                                        TaskCreationOptions.LongRunning);
            SlepperBox.Text = "completed";

       


            /*
            if(SlepperBox.Text == "wtf")
            {

                SlepperBox.Text = "Not WTF";

            }else
            {
                SlepperBox.Text = "wtf";
            }*/

        }

        private void MyBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ProgressBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void Mybar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }
    }
}
