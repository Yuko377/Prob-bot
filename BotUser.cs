using System.Collections.Generic;

namespace kontur_project
{
    public class BotUser
    {
        public long UserId { get; set; }
        public Stack<ICondition> UserConditions { get; set; }
        public List<Distribution> Distribution { get; set; }
        public List<string> Methods { get; set; }
        public List<double[]> Args { get; set; }

        public BotUser(long userId)
        {
            UserId = userId;
            Distribution = new List<Distribution>();
            Methods = new List<string>();
            Args = new List<double[]>();
            UserConditions = new Stack<ICondition>();
            UserConditions.Push(new StartCondition());
        }
    }
}
