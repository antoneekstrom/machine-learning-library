using System;
using System.Collections.Generic;
using System.Text;

namespace MLMath
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public delegate float ElementWiseOperation(float a, float b);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="val"></param>
    /// <returns></returns>
    public delegate float Operation(float val);

    /// <summary>
    /// A consumer of floating point values.
    /// </summary>
    /// <param name="val"></param>
    public delegate void Consumer(float val);

    /// <summary>
    /// 
    /// </summary>
    public static class Operations
    {
        public static ElementWiseOperation Multiply = (a, b) => a * b;
        public static ElementWiseOperation Divide = (a, b) => a / b;
        public static ElementWiseOperation Add = (a, b) => a + b;
        public static ElementWiseOperation Subtract = (a, b) => a - b;
    }
}
