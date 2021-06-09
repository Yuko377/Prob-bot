using System.Collections.Generic;

namespace kontur_project
{
    public class BotUser
    {
        public long UserId { get; set; }
        public List<ICondition> UserConditions { get; set; }
        public List<Distribution> Distributions { get; set; }
        public List<string> Methods { get; set; }
        public List<double[]> Args { get; set; }

        public BotUser(long userId)
        {
            UserId = userId;
            Distributions = new List<Distribution>();
            Methods = new List<string>();
            Args = new List<double[]>();
            UserConditions = new List<ICondition>();
            UserConditions.Add(new StartCondition());
        }
    }
}
