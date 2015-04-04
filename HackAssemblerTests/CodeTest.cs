using HackAssembler;
using NUnit.Framework;

namespace HackAssemblerTests
{
    [TestFixture]
    public class CodeTest
    {
        [Test]
        public void DestCodes()
        {
            Assert.That(Code.BitArrayToString(Code.Dest(null)), Is.EqualTo("000"));
            Assert.That(Code.BitArrayToString(Code.Dest("M")), Is.EqualTo("001"));
            Assert.That(Code.BitArrayToString(Code.Dest("D")), Is.EqualTo("010"));
            Assert.That(Code.BitArrayToString(Code.Dest("MD")), Is.EqualTo("011"));
            Assert.That(Code.BitArrayToString(Code.Dest("A")), Is.EqualTo("100"));
            Assert.That(Code.BitArrayToString(Code.Dest("AM")), Is.EqualTo("101"));
            Assert.That(Code.BitArrayToString(Code.Dest("AD")), Is.EqualTo("110"));
            Assert.That(Code.BitArrayToString(Code.Dest("AMD")), Is.EqualTo("111"));
        }

        [Test]
        public void JumpCodes()
        {
            Assert.That(Code.BitArrayToString(Code.Jump(null)), Is.EqualTo("000"));
            Assert.That(Code.BitArrayToString(Code.Jump("JGT")), Is.EqualTo("001"));
            Assert.That(Code.BitArrayToString(Code.Jump("JEQ")), Is.EqualTo("010"));
            Assert.That(Code.BitArrayToString(Code.Jump("JGE")), Is.EqualTo("011"));
            Assert.That(Code.BitArrayToString(Code.Jump("JLT")), Is.EqualTo("100"));
            Assert.That(Code.BitArrayToString(Code.Jump("JNE")), Is.EqualTo("101"));
            Assert.That(Code.BitArrayToString(Code.Jump("JLE")), Is.EqualTo("110"));
            Assert.That(Code.BitArrayToString(Code.Jump("JMP")), Is.EqualTo("111"));
        }

        [Test]
        public void CompCodes()
        {
            Assert.That(Code.BitArrayToString(Code.Comp("0")), Is.EqualTo("0101010"));
            Assert.That(Code.BitArrayToString(Code.Comp("1")), Is.EqualTo("0111111"));
            Assert.That(Code.BitArrayToString(Code.Comp("-1")), Is.EqualTo("0111010"));
            Assert.That(Code.BitArrayToString(Code.Comp("D")), Is.EqualTo("0001100"));
            Assert.That(Code.BitArrayToString(Code.Comp("A")), Is.EqualTo("0110000"));
            Assert.That(Code.BitArrayToString(Code.Comp("M")), Is.EqualTo("1110000"));
            Assert.That(Code.BitArrayToString(Code.Comp("!D")), Is.EqualTo("0001101"));
            Assert.That(Code.BitArrayToString(Code.Comp("!A")), Is.EqualTo("0110001"));
            Assert.That(Code.BitArrayToString(Code.Comp("!M")), Is.EqualTo("1110001"));
            Assert.That(Code.BitArrayToString(Code.Comp("-D")), Is.EqualTo("0001111"));
            Assert.That(Code.BitArrayToString(Code.Comp("-A")), Is.EqualTo("0110011"));
            Assert.That(Code.BitArrayToString(Code.Comp("-M")), Is.EqualTo("1110011"));
            Assert.That(Code.BitArrayToString(Code.Comp("D+1")), Is.EqualTo("0011111"));
            Assert.That(Code.BitArrayToString(Code.Comp("A+1")), Is.EqualTo("0110111"));
            Assert.That(Code.BitArrayToString(Code.Comp("M+1")), Is.EqualTo("1110111"));
            Assert.That(Code.BitArrayToString(Code.Comp("D-1")), Is.EqualTo("0001110"));
            Assert.That(Code.BitArrayToString(Code.Comp("A-1")), Is.EqualTo("0110010"));
            Assert.That(Code.BitArrayToString(Code.Comp("M-1")), Is.EqualTo("1110010"));
            Assert.That(Code.BitArrayToString(Code.Comp("D+A")), Is.EqualTo("0000010"));
            Assert.That(Code.BitArrayToString(Code.Comp("D+M")), Is.EqualTo("1000010"));
            Assert.That(Code.BitArrayToString(Code.Comp("D-A")), Is.EqualTo("0010011"));
            Assert.That(Code.BitArrayToString(Code.Comp("D-M")), Is.EqualTo("1010011"));
            Assert.That(Code.BitArrayToString(Code.Comp("A-D")), Is.EqualTo("0000111"));
            Assert.That(Code.BitArrayToString(Code.Comp("M-D")), Is.EqualTo("1000111"));
            Assert.That(Code.BitArrayToString(Code.Comp("D&A")), Is.EqualTo("0000000"));
            Assert.That(Code.BitArrayToString(Code.Comp("D&M")), Is.EqualTo("1000000"));
            Assert.That(Code.BitArrayToString(Code.Comp("D|A")), Is.EqualTo("0010101"));
            Assert.That(Code.BitArrayToString(Code.Comp("D|M")), Is.EqualTo("1010101"));
        }
    }
}
