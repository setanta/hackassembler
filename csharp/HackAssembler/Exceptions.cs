using System;

namespace HackAssembler
{
    public class ParserException : Exception
    {
        public string Text { get; private set; }

        public string SourceFile { get; private set; }

        public int LineNumber { get; private set; }

        public ParserException(string text, string sourceFile, int lineNumber)
            : base(string.Format("{0}:{1} '{2}'", sourceFile, lineNumber, text))
        {
            Text = text;
            SourceFile = sourceFile;
            LineNumber = lineNumber;
        }
    }

    public class InvalidSymbolException : ParserException
    {
        public string Symbol { get { return Text; } }

        public InvalidSymbolException(string symbol, string sourceFile, int lineNumber)
            : base(symbol, sourceFile, lineNumber)
        {
        }
    }

    public class InvalidCommandException : ParserException
    {
        public string Command { get { return Text; } }

        public InvalidCommandException(string command, string sourceFile, int lineNumber)
            : base(command, sourceFile, lineNumber)
        {
        }
    }
}
