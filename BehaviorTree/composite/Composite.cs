using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace BehaviorTreeLibrary
{

    /* 
     * All classes that inherit from composite will have children (internal node)
     */
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


        public void Add(Composite composite)
        {
            Children.Add(composite);
        }

        public T Add<T>() where T : class, IBehavior, new()
        {
            var t = new T { Parent = this };
            Children.Add(t);
            return t;
        }

        public override void Reset()
        {
            Status = Status.BhInvalid;

            // since this is a composite, and it has children
            // then we also have to reset the children
            foreach(var behavior in Children)
            {
                behavior.Reset();
            }
        }
    }
}
