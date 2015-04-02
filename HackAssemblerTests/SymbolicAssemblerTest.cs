using NUnit.Framework;

namespace HackAssemblerTests
{
    [TestFixture]
    public class SymbolicAssemblerTest
    {
        [Test]
        public void Max() { Utils.CompileAndCompare(); }

        [Test]
        public void Pong() { Utils.CompileAndCompare(); }

        [Test]
        public void Rect() { Utils.CompileAndCompare(); }
    }
}
