using NUnit.Framework;

namespace HackAssemblerTests
{
    [TestFixture]
    public class SymbolLessAssemblerTest
    {
        [Test]
        public void Add() { Utils.CompileAndCompare(); }

        [Test]
        public void MaxL() { Utils.CompileAndCompare(); }

        [Test]
        public void PongL() { Utils.CompileAndCompare(); }

        [Test]
        public void RectL() { Utils.CompileAndCompare(); }
    }
}
