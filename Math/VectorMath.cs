using MLMath;
using System;
using System.Collections.Generic;
using System.Text;

namespace MLMath
{
    /// <summary>
    /// Methods for comparing vectors.
    /// </summary>
    public static class VectorCompare
    {
        public static bool SameLength(float[] a, float[] b)
        {
            return a.Length == b.Length;
        }

        public static bool MaxLength(float[] vec, int length)
        {
            return vec.Length <= length;
        }
    }

    /// <summary>
    /// Methods for performing mathematics with vectors.
    /// </summary>
    public static class VectorMath
    {
        /// <summary>
        /// Perform an operation
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        public static float[] ElementWiseOperation(float[] a, float[] b, ElementWiseOperation operation)
        {
            if (!VectorCompare.SameLength(a, b)) throw new ArgumentException("Vectors are not of same length.");

            float[] y = new float[a.Length];

            for (int i = 0; i < y.Length; i++)
            {
                y[i] = operation.Invoke(a[i], b[i]);
            }

            return y;
        }

        /// <summary>
        /// Map a certain function to every value of the vector.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static float[] Map(float[] v, Operation f)
        {
            float[] r = new float[v.Length];
            for (int i = 0; i < v.Length; i++)
            {
                r[i] = f(v[i]);
            }
            return r;
        }

        public static float[] CreateVector(int size)
        {
            float[] vec = new float[size];
            for (int i = 0; i < size; i++)
            {
                vec[i] = 1;
            }
            return vec;
        }

        public static float[] CreateVector(int size, Func<float> supplier)
        {
            float[] v = new float[size];
            for (int i = 0; i < v.Length; i++)
            {
                v[i] = supplier();
            }
            return v;
        }

        public static float[] CreateVector(int size, float value)
        {
            return CreateVector(size, () => value);
        }

        public static float[] Add(float[] a, float[] b)
        {
            ElementWiseOperation add = (_a, _b) => _a + _b;
            return ElementWiseOperation(a, b, add);
        }

        public static float[] Add(float[] a, float scalar)
        {
            float[] y = new float[a.Length];
            for (int i = 0; i < a.Length; i++)
            {
                y[i] = a[i] + scalar;
            }
            return y;
        }

        public static float[] Subtract(float[] a, float[] b)
        {
            ElementWiseOperation sub = (_a, _b) => _a - _b;
            return ElementWiseOperation(a, b, sub);
        }

        public static float[] Subtract(float[] a, float scalar)
        {
            float[] y = new float[a.Length];
            for (int i = 0; i < a.Length; i++)
            {
                y[i] = a[i] - scalar;
            }
            return y;
        }

        public static float[] Multiply(float[] a, float[] b)
        {
            ElementWiseOperation mul = (_a, _b) => _a * _b;
            return ElementWiseOperation(a, b, mul);
        }

        public static float[] Divide(float[] a, float[] b)
        {
            ElementWiseOperation div = (_a, _b) => _a / _b;
            return ElementWiseOperation(a, b, div);
        }

        public static float[] Divide(float[] a, float scalar)
        {
            float[] y = new float[a.Length];
            for (int i = 0; i < a.Length; i++)
            {
                y[i] = a[i] / scalar;
            }
            return y;
        }
        public static float[] Multiply(float[] a, float scalar)
        {
            float[] y = new float[a.Length];
            for (int i = 0; i < a.Length; i++)
            {
                y[i] = a[i] * scalar;
            }
            return y;
        }

        public static float[] Negate(float[] a)
        {
            float[] y = new float[a.Length];
            for (int i = 0; i < a.Length; i++)
            {
                y[i] = -a[i];
            }
            return y;
        }

        public static float Dot(float[] a, float[] b)
        {
            if (!VectorCompare.SameLength(a, b)) throw new ArgumentException("Vectors are not of same length.");

            float y = 0;

            for (int l = 0; l < a.Length; l++)
            {
                y += a[l] * b[l];
            }

            return y;
        }
    }
}
