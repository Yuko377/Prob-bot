using System.Linq;
using System.Globalization;
using System.Collections.Generic;

namespace kontur_project
{
    public static class InputsParser
    {
        public static double[] Parse(string text)
        {
            var currParams = text.Split(' ').Where(x => x != "").ToArray();

            var doubleParams = new List<double>(currParams.Length);

            foreach (var el in currParams)
            {
                var newEl = el.Replace(',', '.');
                if (!double.TryParse(newEl, NumberStyles.Any, CultureInfo.InvariantCulture, out var doubleParameter))
                {
                    return null;
                }

                doubleParams.Add(doubleParameter);
            }
            return doubleParams.ToArray();
        }
    }
}