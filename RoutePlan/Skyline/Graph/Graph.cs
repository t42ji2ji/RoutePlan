using System;
using System.Collections.Generic;
using System.IO;
using System.Collections;

/*
 * shortestPathQuery: 指定起終點，傳回一個路徑
 * CreateSubGraph: 指定擴展邊的最高數目，並由起點開始擴展
 * PathString: 傳回路徑代表的字串
 * TransformPath: 將路徑轉換成含有Vertex之路徑，Vertex包含座標，用於傳送資料至前端
 * ReadNode: 從txt檔案讀入「頂點」資訊
 * ReadEdge: 從txt檔案讀入「邊」資訊
 * BinaryRead: 從位元檔讀入邊與頂點資訊
 * CreateBinary: 由txt檔建立一個Binary檔。
 *   struct Edge{
 *      UInt16 EdgeID
 *      UInt16 Vertex1
 *      UInt16 Vertex2
 *      float Weight[...]
 *   }
 *   
 *   struct Node{
 *      UInt16 NodeID
 *      float Longitude
 *      float Latitude
 *   }
 * Display: 輸出整個Graph資訊      
 *   
*/

partial class Graph
{
    static void Main(string[] args)
    {
        Graph graph = new Graph(new BinaryReader());

        Console.WriteLine("start read edge");
        graph.ReadEdge("../res/binary/1000Edge(2)");
        Console.WriteLine("complete read edge");

        Console.WriteLine("start skyline query");
        List<List<string>> paths = graph.SkylineQuery("1", "100");
        Console.WriteLine("complete skyline query");

        foreach (var path in paths)
            Console.WriteLine(PathString(path));

        Console.WriteLine("Complete");
        Console.ReadLine();
    }

    private int dimension;
    private Reader reader;
    private Dictionary<string, List<Edge>> adjacencyList = new Dictionary<string, List<Edge>>();
    private Dictionary<string, Vertex> vertices = new Dictionary<string, Vertex>();
    private Dictionary<string, List<string>> path;
    private HashSet<string> found;
    private Dictionary<string, float> distance;

    public enum DominateResult
    {
        DOMINATE,
        BE_DOMINATE,
        NON_DOMINATE
    }   

    public Graph(Reader reader)
    {
        this.reader = reader;
        this.reader.Graph = this;
    }    

    public List<string> ShortestPathQuery(string start, string end)
    {
        //Initialize
        found = new HashSet<string>();
        distance = new Dictionary<string, float>();
        path = new Dictionary<string, List<string>>();

        found.Add(start);
                
        foreach (string vertex in adjacencyList.Keys)
        {
            distance.Add(vertex, -1);
            path.Add(vertex, new List<string>());            
        } 

        foreach(Edge edge in adjacencyList[start])
        {
            path[edge.Des].Add(start);
            path[edge.Des].Add(edge.Des);
        }
            
        foreach(Edge edge in adjacencyList[start])
            distance[edge.Des] = edge.WeightsSum;

        //Start shortest path query
        while (true)
        {
            string expand = ChooseShortestNeighbor();

            if (expand.Equals("") || expand.Equals(end))
            {
                return path[end];
            }
            else
            {
                found.Add(expand);
                foreach (string vertex in adjacencyList.Keys)
                    Update(expand, vertex);
            }
        }
    }

    public List<List<string>> SkylineQuery(string start, string end)
    {
        return new Skyline(this).SkylineQuery(start, end);
    }

    public static string PathString(List<string> path)
    {
        string buffer = "[";

        if(path.Count > 0)
        {
            buffer += path[0];
            for (int i = 1; i < path.Count; ++i)
                buffer += "-" + path[i];
            return buffer + "]";
        }
        else
        {
            return "no path";
        }       
    }

    private void Update(string expand, string des)
    {
        Edge desEdge = GetEdge(expand, des);
        if (!found.Contains(des) && desEdge != null)
        {
            float sum = distance[expand] + desEdge.WeightsSum;
            if (distance[des] == -1 || distance[des] > sum)
            {
                distance[des] = sum;
                path[des] = new List<string>(path[expand]);
                path[des].Add(des);
            }
        }
    }

    private Edge GetEdge(string vertex1, string vertex2)
    {
        foreach (Edge edge in adjacencyList[vertex1])        
            if (edge.Des.Equals(vertex2)) return edge;        
        return null;
    }

    private string ChooseShortestNeighbor()
    {
        float min = float.MaxValue;
        string choosedVertex = "";

        foreach(string vertex in distance.Keys)
        {
            if(!found.Contains(vertex) &&
                distance[vertex] != -1 &&
                distance[vertex] < min)
            {                
                min = distance[vertex];
                choosedVertex = vertex;                
            }
        }
        return choosedVertex;
    }

    public List<Vertex> TransformPath(List<string> path)
    {
        List<Vertex> vertexPath = new List<Vertex>();

        foreach(string vertex in path){
            vertexPath.Add(vertices[vertex]);
        }
        return vertexPath;
    }

    public List<List<Vertex>> TransformPaths(List<List<string>> paths)
    {
        List<List<Vertex>> vertexPaths = new List<List<Vertex>>();

        foreach (List<string> path in paths)        
            vertexPaths.Add(TransformPath(path));
        
        return vertexPaths;
    }
    
    public void ReadEdge(string filePath)
    {
        reader.ReadEdge(filePath);
    }

    public void ReadNode(string filePath)
    {
        reader.ReadNode(filePath);
    }

    private void AddEdge(string key, Edge edge)
    {
        try
        {
            if(!adjacencyList[key].Contains(edge))
                adjacencyList[key].Add(edge);
        }
        catch (KeyNotFoundException)
        {
            List<Edge> newList = new List<Edge>();
            newList.Add(edge);
            adjacencyList.Add(key, newList);
        }
    }

    public void Display()
    {
        foreach (KeyValuePair<string, List<Edge>> pair in adjacencyList)
        {
            Console.WriteLine("key={0} value=", pair.Key);
            foreach (Edge edge in pair.Value)
                Console.WriteLine("  " + edge);
        }
            

        if(vertices.Count > 0) Console.WriteLine("Verteices: ");
        foreach(Vertex vertex in vertices.Values){
            Console.WriteLine(vertex.ToString());
        }        
    }

    public static DominateResult Dominate(float[] self, float[] other)
    {
        bool dominateOther = true;
        bool beDominate = true;
        for (int i = 0; i < self.Length; ++i)
        {
            if (self[i] > other[i])
                dominateOther = false;
            if (self[i] < other[i])
                beDominate = false;
        }

        if (dominateOther) return DominateResult.DOMINATE; //If weights are the same, then dominate other
        else if (beDominate && !dominateOther) return DominateResult.BE_DOMINATE;
        else return DominateResult.NON_DOMINATE; //If not dominate each other, then dominateOther = false and beDominate = false
    }
}

