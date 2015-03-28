using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackAssembler
{
    public class Bits
    {
        private byte[] bits;

        public Bits(int length)
        {
            bits = new byte[length];
        }

        public override string ToString()
        {
            var str = "";
            foreach (byte b in bits)
                str += b.ToString();
            return str;
        }
    }
}
