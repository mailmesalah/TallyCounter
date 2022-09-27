using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfTallyCounter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
      
        List<string> barcodes = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
            /*DateTime d = DateTime.Now;
            if (new DateTime(2017, 7, 16).CompareTo(d) < 0)
            {
                this.Close();
            }*/

            listBoxBarcodes.ItemsSource = barcodes;
            labelCount.Content = barcodes.Count;
        }

        private void textBoxInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {

                if (!textBoxInput.Text.Trim().Equals(""))
                {
                    textBoxInput.Text = textBoxInput.Text.Replace(".", "");

                    if (barcodes.Contains(textBoxInput.Text.Trim()))
                    {
                        //Remove the item if found and update count
                        barcodes.Remove(textBoxInput.Text.Trim());
                        labelCount.Content = barcodes.Count;
                        listBoxBarcodes.Items.Refresh();
                        imageStatus.Source = new BitmapImage(new Uri("check.png", UriKind.Relative));
                        //updateBackground(true);
                    }else
                    {
                        imageStatus.Source = new BitmapImage(new Uri("cross.png", UriKind.Relative));
                        //updateBackground(false);
                    }
                }

                textBoxInput.Text = "";                                
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", "Do you want to Close", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.No)
            {
                e.Cancel = true;
            }

        }

        private void updateBackground(bool status)
        {

            this.Dispatcher.Invoke(() => {
                Brush b = this.Background;
                if (status)
                {
                    this.Background = Brushes.Green;
                }
                else
                {
                    this.Background = Brushes.Red;
                }
                
            });
        }
    

        private void menuImport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "CSV file (*.csv)|*.csv";
                if (openFileDialog.ShowDialog() == true)
                {
                    string csvData = File.ReadAllText(openFileDialog.FileName);
                    barcodes = csvData.Split(',').ToList();
                    listBoxBarcodes.ItemsSource = barcodes;
                    listBoxBarcodes.Items.Refresh();
                    labelCount.Content = barcodes.Count;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Occured " + ex.Message);
            }
        }
        
    }
}
