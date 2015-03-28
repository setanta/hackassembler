namespace HackAssembler
{
    public class Program
    {
        private static void Main()
        {
            var compiler = new Compiler("HackSamples\\Add.asm");
            compiler.run();
        }
    }
}
