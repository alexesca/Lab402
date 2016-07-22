using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace Lab_04 //Changed namespace to match the project
{
    public class ReadFile
    {
        //This method checks if the file already exists
        public static bool checkIfFileExists(string file)
        {
            try
            {
                //Trying to read the file
                using (StreamReader reader = new StreamReader(file))
                {
                    //it just checks if teh file exists. 
                }
            }
            catch (Exception e)
            {
                //Returning an error message
                //Console.Write("File not found! ");
                return false;
            }
            return true;
        }

        //This checks if the file may be created so that we can read it and write it. 
        public static bool checkIfFileMayBeCreated(String file)
        {
            try
            {
                if (file.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                {
                    return false;
                    //throw new System.InvalidOperationException("File cannot contain especial characters");
                }

                if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), file)))
                {
                    return false;
                }
                return true;
            }
            catch (Exception e)
            {
                //Returning a message
                //Console.Write("File not found");
                return false;
            }
        }

        //Reads the file
        public static List<Student> readFile(String file, DataTable table1)
        {
            List<Student> list = new List<Student>();
            try
            {
                using (StreamReader reader = new StreamReader(file))
                {
                    // Read until we reach the end of the file.
                    string str = "";
                    int line = 1;
                    do
                    {
                        //Reads a line
                        str = reader.ReadLine();
                        
                        //Getting headers
                        if (line==1)
                        {
                            getHeaders(table1,str);
                        }
                        else
                        {
                            //Adding students to the list
                            addStudentToTable(table1, str);
                        }
                        line++;
                    }
                    //Conditional
                    while (!reader.EndOfStream);
                }
            }
            //Catches any exception
            catch (Exception e)
            {
                //Console.WriteLine("{0} Exception .", e);
            }
            return list;
        }

        // Append student to list
        public static void addStudentToTable(DataTable table1, String str)
        {
            //Declaring variables
            string firstName = getFirstName(str);
            string lastName = getLastName(str);
            List<Double> grades = getGrades(str);
            DataRow row = table1.NewRow();
            row[0] = firstName;
            row[1] = lastName;
            int index = 2;
            //Adding rows
            foreach (double grade in grades)
            {

                row[index] = grade;
                index++;
            }
            table1.Rows.Add(row);
        }

        // Append student to list
        public static void getHeaders(DataTable table1, String line)
        {
            //Declaring the headers
            String[] values = line.Split(',');
            table1.Columns.Add("Firstname");
            table1.Columns.Add("Lastname");
            table1.Columns.Add(values[1]);
            table1.Columns.Add(values[2]);
            table1.Columns.Add(values[3]);
            table1.Columns.Add(values[4]);
            table1.Columns.Add(values[5]);
        }

        // Retrieve first name from given data
        public static String getFirstName(string str)
        {
            //Substringing the name
            int index = str.IndexOf(' ');
            string firstName = str.Substring(0, index);
            return firstName;
        }

        // Retrieve last name from given data
        public static string getLastName(string str)
        {
            //Getting the last name
            int index = str.IndexOf(' ');
            string lastName = str.Substring(index + 1, (str.IndexOf(',') - index) - 1);
            return lastName;
        }

        // Check for errors within an array of grades
        public static List<Double> getGrades(string str)
        {
            //Getting grades
            List<String> grades = new List<String>();
            List<Double> gradesStudent = new List<Double>();
            //Exploding the string/line
            grades = str.Split(',').ToList<String>();
            grades.RemoveAt(0);
            //Looping each grade
            for (int i = 0; i < grades.Count; i++)
            {
                try
                {
                    //Adding them to the list
                    Double num = Convert.ToDouble(grades[i]);
                    gradesStudent.Add(num);
                }
                catch (Exception e)
                {
                    gradesStudent.Add(0);
                    //Console.WriteLine("Error reading line {0}", i
                }
            }
            return gradesStudent;
        }

        // Split string by comma
        public static List<String> splitStringLine(String line)
        {
            //Declarin variables
            List<String> data = new List<String>();
            String tempString = "";
            //Iterates the line
            foreach (char character in line)
            {
                if (character.CompareTo(',') == 0)
                {
                    //Add them to the collection
                    data.Add(tempString);
                    tempString = "";
                }
                else
                {
                    //Builds the new string
                    tempString += character;
                }
            }
            //Returning the final value.
            return data;
        }
    }
}
