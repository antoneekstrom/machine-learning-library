using System;
using System.Collections.Generic;
using System.Text;

namespace MLMath
{
    /// <summary>
    /// A vector of variable length.
    /// </summary>
    public class Vector
    {
        /// <summary>
        /// The values of the vector.
        /// </summary>
        public float[] Values { get; protected set; }

        /// <summary>
        /// The size/length of the vector.
        /// </summary>
        public int Size { get { return Values.Length; } }

        // IIndexable
        public float this[int i]
        {
            get { return Values[i]; }
            set { Values[i] = value; }
        }

        /// <summary>
        /// Create a Vector.
        /// </summary>
        /// <param name="v">The values to put in the vector.</param>
        public Vector(params float[] v)
        {
            Values = new float[v.Length];
            for (int i = 0; i < v.Length; i++)
            {
                this[i] = v[i];
            }
        }

        /// <summary>
        /// Create a Vector with a certain size and fill it with ones.
        /// </summary>
        /// <param name="size">The size of the Vector.</param>
        public Vector(int size)
        {
            Values = VectorMath.CreateVector(size);
        }

        /// <summary>
        /// Map an operation to every value of this vector. Modifies the current vector and returns itself.
        /// </summary>
        /// <param name="f">the function to apply</param>
        /// <returns>this vector</returns>
        public Vector Map(Operation f)
        {
            Values = VectorMath.Map(Values, f);
            return this;
        }

        /// <summary>
        /// Map an operation to every element of a vector. Creates a new vector for the result.
        /// </summary>
        /// <param name="v">the vector</param>
        /// <param name="f">the operation</param>
        /// <returns>the result vector</returns>
        public static Vector Map(Vector v, Operation f)
        {
            return v.Copy().Map(f);
        }

        /// <summary>
        /// Static method for creating a new vector object.
        /// </summary>
        /// <param name="size">The length/size of the vector.</param>
        /// <param name="value">Fill the vector with a value.</param>
        /// <returns>The vector object.</returns>
        public static Vector Create(int size, float value)
        {
            return new Vector(VectorMath.CreateVector(size, value));
        }

        /// <summary>
        /// Static method for creating a new vector object and filling it with ones. 
        /// </summary>
        /// <param name="size">The length/size of the vector.</param>
        /// <returns>The vector object.</returns>
        public static Vector Create(int size)
        {
            return Create(size, 1);
        }

        /// <summary>
        /// Create a matrix from this vector.
        /// </summary>
        /// <param name="v">the vector</param>
        /// <returns>the matrix</returns>
        public static Matrix ToMatrix(Vector v)
        {
            return new Matrix(v);
        }

        /// <summary>
        /// Make a copy of this vector.
        /// </summary>
        /// <returns></returns>
        public Vector Copy()
        {
            return new Vector((float[])Values.Clone());
        }

        /// <summary>
        /// Create a matrix from this vector.
        /// </summary>
        /// <returns>the matrix</returns>
        public Matrix ToMatrix()
        {
            return ToMatrix(this);
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
        /// Convert to vector to a readable string.
        /// </summary>
        /// <returns>A string.</returns>
        public string ToString(string message)
        {
            StringBuilder b = new StringBuilder();

            b.Append("Vector (Size: " + Size + ")" + (message.Length > 0 ? " - " : "") + message + "\n");

            b.Append("[");
            for (int i = 0; i < Size; i++)
            {
                b.Append(this[i]);
                if (i + 1 != Size) b.Append(", ");
            }
            b.Append("]\n");

            return b.ToString();
        }


        // Operators

        public static Vector operator +(Vector a, Vector b) => new Vector(VectorMath.Add(a.Values, b.Values));

        public static Vector operator -(Vector a, Vector b) => new Vector(VectorMath.Subtract(a.Values, b.Values));

        public static Vector operator *(Vector a, Vector b) => new Vector(VectorMath.Multiply(a.Values, b.Values));

        public static Vector operator /(Vector a, Vector b) => new Vector(VectorMath.Divide(a.Values, b.Values));

        public static Vector operator +(Vector a, float scalar) => new Vector(VectorMath.Add(a.Values, scalar));

        public static Vector operator -(Vector a, float scalar) => new Vector(VectorMath.Subtract(a.Values, scalar));

        public static Vector operator *(Vector a, float scalar) => new Vector(VectorMath.Multiply(a.Values, scalar));

        public static Vector operator /(Vector a, float scalar) => new Vector(VectorMath.Divide(a.Values, scalar));

        public static Vector operator -(Vector a) => new Vector(VectorMath.Negate(a.Values));

        public static Vector operator %(Vector v, Operation f) => new Vector(VectorMath.Map(v.Values, f));
    }
}
