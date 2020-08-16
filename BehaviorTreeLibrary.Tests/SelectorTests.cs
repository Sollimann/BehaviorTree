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
    }
}
