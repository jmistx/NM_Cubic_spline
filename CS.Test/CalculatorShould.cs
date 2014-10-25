using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using CS.Logic;
using NUnit.Framework;

namespace CS.Test
{
    [TestFixture]
    public class CalculatorShould
    {
        private Calculator calculator;
        private readonly Func<double, double> constantZero = x => 0;

        [SetUp]
        public void SetUp()
        {
            calculator = new Calculator();
        }

        [Test]
        public void ReturnTrivialSpline()
        {
            var spline = calculator.FindSpline(numberOfIntervals: 2, a: 0, b: 1, function: constantZero);

            Expect.CoefficientEquals(spline, 0, new double[]{0, 0, 0, 0});
            Expect.CoefficientEquals(spline, 1, new double[]{0, 0, 0, 0});
            Expect.CoefficientEquals(spline, 2, new double[]{0, 0, 0, 0});

            Assert.AreEqual(spline.Nodes, new[]{0, 0.5, 1});
        }

        [Test]
        public void SplitIntervalByNodes()
        {
            var spline = calculator.FindSpline(numberOfIntervals: 2, a: 0, b: 1, function: constantZero);

            Assert.AreEqual(3, spline.Coefficients.Length);
            Assert.AreEqual(3, spline.Nodes.Length);
            Assert.AreEqual(new[] {0, 0.5, 1}, spline.Nodes);
        }

        [Test]
        public void ApproximateTrivialFunctionExactly()
        {
            var spline = calculator.FindSpline(numberOfIntervals: 2, a: 0, b: 1, function: constantZero);

            Assert.AreEqual(0, spline.Value(0));
            Assert.AreEqual(0, spline.Value(0.25));
            Assert.AreEqual(0, spline.Value(0.75));
            Assert.AreEqual(0, spline.Value(1));
        }

        [Test]
        public void ApproximateLinearFunctionExactly()
        {
            var function = new Func<double, double>(x => x);
            var spline = calculator.FindSpline(numberOfIntervals: 2, a: 0, b: 2, function: function);

            Assert.AreEqual(0, spline.Value(0));
            Assert.AreEqual(0.5, spline.Value(0.5));
            Assert.AreEqual(1, spline.Value(1));
            Assert.AreEqual(1.5, spline.Value(1.5));
            Assert.AreEqual(2, spline.Value(2));
        }


        [Test]
        public void ApproximateSquareFunctionExactly()
        {
            var function = new Func<double, double>(x => 4 * x*x - 2);
            var spline = calculator.FindSpline(numberOfIntervals: 3, a: -16, b: 16, function: function, leftBound: 4, rightBound: 4);

            Assert.AreEqual(-2, spline.Value(0));
            Assert.AreEqual(-1, spline.Value(0.5));
            Assert.AreEqual(-1, spline.Value(-0.5));
            Assert.AreEqual(2, spline.Value(1));
            Assert.AreEqual(2, spline.Value(-1));
        }
        
        [Test]
        public void UseGlueConditions()
        {
            var function = new Func<double, double>(x => 4 * x * x - 2);
            var spline = calculator.FindSpline(numberOfIntervals: 6, a: -4, b: 4, function: function, leftBound: 4, rightBound: 4);
            for (int i = 1; i < 6; i++)
            {
                var x = spline.Nodes[i - 1];
                Expect.FloatsAreEqual(spline.Value(i - 1, x), spline.Value(i, x));
            }
        }

        [Test]
        public void UseGlueConditions2()
        {
            var function = new Func<double, double>(x => 4 * x - 2);
            var spline = calculator.FindSpline(numberOfIntervals: 4, a: -4, b: 4, function: function);
            for (int i = 1; i < 4; i++)
            {
                var x = spline.Nodes[i - 1];
                Assert.AreEqual(spline.Value(i - 1, x), spline.Value(i, x));
            }
        }

        [Test]
        public void SetSplineCoeffAEqualsFunctionValue()
        {
            var function = new Func<double, double>(x => x);
            var spline = calculator.FindSpline(numberOfIntervals: 3, a: 0, b: 3, function: function);

            Assert.AreEqual(0, spline.Coefficients[0].A);
            Assert.AreEqual(1, spline.Coefficients[1].A);
            Assert.AreEqual(2, spline.Coefficients[2].A);
            Assert.AreEqual(3, spline.Coefficients[3].A);
        }

        [Test]
        public void AcceptBoundaryConditions()
        {
            var spline = calculator.FindSpline(numberOfIntervals: 2, a: 0, b: 1, function: constantZero, leftBound: 5,
                rightBound: 6);
            Assert.AreEqual(5, spline.Coefficients[0].C);
            Assert.AreEqual(6, spline.Coefficients[2].C);
        }

        [Test]
        public void DoNotAcceptLessThan2Intervals()
        {
            Expect.Exception<ArgumentException>(() => calculator.FindSpline(numberOfIntervals: 0, a: 0, b: 1, function: constantZero));
            Expect.Exception<ArgumentException>(() => calculator.FindSpline(numberOfIntervals: 1, a: 0, b: 1, function: constantZero));
            Expect.NoException(() => calculator.FindSpline(numberOfIntervals: 2, a: 0, b: 1, function: constantZero));
        }

        [Test]
        public void ProvideTridiagonalMatrixAlgorithm()
        {
            var rightPart = new double[] { 1, 1, 1 };
            var diagonal = new double[,]
            {
                {1, 1, 1},
                {1, 2, 1},
                {1, 1, 0},
            };
            Expect.Exception<ArgumentException>(() => calculator.Solve(diagonal, rightPart));
            diagonal = new double[,]
            {
                {0, 1, 1},
                {1, 2, 1},
                {1, 1, 1},
            };
            Expect.Exception<ArgumentException>(() => calculator.Solve(diagonal, rightPart));
        }

        [Test]
        public void ProvideTridiagonalMatrixAlgorithm2()
        {
            var diagonal = new double[,]
            {
                {0,-2, 1},
                {1, 1, 1},
                {1, 1, 0},
            };
            var rightPart = new double[]
            {
                1, 
                1, 
                1
            };
            var result = calculator.Solve(diagonal, rightPart);

            Assert.AreEqual(new[] { 0, 1, 0 }, result);
        }
    }
}
