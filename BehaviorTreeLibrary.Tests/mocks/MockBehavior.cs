using BehaviorTreeLibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace BehaviorTreeTests
{
    public class MockBehavior : Behavior
    {
        public int InitializeCalled;
        public int UpdateCalled;
        public int TerminateCalled;

        public Status ReturnStatus;
        public Status TerminateStatus;
            
        // constructor
        public MockBehavior()
        {
            InitializeCalled = 0;
            UpdateCalled = 0;
            TerminateCalled = 0;

            ReturnStatus = Status.BhRunning;
            TerminateStatus = Status.BhInvalid;

            Initialize = OnInitialize;
            Update = DoUpdate;
            Terminate = OnTerminate;
        }

        private void OnTerminate(Status obj)
        {
            ++TerminateCalled;
            TerminateStatus = obj;
        }

        private Status DoUpdate()
        {
            ++UpdateCalled;
            return ReturnStatus;
        }

        private void OnInitialize()
        {
            ++InitializeCalled;
        }
    }
}
