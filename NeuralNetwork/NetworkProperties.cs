using MLMath;

namespace MachineLearning
{

    /// <summary>
    /// The default properties for a <see cref="NeuralNetwork"/>
    /// </summary>
    public class DefaultNetworkProperties : NetworkProperties
    {

        public DefaultNetworkProperties()
        {
            LearningRate = 1;
        }

        public override float ActivationFunction(float x)
        {
            return Functions.Sigmoid(x);
        }

        public override float DerivativeActivation(float x)
        {
            return Functions.CoolSigmoidDerivative(x);
        }

        public override float LossFunction(float result, float desired)
        {
            float loss = desired - result;
            return loss; // YEEEEET this should PERHAPS be SQUARED????
        }

        public override float LearningRate { get; set; }
    }

    class CustomNetworkProperties : NetworkProperties
    {
        private ActivationFunction _activationFunction, _derivativeActivation;
        private LossFunction _lossFunction;
        
        public CustomNetworkProperties(ActivationFunction af, ActivationFunction daf, LossFunction lf, float learningRate)
        {
            _activationFunction = af;
            _derivativeActivation = daf;
            _lossFunction = lf;
            LearningRate = learningRate;
        }

        public override float ActivationFunction(float x)
        {
            return _activationFunction(x);
        }

        public override float DerivativeActivation(float x)
        {
            return _derivativeActivation(x);
        }

        public override float LossFunction(float result, float desired)
        {
            return _lossFunction(result, desired);
        }

        public override float LearningRate { get; set; }

    }

    /// <summary>
    /// Properties of a <see cref="NeuralNetwork"/> which can be customized.
    /// </summary>
    public abstract class NetworkProperties
    {
        /// <summary>
        /// The default properties.
        /// </summary>
        public static NetworkProperties Default { get; } = new DefaultNetworkProperties();

        /// <summary>
        /// Create a <see cref="CustomNetworkProperties"/> instance. 
        /// </summary>
        /// <param name="af"></param>
        /// <param name="lf"></param>
        /// <returns></returns>
        public static NetworkProperties Create(ActivationFunction af, ActivationFunction daf, LossFunction lf, float learningRate)
        {
            return new CustomNetworkProperties(af, daf, lf, learningRate);
        }

        /// <summary>
        /// The learning rate of the network.
        /// </summary>
        public abstract float LearningRate { get; set; }

        /// <summary>
        /// The activation function.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public abstract float ActivationFunction(float x);

        /// <summary>
        /// The function to multiply the activation function with which will result in the derivative of said activation function. see also <see cref="Functions.CoolSigmoidDerivative(float)CoolSigmoidDerivative"/>
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public abstract float DerivativeActivation(float x);

        /// <summary>
        /// e
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public abstract float LossFunction(float result, float desired);
    }
}
