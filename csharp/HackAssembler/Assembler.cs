using System;
using System.Collections.Generic;

namespace HackAssembler
{
    public class Assembler
    {
        private Parser parser;

        private SymbolTable symbolTable;

        public List<string> HackCode { get; private set; }

        public Assembler(string sourcePath)
        {
            HackCode = new List<string>();
            symbolTable = new SymbolTable();
            parser = new Parser(sourcePath);
        }

        public void Run()
        {
            // First Pass: fill symbol table.
            int memoryAddress = 0;
            while (parser.HasMoreCommands)
            {
                parser.Advance();
                switch (parser.CurrentCommandType)
                {
                    case Parser.CommandType.A_COMMAND:
                    case Parser.CommandType.C_COMMAND:
                        memoryAddress++;
                        break;

                    case Parser.CommandType.L_COMMAND:
                        symbolTable.AddEntry(parser.Symbol, memoryAddress);
                        break;
                }
            }

            // Second Pass: generate code.
            parser.Reset();
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
                        int address;
                        bool isNumeric = int.TryParse(parser.Symbol, out address);
                        if (!isNumeric)
                        {
                            if (!symbolTable.Contains(parser.Symbol))
                                symbolTable.AddEntry(parser.Symbol);
                            address = symbolTable.GetAddress(parser.Symbol);
                        }
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
