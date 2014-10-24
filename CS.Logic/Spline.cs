namespace CS.Logic
{
    public class Spline
    {
        public Spline(int numberOfSplines)
        {
            Coefficients = new SplineCoefficients[numberOfSplines];
            Nodes = new double[numberOfSplines];

            for (int i = 0; i < numberOfSplines; i++)
            {
                Coefficients[i] = new SplineCoefficients();
            }
        }

        public SplineCoefficients[] Coefficients { get; set; }
        public double[] Nodes { get; set; }

        public double Value(int interval, double x)
        {
            var a = Coefficients[interval].A;
            var b = Coefficients[interval].B;
            var c = Coefficients[interval].C;
            var d = Coefficients[interval].D;
            return a + b * x + (1.0/2.0) * c * x * x + (1.0/6.0) * d * x * x * x;
        }

        public double Value(double x)
        {
            var a = Nodes[0];
            var b = Nodes[Nodes.Length - 1];

            var numberOfIntervals = Coefficients.Length - 1;
            var intervalX = (x - a);
            var intervalIndex = (int)((intervalX / (b - a)) * numberOfIntervals) + 1;

            if (x >= b)
            {
                intervalIndex = numberOfIntervals;
                intervalX = (b - a) / (numberOfIntervals);
            }

            var value = Value(intervalIndex, intervalX);

            return value;
        }
    }
}