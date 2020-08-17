using System;
using System.Collections.Generic;
using System.Text;

namespace BehaviorTreeLibrary.composite
{
    public class Decorator : Composite
    {
        public Func<bool> CanRun { protected get; set; }
        public Status ReturnStatus { protected get; set; }
        // constructor
        public Decorator()
        {
            Update = () =>
            {
                if (CanRun != null && CanRun() && Children != null && Children.Count > 0)
                {
                    return Children[0].Tick();
                }
                return ReturnStatus;
            };
        }
    }
}
