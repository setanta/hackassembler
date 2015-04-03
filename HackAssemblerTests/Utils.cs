using HackAssembler;
using NUnit.Framework;
using System.IO;
using System.Runtime.CompilerServices;

namespace HackAssemblerTests
{
    public static class Utils
    {
        public static readonly string SamplesDir = "HackSamples";
        public static readonly string SourcesDir = Path.Combine(SamplesDir, "Sources");
        public static readonly string BinaryDir = Path.Combine(SamplesDir, "Binaries");

        public static void CompileAndCompare([CallerMemberName] string prog = "")
        {
            var assembler = new Assembler(Path.Combine(SourcesDir, prog + ".asm"));
            assembler.Run();
            var expected = File.ReadAllLines(Path.Combine(BinaryDir, prog + ".hack"));
            for (int i = 0; i < assembler.HackCode.Count; i++)
                Assert.AreEqual(expected[i], assembler.HackCode[i]);
        }
    }
}
