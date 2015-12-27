using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace HackAssembler
{
    // Encapsulates access to the input code. Reads an assembly language command,
    // parses it, and provides convenient access to the command’s components
    // (fields and symbols). In addition, removes all white space and comments.
    public class Parser
    {
        private string[] sourceLines;
        private int currentLine;

        public enum CommandType
        {
            UNKNOWN_COMMAND,
            A_COMMAND,          // Addressing instruction for "@Xxx" (A-instruction)
            C_COMMAND,          // Compute instruction for "dest=comp;jump" (C-instruction)
            L_COMMAND           // Pseucocommand for "(Xxx)" (L-instruction)
        }

        public Parser(string sourceFile)
        {
            SourceFile = sourceFile;
            Reset();
            sourceLines = getSourceFile(sourceFile);
        }

        public string SourceFile { get; private set; }

        public string CurrentSourceLine
        {
            get
            {
                if (sourceLines.Length == 0 || currentLine == sourceLines.Length || currentLine < 0)
                    return "";
                return sourceLines[currentLine];
            }
        }

        // Are there more commands in the input?
        public bool HasMoreCommands
        {
            get
            {
                return sourceLines.Length != 0 && currentLine < sourceLines.Length - 1;
            }
        }

        // Returns the type of the current command:
        // * UNKNOWN_COMMAND for malformed lines
        // * A_COMMAND for @Xxx where Xxx is either a symbol or a decimal number
        // * C_COMMAND for dest=comp;jump
        // * L_COMMAND (actually, pseudocommand) for (Xxx) where Xxx is a symbol.
        public CommandType CurrentCommandType { get; private set; }

        // Returns the symbol or decimal Xxx of the current command @Xxx or (Xxx).
        // Should be called only when CurrentCommandType is A_COMMAND or L_COMMAND.
        public string Symbol { get; private set; }

        // Returns the dest mnemonic in the current C-command (8 possibilities).
        // Should be called only when CurrentCommandType is C_COMMAND.
        public string Dest { get; private set; }

        // Returns the comp mnemonic in the current C-command (28 possibilities).
        // Should be called only when CurrentCommandType is C_COMMAND
        public string Comp { get; private set; }

        // Returns the jump mnemonic in the current C-command (8 possibilities).
        // Should be called only when CurrentCommandType is C_COMMAND.
        public string Jump { get; private set; }

        // Reads the next command from the input and makes it the current
        // command. Should be called only if HasMoreCommands is true.
        // Initially there is no current command.
        public void Advance()
        {
            if (!HasMoreCommands)
                throw new Exception("There ain't no more commands.");

            string line;
            while ((line = removeComment(sourceLines[++currentLine])) == "") { }

            Symbol = Dest = Comp = Jump = null;

            if (line.StartsWith("@"))
            {
                CurrentCommandType = CommandType.A_COMMAND;
                var address = line.Substring(1);
                if (!isValidSymbol(address) && !isValidConstant(address))
                    throw new InvalidSymbolException(address, SourceFile, currentLine + 1);
                Symbol = address;
            }
            else if (line.StartsWith("(") && line.EndsWith(")"))
            {
                CurrentCommandType = CommandType.L_COMMAND;
                var label = line.Substring(1, line.Length - 2).Trim();
                if (!isValidSymbol(label))
                    throw new InvalidSymbolException(label, SourceFile, currentLine + 1);
                Symbol = label;
            }
            else
            {
                // DEST=COMP;JUMP
                CurrentCommandType = CommandType.C_COMMAND;

                // DEST=comp;jump
                var cmdParts = line.Split(new Char[] { '=' });
                if (cmdParts.Length == 2)
                {
                    if (!isValidDest(cmdParts[0]))
                        throw new InvalidCommandException(sourceLines[currentLine], SourceFile, currentLine + 1);
                    Dest = cmdParts[0];
                    line = cmdParts[1];
                }

                // dest=comp;JUMP
                cmdParts = line.Split(new Char[] { ';' });
                if (cmdParts.Length == 2)
                {
                    if (!isValidJump(cmdParts[1]))
                        throw new InvalidCommandException(sourceLines[currentLine], SourceFile, currentLine + 1);
                    Jump = cmdParts[1];
                    line = cmdParts[0];
                }

                if (isValidCommand(line))
                {
                    Comp = line;
                }
                else
                {
                    Dest = Jump = null;
                    throw new InvalidCommandException(sourceLines[currentLine], SourceFile, currentLine + 1);
                }
            }
        }

        // Restarts pointer to source code lines.
        public void Reset()
        {
            currentLine = -1;
            CurrentCommandType = CommandType.UNKNOWN_COMMAND;
            Symbol = Dest = Comp = Jump = null;
        }

        // A user-defined symbol can be any sequence of letters, digits, underscore (_),
        // dot (.), dollar sign ($), and colon (:) that does not begin with a digit.
        private static bool isValidSymbol(string symbol)
        {
            return Regex.IsMatch(symbol, @"^[a-zA-Z_.$:][\w.$:]*$");
        }

        // Constants must be non-negative and are written in decimal notation.
        private static bool isValidConstant(string constant)
        {
            return Regex.IsMatch(constant, @"^\d+$");
        }

        // Valid Commands:
        // 0, 1, -1, D, A, M, !D, !A, !M, -D, -A, -M
        // D+1, A+1, M+1, D-1, A-1, M-1, D+A, D+M, D-A, D-M
        // A-D, M-D, D&A, D&M, D|A, D|M
        private static bool isValidCommand(string command)
        {
            return Regex.IsMatch(command, @"^(0|[-]?1|[-!]?[DAM]|[DAM][+-]1|D[+\-\&\|][AM]|[AM]-D)$");
        }

        private static bool isValidDest(string dest)
        {
            switch (dest)
            {
                case "M":
                case "D":
                case "MD":
                case "A":
                case "AM":
                case "AD":
                case "AMD":
                    return true;
            }
            return false;
        }

        private static bool isValidJump(string jump)
        {
            switch (jump)
            {
                case "JGT":
                case "JEQ":
                case "JGE":
                case "JLT":
                case "JNE":
                case "JLE":
                case "JMP":
                    return true;
            }
            return false;
        }

        private static string removeComment(string line)
        {
            var pos = line.IndexOf("//");
            if (pos > -1)
                line = line.Substring(0, pos).Trim();
            return line;
        }

        private static string[] getSourceFile(string sourceFile)
        {
            if (!File.Exists(sourceFile))
                return new string[0];

            List<string> lines = new List<string>();
            using (var reader = new StreamReader(new FileStream(sourceFile, FileMode.Open)))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                    lines.Add(line.Trim());
            }
            return lines.ToArray();
        }
    }
}
