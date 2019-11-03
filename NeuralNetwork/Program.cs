using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using MLMath;

namespace NeuralNetwork
{
    class Program
    {
        public static void Main(String[] args)
        {
            Program program = new Program();
            program.Run();
        }

        public void TestMatrixMultiplication()
        {
            float[][] a = new float[][] { new float[] { 1, 2, 3 }, new float[] { 4, 5, 6 } };
            Console.WriteLine(new Matrix(a).ToString());

            float[][] b = new float[][] { new float[] { 7, 8 }, new float[] { 9, 10 }, new float[] { 11, 12 } };
            Console.WriteLine(new Matrix(b).ToString());

            float[][] y = MatrixMath.Multiply(a, b);
            Console.WriteLine(new Matrix(y).ToString());


            float[][] v = new float[][] { new float[] { 1 }, new float[] { 2 } };
            Console.WriteLine(new Matrix(v).ToString());

            float[][] z = MatrixMath.Multiply(y, v);
            Console.WriteLine(new Matrix(z).ToString());

            Console.WriteLine(Matrix.Identity(5, 5));
        }

        public void TestMatrixObjects()
        {
            //Matrix a = new Matrix(new float[][] { new float[] { 1, 2, 3 }, new float[] { 4, 5, 6 }, new float[] { 7, 8, 9 } });
            //Matrix b = new Matrix(new float[][] { new float[] { 1, 2, 3 }, new float[] { 4, 5, 6 } });
            //Vector v = new Vector(1, 2, 3);

            Matrix a = new Matrix(1, 3).Randomize(10, true);
            Matrix b = new Matrix(3, 5).Randomize(10, true);
            Matrix c = new Matrix(1, 5).Randomize(10, true);

            a.Print("a");
            b.Print("b");
            c.Print("c");

            Matrix z = a * b;
            z.Print("a * b");
        }

        public void TestPass()
        {
            NeuralNetwork nn = new NeuralNetwork(NetworkProperties.Default, 3, 5, 5, 2);
            nn.Initialize();

            Vector desired = new Vector(1, 0);
            LayerResult[] results = nn.Train(nn.Input.Nodes, desired);

            nn.Input.Nodes.Print("Input");
            desired.Print("Desired Output");
            nn.Output.Nodes.Print("FeedForward Output");

            foreach (LayerResult r in results)
            {
                Console.WriteLine(r.ToString());
            }
        }

        struct TrainingPair
        {
            public Vector input;
            public Vector desiredOutput;

            public TrainingPair(Vector input, Vector desiredOutput)
            {
                this.input = input;
                this.desiredOutput = desiredOutput;
            }
        }

        public void Run()
        {
            NeuralNetwork nn = new NeuralNetwork(NetworkProperties.Default, 2, 3, 1);
            nn.Initialize();

            int trainingIterations = 50000;

            TrainingPair[] trainingSet = new TrainingPair[]
            {
                new TrainingPair(new Vector(0, 0), new Vector(0f)),
                new TrainingPair(new Vector(0, 1), new Vector(1f)),
                new TrainingPair(new Vector(1, 0), new Vector(1f)),
                new TrainingPair(new Vector(1, 1), new Vector(0f))
            };

            for (int i = 0; i < trainingIterations; i++)
            {
                Random r = new Random();
                TrainingPair set = trainingSet[r.Next(0, trainingSet.Length)];
                nn.Train(set.input, set.desiredOutput);

                Console.WriteLine();
                nn.Input.Nodes.Print("Input");
                nn.Output.Nodes.Print("Output");
            }
        }
    }
}
