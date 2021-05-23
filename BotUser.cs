using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace kontur_project
{
    public class BotUser
    {
        public long UserId { get; set; }
        public Stack<ICondition> UserConditions { get; set; }
        public List<IDistribution> Distribution { get; set; }
        public List<string> Methods { get; set; }
        public List<double> Args { get; set; }

        public BotUser(long userId)
        {
            UserId = userId;
            Distribution = new List<IDistribution>();
            Methods = new List<string>();
            Args = new List<double>();
            UserConditions = new Stack<ICondition>();
            UserConditions.Push(new StartCondition());
        }
    }
}
