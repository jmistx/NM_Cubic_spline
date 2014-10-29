using System;
using System.Windows.Input;
using CS.Logic;
using Microsoft.TeamFoundation.MVVM;
using OxyPlot;
using OxyPlot.Series;

namespace CS.Gui
{
    public class MainViewModel : ViewModelBase
    {
        public PlotModel SplinePlot { get; private set; }
        public PlotModel DerivativePlot { get; private set; }
        public SplineViewModel SplineViewModel { get; private set; }
        public ICommand DrawSplineCommand { get; private set; }
        public int NumberOfIntervals { get; set; }
        public double LeftPoint { get; set; }
        public double RightPoint { get; set; }
        public double LeftBound { get; set; }
        public double RightBound { get; set; }
        public ComparisonTable ComparisonTable { get; set; }
        public Func<double, double> Function { get; set; }
        public Func<double, double> Derivative { get; set; }
        public ICommand SetWorkFunctionCommand { get; private set; }
        public ICommand SetTestFunctionCommand { get; private set; }
        public ICommand SetWorkFunctionCos10Command { get; set; }
        public ICommand SetWorkFunctionCos100Command { get; set; }

        readonly Func<double, double> workFunction = x => Math.Pow(1 + x * x, 1.0 / 3.0);
        readonly Func<double, double> workDerivative = x => 2.0 * x / (3.0 * Math.Pow(1 + x * x, 2.0 / 3.0));
        public void SetTestFunction()
        {
            Function = x =>
            {
                if (x <= 0) return x * x * x + 3 * x * x;
                return -x * x * x + 3 * x * x;
            };
            Derivative = x =>
            {
                if (x <= 0) return 3 * x * x + 6 * x;
                return -3 * x * x + 6 * x;
            };

            LeftPoint = -1;
            RightPoint = 1;
            NumberOfIntervals = 2;
            LeftBound = 0;
            RightBound = 0;
            NotifyFunctionChanged();
        }

        private void NotifyFunctionChanged()
        {
            RaisePropertyChanged("LeftPoint");
            RaisePropertyChanged("RightPoint");
            RaisePropertyChanged("NumberOfIntervals");
            RaisePropertyChanged("LeftBound");
            RaisePropertyChanged("RightBound");
        }

        public void SetWorkFunction()
        {
            
            Function = workFunction;
            Derivative = workDerivative;

            LeftPoint = 0;
            RightPoint = 1;
            NumberOfIntervals = 2;
            LeftBound = 2.0/3.0;
            RightBound = Math.Pow(2, 3.0)/9.0;
            NotifyFunctionChanged();
        }

        public void SetWorkFunctionCos10()
        {
            SetWorkFunction();
            Function = x => workFunction(x) + Math.Cos(10*x);
            Derivative = x => workDerivative(x) + 10*(-1)*Math.Sin(10*x);
        }

        public void SetWorkFunctionCos100()
        {
            SetWorkFunction();
            Function = x => workFunction(x) + Math.Cos(100 * x);
            Derivative = x => workDerivative(x) + 100 * (-1) * Math.Sin(10 * x);
        }

        private void DrawSpline()
        {
            SplinePlot = new PlotModel
            {
                Title = "Cubic Spline",
                PlotType = PlotType.Cartesian
            };

            DerivativePlot = new PlotModel
            {
                Title = "Derivatives plot",
                PlotType = PlotType.Cartesian
            };

            var calculator = new Calculator();
            var spline = calculator.FindSpline(NumberOfIntervals, LeftPoint, RightPoint, Function, LeftBound, RightBound);
            
            SplineViewModel = new SplineViewModel(spline);

            const double drawingAccuracy = 0.005;
            SplinePlot.Series.Add(new FunctionSeries(Function, LeftPoint, RightPoint, drawingAccuracy, "function"));
            SplinePlot.Series.Add(new FunctionSeries(spline.Value, LeftPoint, RightPoint, drawingAccuracy, "spline"));
            DerivativePlot.Series.Add(new FunctionSeries(Derivative, LeftPoint, RightPoint, drawingAccuracy, "function"));
            DerivativePlot.Series.Add(new FunctionSeries(spline.DerivativeValue, LeftPoint, RightPoint, drawingAccuracy, "spline"));

            ComparisonTable = calculator.Compare(Function, Derivative, spline, NumberOfIntervals*4);
            RaisePropertyChanged("SplinePlot");
            RaisePropertyChanged("DerivativePlot");
            RaisePropertyChanged("SplineViewModel");
            RaisePropertyChanged("ComparisonTable");
        }

        public MainViewModel()
        {
            DrawSplineCommand = new RelayCommand(DrawSpline);
            SetTestFunctionCommand = new RelayCommand(SetTestFunction);
            SetWorkFunctionCommand = new RelayCommand(SetWorkFunction);
            SetWorkFunctionCos10Command = new RelayCommand(SetWorkFunctionCos10);
            SetWorkFunctionCos100Command = new RelayCommand(SetWorkFunctionCos100);
            SetTestFunction();
        }

        
    }
}