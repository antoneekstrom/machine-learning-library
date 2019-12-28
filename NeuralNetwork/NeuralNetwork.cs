using MLMath;

namespace MachineLearning
{

    /// <summary>
    /// <see cref="NeuralNetwork"/> iterator shape.
    /// </summary>
    /// <param name="prev">the previous layer</param>
    /// <param name="current">the current layer</param>
    /// <param name="next">the next layer</param>
    public delegate void NNIterator(Layer prev, Layer current, Layer next);

    /// <summary>
    /// A neural network.
    /// The <see cref="NeuralNetwork"/> object is a container for its components: <see cref="Layer"/>, <see cref="Weights"/> and biases.
    /// It has methods which manipulate the other objects it encompasses by using static methods from
    /// the <see cref="NNOperations"/> class.
    /// </summary>
    public class NeuralNetwork
    {
        /// <summary>
        /// The input layer.
        /// </summary>
        public Layer Input { get; private set; }

        /// <summary>
        /// The output layer.
        /// </summary>
        public Layer Output { get; private set; }

        /// <summary>
        /// The hidden layers.
        /// </summary>
        public Layer[] Hidden { get; private set; }

        /// <summary>
        /// The weights.
        /// </summary>
        public Matrix[] Weights { get; private set; }

        /// <summary>
        /// The properties of this nn.
        /// </summary>
        public NetworkProperties Properties { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public Layer[] AllLayers
        {
            get
            {
                Layer[] all = new Layer[LayerCount];
                all[0] = Input;
                all[LayerCount - 1] = Output;
                for (int i = 0; i < Hidden.Length; i++)
                {
                    all[i + 1] = Hidden[i];
                }
                return all;
            }
        }

        /// <summary>
        /// The total number of layers in the nn, INCLUDING the input layer
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public int LayerCount { get { return Hidden.Length + 2; } }

        /// <summary>
        /// Create a neural network.
        /// </summary>
        /// <param name="input">the input layer</param>
        /// <param name="output">the output layer</param>
        /// <param name="hidden">the hidden layers</param>
        public NeuralNetwork(NetworkProperties properties, Layer input, Layer output, params Layer[] hidden)
        {
            Properties = properties;
            Input = input;
            Output = output;
            Hidden = hidden;
        }

        /// <summary>
        /// Create a neural network from a series of numbers.
        /// The first number represents the size of the input layer, the last number is the size of the output layer.
        /// The numbers inbetween are the sizes of the hidden layers.
        /// </summary>
        /// <param name="architecture">the sizes of the layers</param>
        public NeuralNetwork(NetworkProperties properties, params int[] architecture)
        {
            int l = architecture.Length;

            Properties = properties;
            Input = new Layer(architecture[0], "Input");
            Output = new Layer(architecture[l - 1], "Output");
            Hidden = new Layer[l - 2];

            for (int i = 1; i < l - 1; i++)
            {
                Hidden[i - 1] = new Layer(architecture[i]);
            }
        }

        [Newtonsoft.Json.JsonConstructor]
        private NeuralNetwork() { }

        /// <summary>
        /// Calculate the results by feeding the input layer forward through the network. The resulting values can end up in the <see cref="Output"/> layer.
        /// </summary>
        public void FeedForward()
        {
            Layer current = Input;

            for (int i = 0; i < Hidden.Length; i++)
            {
                Layer next = Hidden[i];
                next.Nodes = NNOperations.CalculateValues(current, Properties.ActivationFunction);
                current = next;
            }

            Output.Nodes = NNOperations.CalculateValues(current, Properties.ActivationFunction);
        }

        /// <summary>
        /// <para>
        /// Perform one pass of training. Adjust the weights based on the current state of the <see cref="Output"/> layer and the desired values. 
        /// Use <see cref="FeedForward"/> to calculate the output values.
        /// </para>
        /// 
        /// <para>
        /// Calculate the errors/losses of each layer (using <see cref="CalculateLoss(Vector)"/>)
        /// and then adjust the weights accordingly (using <see cref="NNOperations.CalculateDeltas(Layer, Layer, Vector, ActivationFunction, float)"/>).
        /// </para>
        /// </summary>
        /// <param name="desiredOutput">the desired output value of the network</param>
        /// <returns>the results</returns>
        public LayerResult[] AdjustWeights(Vector desiredOutput)
        {
            LayerResult[] results = CalculateLoss(desiredOutput);

            for (int i = results.Length - 1; i >= 0; i--) // Iterate over results backwards
            {
                if (i == 0) break;

                LayerResult L1R = results[i];
                LayerResult L0R = results[i - 1];

                // Get the values to adjust weights and biases
                Deltas L0deltas = NNOperations.CalculateDeltas(L0R.Layer, L1R.Layer, L1R.Loss, Properties.DerivativeActivation, Properties.LearningRate);

                // create new adjusted weights and biases
                Matrix nw = L0R.Layer.Weights + L0deltas.Weights;
                Vector nb = L0R.Layer.Biases + L0deltas.Biases;

                // Apply adjustments
                L0R.Layer.Weights.Values = nw.Values;
                L0R.Layer.Biases.Values = nb.Values;

                results[i - 1].Deltas = L0deltas;
            }

            return results;
        }

        /// <summary>
        /// Train the network once by feeding in an input and adjusting its weights accordingly.
        /// </summary>
        /// <param name="input">the input</param>
        /// <param name="desiredOutput">the desired output</param>
        public LayerResult[] Train(Vector input, Vector desiredOutput)
        {
            Input.Nodes = input;
            FeedForward();
            return AdjustWeights(desiredOutput);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="desired"></param>
        /// <returns></returns>
        public LayerResult[] CalculateLoss(Vector desired)
        {
            Layer[] layers = AllLayers;
            Vector LossL1 = NNOperations.OutputLoss(Output, desired, Properties.LossFunction);

            LayerResult[] results = new LayerResult[LayerCount];

            for (int i = layers.Length - 1; i >= 0; i--)
            {
                if (i == 0) break;

                Layer L1 = layers[i];
                Layer L0 = layers[i - 1];

                Vector LossL0 = NNOperations.PropagateLoss(LossL1, L0);

                LayerResult ResultL1 = new LayerResult(L1, LossL1);
                LayerResult ResultL0 = new LayerResult(L0, LossL0);
                results[i] = ResultL1;
                results[i - 1] = ResultL0;

                LossL1 = LossL0;
            }

            return results;
        }

        /// <summary>
        /// Initialization.
        /// </summary>
        public void Initialize()
        {
            // Initialize layers

            Weights = new Matrix[Hidden.Length + 1];

            Layer current = Input;

            for (int i = 0; i < Hidden.Length; i++)
            {
                Weights[i] = NNOperations.CreateWeights(current, Hidden[i]);
                current.Initialize(i, Weights[i]);
                current = Hidden[i];
            }

            Weights[Hidden.Length] = NNOperations.CreateWeights(current, Output);
            current.Initialize(Hidden.Length, Weights[Hidden.Length]);
            Output.Initialize(Hidden.Length + 1, null);
        }
    }
}
