using System;
using System.Collections.Generic;
using System.IO;

class Graph
{
    static void Main(string[] args)
    {
        try
        {
           


        }
        catch (DimensionErrorException e)
        {
            Console.WriteLine(e.Message);
        }

        // Suspend the screen.
        Console.ReadLine();
    }


    private int _dimension;
    private Dictionary<string, List<Edge>> _adjacencyList = new Dictionary<string, List<Edge>>();
    private Dictionary<string, Vertex> _vertices = new Dictionary<string, Vertex>();
    private Dictionary<string, List<string>> _path;
    private HashSet<string> _found;
    private Dictionary<string, float> _distance;

    public Graph(int dimension)
    {
        _dimension = dimension;
    }

    public List<string> shortestPathQuery(string start, string end)
    {
        //Initialize
        _found = new HashSet<string>();
        _distance = new Dictionary<string, float>();
        _path = new Dictionary<string, List<string>>();

        _found.Add(start);
                
        foreach (string vertex in _adjacencyList.Keys)
        {
            _distance.Add(vertex, -1);
            _path.Add(vertex, new List<string>());            
        } 

        foreach(Edge edge in _adjacencyList[start])
        {
            _path[edge.Des].Add(start);
            _path[edge.Des].Add(edge.Des);
        }
            
        foreach(Edge edge in _adjacencyList[start])
            _distance[edge.Des] = edge.WeightsSum;

        //Start shortest path query
        while (true)
        {
            string expand = chooseShortestNeighbor();

            if (expand.Equals("") || expand.Equals(end))
            {
                return _path[end];
            }
            else
            {
                _found.Add(expand);
                foreach (string vertex in _adjacencyList.Keys)
                    update(expand, vertex);
            }
        }
    }

    public static string pathString(List<string> path)
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

    private void update(string expand, string des)
    {
        Edge desEdge = getEdge(expand, des);
        if (!_found.Contains(des) && desEdge != null)
        {
            float sum = _distance[expand] + desEdge.WeightsSum;
            if (_distance[des] == -1 || _distance[des] > sum)
            {
                _distance[des] = sum;
                _path[des] = new List<string>(_path[expand]);
                _path[des].Add(des);
            }
        }
    }

    private Edge getEdge(string vertex1, string vertex2)
    {
        foreach (Edge edge in _adjacencyList[vertex1])        
            if (edge.Des.Equals(vertex2)) return edge;        
        return null;
    }

    private string chooseShortestNeighbor()
    {
        float min = float.MaxValue;
        string choosedVertex = "";

        foreach(string vertex in _distance.Keys)
        {
            if(!_found.Contains(vertex) &&
                _distance[vertex] != -1 &&
                _distance[vertex] < min)
            {                
                min = _distance[vertex];
                choosedVertex = vertex;                
            }
        }
        return choosedVertex;
    }

    public List<Vertex> transformPath(List<string> path)
    {
        List<Vertex> vertexPath = new List<Vertex>();

        foreach(string vertex in path){
            vertexPath.Add(_vertices[vertex]);
        }
        return vertexPath;
    }

    public void ReadVertices(string path)
    {
        Console.WriteLine("Start reading vertices...");
        try
        {
            //Initializing
            string[] lines = File.ReadAllLines(path);

            //Each line is a Edge
            foreach (String line in lines)
            {
                string[] tokens = line.Split(' ');
                float longitude = float.Parse(tokens[1]);
                float latitude = float.Parse(tokens[2]);

                _vertices.Add(tokens[0], new Vertex(tokens[0], longitude, latitude));
            }
            Console.WriteLine("Complete reading vertices");
        }
        catch (FileNotFoundException e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine("Failed reading vertices");
        }
    }

    public void ReadEdge(string path)
    {
        Console.WriteLine("Start reading edges...");
        try
        {
            //Initializing
            string[] lines = File.ReadAllLines(path);

            //Each line is a Edge
            foreach (String line in lines)
            {
                string[] tokens = line.Split(' ');

                if (tokens.Length - 3 != _dimension)
                    throw new DimensionErrorException(_dimension, tokens.Length - 3);

                float[] weights = new float[_dimension];

                for (int j = 0; j < _dimension; ++j)
                    weights[j] = float.Parse(tokens[j + 3]);

                AddEdge(tokens[1], new Edge(tokens[2], weights));
                AddEdge(tokens[2], new Edge(tokens[1], weights));
            }
            Console.WriteLine("Complete reading edges");
        }
        catch (FileNotFoundException e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine("Failed reading edges");
        }
    }

    private void AddEdge(string key, Edge edge)
    {
        try
        {
            if(!_adjacencyList[key].Contains(edge))
                _adjacencyList[key].Add(edge);
        }
        catch (KeyNotFoundException e)
        {
            List<Edge> newList = new List<Edge>();
            newList.Add(edge);
            _adjacencyList.Add(key, newList);
        }
    }
    

    public void Display()
    {
        foreach (KeyValuePair<string, List<Edge>> pair in _adjacencyList)
        {
            Console.WriteLine("key={0} value=", pair.Key);
            foreach (Edge edge in pair.Value)
                Console.WriteLine("  " + edge);
        }
            

        if(_vertices.Count > 0) Console.WriteLine("Verteices: ");
        foreach(Vertex vertex in _vertices.Values){
            Console.WriteLine(vertex.ToString());
        }
        
    }
}

