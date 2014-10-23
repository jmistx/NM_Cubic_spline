using System;
using System.Data;

namespace CS.Logic
{
    public class Calculator
    {
        public Spline FindSpline(int numberOfIntervals, double a, double b, Func<double, double> function, double leftBound = 0, double rightBound = 0)
        {
            if (numberOfIntervals < 2)
            {
                throw new ArgumentException("Number of intervals should not be less 2");
            }
            var numberOfSplines = numberOfIntervals + 1;

            var spline = new Spline(numberOfSplines);
            CalculateNodes(numberOfSplines, a, b, spline);

            double h = (b - a)/numberOfIntervals;

            var f = new double[numberOfSplines];

            for (int i = 0; i < numberOfSplines; i++)
            {
                f[i] = function(spline.Nodes[i]);
            }

            spline.Coefficients[0].C = leftBound;
            spline.Coefficients[numberOfSplines - 1].C = rightBound;
            //Calculate C

            for (int i = 1; i < numberOfSplines; i++)
            {
                spline.Coefficients[i].A = f[i];
                spline.Coefficients[i].B = (h / 6) * (2 * spline.Coefficients[i].C + spline.Coefficients[i - 1].C) +
                           (f[i] - f[i - 1]) / h;
                spline.Coefficients[i].D = (spline.Coefficients[i].C - spline.Coefficients[i - 1].C) / h +
                           (f[i] - f[i - 1]) / h;
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