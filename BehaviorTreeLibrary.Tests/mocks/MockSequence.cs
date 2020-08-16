using BehaviorTreeTests;
using System;
using System.Collections.Generic;
using System.Text;

namespace BehaviorTreeLibrary.Tests.mocks
{
    public class MockSequence : Sequence
    {
        // class constructor
        // calls the base() constructor
        public MockSequence(int size) : base()
        {
            for (int i = 0; i < size; i++)
            {
                Children.Add(new MockBehavior());
            }
        }

        public MockBehavior this[int i]
        {
            get { return Children[i] as MockBehavior;  }
        }
    }
}
