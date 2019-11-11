using System;
using System.Collections;
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
        /// <summary>
        /// The data.
        /// </summary>
        public D[] Data { get; protected set; }

        /// <summary>
        /// The labels for the data.
        /// </summary>
        public L[] Labels { get; protected set; }

        /// <summary>
        /// The number of items.
        /// </summary>
        public long Size { get; protected set; }
    }
}
