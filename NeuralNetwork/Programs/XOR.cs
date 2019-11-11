using System;
using System.Collections.Generic;
using System.Text;

using System.Diagnostics;

using MLMath;
using NeuralNetwork.Collections;

namespace NeuralNetwork.Programs
{
    public class XOR
    {
        public void Run()
        {
            NeuralNetwork nn = new NeuralNetwork(NetworkProperties.Default, 2, 3, 1);
            nn.Initialize();

            Console.WriteLine("Enter number of training iterations:");
            int trainingIterations = int.Parse(Console.ReadLine());

            TrainingPair[] trainingSet = new TrainingPair[]
            {
                new TrainingPair(new Vector(0, 0), new Vector(0f)),
                new TrainingPair(new Vector(0, 1), new Vector(1f)),
                new TrainingPair(new Vector(1, 0), new Vector(1f)),
                new TrainingPair(new Vector(1, 1), new Vector(0f))
            };

            Stopwatch time = Stopwatch.StartNew();

            for (int i = 0; i < trainingIterations; i++)
            {
                Random r = new Random();
                TrainingPair set = trainingSet[r.Next(0, trainingSet.Length)];
                nn.Train(set.Input, set.Output);

                Console.WriteLine();
                nn.Input.Nodes.Print("Input");
                nn.Output.Nodes.Print("Output");
            }

            long dt = time.ElapsedMilliseconds;

            Console.WriteLine("Training Time: " + dt + "ms");
            Console.WriteLine("Training Complete\n");

            while (true)
            {
                Vector input = new Vector(2);

                Console.WriteLine("Enter Input 1:");
                float input1 = float.Parse(Console.ReadLine());
                Console.WriteLine("Enter Input 2:");
                float input2 = float.Parse(Console.ReadLine());

                input[0] = input1;
                input[1] = input2;

                input.Print("Input Vector");

                nn.Input.Nodes = input;

                nn.FeedForward();
                nn.Output.Nodes.Print("Results");

                Console.WriteLine("Exit? (y/n)");
                if (Console.ReadLine() == "y") break;
            }
        }
    }
}
