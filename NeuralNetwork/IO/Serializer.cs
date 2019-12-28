using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;
using System.IO;

namespace MachineLearning.IO
{
    class Serializer
    {

        JsonSerializer serializer;
        StringBuilder stringBuilder;
        StringWriter stringWriter;

        public Serializer()
        {
            serializer = new JsonSerializer();
            stringBuilder = new StringBuilder();
            stringWriter = new StringWriter(stringBuilder);
        }

        public void Close()
        {
            stringBuilder.Clear();
            stringWriter.Close();
        }

        public string Serialize(NeuralNetwork nn)
        {
            using (JsonWriter writer = new JsonTextWriter(stringWriter))
            {
                writer.CloseOutput = false; /// stringwriter is closed with Serializer#Close()
                writer.Formatting = Formatting.Indented;

                serializer.Serialize(writer, nn);
            }

            return stringBuilder.ToString();
        }

        public NeuralNetwork Deserialize(StreamReader streamReader)
        {
            JsonTextReader reader = new JsonTextReader(streamReader);
            return serializer.Deserialize<NeuralNetwork>(reader);
        }

        public void WriteFile(string s, string path)
        {
            using (StreamWriter writer = new StreamWriter(new FileStream(path, FileMode.Create)))
            {
                writer.Write(s);
            }
        }

        public StreamReader ReadFile(string path)
        {
            return new StreamReader(new FileStream(path, FileMode.Open));
        }

    }
}
