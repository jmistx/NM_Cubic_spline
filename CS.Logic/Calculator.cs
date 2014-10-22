namespace CS.Logic
{
    public class Calculator
    {
        public Spline FindSpline(int numberOfIntervals, int a, int b)
        {
            return new Spline()
            {
                Coefficients = new[] { new SplineCoefficients() },
                Nodes = new double[] {a, b}
            };
        }
    }
}