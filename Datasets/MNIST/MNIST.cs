using System;
using System.Collections.Generic;
using System.Text;

using System.IO;

using MLMath;

namespace Datasets.MNIST
{

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

    public class MnistDataset : Dataset<Image, uint>
    {

        public static MnistDataset Create(int numItems, uint[] labels, Image[] images)
        {
            MnistDataset d = new MnistDataset();
            d.Size = numItems;
            d.Labels = labels;
            d.Data = images;
            return d;
        }
    }

    public class Image
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public uint[] Pixels { get; private set; }
        public int Size { get { return Pixels.Length; } }

        public Vector ToVector()
        {
            float[] values = new float[Size];
            for (int i = 0; i < Size; i++)
            {
                values[i] = Pixels[i];
            }
            return new Vector(values);
        }

        public Matrix ToMatrix()
        {
            Matrix m = new Matrix(Width, Height);

            for (int rows = 0; rows < Width; rows++)
            {
                for (int columns = 0; columns < Height; columns++)
                {
                    m.Values[rows][columns] = (float)Pixels[rows * Width + columns] / 255f;
                }
            }

            return m;
        }

        public uint this[int row, int col]
        {
            get { return Pixels[row * Width + col]; }
            set { Pixels[row * Width + col] = value; }
        }

        public Image(uint[] pixels, int width, int height)
        {
            Pixels = pixels;
            Width = width;
            Height = height;
        }
    }

    public static class MnistReader
    {
        public static uint[] ReadLabels(string labelsPath)
        {
            BinaryReader labelReader = new BinaryReader(new FileStream(labelsPath, FileMode.Open));
            labelReader.BaseStream.Position = 4;

            int numItems = labelReader.ReadIntegerYasss();
            uint[] labels = new uint[numItems];

            for (int i = 0; i < numItems; i++)
            {
                labels[i] = labelReader.ReadByte();
            }

            labelReader.Close();
            return labels;
        }

        public static Image[] ReadImages(string imagesPath)
        {
            BinaryReader imageReader = new BinaryReader(new FileStream(imagesPath, FileMode.Open));

            imageReader.BaseStream.Position = 4;

            int numItems = imageReader.ReadIntegerYasss();
            int numRows = imageReader.ReadIntegerYasss();
            int numCols = imageReader.ReadIntegerYasss();
            Image[] images = new Image[numItems];

            for (int i = 0; i < numItems; i++)
            {
                uint[] pixels = new uint[numRows * numCols];
                for (int k = 0; k < numRows * numCols; k++)
                {
                    pixels[k] = imageReader.ReadByte();
                }
                images[i] = new Image(pixels, numRows, numCols);
            }

            imageReader.Close();
            return images;
        }

        public static MnistDataset LoadDataset(string imagesPath, string labelsPath)
        {
            uint[] labels = ReadLabels(labelsPath);
            Image[] images = ReadImages(imagesPath);
            return MnistDataset.Create(labels.Length, labels, images);
        }
    }
}
