using System;
using System.IO;

partial class Graph
{
    public class TextReader : Reader
    {
        private Graph graph;

        Graph Reader.Graph { set { graph = value; } }

        public void ReadEdge(string filePath)
        {
            try
            {
                //Initializing
                string[] lines = File.ReadAllLines(filePath);
                graph.dimension = lines[0].Split(' ').Length - 3;

                //Each line is a Edge
                foreach (string line in lines)
                {
                    string[] tokens = line.Split(' ');

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

