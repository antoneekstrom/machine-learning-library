using System;
using System.Collections.Generic;
using System.Text;

using Datasets.MNIST;
using MLMath;

namespace NeuralNetwork.Programs
{
    class DigitRecognition
    {
        public void Run()
        {
            string imagesPath = "mnist/train-images.idx3-ubyte";
            string labelsPath = "mnist/train-labels.idx1-ubyte";
            MnistDataset dataset = MnistReader.LoadDataset(imagesPath, labelsPath);

            int inputSize = dataset.Data[0].Size;

            NeuralNetwork nn = new NeuralNetwork(
                NetworkProperties.Default,
                inputSize,
                inputSize / 2,
                inputSize / 3,
                inputSize / 2,
                10
            );

            for (int i = 0; i < 10000; i++)
            {
                Image image = dataset.Data[i];
                uint label = dataset.Labels[i];

                Vector lv = Vector.Create(10, 0f);
                lv[(int)label] = 1;

                nn.Train(image.ToVector(), lv);
            }

            for (int i = 10; i < 10; i++)
            {
                Image image = dataset.Data[i];
                uint label = dataset.Labels[i];

                nn.Input.Nodes = image.ToVector();
                nn.FeedForward();
                Console.WriteLine(nn.Output.Nodes.ToString("Output"));
                Console.WriteLine("Correct: " + label);
            }
        }
    }
}
