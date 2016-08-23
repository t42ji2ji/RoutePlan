using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


partial class Graph
{
    public class FileCreater
    {
        static void Main(string[] args)
        {
            //CreateBinaryEdge("../../res/ming_edges.txt", "../../res/ming.edges");
            //CreateBinaryNode("../../res/ming_nodes.txt", "../../res/ming.nodes");

            Graph graph = new Graph(new BinaryReader());
            graph.ReadEdge("../../res/binary/ming.edges");
            graph.ReadNode("../../res/binary/ming.nodes");

            Console.WriteLine("Compelte");
            Console.Read();




            //CreateSerializeEdge("../../res/oldenburg_edge.txt", "../../res/text/oldenburg.edges");
            //CreateSerializeNode("../../res/text/ming_nodes.txt", "../../res/text/ming_edges.nodes");

            /*try
            {
                if (args.Length == 0)
                {
                    Console.WriteLine("需要至少一個參數");
                    return;
                }
                else
                {
                    foreach (string arg in args)
                    {
                        string[] command = arg.Split('=');
                        if (command.Length != 2)
                        {
                            Console.WriteLine("無此檔案類型指令");
                            return;
                        }

                        if (command[0].Equals("edge")) CreateSerializeEdge(command[1], command[1] + " BinaryEdge");
                        else if (command[0].Equals("node")) CreateSerializeNode(command[1], command[1] + " BinaryNode");
                        else
                        {
                            Console.WriteLine("無此檔案類型指令");
                            return;
                        }
                    }
                }
                Console.WriteLine("finish");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }*/
        }

        public static void CreateSubGraph(Graph graph, int amount, string start)
        {
            Queue<string> nodes = new Queue<string>();
            HashSet<string> visist = new HashSet<string>();
            HashSet<string> beenOutputNode = new HashSet<string>();
            StreamWriter edgeWriter = new StreamWriter("Edges.txt");
            StreamWriter nodeWriter = new StreamWriter("Nodes.txt");
            int count = 1;
            Vertex detail;

            nodes.Enqueue(start);
            visist.Add(start);
            detail = graph.vertices[start];
            nodeWriter.WriteLine(start + " " + detail.Longitude + " " + detail.Latitude);


            while (count <= amount)
            {
                string expand = nodes.Dequeue();
                List<Edge> neighbor = graph.adjacencyList[expand];

                foreach (Edge edge in neighbor)
                {
                    if (visist.Contains(edge.Des)) continue;
                    else visist.Add(edge.Des);

                    detail = graph.vertices[edge.Des];
                    nodes.Enqueue(edge.Des);

                    //write edges
                    edgeWriter.Write(count + " " + expand + " " + edge.Des);
                    foreach (float weight in edge.Weights)
                        edgeWriter.Write(" " + weight);
                    edgeWriter.WriteLine();

                    //write nodes
                    if (!beenOutputNode.Contains(edge.Des))
                    {
                        nodeWriter.WriteLine(edge.Des + " " + detail.Longitude + " " + detail.Latitude);
                        beenOutputNode.Add(edge.Des);
                    }
                    count++;
                }
                edgeWriter.Flush();
                nodeWriter.Flush();
            }
            edgeWriter.Close();
            nodeWriter.Close();
        }

        public static void CreateSerializeEdge(string textFilePath, string outputFilePath)
        {
            FileStream stream = File.Open(outputFilePath, FileMode.Create);
            Graph graph = new Graph(new TextReader());
            BinaryFormatter formatter = new BinaryFormatter();

            graph.ReadEdge(textFilePath);
            formatter.Serialize(stream, graph.adjacencyList);
        }

        public static void CreateSerializeNode(string textFilePath, string outputFilePath)
        {
            FileStream stream = File.Open(outputFilePath, FileMode.Create);
            Graph graph = new Graph(new TextReader());
            BinaryFormatter formatter = new BinaryFormatter();

            graph.ReadNode(textFilePath);
            formatter.Serialize(stream, graph.vertices);
        }

        public static void CreateBinaryEdge(string textFilePath, string outputFilePath)
        {
            BinaryWriter edgeWriter = new BinaryWriter(File.Open(outputFilePath, FileMode.Create));
            Graph graph;

            //create edge binary
            Console.WriteLine("Start reading edges...");
            try
            {
                //Initializing
                graph = new Graph(new TextReader());
                graph.ReadEdge(textFilePath);

                //Write header
                edgeWriter.Write((byte)graph.dimension);

                //Write adjacencyList
                foreach (string key in graph.adjacencyList.Keys)
                {
                    List<Edge> neighbor = graph.adjacencyList[key];                    
                    edgeWriter.Write(key);
                    edgeWriter.Write((byte)neighbor.Count);

                    foreach(Edge edge in neighbor)
                    {
                        edgeWriter.Write(edge.Des);
                        foreach(float weight in edge.Weights)                        
                            edgeWriter.Write(weight);
                    }
                }
                Console.WriteLine("Complete reading edges");
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Failed reading edges");
            }
        }       

        public static void CreateBinaryNode(string textFilePath, string outputFilePath)
        {
            BinaryWriter nodeWriter = new BinaryWriter(File.Open(outputFilePath, FileMode.Create));

            //create binary node
            Console.WriteLine("Start reading vertices...");
            try
            {
                //Initializing
                string[] lines = File.ReadAllLines(textFilePath);

                //Each line is a Edge
                foreach (string line in lines)
                {
                    try
                    {
                        string[] tokens = line.Split(' ');
                        float longitude = float.Parse(tokens[1]);
                        float latitude = float.Parse(tokens[2]);

                        nodeWriter.Write(tokens[0]);
                        nodeWriter.Write(longitude);
                        nodeWriter.Write(latitude);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                }
                Console.WriteLine("Complete reading vertices");
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Failed reading vertices");
            }
        }

        public static void RandomWeight(string filePath, string outputFilePath, int randomAmount)
        {
            StreamWriter writer = new StreamWriter(File.Open(outputFilePath, FileMode.Create));
            string[] lines = File.ReadAllLines(filePath);
            Random random = new Random();

            foreach(string line in lines)
            {
                writer.Write(line);
                for(int i=0; i< randomAmount; ++i)                
                    writer.Write(" " + (float)(random.Next(200) * random.NextDouble()));
                writer.WriteLine();
            }
            writer.Close();
        }
    }
}

/*public static void BinaryEdge(string textFilePath, string outputFilePath)
        {
            BinaryWriter edgeWriter = new BinaryWriter(File.Open(outputFilePath, FileMode.Create));
         
            //create edge binary
            Console.WriteLine("Start reading edges...");
            try
            {
                //Initializing
                string[] lines = File.ReadAllLines(textFilePath);

                //Each line is a Edge
                foreach (string line in lines)
                {
                    string[] tokens = line.Split(' ');

                    edgeWriter.Write((UInt16)int.Parse(tokens[0]));
                    edgeWriter.Write((UInt16)int.Parse(tokens[1]));
                    edgeWriter.Write((UInt16)int.Parse(tokens[2]));
                    for (int i = 3; i < tokens.Length; ++i)
                        edgeWriter.Write(float.Parse(tokens[i]));

                    edgeWriter.Write(float.Parse(tokens[4]));

                }
                Console.WriteLine("Complete reading edges");
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Failed reading edges");
            }
        }*/

