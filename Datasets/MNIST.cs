using System;
using System.Collections.Generic;
using System.Text;

using System.IO;

namespace Datasets.MNIST
{

    public class MnistDataset : Dataset<byte[], uint>
    {

        public int Rows { get; private set; }
        public int Columns { get; private set; }

        public static MnistDataset Create(int numItems, uint[] labels, int rows, int columns)
        {
            MnistDataset d = new MnistDataset();
            d.Size = numItems;
            d.Labels = labels;
            d.Rows = rows;
            d.Columns = columns;
            return d;
        }
    }

    /// <summary>
    /// Extension methods allow for putting a method on an object without subclassing. ngl kinda cringe
    /// </summary>
    static class Extensions
    {
        /// <summary>
        /// Read an integer.
        /// </summary>
        /// <param name="br"></param>
        /// <returns>the integer</returns>
        public static int ReadIntegerYasss(this BinaryReader br)
        {
            var bytes = br.ReadBytes(sizeof(int));
            if (BitConverter.IsLittleEndian) Array.Reverse(bytes); // Flip the bytes if system endian is wrong (the MNIST dataset is bigendian)
            return BitConverter.ToInt32(bytes, 0);
        }
    }

    public static class FileIO
    {
        public static MnistDataset LoadDataset(string imagesPath, string labelsPath)
        {
            BinaryReader imageReader = new BinaryReader(new FileStream(imagesPath, FileMode.Open));
            BinaryReader labelReader = new BinaryReader(new FileStream(labelsPath, FileMode.Open));

            labelReader.BaseStream.Position = 4;
            imageReader.BaseStream.Position = 8;

            int numItems = labelReader.ReadIntegerYasss();
            int numRows = imageReader.ReadIntegerYasss();
            int numCols = imageReader.ReadIntegerYasss();
            uint[] labels = new uint[numItems];

            for (int i = 0; i < numItems; i++)
            {
                labels[i] = labelReader.ReadByte();
            }

            labelReader.Close();
            imageReader.Close();

            return MnistDataset.Create(numItems, labels, numRows, numCols);
        }
    }
}
