using HackAssembler;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Runtime.CompilerServices;

namespace HackAssemblerTests
{
    [TestClass]
    [DeploymentItem(@"..\..\HackSamples\Sources", "Sources")]
    [DeploymentItem(@"..\..\HackSamples\Binaries", "Binaries")]
    public class SymbolLessAssemblerTest
    {
        [TestMethod]
        public void ConstructorTest()
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
        public void Add() { compileAndCompare(); }

        [TestMethod]
        public void MaxL() { compileAndCompare(); }

        [TestMethod]
        public void PongL() { compileAndCompare(); }

        [TestMethod]
        public void RectL() { compileAndCompare(); }

        private static void compileAndCompare([CallerMemberName] string prog = "")
        {
            var assembler = new Assembler(@"Sources\" + prog + ".asm");
            assembler.Run();
            var expected = File.ReadAllLines(@"Binaries\" + prog + ".hack");
            for (int i = 0; i < assembler.HackCode.Count; i++)
                Assert.AreEqual(expected[i], assembler.HackCode[i]);
        }
    }
}
