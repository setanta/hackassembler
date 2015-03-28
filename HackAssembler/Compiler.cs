using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace HackAssembler
{
    public class Compiler
    {
        private Parser parser;
        //private SymbolTable symbolTable;

        public Compiler(string sourcePath)
        {
            parser = new Parser(sourcePath, "HackAssembler");
            //symbolTable = new SymbolTable();
        }

        public void run()
        {
            List<string> hackCode = new List<string>();

            while (parser.HasMoreCommands)
            {
                parser.Advance();

                switch (parser.CurrentCommandType)
                {
                    case Parser.CommandType.C_COMMAND:
                        //               +--------comp-------+ +-dest--+ +-jump-+
                        // Binary: 1 1 1 a  c1 c2 c3 c4  c5 c6 d1 d2  d3 j1 j2 j3
                        var cmd = "111";
                        cmd += (parser.CurrentSourceLine.Contains("M")) ? "1" : "0";
                        cmd += Code.BitArrayToString(Code.Comp(parser.Comp));
                        cmd += Code.BitArrayToString(Code.Dest(parser.Dest));
                        cmd += Code.BitArrayToString(Code.Jump(parser.Jump));
                        hackCode.Add(cmd);
                        break;

                    case Parser.CommandType.A_COMMAND:
                        //           +----value (v = 0 or 1)-----+
                        // Binary: 0 v v v v v v v v v v v v v v v
                        int address = Convert.ToInt16(parser.Symbol);
                        hackCode.Add("0" + Convert.ToString(address, 2).PadLeft(15, '0'));
                        break;
                }
            }
            string code = string.Join("\n", hackCode.ToArray());
            Debug.WriteLine(parser.SourceFile + " -> " + parser.SourceFile.Replace(".asm", ".hack"));
            Debug.WriteLine(code);
        }
    }
}
