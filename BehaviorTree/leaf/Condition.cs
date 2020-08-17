using System;
using System.Collections.Generic;
using System.Text;

namespace BehaviorTreeLibrary.leaf
{
    public class Condition : Behavior
    {
        public Func<bool> CanRun { protected get; set; }

        // constructor
        public Condition()
        {
            Update = () =>
            {
                if (CanRun != null && CanRun())
                {
                    return Status.BhSuccess;
                }
                return Status.BhFailure;
            };
        }
    }
}
