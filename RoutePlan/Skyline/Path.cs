using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Path
{
    public Path(List<string> nodes, float[] weights)
    {
        this.nodes = nodes;
        this.weights = weights;
    }

    public List<string> nodes;
    public float[] weights;

    public override string ToString()
    {
        string buffer = "[";

        if (nodes.Count > 0)
        {
            buffer += nodes[0];
            for (int i = 1; i < nodes.Count; ++i)
                buffer += "-" + nodes[i];
            buffer += "](";

            bool first = true;
            foreach(float weight in weights)
                if (first)
                {
                    buffer += weight;
                    first = false;
                }
                else
                {
                    buffer += "," + weight;
                }
            return buffer + ")";
        }
        else
        {
            return "no path";
        }
    }

    public override bool Equals(object other)
    {
        if(other is Path)
        {
            List<string> otherNodes = ((Path)other).nodes;
            if (otherNodes.Count == nodes.Count)
            {
                for (int i = 0; i < nodes.Count; ++i)
                    if (nodes[i] != otherNodes[i])
                        return false;
                return true;
            }
            else return false;

        }
        else return false; 
    }
}

