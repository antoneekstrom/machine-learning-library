using System;
using System.Collections.Generic;
using System.Text;

namespace MLMath
{

    /// <summary>
    /// How an activation function looks.
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    public delegate float ActivationFunction(float x);

    /// <summary>
    /// How a loss function looks.
    /// </summary>
    /// <param name="result"></param>
    /// <param name="desired"></param>
    /// <returns></returns>
    public delegate float LossFunction(float result, float desired);

    /// <summary>
    /// A collection of functions used in basic machine learning examples.
    /// </summary>
    public static class Functions
    {

        /// <summary>
        /// A sigmoid function.
        /// </summary>
        /// <param name="x">the value</param>
        /// <returns>the result</returns>
        public static float Sigmoid(float x)
        {
            return 1 / (1 + (float)Math.Exp(-x));
        }

        /// <summary>
        /// The derivative of the <see cref="Sigmoid(float)"/> function.
        /// </summary>
        /// <param name="x">the value</param>
        /// <returns>the result</returns>
        public static float SigmoidDerivative(float x)
        {
            float s = Sigmoid(x);
            return s * (1 - s);
        }

        /// <summary>
        /// The derivative of the <see cref="Sigmoid(float)"/> function but it is COOL.
        /// This means that if you multiply something that has been sigmoided with the value of this function it will become the derivative value. YAAAAASSS
        /// </summary>
        /// <param name="y">the value</param>
        /// <returns>the result</returns>
        public static float CoolSigmoidDerivative(float y)
        {
            return y * (1 - y);
        }
    }
}
