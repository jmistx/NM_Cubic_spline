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

            var function = new Func<double, double>(x =>
            {
                if (x <= 0) return x*x*x + 3*x*x;
                return -x*x*x + 3*x*x;         
            });
            var derivative = new Func<double, double>(x =>
            {
                if (x <= 0) return 3 * x * x + 6 * x;
                return -3 * x * x + 6 * x;
            });
            var calculator = new Calculator();
            var spline = calculator.FindSpline(NumberOfIntervals, LeftPoint, RightPoint, function, LeftBound, RightBound);
            
            SplineViewModel = new SplineViewModel(spline);

            SplinePlot.Series.Add(new FunctionSeries(function, LeftPoint, RightPoint, 0.05, "function"));
            SplinePlot.Series.Add(new FunctionSeries(spline.Value, LeftPoint, RightPoint, 0.05, "spline"));
            DerivativePlot.Series.Add(new FunctionSeries(derivative, LeftPoint, RightPoint, 0.05, "function"));
            DerivativePlot.Series.Add(new FunctionSeries(spline.DerivativeValue, LeftPoint, RightPoint, 0.05, "spline"));

            ComparisonTable = calculator.Compare(function, derivative, spline, NumberOfIntervals*4);
            RaisePropertyChanged("SplinePlot");
            RaisePropertyChanged("DerivativePlot");
            RaisePropertyChanged("SplineViewModel");
            RaisePropertyChanged("ComparisonTable");
        }

        public MainViewModel()
        {
            DrawSplineCommand = new RelayCommand(DrawSpline);

            LeftPoint = -1;
            RightPoint = 1;
            NumberOfIntervals = 2;
            LeftBound = 0;
            RightBound = 0;
        }
    }
}