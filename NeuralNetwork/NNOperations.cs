using System;
using System.Collections.Generic;
using System.Text;

using MLMath;

namespace NeuralNetwork
{

    public struct Deltas
    {
        public Matrix Weights { get; set; }
        public Vector Biases { get; set; }

        public override string ToString()
        {
            return ((Weights != null) ? (Weights.ToString() + "\n") : "") + ((Biases != null) ? Biases.ToString() : "");
        }
    }

    /// <summary>
    /// Operations used by the <see cref="NeuralNetwork"/> class.
    /// </summary>
    public static class NNOperations
    {

        /// <summary>
        /// Create a set of wieghts for two given layers, initialized with random values.
        /// </summary>
        /// <param name="l0">the first layer</param>
        /// <param name="l1">the second layer</param>
        /// <returns>the weights</returns>
        public static Matrix CreateWeights(Layer l0, Layer l1)
        {
            return new Matrix(l1.Size, l0.Size).Randomize();
        }

        /// <summary>
        /// Calculate the values of the next layer when feeding forward.
        /// </summary>
        /// <param name="layer">the current layer</param>
        /// <returns>the result</returns>
        public static Vector CalculateValues(Layer layer, ActivationFunction f)
        {
            // activation(W * a + b)
            return (layer.Weights * layer.Nodes + layer.Biases).Map(v => f(v));
        }

        /// <summary>
        /// Get the deltas for the weights of a given layer L0.
        /// </summary>
        /// <param name="L0">first layer</param>
        /// <param name="L1">second layer</param>
        /// <param name="loss">the loss of L0</param>
        /// <param name="daf">the (cool) derivative of the activation function</param>
        /// <param name="learningRate">the learning rate</param>
        /// <returns>the deltas for the weights</returns>
        public static Deltas CalculateDeltas(Layer L0, Layer L1, Vector loss, ActivationFunction daf, float learningRate)
        {
            //Vector da = L1.Nodes.Copy().Map(v => daf(v));
            Vector da = Vector.Map(L1.Nodes, v => daf(v)); // derivative of the values (a) of L1
            Matrix ld = Matrix.ElementWiseOperation(loss.ToMatrix(), da.ToMatrix(), Operations.Multiply); // L1 loss derivative

            Matrix db = ld * learningRate; // delta for biases
            Matrix dw = ld * Matrix.Transpose(L0.Nodes.ToMatrix()) * learningRate; // delta for weights

            // Put them into an object
            Deltas deltas = new Deltas();
            deltas.Weights = dw;
            deltas.Biases = new Vector(Matrix.Transpose(db).Values[0]);

            return deltas;
        }

        /// <summary>
        /// Calculate the loss in a given layer.
        /// </summary>
        /// <param name="prevLoss">the loss of the previous layer</param>
        /// <param name="current">the current layer to calculate the loss of</param>
        /// <returns>the loss of <paramref name="current"/></returns>
        public static Vector PropagateLoss(Vector prevLoss, Layer current)
        {
            return Matrix.Transpose(current.Weights) * prevLoss;
        }

        /// <summary>
        /// Calculate the loss/error of the output nodes.
        /// </summary>
        /// <param name="layer">the output layer</param>
        /// <returns>a vector containing the errors</returns>
        public static Vector OutputLoss(Layer layer, Vector desired, LossFunction f)
        {
            return new Vector(VectorMath.ElementWiseOperation(layer.Nodes.Values, desired.Values, (a, b) => f(a, b)));
        }
    }
}
