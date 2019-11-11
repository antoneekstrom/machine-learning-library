using System;
using System.Collections.Generic;
using System.Text;

using NeuralNetwork.Programs;

namespace NeuralNetwork
{
    class Run
    {
        public static void Main(string[] args)
        {
            DigitRecognition program = new DigitRecognition();
            program.Run();
        }
    }
}
