using BehaviorTreeTests;
using System;
using System.Collections.Generic;
using System.Text;

namespace BehaviorTreeLibrary.Tests
{
    public class MockSelector : Selector
    {
        public MockBehavior(int size) : base()
        {
            for(int i = 0; i < size; i++)
            {
                children.add(new MockBehavior());
            }
        }

        public MockBehavior this [int i]
        {
            get { return Children[i] as MockBehavior; }
        }
    }
}
