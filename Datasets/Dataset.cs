using System;
using System.Collections.Generic;
using System.Text;

namespace Datasets
{
    /// <summary>
    /// A dataset.
    /// </summary>
    /// <typeparam name="D">the type of data</typeparam>
    /// <typeparam name="L">the type of label</typeparam>
    public abstract class Dataset<D, L>
    {
        public D[] Data { get; protected set; }

        public L[] Labels { get; protected set; }

        public long Size { get; protected set; }
    }
}
