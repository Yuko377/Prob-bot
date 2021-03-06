using System;

namespace kontur_project
{
    [AttributeUsage(AttributeTargets.Method)]
    public class NameMethodAttribute: Attribute
    {
        public readonly string methodName;

        public NameMethodAttribute(string name)
        {
            this.methodName = name;
        }

        public override string ToString()
        {
            return methodName;
        }
    }
}
