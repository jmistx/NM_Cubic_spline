namespace CS.Logic
{
    public class Calculator
    {
        public Spline FindSpline(int numberOfIntervals, double a, double b)
        {
            var spline = new Spline(numberOfIntervals);
            CalculateNodes(numberOfIntervals, a, b, spline);
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