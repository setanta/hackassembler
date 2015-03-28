using System;

namespace HackAssembler
{
    // Translates Hack assembly language mnemonics into binary codes.
    public static class Code
    {
        // Returns the binary code of the dest mnemonic.
        // dest    d1 d2 d3
        // null    0  0  0
        // M       0  0  1
        // D       0  1  0
        // MD      0  1  1
        // A       1  0  0
        // AM      1  0  1
        // AD      1  1  0
        // AMD     1  1  1
        public static byte[] Dest(string mnemonic)
        {
            if (string.IsNullOrEmpty(mnemonic))
                return new byte[3];
            return new byte[] {
                Convert.ToByte(mnemonic.Contains("A")),
                Convert.ToByte(mnemonic.Contains("D")),
                Convert.ToByte(mnemonic.Contains("M"))
            };
        }

        // Returns the binary code of the jump mnemonic.
        // jump    j1 j2 j3
        // null    0  0  0
        // JGT     0  0  1
        // JEQ     0  1  0
        // JGE     0  1  1
        // JLT     1  0  0
        // JNE     1  0  1
        // JLE     1  1  0
        // JMP     1  1  1
        public static byte[] Jump(string mnemonic)
        {
            switch (mnemonic)
            {
                case "JGT":
                    return new byte[] { 0, 0, 1 };

                case "JEQ":
                    return new byte[] { 0, 1, 0 };

                case "JGE":
                    return new byte[] { 0, 1, 1 };

                case "JLT":
                    return new byte[] { 1, 0, 0 };

                case "JNE":
                    return new byte[] { 1, 0, 1 };

                case "JLE":
                    return new byte[] { 1, 1, 0 };

                case "JMP":
                    return new byte[] { 1, 1, 1 };
            }
            return new byte[3];
        }

        // Returns the binary code of the comp mnemonic.

        // C-instruction: dest=comp;jump
        // Either the dest or jump fields may be empty.
        // If dest is empty, the "=" is omitted;
        // If jump is empty, the ";" is omitted.
        //               +--------comp-------+ +-dest--+ +-jump-+
        // Binary: 1 1 1 a  c1 c2 c3 c4  c5 c6 d1 d2  d3 j1 j2 j3
        //
        //       comp                       comp
        // (when a=0)   c1 c2 c3 c4 c5 c6   (when a=1)
        // -------------------------------------------
        //          0   1  0  1  0  1  0
        //          1   1  1  1  1  1  1
        //         -1   1  1  1  0  1  0
        //          D   0  0  1  1  0  0
        //          A   1  1  0  0  0  0    M
        //         !D   0  0  1  1  0  1
        //         !A   1  1  0  0  0  1    !M
        //         -D   0  0  1  1  1  1
        //         -A   1  1  0  0  1  1    -M
        //        D+1   0  1  1  1  1  1
        //        A+1   1  1  0  1  1  1    M+1
        //        D-1   0  0  1  1  1  0
        //        A-1   1  1  0  0  1  0    M-1
        //        D+A   0  0  0  0  1  0    D+M
        //        D-A   0  1  0  0  1  1    D-M
        //        A-D   0  0  0  1  1  1    M-D
        //        D&A   0  0  0  0  0  0    D&M
        //        D|A   0  1  0  1  0  1    D|M
        public static byte[] Comp(string mnemonic)
        {
            switch (mnemonic)
            {
                case "0":
                    return new byte[] { 1, 0, 1, 0, 1, 0 };

                case "1":
                    return new byte[] { 1, 1, 1, 1, 1, 1 };

                case "-1":
                    return new byte[] { 1, 1, 1, 0, 1, 0 };

                case "D":
                    return new byte[] { 0, 0, 1, 1, 0, 0 };

                case "A":
                case "M":
                    return new byte[] { 1, 1, 0, 0, 0, 0 };

                case "!D":
                    return new byte[] { 0, 0, 1, 1, 0, 1 };

                case "!A":
                case "!M":
                    return new byte[] { 1, 1, 0, 0, 0, 1 };

                case "-D":
                    return new byte[] { 0, 0, 1, 1, 1, 1 };

                case "-A":
                case "-M":
                    return new byte[] { 1, 1, 0, 0, 1, 1 };

                case "D+1":
                    return new byte[] { 0, 1, 1, 1, 1, 1 };

                case "A+1":
                case "M+1":
                    return new byte[] { 1, 1, 0, 1, 1, 1 };

                case "D-1":
                    return new byte[] { 0, 0, 1, 1, 1, 0 };

                case "A-1":
                case "M-1":
                    return new byte[] { 1, 1, 0, 0, 1, 0 };

                case "D+A":
                case "D+M":
                    return new byte[] { 0, 0, 0, 0, 1, 0 };

                case "D-A":
                case "D-M":
                    return new byte[] { 0, 1, 0, 0, 1, 1 };

                case "A-D":
                case "M-D":
                    return new byte[] { 0, 0, 0, 1, 1, 1 };

                case "D&A":
                case "D&M":
                    return new byte[] { 0, 0, 0, 0, 0, 0 };

                case "D|A":
                case "D|M":
                    return new byte[] { 0, 1, 0, 1, 0, 1 };
            }
            return new byte[7];
        }

        public static string BitArrayToString(byte[] bitArray)
        {
            string str = "";
            if (bitArray == null || bitArray.Length == 0)
                return str;

            foreach (byte b in bitArray)
                str += b.ToString();

            return str;
        }
    }
}
