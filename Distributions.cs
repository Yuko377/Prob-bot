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
        public double Density(double x);
        public double ProbabilityFunction(double x);

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

        public double Density(double x)
            => 1 / (distParams[1] - distParams[0]);


        public double ProbabilityFunction(double x)
        {
            if (x <= distParams[0])
                return 0;
            if (distParams[0] < x && x < distParams[1])
                return (x - distParams[0]) / (distParams[1] - distParams[0]);
            else
                return 1;
        }


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

        public double Density(double x)
            => distParams[0] * Math.Exp(-distParams[0] * x);

        public double ProbabilityFunction(double x)
        {
            if (x <= 0)
                return 0;
            else
                return 1 - Math.Exp(-distParams[0] * x);
        }
    }
}
