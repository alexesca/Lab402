//Project: Lab 4
//Description: Read/Write using WPF application
//Name: Alex Moreno, Andy Wold, Bethaly Tenango
//Date: 13 Jul 2016
//Instructor: Brother Daniel Masterson
//Course: CS 176 - Windows Desktop Development

using System;
using System.Collections.Generic;
using System.Data;
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
using Microsoft.Win32;
namespace Lab_04
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DataTable table1;

        public MainWindow()
        {
            //Triggers the app
            InitializeComponent();
        }

        //Method from clicking on File >> Load in the menu bar [Andy]
        private void menuLoad_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            OpenFileDialog dlg = new OpenFileDialog();
            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".csv";
            dlg.Filter = "CSV files (*.csv)|*.csv";
            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();
            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Gets the full path of the doc 
                string filename = dlg.FileName;
                textBoxPath.Text = filename;
                // Create the DataTable.
                table1 = new DataTable("grades");
                ReadFile.readFile(filename, table1);
                // Create a DataSet and put the table in it.
                DataSet set = new DataSet("Students");
                set.Tables.Add(table1);
                dataGrid.ItemsSource = set.Tables["grades"].DefaultView;
            }
        }

        private void menuSave_Click(object sender, RoutedEventArgs e)
        {
            //Save data to CSV file here
            // Create OpenFileDialog 
            SaveFileDialog dlg = new SaveFileDialog();
            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();
            // Get the selected file name and call the  OnExportGridToCSV() function
            if (result == true)
            {
                //Exports the file
                WriteFile.OnExportGridToCSV(table1,dlg.FileName);
            }

        }

        private void menuExit_Click(object sender, RoutedEventArgs e)
        {
            // Code to exit program [Andy]
            Environment.Exit(0);
        }

        private void buttonDone_Click(object sender, RoutedEventArgs e)
        {
            // Code to exit program
            Environment.Exit(0);
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            //Adds a new blank row [Andy]
            table1.Rows.Add();
        }

        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            //Checks if the user selected a row
            if (dataGrid.SelectedIndex >= 0)
            {
                //Gets rid of the selected row
                try
                {
                    table1.Rows.RemoveAt(dataGrid.SelectedIndex);
                }
                catch
                {

                }
            }
        }

        private void buttonGrade_Click(object sender, RoutedEventArgs e)
        {
            //Adds a columns named "GPA"
            table1.Columns.Add("GPA");

            //Need to figure out how to refresh the dataGrid's view to show the added column [Andy]
        }
    }
}
