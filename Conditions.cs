using System.Collections.Generic;

namespace kontur_project
{
    public interface ICondition
    {
        public List<Command> Commands { get; }
    }

    public class StartCondition : ICondition
    {
        public List<Command> Commands
        {
            get { return new List<Command> {new StartCommand()}; }
        }
    }

    public class DistributionWaitingCondition : ICondition
    {
        public List<Command> Commands
        {
            get { return new List<Command> { new DistributionReadingCommand() }; }
        }
    }

    public class DistributionParamsWaitingCondition : ICondition
    {
        public List<Command> Commands
        {
            get { return new List<Command> { new ParameterReadingCommand() }; }
        }
    }

    public class MethodWaitingCondition : ICondition
    {
        public List<Command> Commands
        {
            get { return new List<Command> { new MethodReadingCommand() }; }
        }
    }

    public class MethodArgsWaitingCondition : ICondition
    {
        public List<Command> Commands
        {
            get { return new List<Command> { new MethodArgsWaitingCommand(), new ChangesCommand(), }; }
        }
    }
}