using System;
using System.Collections.Generic;
using System.Text;

namespace BehaviorTreeLibrary
{
    public interface IBehavior
    {

        // attributes
        Status Status { get; set; }
        IBehavior Parent { get; set; }
        Action Initialize { set; }
        Func<Status> Update { set; }
        Action<Status> Terminate { set; }

        // functions
        Status Tick();
        void Reset();
    }
}
