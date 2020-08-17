using NUnit.Framework;
using NUnit.Framework.Internal;
using BehaviorTreeLibrary.composite;
using BehaviorTreeTests;
using BehaviorTreeLibrary.Tests.mocks;

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
        public void TickOnSecondTickResetSequences()
        {
            PrioritySelector selector = new PrioritySelector();
            MockSequence sequence = new MockSequence(2); // sequence with two children
            MockSequence sequence1 = new MockSequence(2);

            selector.Add(sequence);
            selector.Add(sequence1);

            selector.Tick();
            Assert.AreEqual(Status.BhRunning, selector.Status);
            sequence[0].ReturnStatus = Status.BhSuccess;
            sequence[1].ReturnStatus = Status.BhFailure;

            selector.Tick();
            Assert.AreEqual(Status.BhRunning, selector.Status);
            Assert.AreEqual(Status.BhRunning, sequence1.Status);

            sequence1[0].ReturnStatus = Status.BhSuccess;
            sequence1[1].ReturnStatus = Status.BhFailure;

            selector.Tick();
            Assert.AreEqual(Status.BhFailure, selector.Status);
            Assert.AreEqual(Status.BhFailure, sequence1.Status);


            sequence1[0].ReturnStatus = Status.BhSuccess;
            sequence1[1].ReturnStatus = Status.BhRunning;
            selector.Tick();
            Assert.AreEqual(Status.BhRunning, selector.Status);
            Assert.AreEqual(Status.BhRunning, sequence1.Status);

            sequence[0].ReturnStatus = Status.BhRunning;
            selector.Tick();
            Assert.AreEqual(Status.BhRunning, selector.Status);
            Assert.AreEqual(Status.BhInvalid, sequence1.Status);
        }
    }
}