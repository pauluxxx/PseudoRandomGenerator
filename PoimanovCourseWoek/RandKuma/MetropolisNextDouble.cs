using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoimanovCourseWoek
{
    class MetropolisNextDouble : RandomKumarasau, VichisMetodKum
    {
        System.Random r = new System.Random();
      
        public MetropolisNextDouble(RandomKumarasau randomKumarasau):base(randomKumarasau)
        {
            // TODO: Complete member initialization
            max = base.findMax(0.000001, 0.999999999, 0.000001);
        }
        public override double functionDistribKum(double x) {
            if (x < 0 || x > 1) { return 0; } else return base.functionDistribKum(x);
        }
        double max;
        public override double NextDouble()
        {
            double metrop = 0;
            while (true)
            {
                double xLast = max;
                double xCur = xLast + (2 * r.NextDouble() - 1);

                double alpha = functionDistribKum(xCur) / functionDistribKum(xLast);
                if (alpha >= 1)
                {
                    metrop = xCur;
                    break;
                }
                else if (alpha > r.NextDouble())
                {
                    metrop = xCur;
                    break;
                }
                       
            }
            
            
            return metrop;
        }
    }
}
