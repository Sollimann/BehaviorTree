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
            Assert.AreEqual(1, sequence2[1].TerminateCalled);
        }

        /* 
         * 
         * This test validates that if a children of a sequence fails, then the selector
         * should terminate that sequence 1 and proceed to sequence 2. If the last children
         * of the second sequence fails, then the whole sequence should fail and
         * since there are no more sequences to proceed to (no more children can succeed), then
         * the whole selector will fail
         */
        [Test]
        public void TickSelectorParentSequenceChildrenSelectorFailsOnSecondSequence()
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

            // set our second child of sequence 2 to to fail
            sequence2[1].ReturnStatus = Status.BhFailure;
            selector.Tick();
            Assert.AreEqual(Status.BhFailure, selector.Status);
            Assert.AreEqual(1, sequence2[1].TerminateCalled);
        }

        /*
         * This test validates that a parent sequence needs both its children
         * selectors to succeed to be successful. Each selector has two children,
         * but only one of the children (from left to right) has to be successful 
         * for the selector to return succeess. If the first child of the selector
         * returns success, then the second child of the same selector should remain
         * invalid (un-called) and the first child that succeeded should also be terminated
         * at the same time.
         */
        [Test]
        public void TickSequenceParentSelectorChildrenSequenceSucceedes()
        {
            Sequence sequence = new Sequence();
            MockSelector selector1 = new MockSelector(2);
            MockSelector selector2 = new MockSelector(2);
            sequence.Add(selector1);
            sequence.Add(selector2);

            sequence.Tick();
            Assert.AreEqual(Status.BhRunning, sequence.Status);
            Assert.AreEqual(1, selector1[0].InitializeCalled);

            selector1[0].ReturnStatus = Status.BhSuccess;
            sequence.Tick();
            Assert.AreEqual(Status.BhRunning, sequence.Status);

            // if the first selector succeedes, then none of the
            // other childrne of the selector will be touched
            Assert.AreEqual(Status.BhInvalid, selector1[1].Status);
            Assert.AreEqual(1, selector1[0].TerminateCalled);

            // since a sequence has to go trough all of its selectors (if any)
            // if they succeed. So the first child of the second selector
            // should now be initialized
            Assert.AreEqual(1, selector2[0].InitializeCalled);

            sequence.Tick();
            Assert.AreEqual(Status.BhRunning, sequence.Status);
            Assert.AreEqual(0, selector2[0].TerminateCalled);
            Assert.AreEqual(0, selector2[1].TerminateCalled);
            Assert.AreEqual(Status.BhRunning, selector2[0].Status);

            selector2[0].ReturnStatus = Status.BhSuccess;
            sequence.Tick();
            Assert.AreEqual(Status.BhSuccess, sequence.Status);
        }

        /* 
         * This test validates that even if one (or even several) of our selector children fails,
         * then the selector can still be successful as long as at least one of the selector children
         * are successful. Since all our selectors are in a sequence, each selector will have to succeed
         * for the sequence to return successfully.
         */
        [Test]
        public void TickSequenceParentSelectorChildrenFirstChildFailsSequenceSucceedes()
        {
            Sequence sequence = new Sequence();
            MockSelector selector1 = new MockSelector(2);
            MockSelector selector2 = new MockSelector(2);
            sequence.Add(selector1);
            sequence.Add(selector2);

            sequence.Tick();
            Assert.AreEqual(Status.BhRunning, sequence.Status);
            Assert.AreEqual(1, selector1[0].InitializeCalled);

            selector1[0].ReturnStatus = Status.BhFailure;
            sequence.Tick();
            Assert.AreEqual(Status.BhRunning, sequence.Status);
            Assert.AreEqual(Status.BhRunning, selector1[1].Status);

            // if the child of the first selector fails, then
            // the second child should be running and the first
            // child should be terminated
            Assert.AreEqual(1, selector1[0].TerminateCalled);
            Assert.AreEqual(1, selector1[1].InitializeCalled);
            Assert.AreEqual(0, selector2[0].InitializeCalled);


            selector1[1].ReturnStatus = Status.BhSuccess;
            sequence.Tick();
            Assert.AreEqual(Status.BhRunning, selector2[0].Status);
            Assert.AreEqual(1, selector1[1].TerminateCalled);
            Assert.AreEqual(1, selector2[0].InitializeCalled);

            // mark the first child of the second selector as successful
            selector2[0].ReturnStatus = Status.BhSuccess;
            sequence.Tick();
            Assert.AreEqual(Status.BhSuccess, sequence.Status);

        }

        /* 
         * This test validates that even both of the children in the second
         * selector fails, then the whole selctor fails and thus the sequence
         * cannot return successfully
         */
        [Test]
        public void TickSequenceParentSelectorChildrenFirstChildFailsOnBothSequenceFails()
        {
            Sequence sequence = new Sequence();
            MockSelector selector1 = new MockSelector(2);
            MockSelector selector2 = new MockSelector(2);
            sequence.Add(selector1);
            sequence.Add(selector2);

            sequence.Tick();
            Assert.AreEqual(Status.BhRunning, sequence.Status);
            Assert.AreEqual(1, selector1[0].InitializeCalled);

            selector1[0].ReturnStatus = Status.BhFailure;
            sequence.Tick();
            Assert.AreEqual(Status.BhRunning, sequence.Status);

            // if the child of the first selector fails, then
            // the second child should be running and the first
            // child should be terminated
            Assert.AreEqual(1, selector1[1].InitializeCalled);
            Assert.AreEqual(Status.BhRunning, selector1[1].Status);
            Assert.AreEqual(1, selector1[0].TerminateCalled);

            // the first child of the second selector should not
            // yet be called
            Assert.AreEqual(0, selector2[0].InitializeCalled);


            selector1[1].ReturnStatus = Status.BhSuccess;
            sequence.Tick();
            Assert.AreEqual(Status.BhRunning, selector2[0].Status);
            Assert.AreEqual(1, selector1[1].TerminateCalled);
            Assert.AreEqual(1, selector2[0].InitializeCalled);

            // mark the first child of the second selector as failed
            selector2[0].ReturnStatus = Status.BhFailure;
            sequence.Tick();
            Assert.AreEqual(Status.BhRunning, sequence.Status);

            selector2[1].ReturnStatus = Status.BhFailure;
            sequence.Tick();
            Assert.AreEqual(Status.BhFailure, selector2.Status);
            Assert.AreEqual(Status.BhFailure, sequence.Status);
        }

        /* 
         * This test validates that our sequence will return failure
         * since both the children of our first selector in the sequence
         * fails, and therefore the sequence cannot succeed. Also we 
         * know that if the first selector fails, then the children of the
         * next selector will not be initialized
         */
        [Test]
        public void TickSequenceParentSelectorChildrenFirstSelectorFailsOnBothChildrenSequenceFails()
        {
            Sequence sequence = new Sequence();
            MockSelector selector1 = new MockSelector(2);
            MockSelector selector2 = new MockSelector(2);
            sequence.Add(selector1);
            sequence.Add(selector2);

            sequence.Tick();
            Assert.AreEqual(Status.BhRunning, sequence.Status);
            Assert.AreEqual(1, selector1[0].InitializeCalled);

            selector1[0].ReturnStatus = Status.BhFailure;
            sequence.Tick();
            Assert.AreEqual(Status.BhRunning, sequence.Status);

            // if the child of the first selector fails, then
            // the second child should be running and the first
            // child should be terminated
            Assert.AreEqual(1, selector1[1].InitializeCalled);
            Assert.AreEqual(Status.BhRunning, selector1[1].Status);
            Assert.AreEqual(1, selector1[0].TerminateCalled);

            // the first child of the second selector should not
            // yet be called
            Assert.AreEqual(0, selector2[0].InitializeCalled);


            selector1[1].ReturnStatus = Status.BhFailure;
            sequence.Tick();
            Assert.AreEqual(Status.BhFailure, sequence.Status);
            Assert.AreEqual(Status.BhInvalid, selector2[0].Status);
            Assert.AreEqual(1, selector1[1].TerminateCalled);
            Assert.AreEqual(0, selector2[0].InitializeCalled);
        }
    }
}
