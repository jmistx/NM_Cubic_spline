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
        private Calculator calculator;

        [SetUp]
        public void SetUp()
        {
            calculator = new Calculator();
        }

        [Test]
        public void ReturnTrivialSpline()
        {
            var function = new Func<double, double>(x => 0);
            var spline = calculator.FindSpline(numberOfIntervals: 1, a: 0, b: 1, function: function);

            Assert.AreEqual(spline.Coefficients[0].A, 0);
            Assert.AreEqual(spline.Coefficients[0].B, 0);
            Assert.AreEqual(spline.Coefficients[0].C, 0);
            Assert.AreEqual(spline.Coefficients[0].D, 0);
            Assert.AreEqual(spline.Nodes, new[]{0, 1});
        }

        [Test]
        public void SplitIntervalByNodes()
        {
            var function = new Func<double, double>(x => 0);
            var spline = calculator.FindSpline(numberOfIntervals: 2, a: 0, b: 1, function: function);

            Assert.AreEqual(2, spline.Coefficients.Length);
            Assert.AreEqual(3, spline.Nodes.Length);
            Assert.AreEqual(new[] {0, 0.5, 1}, spline.Nodes);
        }

        [Test]
        public void ApproximateTrivialFunctionExactly()
        {
            var function = new Func<double, double>(x => 0);
            var spline = calculator.FindSpline(numberOfIntervals: 1, a: 0, b: 1, function: function);

            Assert.AreEqual(0, spline.Value(0));
            Assert.AreEqual(0, spline.Value(0.5));
            Assert.AreEqual(0, spline.Value(1));
        }

        [Ignore]
        [Test]
        public void ApproximateLinearFunctionExactly()
        {
            var function = new Func<double, double>(x => x);
            var spline = calculator.FindSpline(numberOfIntervals: 1, a: 0, b: 2, function: function);

            Assert.AreEqual(0, spline.Value(0));
            Assert.AreEqual(1, spline.Value(1));
            Assert.AreEqual(2, spline.Value(2));
        }

        [Test]
        public void SetSplineCoeffAEqualsFunctionValue()
        {
            var function = new Func<double, double>(x => x);
            var spline = calculator.FindSpline(numberOfIntervals: 3, a: 0, b: 3, function: function);

            Assert.AreEqual(1, spline.Coefficients[0].A);
            Assert.AreEqual(2, spline.Coefficients[1].A);
            Assert.AreEqual(3, spline.Coefficients[2].A);
        }
    }
}
