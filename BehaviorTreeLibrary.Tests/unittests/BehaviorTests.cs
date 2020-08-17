using NUnit.Framework;
using NUnit.Framework.Internal;
using BehaviorTreeLibrary;

namespace BehaviorTreeTests
{
    [TestFixture]
    public class BehaviorTests
    {
      [Test]
      public void TickInitializeSuccessfully()
        {
            MockBehavior t = new MockBehavior();

            Assert.AreEqual(0, t.InitializeCalled);

            t.Tick();

            Assert.AreEqual(1, t.InitializeCalled);
        }

      [Test]
      public void TickUpdateCalledReturnsSuccess()
        {
            MockBehavior t = new MockBehavior();

            t.Tick();
            Assert.AreEqual(1, t.UpdateCalled);

            t.ReturnStatus = Status.BhSuccess;

            t.Tick();
            Assert.AreEqual(2, t.UpdateCalled);
        }
       [Test]
       public void TickTerminateCalledReturnsSuccess()
        {
            MockBehavior t = new MockBehavior();

            t.Tick();
            Assert.AreEqual(0, t.TerminateCalled);

            t.ReturnStatus = Status.BhSuccess;
            t.Tick();
            Assert.AreEqual(1, t.TerminateCalled);
        }
    }
      


}
