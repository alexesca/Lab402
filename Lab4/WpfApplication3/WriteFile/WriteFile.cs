using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_04
{
    public class WriteFile
    {
        
        public static void OnExportGridToCSV(DataTable table, string path)
        {
            // Create the CSV file to which grid data will be exported.
            StreamWriter sw = new StreamWriter(path, false);
            // First we will write the headers.
            DataTable dt = table;
            //Counting cols
            int iColCount = dt.Columns.Count;
            for (int i = 0; i < iColCount; i++)
            {   
                //Adding values
                sw.Write(dt.Columns[i]);
                if (i < iColCount - 1)
                {
                    sw.Write(",");
                }
            }
            sw.Write(sw.NewLine);
            // Now write all the rows.
            foreach (DataRow dr in dt.Rows)
            {
                //Looping through the data table
                for (int i = 0; i < iColCount; i++)
                {
                    if (!Convert.IsDBNull(dr[i]))
                    {
                        sw.Write(dr[i].ToString());
                    }
                    if (i < iColCount - 1)
                    {
                        sw.Write(System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator);
                    }
                }
                sw.Write(sw.NewLine);
            }
            sw.Close();
        }
    }
}
