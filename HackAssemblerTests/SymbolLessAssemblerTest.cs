using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace HackAssemblerTests
{
    [TestClass]
    [DeploymentItem(@"..\..\HackSamples\Sources", "Sources")]
    [DeploymentItem(@"..\..\HackSamples\Binaries", "Binaries")]
    public class SymbolLessAssemblerTest
    {
        [TestMethod]
        public void SymbolLessAssemblerConstructorTest()
        {
            Assert.IsTrue(File.Exists(@"Sources\Add.asm"));
            Assert.IsTrue(File.Exists(@"Sources\MaxL.asm"));
            Assert.IsTrue(File.Exists(@"Sources\PongL.asm"));
            Assert.IsTrue(File.Exists(@"Sources\RectL.asm"));

            Assert.IsTrue(File.Exists(@"Binaries\Add.hack"));
            Assert.IsTrue(File.Exists(@"Binaries\MaxL.hack"));
            Assert.IsTrue(File.Exists(@"Binaries\PongL.hack"));
            Assert.IsTrue(File.Exists(@"Binaries\RectL.hack"));
        }

        [TestMethod]
        public void Add() { Utils.CompileAndCompare(); }

        [TestMethod]
        public void MaxL() { Utils.CompileAndCompare(); }

        [TestMethod]
        public void PongL() { Utils.CompileAndCompare(); }

        [TestMethod]
        public void RectL() { Utils.CompileAndCompare(); }
    }
}
