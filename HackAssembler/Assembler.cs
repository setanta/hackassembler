using System;
using System.Collections.Generic;

namespace HackAssembler
{
    public class Assembler
    {
        private Parser parser;

        //private SymbolTable symbolTable;

        public List<string> HackCode { get; private set; }

        public Assembler(string sourcePath)
        {
            HackCode = new List<string>();
            parser = new Parser(sourcePath);
            //symbolTable = new SymbolTable();
        }

        public void Run()
        {
            HackCode.Clear();

            while (parser.HasMoreCommands)
            {
                parser.Advance();

                switch (parser.CurrentCommandType)
                {
                    case Parser.CommandType.C_COMMAND:
                        //               +--------comp-------+ +-dest--+ +-jump-+
                        // Binary: 1 1 1 a  c1 c2 c3 c4  c5 c6 d1 d2  d3 j1 j2 j3
                        var cmd = "111";
                        cmd += Code.BitArrayToString(Code.Comp(parser.Comp));
                        cmd += Code.BitArrayToString(Code.Dest(parser.Dest));
                        cmd += Code.BitArrayToString(Code.Jump(parser.Jump));
                        HackCode.Add(cmd);
                        break;

                    case Parser.CommandType.A_COMMAND:
                        //           +----value (v = 0 or 1)-----+
                        // Binary: 0 v v v v v v v v v v v v v v v
                        int address = Convert.ToInt16(parser.Symbol);
                        HackCode.Add("0" + Convert.ToString(address, 2).PadLeft(15, '0'));
                        break;
                }
            }
        }

        public bool Save(string binaryFile)
        {
            if (HackCode.Count == 0)
                return false;
            System.IO.File.WriteAllLines(binaryFile, HackCode);
            return true;
        }
    }
}
