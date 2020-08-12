using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;

namespace BehaviorTreeLibrary
{
    public class Behavior : IBehavior
    {
        public Action Initialize { protected get; set; }
        public Status Status { get; set; }
        public Status Tick()
        {
            if(Status == Status.BhInvalid && Initialize != null)
            {
                Initialize();
            }

            return Status;
        }
    }
}
