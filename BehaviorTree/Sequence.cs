using System;
using System.Collections.Generic;
using System.Text;

namespace BehaviorTreeLibrary
{
    public class Sequence : Composite
    {
        private int _sequence;
        
        // constructor
        public Sequence()
        {
            Update = () =>
            {
                for (; ; )
                {
                    Status s = GetChild(_sequence).Tick();
                    if (s != Status.BhSuccess)
                    {
                        if (s == Status.BhFailure)
                        {
                            _sequence = 0;
                        }
                        return s;
                    }

                    // if after I have incremented sequence
                    if (++_sequence == ChildCount)
                    {
                        _sequence = 0;
                        return Status.BhSuccess;
                    }
                }
            };

            /*
             * If we return BhRunning, we want to always want to go back and re-initialize
             * and then work from the beginning of the list/sequence again
             * */

            Initialize = () => { _sequence = 0; };
        }
    }
}
