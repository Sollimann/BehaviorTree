using System;
using System.Collections.Generic;
using System.Text;

namespace BehaviorTreeLibrary
{
    public abstract class Composite : Behavior
    {
        protected List<IBehavior> Children { get; set; }

        // constructor
        protected Composite ()
        {
            Children = new List<IBehavior>();
            Initialize = () => { }; // takes in nothing, returns empty array
            Terminate = Status => { }; // takes in status, returns empty array
            Update = () => Status.BhRunning; // takes in nothing, return running
        }

        public IBehavior GetChild(int index)
        {
            return Children[index];
        }

        public int ChildCount
        {
            get { return Children.Count; }
        }
    }
}
