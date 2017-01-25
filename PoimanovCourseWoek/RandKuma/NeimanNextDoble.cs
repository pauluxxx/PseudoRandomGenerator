using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoimanovCourseWoek
{
    class NeimanNextDoble : RandomKumarasau, VichisMetodKum
    {
        System.Random r;

        public NeimanNextDoble(RandomKumarasau randomKumarasau):base(randomKumarasau)
        {
            r=new System.Random();
            max= findMax(0.000001, 0.999999999, 0.0000001);//is better;
            max = functionDistribKum(max);
        }
        double max;
        public override double NextDouble()
        {
            double gamma = base.NextDouble();
        
            while (true)
            {
                double rX = base.NextDouble();
                double rY = base.NextDouble() * (max - 0);
                if (functionDistribKum(rX) > rY)
                {
                    //it is the right value
                    gamma = rX;
                    break;
                }
            }
            return gamma;
        }
       
    }
}
