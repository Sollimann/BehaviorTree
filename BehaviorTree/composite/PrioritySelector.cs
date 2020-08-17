using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BehaviorTreeLibrary.composite
{
    public class PrioritySelector : Selector
    {
        private int _lastSelector;
        
        public PrioritySelector()
        {
            Update = () =>
            {
                _selector = 0; // reset ourselves
                for (; ; ) // could use a while instead
                {
                    Status s = GetChild(_selector).Tick();
                    if (s != Status.BhFailure)
                    {
                        for (int i = _selector+1; i <= _lastSelector; i++)
                        {
                            GetChild(i).Reset();
                        }
                        _lastSelector = _selector;
                        return s;
                    }
                    if(++_selector == ChildCount)
                    {
                        return Status.BhFailure;
                    }
                }
            };
        }
    }
}
