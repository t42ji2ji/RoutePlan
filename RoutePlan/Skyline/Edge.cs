using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Edge
{
    public string Des { get; set; }
    public float[] Weights { get; set; }

    public float WeightsSum()
    {   
        float sum = 0;
        foreach (int value in Weights)
            sum += value;
        return sum;        
    }

    public Edge(string destinationVertex, float[] weights)
    {
        Des = destinationVertex;
        Weights = weights;
    }

    public override string ToString()
    {
        return string.Format("{0}{1}", Des, WeightString());
    }

    private string WeightString()
    {
        if (Weights == null) return "No Weights";

        string weightString = "[" + Weights[0];
        int i;
        for (i = 1; i < Weights.Length; ++i)
            weightString += "," + Weights[i];
        weightString += "]";
        return weightString;
    }

    public override bool Equals(object otherEdge)
    {
        if (otherEdge is Edge)
            return ((Edge)otherEdge).Des.Equals(Des);        
        else return false;
    }
}