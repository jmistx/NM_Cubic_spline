using System;
using System.Data;

namespace CS.Logic
{
    public class Calculator
    {
        public Spline FindSpline(int numberOfIntervals, double a, double b, Func<double, double> function)
        {
            var numberOfSplines = numberOfIntervals + 1;

            var spline = new Spline(numberOfSplines);
            CalculateNodes(numberOfSplines, a, b, spline);

            double h = (b - a)/numberOfIntervals;

            var f = new double[numberOfSplines];

            for (int i = 0; i < numberOfSplines; i++)
            {
                f[i] = function(spline.Nodes[i]);
            }

            for (int i = 1; i < numberOfSplines; i++)
            {
                spline.Coefficients[i].A = f[i];
            }

            for (int i = 1; i < numberOfSplines; i++)
            {
                spline.Coefficients[i].D = (spline.Coefficients[i].C - spline.Coefficients[i - 1].C)/h +
                                           (f[i] - f[i - 1])/h;
            }
            
            return spline;
        }

        private static void CalculateNodes(int numberOfSplines, double a, double b, Spline spline)
        {
            int numberOfNodes = numberOfSplines;
            int numberOfIntervals = numberOfSplines - 1;

            for (int i = 0; i < numberOfNodes; i++)
            {
                spline.Nodes[i] = a + ((b - a)/numberOfIntervals)*i;
            }
        }
    }
}