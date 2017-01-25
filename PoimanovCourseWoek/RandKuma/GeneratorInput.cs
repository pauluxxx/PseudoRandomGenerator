using PoimanovCourseWoek.RandKuma;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoimanovCourseWoek
{
    public delegate double NextDouble();
    class GeneratorInput
    {
        RandomKumarasau rKum;
        private int NumberOfColums = 0;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        
        private long stat;

        public GeneratorInput()
        {
            rKum = new RandomKumarasau();
         
        }
    

        public GeneratorInput(double p1, double p2):this()
        {
            rKum = new RandomKumarasau(p1,p2);
        }

        public GeneratorInput(double p1, double p2, int p3, object sender):this(p1,p2)
        {
            this.NumberOfColums = p3;
            this.backgroundWorker = sender as BackgroundWorker;
        }

        public GeneratorInput(double p1, double p2, int p3, object sender, long p4)
            : this(p1, p2, p3, sender)
        {
          
            this.stat = p4;
        }

        VichisMetodKum iMeth;
     
        internal object Neiman()
        {
            // rand and know where it gets
            object ReadyMetGisto = null;
            iMeth = new NeimanNextDoble(this.rKum);
            NextDouble handler = iMeth.NextDouble;

            ReadyMetGisto = GODispatcher( handler);
            return ReadyMetGisto;

        }
        internal object Metropolis()
        {
  
            iMeth = new MetropolisNextDouble(this.rKum);
            NextDouble handler = iMeth.NextDouble;
            return GODispatcher(handler);
        }

        internal object ReverseFunction()
        {
            iMeth = new ReverseFunction(this.rKum);
            NextDouble handler = iMeth.NextDouble;
            return GODispatcher(handler);
        }
        private IEnumerable<double> GODispatcher(NextDouble d)
        {
            int colums = NumberOfColums;
            double[] den = new double[colums];
            double[,] intervals = new double[colums, 2];
            double widthInterval = 1.0 / colums;
            double mathsum = 0.0;
            double mathsumSqr = 0.0;
            for (int i = 0; i < colums; i++)
            {
                den[i] = 0;
            }
            //int i=0,j=0;
            for (int iter = 0; iter < colums; iter++)
            {
                intervals[iter, 0] = iter * widthInterval;
                intervals[iter, 1] = (iter + 1) * widthInterval;
            } 
            //main work there
            int countExpirement = 0;
            for (countExpirement = 0; countExpirement < stat; countExpirement++)
            {
                 double item = d();
                 mathsum += item;//mathoghid
                 mathsumSqr += item * item;
                for (int j = 0; j < colums; j++)
                {
                    if (item > intervals[j, 0] && item <= intervals[j, 1])
                    {
                        den[j]++;
                        break;
                    }
                }
                if (backgroundWorker != null)
                {
                    if (backgroundWorker.CancellationPending)
                    {
                        break;
                    }
                }
                long iteration = stat / 100;
                if ((countExpirement % iteration == 0) && (backgroundWorker != null))
                {
                    if (backgroundWorker.WorkerReportsProgress)
                    {
                        //float progress = ((float)(i + 1)) / list.Length * 100;
                        int val= (int)(countExpirement/iteration);
                        backgroundWorker.ReportProgress(val);
                        //(int)Math.Round(progress));
                    }
                }

            }

            int countVal = countExpirement;
            MathExpected = mathsum / countVal;
            MathDisperse =( mathsumSqr - MathExpected * MathExpected)/(countVal-1);
            SQotklonenie = Math.Sqrt(MathDisperse/countVal);
            for (int i = 0; i < colums; i++)
            {
  //              double maxKum2 = this.rKum.findMax(0.00001, 0.999999999, 0.0000001);//is better;
//                double val = this.rKum.functionDistribKum(maxKum2);
                den[i] = (den[i]) / countVal;
                den[i] = den[i] * colums;//value for mashtab
            }
            return den;
        }





        public double MathExpected { get; set; }

        public double MathDisperse { get; set; }

        public double SQotklonenie { get; set; }
    }
}
