
using System;

namespace kontur_project
{ 
    public abstract class Distribution
    {
        public abstract long Id { get; set; }
        public abstract string Name { get; }
        public abstract int ParamNum { get; }
        public abstract double[] distParams { get; set; }

        [NameMethod("плотность")]
        public abstract double Density(double x);

        [NameMethod("функция распределения")]
        public abstract double ProbabilityFunction(double x);

        public virtual string CheckParamsValid(params double[] parametres)
            => BaseParamsCheking(parametres);

        public string BaseParamsCheking(params double[] parametres)
            => parametres.Length != ParamNum ? "Число параметров не соответствует распределению" : null;

        internal bool GetCustomAttributes(bool v)
        {
            throw new NotImplementedException();
        }
    }
}
