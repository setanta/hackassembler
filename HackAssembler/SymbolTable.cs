using System.Collections.Generic;

namespace HackAssembler
{
    public class SymbolTable
    {
        private Dictionary<string, int> symbolTable;

        public SymbolTable()
        {
            // Symbol -> RAM Address
            symbolTable = new Dictionary<string, int>()
            {
                { "SP",     0x0000 },
                { "LCL",    0x0001 },
                { "ARG",    0x0002 },
                { "THIS",   0x0003 },
                { "THAT",   0x0004 },
                { "SCREEN", 0x4000 },
                { "KBD",    0x6000 }
            };
            for (var i = 0; i < 16; i++)
                symbolTable["R" + i] = i;
        }

        public void AddEntry(string symbol, int address)
        {
            symbolTable[symbol] = address;
        }

        public bool Contains(string symbol)
        {
            return symbolTable.ContainsKey(symbol);
        }

        public int GetAddress(string symbol)
        {
            int address;
            if (!symbolTable.TryGetValue(symbol, out address))
                address = -1;
            return address;
        }
    }
}
