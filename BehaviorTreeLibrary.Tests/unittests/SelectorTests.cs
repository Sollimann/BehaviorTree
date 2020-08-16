using NUnit.Framework;
using NUnit.Framework.Internal;
using BehaviorTreeLibrary;

namespace BehaviorTreeLibrary.Tests
{
    [TestFixture]
    public class SelectorTests
    {
        [Test]
        public void TickPassThroughToSiblingReturnTerminated()
        {
            Status[] status = { Status.BhSuccess, Status.BhFailure };
            for (int i = 0; i < 2; i++)
            {
                MockSelector selector = new MockSelector(1);

                selector.Tick();

                // selector is running
                Assert.AreEqual(selector.Status, Status.BhRunning);

                // first child has not run its TerminateCalled
                Assert.AreEqual(0, selector[0].TerminateCalled);

                // set the return status of selectors first child
                selector[0].ReturnStatus = status[i];
                selector.Tick();

                // Check that the selector status is equal to the return status
                // of the first child
                Assert.AreEqual(selector.Status, status[i]);

                // check that the first child has terminated
                Assert.AreEqual(1, selector[0].TerminateCalled);
            }
        }

        [Test]
        public void TickSelectorFirstChildFailsSecondChildRunsEndsWithSuccess()
        {
            // selector with two children
            MockSelector selector = new MockSelector(2); 

            selector.Tick();

            // Selector is running
            Assert.AreEqual(selector.Status, Status.BhRunning);

            // assert that our first child has not had its TerminateCalled
            Assert.AreEqual(0, selector[0].TerminateCalled);

            // set the first child to fail
            selector[0].ReturnStatus = Status.BhFailure;
            selector.Tick();

            // selector is running, but has has not yet succeeded
            Assert.AreEqual(selector.Status, Status.BhRunning);

            // first child terminates
            Assert.AreEqual(1, selector[0].TerminateCalled);

            // second child initialized since selector is still running
            Assert.AreEqual(1, selector[1].InitializeCalled);
        }

        [Test]
        public void TickSelectorFirstSecondNotInitializedEndsWithSuccess()
        {
            MockSelector selector = new MockSelector(2); // selector with two children

            selector.Tick();

            // Selector is running
            Assert.AreEqual(selector.Status, Status.BhRunning);

            // terminate has not been called
            Assert.AreEqual(0, selector[0].TerminateCalled);

            // selector child 1 returns success
            selector[0].ReturnStatus = Status.BhSuccess;

            selector.Tick();
            
            // Selector status returns success
            Assert.AreEqual(selector.Status, Status.BhSuccess);

            // child 1 successfully terminates
            Assert.AreEqual(1, selector[0].TerminateCalled);

            // second child is never initialized
            Assert.AreEqual(0, selector[1].InitializeCalled);
        }
    }
}
