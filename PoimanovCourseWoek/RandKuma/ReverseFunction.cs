using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoimanovCourseWoek.RandKuma
{
    class ReverseFunction : RandomKumarasau, VichisMetodKum
    {
        System.Random r;
        public ReverseFunction(RandomKumarasau randomKumarasau)
            : base(randomKumarasau)
        {
            r = new System.Random();
        }
        public override double NextDouble()
        {
            double gamma = r.NextDouble();
            gamma = base.UnderCumulativeDistFun(base.NextDouble());
            return gamma;
        }
    }
}
