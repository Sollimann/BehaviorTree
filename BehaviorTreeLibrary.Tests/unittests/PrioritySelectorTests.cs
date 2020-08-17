using NUnit.Framework;
using NUnit.Framework.Internal;
using BehaviorTreeLibrary.composite;
using BehaviorTreeTests;

namespace BehaviorTreeLibrary.Tests.unittests
{
    [TestFixture]
    public class PrioritySelectorTests
    {
        [Test]
        public void TickOnSecondTickRerunsChildren()
        {
            PrioritySelector selector = new PrioritySelector();
            MockBehavior behavior = selector.Add<MockBehavior>();
            MockBehavior behavior1 = selector.Add<MockBehavior>();
            MockBehavior behavior2 = selector.Add<MockBehavior>();
            behavior.ReturnStatus = Status.BhFailure;
            behavior1.ReturnStatus = Status.BhRunning;
            behavior2.ReturnStatus = Status.BhRunning;

            selector.Tick();
            Assert.AreEqual(Status.BhRunning, selector.Status);

            behavior.ReturnStatus = Status.BhRunning;
            behavior1.ReturnStatus = Status.BhFailure;
            behavior2.ReturnStatus = Status.BhFailure;

            selector.Tick();
            Assert.AreEqual(Status.BhRunning, selector.Status);
        }

        [Test]
        public void TickOnSecondTickReturnsPreviousStatus()
        {

        }
    }
}
