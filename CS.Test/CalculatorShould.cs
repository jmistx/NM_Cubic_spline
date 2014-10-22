using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CS.Logic;
using NUnit.Framework;

namespace CS.Test
{
    [TestFixture]
    public class CalculatorShould 
    {
        [Test]
        public void ReturnTrivialSpline()
        {
            var calculator = new Calculator();
            var spline = calculator.FindSpline(numberOfIntervals: 1, a: 0, b: 1);

            Assert.AreEqual(spline.Coefficients[0].A, 0);
            Assert.AreEqual(spline.Coefficients[0].B, 0);
            Assert.AreEqual(spline.Coefficients[0].C, 0);
            Assert.AreEqual(spline.Coefficients[0].D, 0);
            Assert.AreEqual(spline.Nodes, new[]{0, 1});
        }

        [Test]
        public void SplitIntervalByNodes()
        {
            var calculator = new Calculator();
            var spline = calculator.FindSpline(numberOfIntervals: 2, a: 0, b: 1);
            Assert.AreEqual(2, spline.Coefficients.Length);
            Assert.AreEqual(3, spline.Nodes.Length);
            Assert.AreEqual(new[] {0, 0.5, 1}, spline.Nodes);
        }
    }
}
