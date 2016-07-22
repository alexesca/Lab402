using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_04
{
    // Create student class that carries an ID of a name and a list of grades from different assignments
    public class Student
    {
        public List<Double> grades = new List<Double>();
        public string firstName { set; get; }
        public string lastName { set; get; }
        public double gpa { set; get; }

        public Student(string firstName, string lastName, List<Double> grades)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.grades = grades;
        }

        // Calculate each student's individual average
        public void getStudentAverage()
        {
            double total = 0;
            foreach (double grade in this.grades)
                total += grade;
            double studentAverage = total / grades.Count;
            this.gpa = studentAverage;
        }

        // Calcuate average grade for entire class
        public static void studentAverage(List<Student> students)
        {
            foreach (var student in students)
            {
                double gpa = 0;
                for (int i = 0; i < student.grades.Count; i++)
                {
                    gpa += student.grades[i];
                }
                gpa = gpa / student.grades.Count;
                student.gpa = gpa;
            }
        }
    }
}
