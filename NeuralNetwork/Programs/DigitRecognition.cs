using System;

using System.Diagnostics;

using Datasets.MNIST;
using MLMath;

namespace NeuralNetwork.Programs
{
    public class DigitRecognition
    {

        MnistDataset trainingSet, testingSet;
        NeuralNetwork nn;

        void Init()
        {
            string trainingImages = "mnist/train-images.idx3-ubyte";
            string trainingLabels = "mnist/train-labels.idx1-ubyte";

            string testingImages = "mnist/t10k-images.idx3-ubyte";
            string testingLabels = "mnist/t10k-labels.idx1-ubyte";

            trainingSet = MnistReader.LoadDataset(trainingImages, trainingLabels);
            testingSet = MnistReader.LoadDataset(testingImages, testingLabels);

            Image imgSpecimen = trainingSet.Data[0];
            int inputSize = imgSpecimen.Size;

            nn = new NeuralNetwork(
                NetworkProperties.Default,
                inputSize,
                25,
                25,
                10
            );

            nn.Initialize();
        }

        void Train()
        {
            Stopwatch t = new Stopwatch();
            t.Start();

            for (int i = 0; i < trainingSet.Size; i++)
            {
                Image image = trainingSet.Data[i];
                uint label = trainingSet.Labels[i];

                Vector labelVector = Vector.Create(10, 0f);
                labelVector[(int)label] = 1;

                nn.Train(new Vector(image.NormalizedPixels), labelVector);

                Vector loss = NNOperations.OutputLoss(nn.Output, labelVector, nn.Properties.LossFunction);
                float avgLoss = 0;
                loss.ForEach(v => avgLoss += v);
                avgLoss /= loss.Length;

                Console.WriteLine(loss.ToString("Loss (Index: " + i + ") (avg: " + avgLoss + ")"));
            }
            t.Stop();
            Console.WriteLine("Training time: " + t.ElapsedMilliseconds + "ms");
            Console.ReadKey();
        }

        void Test()
        {
            Stopwatch t = new Stopwatch();
            t.Start();

            for (int i = 0; i < testingSet.Size; i++)
            {
                Image image = trainingSet.Data[i];
                uint label = trainingSet.Labels[i];

                Vector labelVector = Vector.Create(10, 0f);
                labelVector[(int)label] = 1;

                nn.Input.Nodes = new Vector(image.NormalizedPixels);
                nn.FeedForward();

                Vector loss = NNOperations.OutputLoss(nn.Output, labelVector, nn.Properties.LossFunction);
                float avgLoss = 0;
                loss.ForEach(v => avgLoss += v);
                avgLoss /= loss.Length;

                Console.WriteLine(nn.Output.Nodes.ToString("Output (Index: " + i + ") (avg: " + avgLoss + ")"));
            }

            t.Stop();
            Console.WriteLine("Testing time: " + t.ElapsedMilliseconds + "ms");
        }

        public void Run()
        {
            Init();
            Train();
            Test();
        }
    }
}
