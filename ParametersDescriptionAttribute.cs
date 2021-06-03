using System;

namespace kontur_project
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ParametersDescriptionAttribute : Attribute
    {
        public readonly string description;

        public ParametersDescriptionAttribute(string description)
        {
            this.description = description;
        }

        public override string ToString()
        {
            return description.ToString();
        }
    }
}
