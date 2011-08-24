using NUnit.Framework;
using ProductCatalog.Writer.Tasks;

namespace ProductCatalog.Tests.Writer.Tasks
{
    [TestFixture]
    public class TerminateTests
    {
        [Test]
        public void IsTerminalState()
        {
            Assert.IsTrue(new Terminate().IsLastTask);
        }
    }
}