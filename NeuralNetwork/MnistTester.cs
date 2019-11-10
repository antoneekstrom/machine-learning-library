using System;
using System.Collections.Generic;
using System.Text;

using Datasets.MNIST;

namespace NeuralNetwork
{
    public static class MnistTester
    {
        public static void Run()
        {
            string imagesPath = "mnist/train-images.idx3-ubyte";
            string labelsPath = "mnist/train-labels.idx1-ubyte";
            MnistDataset dataset = FileIO.LoadDataset(imagesPath, labelsPath);
        }
    }
}
