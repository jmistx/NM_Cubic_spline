using System;

namespace CS.Logic
{
    public class Calculator
    {
        public Spline FindSpline(int numberOfIntervals, double a, double b, Func<double, double> function,
            double leftBound = 0, double rightBound = 0)
        {
            if (numberOfIntervals < 2)
            {
                throw new ArgumentException("Number of intervals should not be less 2");
            }
            int numberOfSplines = numberOfIntervals + 1;

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
            CalculateCoefficientsC(spline.Coefficients, f, h);

            for (int i = 1; i < numberOfSplines; i++)
            {
                spline.Coefficients[i].A = f[i];
                spline.Coefficients[i].B = (h/6)*(2*spline.Coefficients[i].C + spline.Coefficients[i - 1].C) +
                                           (f[i] - f[i - 1])/h;
                spline.Coefficients[i].D = (spline.Coefficients[i].C - spline.Coefficients[i - 1].C)/h +
                                           (f[i] - f[i - 1])/h;
            }
            return spline;
        }

        private void CalculateCoefficientsC(SplineCoefficients[] coefficients, double[] function, double h)
        {
            int numberOfSplines = coefficients.Length;
            var diagonal = new double[numberOfSplines, 3];
            var rightPart = new double[numberOfSplines];

            diagonal[0, 1] = 1;
            diagonal[numberOfSplines - 1, 1] = 1;
            rightPart[0] = coefficients[0].C;
            rightPart[numberOfSplines - 1] = coefficients[numberOfSplines - 1].C;

            for (int i = 1; i < numberOfSplines - 1; i++)
            {
                diagonal[i, 0] = h;
                diagonal[i, 1] = 4*h;
                diagonal[i, 2] = h;
            }

            for (int i = 1; i < numberOfSplines - 2; i++)
            {
                double[] f = function;
                rightPart[i] = 6*(f[i + 1] - 2*f[i] + f[i - 1])/(2*h);
            }

            var alpha = new double[numberOfSplines];
            var beta = new double[numberOfSplines];
            {
                int i = 0;

                double a = diagonal[i, 0];
                double c = diagonal[i, 1];
                double b = diagonal[i, 2];
                double f = rightPart[i];

                alpha[i] = (-b)/c;
                beta[i] = f/c;
            }

            for (int i = 1; i < numberOfSplines; i++)
            {
                double a = diagonal[i, 0];
                double c = diagonal[i, 1];
                double b = diagonal[i, 2];
                double f = rightPart[i];

                alpha[i] = (-b)/(a*alpha[i - 1] + c);
                beta[i] = (f - a*beta[i - 1])/(a*alpha[i - 1] + c);
            }
            for (int i = numberOfSplines - 1 - 1; i >= 1; i--)
            {
                coefficients[i].C = coefficients[i + 1].C*alpha[i + 1] + beta[i + 1];
            }
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