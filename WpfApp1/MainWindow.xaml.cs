using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;

namespace WpfApp1
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.RunWorkerCompleted += RunWorkerCompleted;
            worker.WorkerReportsProgress = true;
            worker.DoWork += WorkerDoWork;
            worker.ProgressChanged += WorkerProgressChanged;
            worker.RunWorkerAsync();
        }

        private void WorkerProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            StartButton.IsEnabled = false;
            ProgressBar.Value = e.ProgressPercentage;
            string[] array = (string[])e.UserState;
            MetadataTextBox.Text = array[0];
            CodeTextBox.Text = array[1];
        }

        private void WorkerDoWork(object sender, DoWorkEventArgs e)
        {
            //thisWorker.ReportProgress(0, String.Format("XXX 1"));
            BusinessLogic businessLogic = new BusinessLogic();
            var thisWorker = sender as BackgroundWorker;

            // How to change 1 000 000 to CounterTextBox.Value?

            for (int i = 0; i < 1_000_000; i++)
            {
                if (i % 1000 == 0)
                {
                    Thread.Sleep(10);
                    string metadata = businessLogic.metadataProvider.ProvideMetadata();
                    string code = businessLogic.GetCode(metadata);
                    businessLogic.validator.AssertCodeIsValid(code, metadata);
                    thisWorker.ReportProgress((i + 1) / 10000, new string[] { metadata, code });
                }                
            }
        }
        //thisWorker.ReportProgress(100, "XXX na 100%");

        private void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ProgressBar.Value = 100;
            MessageBox.Show("Test completed!");
            ProgressBar.Value = 0;
            MetadataTextBox.Text = String.Empty;
            StartButton.IsEnabled = true;
        }
    }
}