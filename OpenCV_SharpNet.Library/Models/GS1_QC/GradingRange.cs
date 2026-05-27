using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CsplCam.Library.Models.GS1_QC
{
    /// <summary>
    /// set min and Max for grading
    /// </summary>
    /// <param name="Min"></param>
    /// <param name="Max"></param>
    public readonly record struct GradingRange
    {
        //public GradingSystems GradingSystem { get; init; }
        public double GradeValue { get; init; }
        public Grades Grades { get; init; }

        public ComparisonOperators ComparisonOperators { get; init; }

        //creat private constructor
        private GradingRange(Grades grades,double gradeValue, ComparisonOperators operators)
        {
            Grades = grades;
            GradeValue = gradeValue;
            ComparisonOperators = operators;
        }

        //create function for create instance of class
        public static GradingRange Create(Grades grades, double gradeValue, ComparisonOperators operators)
        {
            return new GradingRange(grades, gradeValue, operators);
        }

        //check is value in in grading range or not 
        public bool IsWithinRange(double value,ComparisonOperators operators)
        {
            return operators switch
            {
                ComparisonOperators.LessThanEqualTo => value <= GradeValue,
                ComparisonOperators.GreaterThanEqualTo => value >= GradeValue,
                _ => false
            };
        }

        //safe factory for default or empty constructor
        public static GradingRange Empty => new GradingRange(Grades.F, 0, ComparisonOperators.LessThanEqualTo);
    }
}
