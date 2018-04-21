using System.Threading;

namespace DotFlow.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var controller = new FileProcessController();

            controller.Start();
            Thread.Sleep(-1);
            controller.Stop();
            controller.Wait();

        }
    }
}
