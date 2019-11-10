using MLMath;
using System;
using System.Collections.Generic;
using System.Text;

namespace MLMath
{
    public class Vector2 : Vector
    {

        float x
        {
            get { return Values[0]; }
            set { Values[0] = value; }
        }

        float y
        {
            get { return Values[1]; }
            set { Values[1] = value; }
        }


        public Vector2(float x, float y) : base(new float[] { x, y }) { }

        public Vector2(float[] v) : base(v) { }

        public Vector2() : this(1, 1) {}


        public static Vector2 operator +(Vector2 a, Vector2 b) => new Vector2(VectorMath.Add(a.Values, b.Values));

        public static Vector2 operator -(Vector2 a, Vector2 b) => new Vector2(VectorMath.Subtract(a.Values, b.Values));

        public static Vector2 operator *(Vector2 a, Vector2 b) => new Vector2(VectorMath.Multiply(a.Values, b.Values));

        public static Vector2 operator /(Vector2 a, Vector2 b) => new Vector2(VectorMath.Divide(a.Values, b.Values));

        public static Vector2 operator +(Vector2 a, float scalar) => new Vector2(VectorMath.Add(a.Values, scalar));

        public static Vector2 operator -(Vector2 a, float scalar) => new Vector2(VectorMath.Subtract(a.Values, scalar));

        public static Vector2 operator *(Vector2 a, float scalar) => new Vector2(VectorMath.Multiply(a.Values, scalar));

        public static Vector2 operator /(Vector2 a, float scalar) => new Vector2(VectorMath.Divide(a.Values, scalar));

        public static Vector2 operator -(Vector2 a) => new Vector2(VectorMath.Negate(a.Values));


        public float Dot(Vector2 b)
        {
            return VectorMath.Dot(Values, b.Values);
        }


        public override string ToString()
        {
            return "Vector2(" + x + "," + y + ")";
        }
    }
}
