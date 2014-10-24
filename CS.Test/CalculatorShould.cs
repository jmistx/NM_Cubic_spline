﻿using System;
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
            var spline = calculator.FindSpline(numberOfIntervals: 2, a: 0, b: 2, function: function, leftBound: 1, rightBound: 1);

            Assert.AreEqual(0, spline.Value(0));
            Assert.AreEqual(0.5, spline.Value(0.5));
            Assert.AreEqual(1, spline.Value(1));
            Assert.AreEqual(1.5, spline.Value(1.5));
            Assert.AreEqual(2, spline.Value(2));
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
    }
}
