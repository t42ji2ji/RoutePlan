using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
class Edge
{
    private string _destinationVertex;
    private float[] _weights;

    public string Des
    {
        get
        {
            return _destinationVertex;
        }
    }

    public float[] Weights
    {
        get
        {
            return _weights;
        }
    }

    public float WeightsSum
    {
        get
        {
            float sum = 0;
            foreach (int value in _weights)
                sum += value;
            return sum;
        }
    }

    public Edge(string destinationVertex, float[] weights)
    {
        _destinationVertex = destinationVertex;
        _weights = weights;
    }

    public override string ToString()
    {
        return string.Format("{0}{1}", _destinationVertex, WeightString());
    }

    private string WeightString()
    {
        if (_weights == null) return "No Weights";

        string weightString = "[" + _weights[0];
        int i;
        for (i = 1; i < _weights.Length; ++i)
            weightString += "," + _weights[i];
        weightString += "]";
        return weightString;
    }

    public override bool Equals(object otherEdge)
    {
        return ((Edge)otherEdge)._destinationVertex.Equals(_destinationVertex);
    }
}

