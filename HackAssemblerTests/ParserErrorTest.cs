using HackAssembler;
using NUnit.Framework;

namespace HackAssemblerTests
{
    [TestFixture]
    public class ParserErrorTest
    {
        [Test]
        public void InnerWhitespace()
        {
            var e = Assert.Throws<InvalidCommandException>(() => Utils.Compile());
            Assert.That(e.Command, Is.EqualTo("D= A"));
            Assert.That(e.LineNumber, Is.EqualTo(2));
        }

        [Test]
        public void InvalidCommand()
        {
            var e = Assert.Throws<InvalidCommandException>(() => Utils.Compile());
            Assert.That(e.Command, Is.EqualTo("D=D*A"));
            Assert.That(e.LineNumber, Is.EqualTo(2));
        }

        [Test]
        public void InvalidJump()
        {
            var e = Assert.Throws<InvalidCommandException>(() => Utils.Compile());
            Assert.That(e.Command, Is.EqualTo("0;jmp"));
            Assert.That(e.LineNumber, Is.EqualTo(3));
        }

        [Test]
        public void InvalidDest()
        {
            var e = Assert.Throws<InvalidCommandException>(() => Utils.Compile());
            Assert.That(e.Command, Is.EqualTo("d=M"));
            Assert.That(e.LineNumber, Is.EqualTo(2));
        }

        [Test]
        public void LowerCaseCommand()
        {
            var e = Assert.Throws<InvalidCommandException>(() => Utils.Compile());
            Assert.That(e.Command, Is.EqualTo("D=d+1"));
            Assert.That(e.LineNumber, Is.EqualTo(3));
        }

        [Test]
        public void MalformedCommand()
        {
            var e = Assert.Throws<InvalidCommandException>(() => Utils.Compile());
            Assert.That(e.Command, Is.EqualTo("0;JMP=D"));
            Assert.That(e.LineNumber, Is.EqualTo(1));
        }

        [Test]
        public void InvalidLabel()
        {
            var e = Assert.Throws<InvalidSymbolException>(() => Utils.Compile());
            Assert.That(e.Symbol, Is.EqualTo("1INFINITE_LOOP"));
            Assert.That(e.LineNumber, Is.EqualTo(1));
        }

        [Test]
        public void InvalidVariable()
        {
            var e = Assert.Throws<InvalidSymbolException>(() => Utils.Compile());
            Assert.That(e.Symbol, Is.EqualTo("1var"));
            Assert.That(e.LineNumber, Is.EqualTo(3));
        }
    }
}
