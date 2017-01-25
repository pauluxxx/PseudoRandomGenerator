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
using System.Windows.Shapes;

namespace PoimanovCourseWoek
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private Dictionary<string, double[]> math;

        public Window1()
        {

            InitializeComponent();
        }

        public Window1(Dictionary<string, double[]> math):this()
        {
            // TODO: Complete member initialization
            this.math = math;
            double[] das = math["Metropolis"];
            _1.Content = das[0];
            _2.Content = das[1];
            _3.Content = das[2];
             das = math["Neimana"];
            _4.Content = das[0];
            _5.Content = das[1];
            _6.Content = das[2];
            das = math["ReverseFunc"];
            _7.Content = das[0];
            _8.Content = das[1];
            _9.Content = das[2];

        }
    }
}
