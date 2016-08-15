using System;
using System.Collections.Generic;

partial class Graph
{
    private class Skyline
    {
        private Graph graph;
        //Use to enum probable path
        private List<List<string>> enumPath;
        private List<float[]> enumWeights;
        private HashSet<string> onPath;
        private float[] pathWeights;

        public Skyline(Graph graph)
        {
            this.graph = graph;
        }

        public List<List<string>> SkylineQuery(string start, string end)
        {
            //Initialize
            List<List<string>> skylinePaths = new List<List<string>>();
            enumPath = new List<List<string>>();
            onPath = new HashSet<string>();
            enumWeights = new List<float[]>();
            pathWeights = new float[graph.dimension];

            for (int i = 0; i < pathWeights.Length; ++i)
                pathWeights[i] = 0;

            //Enumerate paths
            Enumerate(start, end);

            //Find skyline from enumerate paths results
            for (int i = 0; i < enumWeights.Count; i++)
            {
                bool beDominateFlag = false;
                for (int j = 0; j < enumWeights.Count; j++)
                    if (i != j &&
                        Dominate(enumWeights[i], enumWeights[j]) == DominateResult.BE_DOMINATE)
                    {
                        beDominateFlag = true;
                        break;
                    }

                if (!beDominateFlag)
                    skylinePaths.Add(enumPath[i]);
            }
            return skylinePaths;
        }

        private void Enumerate(string start, string end)
        {
            onPath.Add(start);

            if (start == end)
            {
                enumPath.Add(new List<string>(onPath));
                enumWeights.Add((float[])pathWeights.Clone());
                onPath.Remove(start);
                Console.WriteLine("Enum " + enumPath.Count + " paths");
                return;
            }

            foreach (Edge neighbor in graph.adjacencyList[start])
                if (!onPath.Contains(neighbor.Des))
                {
                    addToPathWeights(neighbor.Weights);
                    //detect be dominate
                    Enumerate(neighbor.Des, end);
                    SubstractToPathWeights(neighbor.Weights);
                }

            onPath.Remove(start);
        }

        private void addToPathWeights(float[] weights)
        {
            for (int i = 0; i < pathWeights.Length; ++i)
                pathWeights[i] += weights[i];
        }

        private void SubstractToPathWeights(float[] weights)
        {
            for (int i = 0; i < pathWeights.Length; ++i)
                pathWeights[i] -= weights[i];
        }
    }
}

