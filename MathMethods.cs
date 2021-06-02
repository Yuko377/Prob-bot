using System;

namespace kontur_project
{
    static class MathMethods
    {
        public static double Integral(double a, double b, Func<double, double> f)
        {
            var step = 0.0001;
            int size = (int)((b - a) / step);
            var arg = new double[size];
            for (var i = 0; i < size - 1; i++)
                arg[i] = step * i + a;
            var sum = 0.0;
            for (var i = 1; i < size - 1; i += 2)
                sum += f(arg[i - 1]) + 4 * f(arg[i]) + f(arg[i + 1]);
            return sum * step / 3;
        }
    }
}
