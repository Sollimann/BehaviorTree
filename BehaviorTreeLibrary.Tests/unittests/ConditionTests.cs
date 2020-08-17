using NUnit.Framework;
using NUnit.Framework.Internal;
using BehaviorTreeLibrary;

namespace BehaviorTreeLibrary.Tests.unittests
{
    [TestFixture]
    public class ConditionTests
    {
        [Test]
        public void TickConditionTrueReturnsSuccess()
        {
            /*
            int health = 10;
            Condition condition = new Condition();
            condition.canRun(() =>
            {
                if (health < 50)
                {
                    return true;
                }
                return false;
            });
            condition.Tick();
            Assert.AreEqual(Status.BhSuccess, condition.Status);
            */
        }
    }
}
