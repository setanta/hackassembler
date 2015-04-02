using System;
using System.IO;

namespace HackAssembler
{
    public class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: HackAssembler SOURCE.asm [BINARY.hack]");
                return;
            }
            else if (!File.Exists(args[0]))
            {
                Console.Error.WriteLine(string.Format("Source file {0} does not exist.", args[0]));
                return;
            }

            var assembler = new Assembler(args[0]);
            assembler.Run();
            if (args.Length == 1)
                Console.WriteLine(string.Join("\n", assembler.HackCode.ToArray()));
            else if (args.Length == 2 && assembler.Save(args[1]))
                Console.WriteLine(string.Format("Built {0} into {1}", args[0], args[1]));
        }
    }
}
