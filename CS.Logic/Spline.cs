namespace CS.Logic
{
    public class Spline
    {
        public Spline(int numberOfIntervals)
        {
            int numberOfNodes = numberOfIntervals + 1;
            Coefficients = new SplineCoefficients[numberOfIntervals];
            Nodes = new double[numberOfNodes];

            for (int i = 0; i < numberOfIntervals; i++)
            {
                Coefficients[i] = new SplineCoefficients();
            }
        }

        public SplineCoefficients[] Coefficients { get; set; }
        public double[] Nodes { get; set; }
    }
}