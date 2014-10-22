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
    }
}
