using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace BehaviorTreeTests
{
    [TestFixture]
    public class BehaviorTests
    {
      [Test]
      public void InitializeSuccessfully()
        {
            MockBehavior t = new MockBehavior();

            Assert.AreEqual(0, t._iInitializeCalled);

            t.Tick();

        }
    }
}
