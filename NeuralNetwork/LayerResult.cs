using System;
using System.Collections.Generic;
using System.Text;

using MLMath;

namespace NeuralNetwork.Structure
{
    /// <summary>
    /// A result from <see cref="NeuralNetwork.CalculateLoss(Vector)"/>.
    /// </summary>
    public struct LayerResult
    {
        /// <summary>
        /// The layer.
        /// </summary>
        public Layer Layer { get; set; }

        /// <summary>
        /// The loss of that layer.
        /// </summary>
        public Vector Loss { get; set; }

        /// <summary>
        /// Deltas for weights and biases.
        /// </summary>
        public Deltas Deltas { get; set; }

        /// <summary>
        /// Create a result.
        /// </summary>
        /// <param name="layer">the layer</param>
        /// <param name="loss">the loss</param>
        public LayerResult(Layer layer, Vector loss)
        {
            Layer = layer;
            Loss = loss;
            Deltas = new Deltas();
        }

        public override string ToString()
        {
            StringBuilder b = new StringBuilder();

            b.AppendLine("-- Layer Result --");
            b.AppendLine(Layer.ToString());
            b.AppendLine(Loss.ToString("Loss"));
            b.AppendLine(Deltas.ToString());

            return b.ToString();
        }
    }
}
