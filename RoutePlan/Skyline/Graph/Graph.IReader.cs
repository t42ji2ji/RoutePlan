public partial class Graph
{
    public interface IReader
    {
        Graph Graph { set; }
        void ReadNode(string filePath);
        void ReadEdge(string filePath);
    }
}




