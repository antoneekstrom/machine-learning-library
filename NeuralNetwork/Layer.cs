using System;
using System.Collections.Generic;
using System.Text;

using MLMath;

namespace NeuralNetwork
{
    public class Layer
    {
        public float this[int index] { get => Nodes[index]; set => Nodes[index] = value; }

        /// <summary>
        /// The size of the layer.
        /// </summary>
        public int Size { get; private set; }

        /// <summary>
        /// The biases for each node of the NEXT layer.
        /// The size of this vector is based on the supplied weights when calling <see cref="Initialize(Matrix)"/>.
        /// If weights are null then no biases will be created.
        /// </summary>
        public Vector Biases { get; private set; }

        /// <summary>
        /// The weights of the layer.
        /// </summary>
        public Matrix Weights { get; private set; }

        /// <summary>
        /// The index of the layer in its <see cref="NeuralNetwork"/>.
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        private Vector _nodes;

        /// <summary>
        /// The values of the nodes in the layer.
        /// </summary>
        public Vector Nodes
        {
            get { return _nodes; }
            set
            {
                if (value.Length != Size) throw new Exception("Node vector length must match size of layer.");
                _nodes = value;
            }
        }

        /// <summary>
        /// Create a layer with a given size.
        /// </summary>
        /// <param name="size">the size</param>
        public Layer(int size, string name)
        {
            Size = size;
            Name = name;
        }

        /// <summary>
        /// Create a layer with a given size.
        /// </summary>
        /// <param name="size">the size</param>
        public Layer(int size) : this(size, null) { }

        /// <summary>
        /// Initialize the layer with a set of weights.
        /// </summary>
        /// <param name="weights">the weights</param>
        /// <param name="index">the index of this layer</param>
        public void Initialize(int index, Matrix weights)
        {
            Random r = new Random();

            Nodes = Vector.Create(Size, 1);
            Index = index;

            if (weights != null)
            {
                Biases = new Vector(VectorMath.CreateVector(weights.Rows, () => (float)r.NextDouble()));
                Weights = weights;
            }
        }

        public override string ToString()
        {
            StringBuilder b = new StringBuilder();
            b.AppendLine("Layer " + (Name == null || Name == "" ? "(" + Index + ")" : Name));
            b.Append(Nodes.ToString("Nodes"));
            return b.ToString();
        }
    }
}
