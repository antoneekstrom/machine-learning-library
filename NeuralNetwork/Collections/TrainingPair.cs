using System;
using System.Collections.Generic;
using System.Text;

using MLMath;

namespace NeuralNetwork.Collections
{
    public struct TrainingPair
    {

        public Vector Input { get; private set; }
        public Vector Output { get; private set; }

        public TrainingPair(Vector input, Vector output)
        {
            Input = input;
            Output = output;
        }
    }
}
