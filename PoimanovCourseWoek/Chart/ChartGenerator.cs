using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Definitions.Series;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
namespace PoimanovCourseWoek
{
    class ChartGenerator
    {
        SeriesCollection sr;
        public SeriesCollection Seria { get; set; }
        private string[] ax;
        public ChartGenerator() { Seria = new SeriesCollection(); }
        public void addToSeria(ISeriesView v)
        {
            Seria.Add(v);
            MaxCountOnGraph++;
        }
        int MaxCountOnGraph = 0;
        public void addToSeria(IEnumerable<double> values, string s, string title)
        {
            MaxCountOnGraph++;
            ISeriesView v=null;
            
            switch (s)
            {
                case "L":
                    {
                        if (title.Equals("Density"))
                        {
                            v = new LineSeries { Title = title, Values = new ChartValues<double>(values), PointGeometry = null };
                            Panel.SetZIndex((System.Windows.UIElement)v, 1);
                        }
                        break;
                    }

                case "C":
                        {
                            v=new ColumnSeries { Title = title, Values = new ChartValues<double>(values) ,ColumnPadding=0,MinWidth=100};
                          //  Panel.SetZIndex((System.Windows.UIElement)v, MaxCountOnGraph);
                            break;
                        }
                default:
                    break;
            }
            this.Seria.Add(v);
            

        }
        public void AxisReload(double p1, double p2, int Numbers)
        {
            this.ax = getAxis(p1, p2, Numbers);
        }

        internal SeriesCollection getKumarasauDis(int NumChartPoints, double a, double b, ref string[] axis)
        {
            SeriesCollection sr;
            IEnumerable<double> densety = generateKumarasauDistr(NumChartPoints, a, b);

            sr = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Kumarakau distrbute",
                    Values = new ChartValues<double> (densety),
                    PointGeometry = null
                }

    };
            axis = getAxis(0, 1, NumChartPoints);
            return sr;
        }

        private string[] getAxis(double p1, double p2, int NumChartPoints)
        {
            string[] axis = new string[NumChartPoints];
            double delta = Math.Round((p2 - p1) / NumChartPoints, 4);
            int c = 0;
            for (double i = p1; i < p2; i += delta)
            {
                axis[c] = i.ToString();
                c++;
            }
            return axis;
        }
        public static IEnumerable<double> generateKumarasauDistr(int Num, double a, double b)
        {
            double delta = Math.Round(1.0 / Num, 4);
            double[] fmas = new double[Num];
            int i = 0;
            for (double id = 0.001; id < 1; id += delta)
            {
                double r = RandomKumarasau.functionDistribKum(id, a, b);
                fmas[i] = r;
                i++;
            }
            return fmas;
        }
    }
}
