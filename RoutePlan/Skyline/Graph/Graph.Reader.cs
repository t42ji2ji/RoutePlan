partial class Graph
{
    public interface Reader
    {
        Graph Graph { set;}
        void ReadNode(string filePath);
        void ReadEdge(string filePath);
    }
}

