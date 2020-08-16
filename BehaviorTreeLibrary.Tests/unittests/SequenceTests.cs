using NUnit.Framework;
using NUnit.Framework.Internal;
using BehaviorTreeLibrary;
using BehaviorTreeLibrary.Tests.mocks;

namespace BehaviorTreeLibrary.Tests.unittests
{
    [TestFixture]
    public class SequenceTests
    {

        // if our child returns successful, our sequence should return successful
        // if our child returns false, our sequence should return false
        public void TickOnePassThroughReturnsResult()
        {
            Status[] status = { Status.BhSuccess, Status.BhFailure };
            for (int i = 0; i < 2; i++)
            {
                MockSequence sequence = new MockSequence(1); // one child


                sequence.Tick();
                Assert.AreEqual(sequence.Status, Status.BhRunning);
                Assert.AreEqual(0, sequence[0].TerminateCalled);

                sequence[0].ReturnStatus = status[i];
                sequence.Tick();
                Assert.AreEqual(sequence.Status, status[i]);
                Assert.AreEqual(1, sequence[0].TerminateCalled);
            }
        }
    }
}
