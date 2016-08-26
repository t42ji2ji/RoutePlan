using System;
using System.IO;
using System.Collections.Generic;

public partial class Graph
{
    public class BinaryReader : IReader
    {
        private Graph graph;
        Graph IReader.Graph { set { graph = value; } }

        public void ReadEdge(string filePath)
        {
            if (!filePath.EndsWith(".edges"))
                throw new GraphException("不可使用BinaryReader.ReadEdge讀取非edges檔");

            System.IO.BinaryReader reader = new System.IO.BinaryReader(File.Open(filePath, FileMode.Open));
            try
            {
                int dimension = reader.ReadByte();
                graph.dimension = dimension;

                while (true)
                {
                    string centerNode = reader.ReadString();
                    byte length = reader.ReadByte();
                    List<Edge> neighbor = new List<Edge>();

                    for (int i = 0; i < length; ++i)
                    {
                        string destinationNode = reader.ReadString();
                        float[] weights = new float[dimension];

                        for (int j = 0; j < dimension; ++j)
                            weights[j] = reader.ReadSingle();
                        neighbor.Add(new Edge(destinationNode, weights));
                    }

                    graph.adjacencyList.Add(centerNode, neighbor);
                }
            }
            catch (EndOfStreamException) { }
            reader.Close();
        }

        public void ReadNode(string filePath)
        {
            if (!filePath.EndsWith(".nodes"))
                throw new GraphException("不可使用BinaryReader.ReadNode讀取非nodes檔");
            System.IO.BinaryReader reader = new System.IO.BinaryReader(File.Open(filePath, FileMode.Open));
            try
            {
                while (true)
                {
                    string name = reader.ReadString();
                    float longitude = reader.ReadSingle();
                    float latitude = reader.ReadSingle();

                    graph.vertices.Add(name, new Vertex(name, longitude, latitude));
                }
            }
            catch (EndOfStreamException) { }
            reader.Close();
        }
    }
}

