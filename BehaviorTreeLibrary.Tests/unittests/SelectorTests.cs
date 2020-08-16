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
                Assert.AreEqual(selector.Status, Status.BhRunning);
                Assert.AreEqual(0, selector[0].TerminateCalled);

                selector[0].ReturnStatus = status[i];
                selector.Tick();
                Assert.AreEqual(selector.Status, status[i]);
                Assert.AreEqual(1, selector[0].TerminateCalled);
            }
        }

        [Test]
        public void TickSelectorFirstChildFailsSecondChildRunsEndsWithSuccess()
        {
            // selector with two children
            MockSelector selector = new MockSelector(2); 

            selector.Tick();
            Assert.AreEqual(selector.Status, Status.BhRunning);

            // assert that our first child has not had its TerminateCalled
            Assert.AreEqual(0, selector[0].TerminateCalled);

            // set the first child to fail
            selector[0].ReturnStatus = Status.BhFailure;
            selector.Tick();
            Assert.AreEqual(selector.Status, Status.BhRunning);
            Assert.AreEqual(1, selector[0].TerminateCalled);
            Assert.AreEqual(1, selector[1].InitializeCalled);
        }
    }
}
