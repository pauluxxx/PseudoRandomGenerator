using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LiveCharts;
using LiveCharts.Wpf;
using System.ComponentModel;
using System.Globalization;
using TAlex.MathCore;
using LiveCharts.Definitions.Series;
namespace PoimanovCourseWoek
{

    public delegate double DistribFunction(double x, double A, double B);
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BackgroundWorker backgroundWorker;
        ChartGenerator chartGenerator;
        public double max = 1;
        public Dictionary<string,double[]> math;
        public SeriesCollection Distribution { get; set; }
        public SeriesCollection Generated { get; set; }
        private bool needRestartG = false;
        public int NumChartPoints = 200;//oints will be showed
        private string[] ax;
        public string[] AxisX
        {
            get { return ax; }
            set { ax = value; }
        }
        public MainWindow()
        {
            InitializeComponent();
            RandomKumarasau ran = new RandomKumarasau(a.Value, b.Value);
            max = ran.findMax(0.000001, 0.999999999, 0.0000001);//is better;
            max = ran.functionDistribKum(max) + 1;

            backgroundWorker = ((BackgroundWorker)this.FindResource("backgroundWorker"));
            creteGraph();
        }
        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {//todo only stattic object does not support
            Dictionary<string, object> d = (Dictionary<string, object>)e.Argument;
            double A = (double)d["A"];
            double B = (double)d["B"];
            GeneratorInput g = new GeneratorInput(A, B, NumChartPoints, sender, (long)d["Stat"]);
            d.Remove("A");
            d.Remove("B");
            d.Remove("K");
            d.Remove("Stat");
            math = new Dictionary<string,double[]>();
            double [] das;
            if (d.ContainsKey("Neimana"))
            {
                d["Neimana"] = g.Neiman();//it is probabilities in the intervals
               das = new double[] {g.MathExpected,g.MathDisperse,g.SQotklonenie};
                math.Add("Neimana", das);
            }

            if (d.ContainsKey("Metropolis"))
            {

                d["Metropolis"] = g.Metropolis();
                das = new double[] { g.MathExpected, g.MathDisperse, g.SQotklonenie };
                math.Add("Metropolis", das);
            }
            if (d.ContainsKey("ReverseFunc"))
            {
                d["ReverseFunc"] = g.ReverseFunction();
                das = new double[] { g.MathExpected, g.MathDisperse, g.SQotklonenie };
                math.Add("ReverseFunc", das);

            }

            e.Result = d;
        }
        IChartValues dMetr;
        private ChartValues<double> dNeim;
        private ChartValues<double> dInv;
        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {


            if (e.Error != null)
            {
                // Ошибка была сгенерирована обработчиком события DoWork
                MessageBox.Show(e.Error.Message, "Произошла ошибка");
            }
            else
            {

                Dictionary<string, object> d = (Dictionary<string, object>)e.Result;
                ICollection<string> keys = d.Keys;
                foreach (string j in keys)
                    chartGenerator.addToSeria(d[j] as IEnumerable<double>, "C", j);
                ResProgres.Value = 0;
                IEnumerable<double> val;
                val = ch2.Series[1].Values.Cast<double>();
                IEnumerable<double> val1;
                
                val1 = ch2.Series[2].Values.Cast<double>();
                IEnumerable<double> val2;
                
                val2 = ch2.Series[3].Values.Cast<double>();

                dMetr = new ChartValues<double>(val);
                dNeim = new ChartValues<double>(val1);
                dInv = new ChartValues<double>(val2);
                ch2.Series[1].Values.Clear();
                cbMetr.IsChecked = true;
                cbNeim.IsChecked = true;
                cbRevs.IsChecked = true;
                Start.IsEnabled = true;
                Spbtn.IsEnabled = false;
                Main.IsEnabled = true;
                needRestartG = false;
            }
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ResProgres.Value = e.ProgressPercentage;
        }
        private void creteGraph()
        {
            chartGenerator = new ChartGenerator();
            double a = this.a.Value;
            double b = this.b.Value;
            AxisX = getAxis(NumChartPoints, 0, 1);
            chartGenerator.addToSeria(ChartGenerator.generateKumarasauDistr(NumChartPoints, a, b), "L", "Density");
            Generated = chartGenerator.Seria;
            YFormatter = value => value.ToString("F2");
            DataContext = this;
        }
        private string[] getAxis(int colums, int p1, int p2)
        {
            string[] lab = new string[colums];
            double widthInterval = (double)(p2 - p1) / colums;
            for (int iter = 0; iter < colums; iter++)
            {
                lab[iter] = (iter * widthInterval).ToString();
            }
            return lab;
        }
        public Func<double, string> YFormatter { get; set; }
        private void resetGraph()
        {
            double a = this.a.Value;
            double b = this.b.Value;
            chartGenerator.Seria.Clear();
            chartGenerator.addToSeria(ChartGenerator.generateKumarasauDistr(NumChartPoints, a, b), "L", "Density");
            Generated = chartGenerator.Seria;

        }
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            resetGraph();
            BindValues(ch2, "Generated");
            needRestartG = true;
        }
        private void BindValues(CartesianChart ch1, string p)
        {
            //   BindingOperations.ClearAllBindings(ch1);
            Binding myBinding = new Binding();
            myBinding.Source = this;
            myBinding.Path = new PropertyPath(p);
            myBinding.Mode = BindingMode.TwoWay;
            myBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            BindingOperations.SetBinding(ch1, CartesianChart.SeriesProperty, myBinding);
        }
        private void Start_Click(object sender, RoutedEventArgs e)
        {

            Dictionary<string, object> d = new Dictionary<string, object>();
            d.Add("A", a.Value);
            d.Add("B", b.Value);
            d.Add("K", int.Parse(K.Text));
            long statistic = Int64.Parse(this.Stat.Text, NumberStyles.AllowThousands);
            d.Add("Stat", statistic);
            bool somecheck = false;
            d.Add("Metropolis", new double[statistic]);
            d.Add("Neimana", new double[statistic]);
            d.Add("ReverseFunc", new double[statistic]);
            Spbtn.IsEnabled = true;
            Start.IsEnabled = false;
            Main.IsEnabled = false;
            if (needRestartG)
            {
                backgroundWorker.RunWorkerAsync(d);
                Button_Click_3(null, null);
            }


        }

        private void frezeForm()
        {

            Main.IsEnabled = tooggle(Main.IsEnabled);
            Main1.IsEnabled = tooggle(Main1.IsEnabled);
            Spbtn.IsEnabled = tooggle(Spbtn.IsEnabled);
            Stat.IsEnabled = tooggle(Stat.IsEnabled);
            Start.IsEnabled = tooggle(Start.IsEnabled);

        }

        private bool tooggle(bool p)
        {

            if (p)
            {
                return false;
            }
            else return true;
        }
        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            backgroundWorker.CancelAsync();
        }

        private void a_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            needRestartG = true;
        }

        private void b_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            needRestartG = true;
        }

        private void cbRevs_Checked(object sender, RoutedEventArgs e)
        {
            checkcxb(new ChartValues<double>(dMetr.Cast<double>()), 1);


        }
        private void checkcxb(ChartValues<double> chartValues, int p)
        {

            ch2.Series[p].Values = chartValues;        
        
        }
        private void GetMath(object sender, RoutedEventArgs e)
        {
            Window w = new Window1(math);
            w.ShowDialog();
        }

        private void cbMetr_Unchecked(object sender, RoutedEventArgs e)
        {
            unck(1);
        }

        private void unck(int p)
        {
            ch2.Series[p].Values.Clear();
        }

        private void cbNeim_Unchecked(object sender, RoutedEventArgs e)
        {
            unck(2);
        }

        private void cbRevs_Unchecked(object sender, RoutedEventArgs e)
        {
            unck(3);
        }

        private void cbINv(object sender, RoutedEventArgs e)
        {

            checkcxb(new ChartValues<double>(dInv.Cast<double>()), 3);  
        }

       

        private void cbNeimf(object sender, RoutedEventArgs e)
        {
            checkcxb(new ChartValues<double>(dNeim.Cast<double>()), 2);
      

        }

        private void Statistic_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            needRestartG = true;
        }

     

    }
}
