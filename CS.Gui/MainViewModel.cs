using System;
using System.Windows.Input;
using AP.Logic;
using CS.Logic;
using Microsoft.TeamFoundation.MVVM;
using OxyPlot;
using OxyPlot.Series;

namespace CS.Gui
{
    public class MainViewModel : ViewModelBase
    {
        public PlotModel SplinePlot { get; private set; }
        public SplineViewModel SplineViewModel { get; private set; }
        public ICommand DrawSplineCommand { get; private set; }
        public int NumberOfIntervals { get; set; }
        public double LeftPoint { get; set; }
        public double RightPoint { get; set; }
        public double LeftBound { get; set; }
        public double RightBound { get; set; }

        private void DrawSpline()
        {
            SplinePlot = new PlotModel
            {
                Title = "Cubic Spline",
                PlotType = PlotType.Cartesian
            };

            var function = new Func<double, double>(x => -293.813+x*(605.642+x*(-437.886+x*(152.155+x*(-27.4955+(2.48621-0.0888052*x)*x)))));
            //var function = new Func<double, double>(Math.Cos);
            var calculator = new Calculator();
            var spline = calculator.FindSpline(NumberOfIntervals, LeftPoint, RightPoint, function, LeftBound, RightBound);
            SplineViewModel = new SplineViewModel(spline);

            var functionSeries = new FunctionSeries(function, LeftPoint, RightPoint, 0.1, "function");
            SplinePlot.Series.Add(functionSeries);
            var splineSeries = new FunctionSeries(spline.Value, LeftPoint, RightPoint, 0.1, "spline");
            SplinePlot.Series.Add(splineSeries);

            RaisePropertyChanged("SplinePlot");
            RaisePropertyChanged("SplineViewModel");
        }



        public MainViewModel()
        {
            DrawSplineCommand = new RelayCommand(DrawSpline);

            LeftPoint = 1;
            RightPoint = 8;
            NumberOfIntervals = 2;
            LeftBound = 4;
            RightBound = 4;
        }
    }
}