using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Text;

namespace BehaviorTreeLibrary
{
    public class Behavior : IBehavior
    {
        public Action Initialize { protected get; set; }
        public Func<Status> Update { protected get; set; } // delegate for function with return type
        public Action<Status> Terminate { protected get; set; } // delegate for void function
        public IBehavior Parent { get; set; }
        public Status Status { get; set; }
        public Status Tick()
        {
            // if status if invalid and the node has not been initialized before
            // then initialize now.
            if(Status == Status.BhInvalid && Initialize != null)
            {
                Initialize();
            }

            Status = Update();

            // if not yet terminate, and the node was set to either
            // succeed, fail or invalid in the last tick, then proceed
            // to terminate the node with the current status. Also return
            // that status
            if(Status != Status.BhRunning && Terminate != null)
            {
                Terminate(Status);
            }

            return Status;
        }
    }
}
