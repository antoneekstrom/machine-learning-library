using System;
using System.Text;

namespace MLMath
{

    /// <summary>
    /// 
    /// </summary>
    public interface IMatrixShape
    {
        int Rows { get; }
        int Columns { get; }
    }

    struct MatrixShape : IMatrixShape
    {
        public int Rows { get; private set; }
        public int Columns { get; private set; }

        public MatrixShape(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
        }
    }

    public class Matrix
    {
        /// <summary>
        /// The two-dimensional array of floats which contains the values of the matrix.
        /// </summary>
        public float[][] Values { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Columns { get { return MatrixMath.Columns(Values); } }

        /// <summary>
        /// 
        /// </summary>
        public int Rows { get { return MatrixMath.Rows(Values); } }

        /// <summary>
        /// 
        /// </summary>
        public IMatrixShape Shape { get { return new MatrixShape(Rows, Columns); } }

        // 2D Indexable
        public float this[int x, int y]
        {
            get { return Values[x][y]; }
            set { Values[x][y] = value; }
        }

        /// <summary>
        /// <para>
        ///     Create a Matrix object.
        /// </para>
        /// <para>
        ///     See also
        ///     <seealso cref="MLMath.MatrixMath.CreateMatrix(int, int)"/>
        /// </para>
        /// </summary>
        /// <param name="matrix">The values to supply the Matrix with.</param>
        public Matrix(float[][] matrix)
        {
            Values = matrix;
        }

        /// <summary>
        /// Create a new matrix object.
        /// </summary>
        /// <param name="rows">number of rows</param>
        /// <param name="columns">number of columns</param>
        public Matrix(int rows, int columns)
        {
            Values = MatrixMath.CreateMatrix(rows, columns);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shape"></param>
        public Matrix(IMatrixShape shape)
        {
            Values = MatrixMath.CreateMatrix(shape.Rows, shape.Columns);
        }

        /// <summary>
        /// Create a one column matrix from a vector.
        /// </summary>
        /// <param name="v">the vector</param>
        public Matrix(Vector v)
        {
            Values = MatrixMath.VectorToMatrix(v.Values);
        }

        /// <summary>
        /// Map an operation to every value in this matrix. This modifies the current matrix and returns itself.
        /// </summary>
        /// <param name="f">the function</param>
        /// <returns>this matrix but modified</returns>
        public Matrix Map(Operation f)
        {
            for (int i = 0; i < Rows; i++)
            {
                Values[i] = VectorMath.Map(Values[i], f);
            }
            return this;
        }

        /// <summary>
        /// Randomize the values of this matrix. 
        /// </summary>
        /// <param name="multiplier">multiply the random value by a number</param>
        /// <param name="round">if the value should be rounded to the closest whole number</param>
        /// <returns>this matrix</returns>
        public Matrix Randomize(float multiplier = 1, bool round = false)
        {
            Random random = new Random();
            Operation randomValue = z => {
                double v = (float)random.NextDouble() * multiplier;
                if (round) v = Math.Round(v);
                return (float)v;
            };
            return Map(randomValue);
        }

        /// <summary>
        /// Reset this matrix to its identity. This affects the current matrix.
        /// </summary>
        /// <returns>this matrix, but identity</returns>
        public Matrix Identity()
        {
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Columns; c++)
                {
                    this[r, c] = r == c ? 1 : 0;
                }
            }
            return this;
        }

        /// <summary>
        /// Get a row from the matrix.
        /// </summary>
        /// <param name="index">the index of the row</param>
        /// <returns>the row</returns>
        public Vector Row(int index)
        {
            return new Vector(Values[index]);
        }

        /// <summary>
        /// Create an identity matrix.
        /// </summary>
        /// <param name="rows">number of rows</param>
        /// <param name="columns">number of columns</param>
        /// <returns>a new identity matrix</returns>
        public static Matrix Identity(int rows, int columns)
        {
            return new Matrix(rows, columns).Identity();
        }

        /// <summary>
        /// Transpose a matrix and return a new object containing the result.
        /// </summary>
        /// <param name="m">the matrix</param>
        /// <returns>the transposed matrix</returns>
        public static Matrix Transpose(Matrix m)
        {
            return new Matrix(MatrixMath.Transpose(m.Values));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        public static Matrix ElementWiseOperation(Matrix a, Matrix b, ElementWiseOperation operation)
        {
            return new Matrix(MatrixMath.ElementWiseOperation(a.Values, b.Values, operation));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Matrix Multiply(Matrix a, Matrix b)
        {
            return new Matrix(MatrixMath.Multiply(a.Values, b.Values));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector Multiply(Matrix m, Vector v)
        {
            if (m.Columns != v.Size) throw new ArgumentException("Matrix columns must match size of vector.");
            Matrix vtm = Vector.ToMatrix(v);
            Matrix r = m * vtm;
            return new Vector(Transpose(r).Values[0]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        /// <param name="scalar"></param>
        /// <returns></returns>
        public static Matrix Scale(Matrix m, float scalar)
        {
            return new Matrix(MatrixMath.Scale(m.Values, scalar));
        }

        /// <summary>
        /// Print the matrix with an optional message.
        /// </summary>
        /// <param name="message">an optional message</param>
        public void Print(string message = "")
        {
            Console.WriteLine(ToString(message));
        }

        /// <summary>
        /// Convert the matrix to a readable string.
        /// </summary>
        /// <returns>A string.</returns>
        public override string ToString()
        {
            return ToString("");
        }

        /// <summary>
        /// Convert the matrix to a readable string.
        /// </summary>
        /// /// <param name="message">an optional message</param>
        /// <returns>A string.</returns>
        public string ToString(string message)
        {
            StringBuilder b = new StringBuilder();

            b.Append("Matrix (" + MatrixMath.Rows(Values) + "x" + MatrixMath.Columns(Values) + ")" + (message.Length > 0 ? " - " : "") + message + "\n");

            for (int r = 0; r < MatrixMath.Rows(Values); r++)
            {
                b.Append("[");
                for (int c = 0; c < MatrixMath.Columns(Values); c++)
                {
                    b.Append(Values[r][c]);
                    if (c + 1 != MatrixMath.Columns(Values)) b.Append(", ");
                }
                b.Append("]\n");
            }

            return b.ToString();
        }


        // Operators

        public static Matrix operator +(Matrix a, Matrix b) => ElementWiseOperation(a, b, Operations.Add);

        public static Matrix operator -(Matrix a, Matrix b) => ElementWiseOperation(a, b, Operations.Subtract);

        public static Matrix operator *(Matrix a, Matrix b) => Multiply(a, b);

        public static Matrix operator *(Matrix m, float scalar) => Scale(m, scalar);

        public static Vector operator *(Matrix m, Vector v) => Multiply(m, v);
    }
}
