using MLMath;
using System;
using System.Collections.Generic;
using System.Text;

namespace MLMath
{

    /// <summary>
    /// Methods for comparing dimensions of matrices.
    /// </summary>
    public static class MatrixCompare
    {

        public static bool SameRows(float[][] a, float[][] b)
        {
            return a.Length == b.Length;
        }

        public static bool SameColumns(float[][] a, float[][] b)
        {
            return a.Length == b.Length;
        }

        public static bool CompareColumnsWithRows(float[][] a, float[][] b)
        {
            return MatrixMath.Columns(a) == MatrixMath.Rows(b);
        }

        public static bool SameSize(float[][] a, float[][] b)
        {
            return SameRows(a, b) && SameColumns(a, b);
        }
    }
    
    /// <summary>
    /// Methods for performing mathematics with matrices.
    /// </summary>
    public static class MatrixMath
    {

        /// <summary>
        /// The count of rows in a given matrix.
        /// </summary>
        /// <param name="m">the matrix</param>
        /// <returns>the count of rows</returns>
        public static int Rows(float[][] m)
        {
            return m.Length;
        }

        /// <summary>
        /// The count of columns in a given matrix.
        /// </summary>
        /// <param name="m">the matrix</param>
        /// <returns>the count of columns</returns>
        public static int Columns(float[][] m)
        {
            return m[0].Length;
        }

        /// <summary>
        /// Select a row from a given matrix.
        /// </summary>
        /// <param name="m">the matrix</param>
        /// <param name="row">the index of the row</param>
        /// <returns>the row</returns>
        public static float[] Row(float[][] m, int row)
        {
            return m[row];
        }

        /// <summary>
        /// Create a matrix.
        /// </summary>
        /// <param name="rows">The number of rows.</param>
        /// <param name="columns">The number of columns.</param>
        /// <returns>the matrix</returns>
        public static float[][] CreateMatrix(int rows, int columns)
        {
            float[][] mat = new float[rows][];

            for (int i = 0; i < mat.Length; i++)
            {
                mat[i] = new float[columns];
            }

            return mat;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static float[][] VectorToMatrix(float[] vector)
        {
            float[][] m = new float[vector.Length][];
            for (int i = 0; i < vector.Length; i++)
            {
                m[i] = new float[] { vector[i] };
            }
            return m;
        }

        /// <summary>
        /// Element-wise addition.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float[][] Add(float[][] a, float[][] b)
        {
            if (!MatrixCompare.SameSize(a, b)) throw new ArgumentException("Matrices are not of same size.");

            float[][] y = CreateMatrix(Rows(a), Columns(a));

            for (int i = 0; i < Rows(y); i++)
            {
                y[i] = VectorMath.Add(Row(a, i), Row(b, i));
            }

            return y;
        }

        /// <summary>
        /// Element-wise subtraction.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float[][] Subtract(float[][] a, float[][] b)
        {
            if (!MatrixCompare.SameSize(a, b)) throw new ArgumentException("Matrices are not of same size.");

            float[][] y = CreateMatrix(Rows(a), Columns(a));

            for (int i = 0; i < Rows(y); i++)
            {
                y[i] = VectorMath.Subtract(Row(a, i), Row(b, i));
            }

            return y;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        public static float[][] ElementWiseOperation(float[][] a, float[][] b, ElementWiseOperation operation)
        {
            if (!MatrixCompare.SameSize(a, b)) throw new ArgumentException("Matrices are not of same size.");

            float[][] y = CreateMatrix(Rows(a), Columns(b));

            for (int i = 0; i < Rows(y); i++)
            {
                y[i] = VectorMath.ElementWiseOperation(a[i], b[i], operation);
            }

            return y;
        }

        /// <summary>
        /// Element-wise multiplication by a scalar.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="scalar"></param>
        /// <returns></returns>
        public static float[][] Scale(float[][] a, float scalar)
        {
            float[][] y = CreateMatrix(Rows(a), Columns(a));

            for (int i = 0; i < Rows(y); i++)
            {
                y[i] = VectorMath.Multiply(a[i], scalar);
            }

            return y;
        }

        /// <summary>
        /// Returns a new matrix which is the result of transposing a given matrix.
        /// </summary>
        /// <param name="m">the matrix to transpose</param>
        /// <returns>the result</returns>
        public static float[][] Transpose(float[][] m)
        {
            float[][] y = CreateMatrix(Columns(m), Rows(m));

            for (int r = 0; r < Rows(m); r++)
            {
                for (int c = 0; c < Columns(m); c++)
                {
                    y[c][r] = m[r][c];
                }
            }

            return y;
        }

        /// <summary>
        /// Multiply two matrices.
        /// </summary>
        /// <param name="a">matrix a</param>
        /// <param name="b">matrix b</param>
        /// <returns>a new matrix which is the result of the multiplication</returns>
        public static float[][] Multiply(float[][] a, float[][] b)
        {
            float[][] y = CreateMatrix(Rows(a), Columns(b));
            float[][] bt = Transpose(b);

            if (!MatrixCompare.CompareColumnsWithRows(a, b)) throw new ArgumentException("Illegal matrix dimensions.");

            for (int r = 0; r < Rows(a); r++)
            {
                for (int c = 0; c < Columns(b); c++)
                {
                    float[] ra = Row(a, r);
                    float[] rb = Row(bt, c);
                    y[r][c] = VectorMath.Dot(ra, rb);
                }
            }

            return y;
        }

        /// <summary>
        /// Multiply a matrix with a vector.
        /// </summary>
        /// <param name="a">the matrix</param>
        /// <param name="b">the vector</param>
        /// <returns>a new vector which is the result of the multiplication</returns>
        //public static float[] Multiply(float[][] m, float[] v)
        //{
        //    float[] y = new float[v.Length];

        //    if (Rows(m) != v.Length) throw new ArgumentException("Illegal matrix dimensions.");

        //    for (int r = 0; r < y.Length; r++)
        //    {
        //        y[r] = VectorMath.Dot(m[r], v);
        //    }

        //    return y;
        //}

    }
}
