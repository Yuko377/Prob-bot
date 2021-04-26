using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kontur_project
{
    public static class AppSettings
    {
        public static Dictionary<string, Type> Repository = new Dictionary<string, Type>
        {
            {"Равномерное", typeof(UniformDistribution) },
            {"Показательное", typeof(ExpDistribution) }
        };

        public static Dictionary<long, IDistribution> MyDistributions = new Dictionary<long, IDistribution>();
        public static string Name { get; set; } = "<BOT_NAME>";
        public static string Key { get; set; } = "1748341623:AAHKn6GhefvaAAMSmY_IsdycvmM39jEARFI";
    }
}
