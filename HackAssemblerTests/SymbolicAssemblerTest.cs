using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace HackAssemblerTests
{
    [TestClass]
    [DeploymentItem(@"..\..\HackSamples\Sources", "Sources")]
    [DeploymentItem(@"..\..\HackSamples\Binaries", "Binaries")]
    public class SymbolicAssemblerTest
    {
        [TestMethod]
        public void SymbolicAssemblerConstructorTest()
        {
            Assert.IsTrue(File.Exists(@"Sources\Max.asm"));
            Assert.IsTrue(File.Exists(@"Sources\Pong.asm"));
            Assert.IsTrue(File.Exists(@"Sources\Rect.asm"));

            Assert.IsTrue(File.Exists(@"Binaries\Max.hack"));
            Assert.IsTrue(File.Exists(@"Binaries\Pong.hack"));
            Assert.IsTrue(File.Exists(@"Binaries\Rect.hack"));
        }

        [TestMethod]
        public void Max() { Utils.CompileAndCompare(); }

        [TestMethod]
        public void Pong() { Utils.CompileAndCompare(); }

        [TestMethod]
        public void Rect() { Utils.CompileAndCompare(); }
    }
}
