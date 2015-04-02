using HackAssembler;
using NUnit.Framework;
using System.IO;
using System.Runtime.CompilerServices;

namespace HackAssemblerTests
{
    public static class Utils
    {
        public static void CompileAndCompare([CallerMemberName] string prog = "")
        {
            var assembler = new Assembler(@"HackSamples\Sources\" + prog + ".asm");
            assembler.Run();
            var expected = File.ReadAllLines(@"HackSamples\Binaries\" + prog + ".hack");
            for (int i = 0; i < assembler.HackCode.Count; i++)
                Assert.AreEqual(expected[i], assembler.HackCode[i]);
        }
    }
}
