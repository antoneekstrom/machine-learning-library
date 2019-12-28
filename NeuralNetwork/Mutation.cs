using System;
using System.Collections.Generic;
using System.Text;

namespace MachineLearning.Evolution
{

    /// <summary>
    /// 
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public delegate float Mutator(float val);

    /// <summary>
    /// Mutating neural networks.
    /// </summary>
    public static class Mutation
    {

        private static Random r = new Random();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public static float DefaultMutator(float val)
        {
            return (float)r.NextDouble() * 2 - 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        /// <param name="m"></param>
        /// <param name="rate"></param>
        /// <returns></returns>
        public static float ApplyMutator(float val, Mutator mutator, float rate)
        {
            return ((float)r.NextDouble() < rate ? mutator(val) : val);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="l"></param>
        /// <param name="mutator"></param>
        /// <param name="rate"></param>
        public static void MutateLayer(Layer l, Mutator mutator, float rate)
        {
            float op(float v) => ApplyMutator(v, mutator, rate);
            l.Weights.Map(op);
            l.Biases.Map(op);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nn"></param>
        /// <param name="mutator"></param>
        /// <param name="rate"></param>
        public static void MutateNetwork(NeuralNetwork nn, Mutator mutator, float rate)
        {
            foreach (Layer l in nn.AllLayers)
                MutateLayer(l, mutator, rate);
        }
    }
}
