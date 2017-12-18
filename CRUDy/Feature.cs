using System;

namespace CRUDy
{
    public class Feature
    {
        public string Name { get; }
        public Action Workflow { get; }

        public Feature(string name, Action action)
        {
            Name = name;
            Workflow = action;
        }
    }
}
