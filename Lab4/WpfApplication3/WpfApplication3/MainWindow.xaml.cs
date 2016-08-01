//Project: Lab 4
//Description: Read/Write using WPF application
//Name: Alex Moreno, Andy Wold, Bethaly Tenango
//Date: 01 Aug 2016
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
        DataSet set;

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
                set = new DataSet("Students");
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
            DataView dv = (DataView)dataGrid.ItemsSource;
            DataRow dr = dv.Table.NewRow();
//            dv.Table.Rows.Add();
            dataGrid.ItemsSource = null;
            dataGrid.ItemsSource = dv;
        }

        //Delete the select row
        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            //Checks if the user selected a row
            if (dataGrid.SelectedIndex >= 0)
            {
                //Gets rid of the selected row
                try
                {
                    DataView dv = (DataView)dataGrid.ItemsSource;
                    dv.Table.Rows.RemoveAt(dataGrid.SelectedIndex);
                }
                catch
                {

                }
            }
        }

        //Grade the entire class
        private void buttonGrade_Click(object sender, RoutedEventArgs e)
        {
            //Adds a new column named "GPA" [Andy]
            DataView dv = (DataView)dataGrid.ItemsSource;

            if (!dv.Table.Columns.Contains("GPA"))
            {
                dv.Table.Columns.Add("GPA");
            }

            //Refreshes the dataGrid's view to show the added column [Andy]
            dataGrid.ItemsSource = null;
            dataGrid.ItemsSource = dv;

            //Calculate the SUM of the grades for each student
            int RowCount = table1.Rows.Count;
            if (table1.Rows[table1.Rows.Count - 1]["Firstname"] == "COURSE")
            {
                RowCount -= 1;
            }
            for (int i = 0; i < RowCount; i++)
            {
                table1.Rows[i]["GPA"] =
                    (
                        Convert.ToInt64(table1.Rows[i]["Score1"]) +
                        Convert.ToInt64(table1.Rows[i]["Score2"]) +
                        Convert.ToInt64(table1.Rows[i]["Score3"]) +
                        Convert.ToInt64(table1.Rows[i]["Score4"]) +
                        Convert.ToInt64(table1.Rows[i]["Score5"])
                    ) / 5;

            }
        }

        //Code to obtain the class Average
        private void buttonClassAverage_Click(object sender, RoutedEventArgs e)
        {
            DataView dv = (DataView)dataGrid.ItemsSource;
            try
            {
                double avg = 0;
                DataRow newRow = dv.Table.NewRow();
                newRow[0] = "COURSE";
                newRow[1] = "AVERAGE";
                for (int i = 2; i < 8; i++)
                {
                    foreach (DataRow row in dv.Table.Rows)
                    {
                        avg += Convert.ToDouble(row[i].ToString());
                    }
                    newRow[i] = Math.Round(avg / dv.Table.Rows.Count, 2);
                    avg = 0;
                }
                dv.Table.Rows.Add(newRow);
                //buttonGrade.IsEnabled = false;
                //buttonDelete.IsEnabled = false;
                //buttonAdd.IsEnabled = false;
                //buttonClassAverage.IsEnabled = false;
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        //Code to check only are numbers in the cell of Scores
        private void dataGrid_CurrentCellChanged(object sender, EventArgs e)
        {
          //If we select a row 
            if (dataGrid.SelectedIndex >= 0)
            {
                try
                { 
                    DataView dv = (DataView)dataGrid.ItemsSource;
                    //Check if all score columns are numbers 
                    int count = table1.Columns.Count;
                    if (dv.Table.Columns.Equals("GPA"))
                    {
                        count -= 1;
                    }
                    //for (int i = 2; i < table1.Columns.Count; i++)
                    for (int i = 2; i < count; i++)
                        {
                            //the index of the cell
                            string str = dv.Table.Rows[dataGrid.SelectedIndex][i].ToString();
                        double inNumber;
                        //Convert the object to a double and send false if it isn't
                        bool isNumeric = double.TryParse(str, out inNumber);
                        //Value the boolean
                        if (isNumeric == false )
                        {
                            //Reject changes and print 0 instance of the string
                            dv.Table.RejectChanges();
                            dv.Table.Rows[dataGrid.SelectedIndex][i] = 0;
                            MessageBox.Show("Please enter a valid number.");
                        }
                        else
                            //Accept the changes
                            dv.Table.AcceptChanges();
                    }
                }
                catch (Exception)
                {

                }
            }
        }

      
        private void dataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            //if the user select the column "GAP"
            if (e.Column.DisplayIndex == 7)
            {
                //Reject change and recover the original value
                e.Cancel = true;
            
            }
       
        }
    }
}
