using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace HackAssembler
{
    // Encapsulates access to the input code. Reads an assembly language command,
    // parses it, and provides convenient access to the command’s components
    // (fields and symbols). In addition, removes all white space and comments.
    public class Parser
    {
        private string[] sourceLines;
        private int currentLine;

        // TODO: Add and UNKNOWN_COMMAND for errors?
        public enum CommandType
        {
            A_COMMAND, // Addressing instruction for "@Xxx" (A-instruction)
            C_COMMAND, // Compute instruction for "dest=comp;jump" (C-instruction)
            L_COMMAND  // Pseucocommand for "(Xxx)" (L-instruction)
        }

        public Parser(string sourceFile)
        {
            SourceFile = sourceFile;
            currentLine = -1;
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
                throw new System.Exception("There ain't no more commands.");

            currentLine++;
            var line = sourceLines[currentLine];

            Symbol = Dest = Comp = Jump = null;

            // TODO: check if the symbols follow the construction rules.

            if (line.StartsWith("@"))
            {
                CurrentCommandType = CommandType.A_COMMAND;
                Symbol = line.Substring(1);
            }
            else if (line.StartsWith("(") && line.EndsWith(")"))
            {
                CurrentCommandType = CommandType.L_COMMAND;
                Symbol = line.Substring(1, line.Length - 2);
            }
            else
            {
                CurrentCommandType = CommandType.C_COMMAND;

                // TODO: check if the commands are valid.

                // dest=...
                var cmdParts = line.Split(new Char[] { '=' });
                if (cmdParts.Length == 2)
                {
                    Dest = cmdParts[0];
                    line = cmdParts[1];
                }

                // ...;jump
                cmdParts = line.Split(new Char[] { ';' });
                if (cmdParts.Length == 2)
                {
                    Jump = cmdParts[1];
                    line = cmdParts[0];
                }

                Comp = line;
            }
        }

        private string[] getSourceFile(string sourceFile)
        {
            if (!File.Exists(sourceFile))
                return new string[0];

            List<string> lines = new List<string>();
            using (var reader = new StreamReader(new FileStream(sourceFile, FileMode.Open)))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    line = line.Trim();

                    // TODO: keep the line counting for error messages?
                    if (line.StartsWith("//") || line == "")
                        continue;

                    var pos = line.IndexOf("//");
                    if (pos > -1)
                        line = line.Substring(0, pos).Trim();

                    lines.Add(line);
                }
            }
            return lines.ToArray();
        }

        private static Stream getFileStream(string filePath, string resourceNamespace)
        {
            if (resourceNamespace == null)
            {
                try
                {
                    return new FileStream(filePath, FileMode.Open);
                }
                catch (System.Exception)
                {
                    return null;
                }
            }
            string pseudoName = filePath.Replace('\\', '.');
            Assembly assembly = Assembly.GetExecutingAssembly();
            return assembly.GetManifestResourceStream(resourceNamespace + "." + pseudoName);
        }
    }
}
