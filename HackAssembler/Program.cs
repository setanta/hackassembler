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
                Console.ReadKey();
                return;
            }
            else if (!File.Exists(args[0]))
            {
                Console.Error.WriteLine(string.Format("Source file {0} does not exist.", args[0]));
                Console.ReadKey();
                return;
            }

            var assembler = new Assembler(args[0]);
            assembler.Run();
            if (args.Length == 1)
                Console.WriteLine(assembler.HackCode);
            else if (args.Length == 2 && assembler.Save(args[1]))
                Console.WriteLine(string.Format("Built {0} into {1}", args[0], args[1]));
            Console.ReadKey();
        }
    }
}
