using System;

using System.Diagnostics;

using Datasets.MNIST;
using MLMath;

namespace MachineLearning.Programs
{
    public class DigitRecognition
    {

        public MnistDataset TrainingSet { get; private set; }
        public MnistDataset TestingSet { get; private set; }
        public NeuralNetwork Network { get; private set; }

        void Init()
        {
            string trainingImages = "mnist/train-images.idx3-ubyte";
            string trainingLabels = "mnist/train-labels.idx1-ubyte";

            string testingImages = "mnist/t10k-images.idx3-ubyte";
            string testingLabels = "mnist/t10k-labels.idx1-ubyte";

            TrainingSet = MnistReader.LoadDataset(trainingImages, trainingLabels);
            TestingSet = MnistReader.LoadDataset(testingImages, testingLabels);

            Image imgSpecimen = TrainingSet.Data[0];
            int inputSize = imgSpecimen.Size;

            Network = new NeuralNetwork(
                NetworkProperties.Default,
                inputSize,
                16,
                16,
                10
            );

            Network.Initialize();
        }

        void Train()
        {
            Stopwatch t = new Stopwatch();
            t.Start();

            for (int i = 0; i < TrainingSet.Size / 2; i++)
            {
                Image image = TrainingSet.Data[i];
                uint label = TrainingSet.Labels[i];

                Vector labelVector = Vector.Create(10, 0f);
                labelVector[(int)label] = 1;

                Network.Train(new Vector(image.NormalizedPixels), labelVector);

                Vector loss = NNOperations.OutputLoss(Network.Output, labelVector, Network.Properties.LossFunction);
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

            for (int i = 0; i < TestingSet.Size; i++)
            {
                Image image = TrainingSet.Data[i];
                uint label = TrainingSet.Labels[i];

                Vector labelVector = Vector.Create(10, 0f);
                labelVector[(int)label] = 1;

                Network.Input.Nodes = new Vector(image.NormalizedPixels);
                Network.FeedForward();

                Vector loss = NNOperations.OutputLoss(Network.Output, labelVector, Network.Properties.LossFunction);
                float avgLoss = 0;
                loss.ForEach(v => avgLoss += v);
                avgLoss /= loss.Length;

                Console.WriteLine(Network.Output.Nodes.ToString("Output (Index: " + i + ") (avg: " + avgLoss + ")"));
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
