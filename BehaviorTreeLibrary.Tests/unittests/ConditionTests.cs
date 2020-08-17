using NUnit.Framework;
using NUnit.Framework.Internal;
using BehaviorTreeLibrary.leaf;

namespace BehaviorTreeLibrary.Tests.unittests
{
    [TestFixture]
    public class ConditionTests
    {
        [Test]
        public void TickConditionTrueReturnsSuccess()
        {

            int health = 10;
            Condition condition = new Condition
            {
                CanRun = () =>
                {
                    if (health < 50)
                    {
                        return true;
                    }
                    return false;
                }
            };
            condition.Tick();
            Assert.AreEqual(Status.BhSuccess, condition.Status);

        }

        [Test]
        public void TickConditionFalseReturnsFailure()
        {

            int health = 60;
            Condition condition = new Condition
            {
                CanRun = () =>
                {
                    if (health < 50)
                    {
                        return true;
                    }
                    return false;
                }
            };
            condition.Tick();
            Assert.AreEqual(Status.BhFailure, condition.Status);

        }
    }
}
