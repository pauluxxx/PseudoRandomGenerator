using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoimanovCourseWoek
{
    class RandomKumarasau: System.Random
    {

        public virtual double functionDistribKum(double x)
        {
            double f = A * B;
            f *= Math.Pow(x, (A - 1));
            f *= Math.Pow((1 - Math.Pow(x, A)), (B - 1));
            return f;
        }
        public static  double functionDistribKum(double x,double a, double b)
        {
            double f = a * b;
            f *= Math.Pow(x, (a - 1));
            f *= Math.Pow((1 - Math.Pow(x, a)), (b- 1));
            return f;
        }

        public  double A { get; set; }

        public  double B { get; set; }
        public RandomKumarasau() : base() {
            A = 0.5;
            B = 0.5;
        }
        public RandomKumarasau(double a, double b):base()
        {
            this.A = a;
            this.B = b;
        }

        public RandomKumarasau(RandomKumarasau randomKumarasau)
        {
            this.A = randomKumarasau.A;
            this.B = randomKumarasau.B;
        }
        public double UnderCumulativeDistFun(double y)
        {
            double doubeD;
            doubeD = Math.Pow((1 - Math.Pow((1 - y), 1.0 / this.B)), 1.0 / this.A);
            return doubeD;
        }
        double PHI = (1 + Math.Sqrt(5)) / 2;
        public double findMax(double a, double b, double e)
        {
            double x1, x2;
            while (true)
            {
                x1 = b - (b - a) / PHI;
                x2 = a + (b - a) / PHI;
                if (functionDistribKum(x1) <= functionDistribKum(x2))
                    a = x1;
                else
                    b = x2;
                if (Math.Abs(b - a) < e)
                    break;
            }
            return (a + b) / 2;
        }
    }
}
