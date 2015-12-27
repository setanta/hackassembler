using System.Collections.Generic;
using System.Linq;

namespace HackAssembler
{
    public class SymbolTable
    {
        private Dictionary<string, int> symbolTable;
        private int nextMemoryPos;

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
            nextMemoryPos = 16;
        }

        public void AddEntry(string symbol, int address = -1)
        {
            if (address == -1)
            {
                address = nextMemoryPos;
                nextMemoryPos++;
            }
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

        public override string ToString()
        {
            string str = "<SymbolTable>";
            int lastValue = -1;
            foreach (var item in from e in symbolTable orderby e.Value ascending select e)
            {
                if (item.Value == lastValue)
                {
                    str += ", ";
                }
                else
                {
                    lastValue = item.Value;
                    str += string.Format("\n0x{0:X4}: ", item.Value);
                }
                str += item.Key;
            }
            return str;
        }
    }
}
