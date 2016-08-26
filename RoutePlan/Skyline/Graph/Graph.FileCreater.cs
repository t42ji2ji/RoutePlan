using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;


public partial class Graph
{
    public class FileCreater
    {
        void Main(string[] args)
        {
            AdjustEdgeText();

            Console.WriteLine("完成");
            Console.Read();
        }

        public static void JsonSerializeEdge()
        {
            Graph graph = new Graph(new BinaryReader());
            graph.ReadEdge("res/binary/oldenburg.edges");
            File.WriteAllText("oldenburg_edges.json", JsonConvert.SerializeObject(graph.adjacencyList));
            
        }

        public static void BinaryFileCreate()
        {
            try
            {
                StreamReader reader = new StreamReader(File.Open("config.json", FileMode.Open));
                Setting setting = JsonConvert.DeserializeObject<Setting>(reader.ReadToEnd());

                if (setting.EdgeFile != null && setting.EdgeOutput != null)
                    CreateBinaryEdge(setting.EdgeFile, setting.EdgeOutput);

                if (setting.NodeFile != null && setting.NodeOutput != null)
                    CreateBinaryNode(setting.NodeFile, setting.NodeOutput);

            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("config.json設定檔");
                StreamWriter writer = new StreamWriter(File.Open("config.json", FileMode.Create));
                writer.Write(JsonConvert.SerializeObject(new Setting(), Formatting.Indented));

                Console.WriteLine("已創建config.json設定檔，設定後再開啟程式");
                writer.Close();
            }
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

        public static void AdjustEdgeText()
        {
            string[] origin = File.ReadAllLines("res/text/ming_nodes (origin).txt");
            StreamWriter writer = new StreamWriter(File.Open("res/text/ming_nodes2.txt", FileMode.Create));
            string[] newLines = new string[origin.Length];

            foreach(string line in origin)
            {
                string[] token = line.Split(' ');
                string newLine = "";
                for (int i = 1; i < token.Length; ++i)
                {
                    if (i != 1) newLine += " ";
                     newLine += token[i];
                }
                writer.WriteLine(newLine);
            }
            writer.Close();
        }

        private class Setting
        {
            public string EdgeFile { get; set; }
            public string EdgeOutput { get; set; }
            public string NodeFile { get; set; }
            public string NodeOutput { get; set; }
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

