using System;
using System.Data;

namespace CS.Logic
{
    public class Calculator
    {
        public Spline FindSpline(int numberOfIntervals, double a, double b, Func<double, double> function)
        {
            var spline = new Spline(numberOfIntervals);
            CalculateNodes(numberOfIntervals, a, b, spline);
            
            for (int i = 0; i < numberOfIntervals; i++)
            {
                spline.Coefficients[i].A = function(spline.Nodes[i + 1]);
            }
            
            return spline;
        }

        private static void CalculateNodes(int numberOfIntervals, double a, double b, Spline spline)
        {
            int numberOfNodes = numberOfIntervals + 1;

            for (int i = 0; i < numberOfNodes; i++)
            {
                spline.Nodes[i] = a + ((b - a)/numberOfIntervals)*i;
            }
        }
    }
}