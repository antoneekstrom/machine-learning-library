using System;
using System.Threading;

using MachineLearning.Server;
using MLMath;

namespace MachineLearning.Programs
{
    class MainExecutable
    {
        public static void Main(string[] args)
        {
            XOR program = new XOR();
            program.Run();

            NNServer nns = new NNServer(program.Network, 8080);
        }
    }
}
