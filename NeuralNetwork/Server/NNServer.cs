using System;
using System.Text;
using System.Collections.Generic;
using System.Collections.Specialized;

using System.Net;

using MLMath;

namespace MachineLearning.Server
{

    struct Response
    {
        public string data;

        public Response(string data)
        {
            this.data = data;
        }
    }

    struct Path
    {
        public Uri uri;
        public PathDelegate pd;

        public Path(Uri uri, PathDelegate pd)
        {
            this.uri = uri;
            this.pd = pd;
        }
    }

    delegate Response PathDelegate(Path path, NNServer server, NameValueCollection queries);

    class NNServer
    {

        public NeuralNetwork Network { get; private set; }
        private int port;

        public bool IsRunning { get; set; } = true;

        private UriBuilder uriBuilder;

        private HttpListener listener;
        private List<Path> paths;

        private HttpListenerContext context;
        private HttpListenerRequest request;
        private HttpListenerResponse response;

        public NNServer(NeuralNetwork nn, int port = 8080)
        {
            Network = nn;
            this.port = port;

            uriBuilder = new UriBuilder();

            listener = new HttpListener();
            paths = new List<Path>();
        }

        Uri CreateUri(string path, string host = "localhost")
        {
            uriBuilder.Host = host;
            uriBuilder.Port = port;
            uriBuilder.Path = path;
            return uriBuilder.Uri;
        }

        void Listen()
        {
            context = listener.GetContext();
            request = context.Request;
            response = context.Response;

            Console.WriteLine("Request Received");

            Delegate(request);
        }

        void Delegate(HttpListenerRequest request)
        {
            foreach (Path path in paths)
            {
                string requestPath = request.Url.LocalPath;
                if (path.uri.LocalPath == requestPath)
                {
                    Response res = path.pd(path, this, request.QueryString);
                    Respond(res.data);
                }
            }
        }

        void Respond(string data)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(data);
            response.ContentLength64 = buffer.Length;
            response.OutputStream.Write(buffer, 0, buffer.Length);
            response.OutputStream.Close();
        }

        public void Start()
        {
            paths.Add(new Path(CreateUri("/nn/get/"), (Path path, NNServer server, NameValueCollection queries) =>
            {
                int index = int.Parse(queries.Get("index"));
                string data = server.Network.AllLayers[index].ToString();
                return new Response(data);
            }));

            paths.Add(new Path(CreateUri("/nn/set/"), (Path path, NNServer server, NameValueCollection queries) =>
            {
                int index = int.Parse(queries.Get("index"));

                string vstring = queries.Get("vector");
                string[] values = vstring.Split(",");
                Vector v = new Vector(values.Length);
                for (int i = 0; i < values.Length; i++)
                {
                    v.Values[i] = float.Parse(values[i]);
                }

                server.Network.AllLayers[index].Nodes = v;

                return new Response(v.ToString());
            }));

            paths.Add(new Path(CreateUri("/nn/feedforward/"), (Path path, NNServer server, NameValueCollection queries) =>
            {
                NeuralNetwork nn = server.Network;
                nn.FeedForward();
                return new Response(nn.Output.Nodes.ToString());
            }));

            paths.Add(new Path(CreateUri("/nn/train/"), (Path path, NNServer server, NameValueCollection queries) =>
            {
                return new Response("yeeet");
            }));

            paths.Add(new Path(CreateUri("/nn/stop/"), (Path path, NNServer server, NameValueCollection queries) =>
            {
                IsRunning = false;
                return new Response("stopping..");
            }));

            foreach (Path path in paths)
            {
                listener.Prefixes.Add(path.uri.AbsoluteUri);
            }

            listener.Start();
            Console.WriteLine("NNServer listening..");

            while (IsRunning)
            {
                Listen();
            }

            listener.Stop();
            Console.WriteLine("NNServer stopped.");
        }
    }
}
