using System;
using System.Threading;

using MachineLearning.IO;
using MLMath;
using System.IO;

namespace MachineLearning.Programs
{
    class MainExecutable
    {
        public static void Main(string[] args)
        {
            Serializer serializer = new Serializer();

            NeuralNetwork nn = DeserializeNetwork(serializer);

            serializer.Close();
        }

        public static NeuralNetwork DeserializeNetwork(Serializer serializer)
        {
            return serializer.Deserialize(serializer.ReadFile("neuralnetwork.json"));
        }

        public static void SerializeNetwork(Serializer serializer)
        {
            XOR xor = new XOR();
            xor.Run();
            serializer.WriteFile(serializer.Serialize(xor.Network), "neuralnetwork.json");
        }
    }
}
