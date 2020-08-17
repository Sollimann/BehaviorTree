using NUnit.Framework;
using NUnit.Framework.Internal;
using BehaviorTreeLibrary.composite;
using BehaviorTreeTests;

namespace BehaviorTreeLibrary.Tests.unittests
{
    [TestFixture]
    public class DecoratorTests
    {
        [Test]
        public void TickConditionTrueRunsChild()
        {
            int health = 10;
            Decorator decorator = new Decorator();
            MockBehavior behavior = decorator.Add<MockBehavior>();

            decorator.CanRun = () =>
            {
                if (health < 50)
                {
                    return true;
                }
                return false;
            };

            decorator.Tick();
            Assert.AreEqual(Status.BhRunning, behavior.Status);
        }

        [Test]
        public void TickConditionFalseReturnsValue()
        {
            int health = 60;
            Decorator decorator = new Decorator();
            MockBehavior behavior = decorator.Add<MockBehavior>();
            decorator.ReturnStatus = Status.BhSuccess;

            decorator.CanRun = () =>
            {
                if (health < 50)
                {
                    return true;
                }
                return false;
            };

            decorator.Tick();
            Assert.AreEqual(Status.BhInvalid, behavior.Status);
            Assert.AreEqual(Status.BhSuccess, decorator.Status);
        }
    }
}
