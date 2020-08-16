using NUnit.Framework;
using NUnit.Framework.Internal;
using BehaviorTreeLibrary;
using BehaviorTreeLibrary.Tests.mocks;
using System;

namespace BehaviorTreeLibrary.Tests.integrationtests
{
    [TestFixture]
    public class BehaviorTreeTests
    {
        /* 
         * This test validates that in a sequence, if all of the children
         * in a sequence comes back successful. It is going to notify the
         * parent that it is successful. And that parent - since it is a selector -
         * knows that if anyone of its children succeedes, its a success and it
         * can return back to the start.
         */
        [Test]
        public void TickSelectorParentSequenceChildrenSelectorSucceedes()
        {
            Selector selector = new Selector();
            MockSequence sequence1 = new MockSequence(2);
            MockSequence sequence2 = new MockSequence(2);
            selector.Add(sequence1);
            selector.Add(sequence2);

            selector.Tick();
            Assert.AreEqual(Status.BhRunning, selector.Status);
            Assert.AreEqual(0, sequence1[0].TerminateCalled);
            Assert.AreEqual(1, sequence1[0].InitializeCalled);

            sequence1[0].ReturnStatus = Status.BhSuccess;
            selector.Tick();
            Assert.AreEqual(Status.BhRunning, selector.Status);
            Assert.AreEqual(1, sequence1[0].TerminateCalled);
            Assert.AreEqual(1, sequence1[0].InitializeCalled);

            sequence1[1].ReturnStatus = Status.BhSuccess;
            selector.Tick();
            Assert.AreEqual(Status.BhSuccess, selector.Status);
            Assert.AreEqual(1, sequence1[1].TerminateCalled);
        }

        /* 
         * 
         * This test validates that if a children of a sequence fails, then the selector
         * should terminate that sequence 1 and proceed to sequence 2. If both children
         * of the second sequence succeedes, then the whole sequence should succeed and
         * then the whole selector should succeed
         */
        [Test]
        public void TickSelectorParentSequenceChildrenSelectorSucceedesOnSecondSequence()
        {
            Selector selector = new Selector();
            MockSequence sequence1 = new MockSequence(2);
            MockSequence sequence2 = new MockSequence(2);
            selector.Add(sequence1);
            selector.Add(sequence2);

            selector.Tick();
            Assert.AreEqual(Status.BhRunning, selector.Status);
            Assert.AreEqual(0, sequence1[0].TerminateCalled);
            Assert.AreEqual(1, sequence1[0].InitializeCalled);

            sequence1[0].ReturnStatus = Status.BhFailure; // child 1 of seq 1 fails
            selector.Tick();
            Assert.AreEqual(Status.BhRunning, selector.Status);
            Assert.AreEqual(1, sequence1[0].TerminateCalled);
            Assert.AreEqual(0, sequence1[1].InitializeCalled); // second child of seq 1 is not called

            // selector fails in sequence 1, should proceed to sequence 2
            Assert.AreEqual(1, sequence2[0].InitializeCalled);

            // set the first child of sequence2 as success
            sequence2[0].ReturnStatus = Status.BhSuccess;
            selector.Tick();
            Assert.AreEqual(Status.BhRunning, selector.Status);
            Assert.AreEqual(1, sequence2[0].TerminateCalled);
            Assert.AreEqual(1, sequence2[1].InitializeCalled);

            // set our second child of sequence 2 to return success
            sequence2[1].ReturnStatus = Status.BhSuccess;
            selector.Tick();
            Assert.AreEqual(Status.BhSuccess, selector.Status);
            Assert.AreEqual(1, sequence2[0].TerminateCalled);
        }
    }
}
