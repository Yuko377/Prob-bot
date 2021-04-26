using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kontur_project
{ 
    public interface IDistribution
    {
        public long Id { get; set; }
        public string Name { get; }
        public int ParamNum { get; set; }
        public List<double> distParams { get; set; }
        public string Method1();
        public string Method2();
        public string Method3();

    }
    class UniformDistribution : IDistribution
    {
        public long Id { get; set; }
        public int ParamNum { get; set; } 
        public string Name => "Равномерное";

        public UniformDistribution()
        {
            ParamNum = 2;
        }
        public List<double> distParams { get; set; }

        public string Method1()
            => "Метод 1 равномерного распределения - 1 параметр " + distParams[0].ToString();


        public string Method2()
            => "Метод 2 равномерного распределения";


        public string Method3()
            => "Метод 3 равномерного распределения";
    }

    public class ExpDistribution : IDistribution
    {
        public long Id { get; set; }

        public int ParamNum { get; set; }

        public string Name => "Показательное";

        public ExpDistribution()
        {
            ParamNum = 1;
        }

        public List<double> distParams { get; set; }

        public string Method1()
            => "Метод 1 показательного распределения";

        public string Method2()
            => "Метод 2 показательного распределения";

        public string Method3()
            => "Метод 3 показательного распределения";

        public string Method4()
            => "Метод 4 показательного распределения";

    }
}
