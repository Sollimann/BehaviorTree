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
        [Test]
        public void TickOnePassThroughReturnsResult()
        {
            Status[] status = { Status.BhSuccess, Status.BhFailure };
            for (int i = 0; i < 2; i++)
            {
                MockSequence sequence = new MockSequence(1); // one child

                // update the tree
                sequence.Tick();

                // check that the sequence is running
                Assert.AreEqual(sequence.Status, Status.BhRunning);

                // make sure the first child has not been terminated
                Assert.AreEqual(0, sequence[0].TerminateCalled);

                // set status of child
                sequence[0].ReturnStatus = status[i];

                // the next time we go through the sequence
                sequence.Tick();

                // the status of the sequnce is equal to the status
                // of the first child
                Assert.AreEqual(sequence.Status, status[i]);

                // the first child has now been terminated
                Assert.AreEqual(1, sequence[0].TerminateCalled);
            }
        }


        // If the first child fails, the whole sequence should fail
        [Test]
        public void TickTwoChildrenFailsReturnsFailure()
        {
            MockSequence sequence = new MockSequence(2);

            // update the tree
            sequence.Tick();

            // After initialization, the seqeunce has status running
            Assert.AreEqual(sequence.Status, Status.BhRunning);

            // checks that the first child in sequence has not been terminated
            Assert.AreEqual(0, sequence[0].TerminateCalled);

            // we set the status of the first child to fail
            sequence[0].ReturnStatus = Status.BhFailure;

            // the next time we go through the sequence
            sequence.Tick();

            // The status of the sequence has now failed
            Assert.AreEqual(sequence.Status, Status.BhFailure);

            // we terminated that first child
            Assert.AreEqual(1, sequence[0].TerminateCalled);

            // the second child has never been touched because
            // it has not been initialized
            Assert.AreEqual(0, sequence[1].InitializeCalled);
        }
    }
}
