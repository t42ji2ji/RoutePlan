using System;
using System.IO;

public partial class Graph
{
    public class TextReader : IReader
    {
        private Graph graph;

        Graph IReader.Graph { set { graph = value; } }

        public void ReadEdge(string filePath)
        {
            if (!filePath.EndsWith(".txt"))
                throw new GraphException("不可使用TextReader讀取非txt檔");

            try
            {
                //Initializing
                string[] lines = File.ReadAllLines(filePath);
                graph.dimension = lines[0].Split((char[])null, StringSplitOptions.RemoveEmptyEntries).Length - 3;

                //Each line is a Edge
                foreach (string line in lines)
                {
                    string[] tokens = line.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);

                    if (tokens.Length - 3 < graph.dimension)
                        throw new DimensionErrorException(graph.dimension, tokens.Length - 3);

                    float[] weights = new float[graph.dimension];

                    for (int j = 0; j < graph.dimension; ++j)
                        weights[j] = float.Parse(tokens[j + 3]);

                    graph.AddEdge(tokens[1], new Edge(tokens[2], weights));
                    graph.AddEdge(tokens[2], new Edge(tokens[1], weights));
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void ReadNode(string filePath)
        {
            if (!filePath.EndsWith(".txt"))
                throw new GraphException("不可使用TextReader讀取非txt檔");

            try
            {
                //Initializing
                string[] lines = File.ReadAllLines(filePath);

                //Each line is a Edge
                foreach (String line in lines)
                {
                    try
                    {
                        string[] tokens = line.Split(' ');
                        float longitude = float.Parse(tokens[1]);
                        float latitude = float.Parse(tokens[2]);

                        graph.vertices.Add(tokens[0], new Vertex(tokens[0], longitude, latitude));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}


