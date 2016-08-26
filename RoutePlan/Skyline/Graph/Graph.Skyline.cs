﻿using System;
using System.Collections.Generic;

partial class Graph
{
    private class Skyline
    {
        private Graph graph;
        //Use to enum probable path
        private List<Path> enumPaths;
        private HashSet<string> onPath;
        private float[] pathWeights;

        public Skyline(Graph graph)
        {
            this.graph = graph;
        }

        public List<Path> SkylineQuery(string start, string end)
        {
            //Initialize
            enumPaths = new List<Path>();
            onPath = new HashSet<string>();
            pathWeights = new float[graph.dimension];

            for (int i = 0; i < pathWeights.Length; ++i)
                pathWeights[i] = 0;

            //Enumerate paths and select skyline paths
            Enumerate(start, end);

            return enumPaths;
        }

        //If be dominate by skylinePahts than return false,
        //else return true and add this path to skylinePahts
        private void DominateAdd(Path newPath)
        {
            List<Path> removePaths = new List<Path>();
            bool beDominateFlag = false;

            foreach(Path path in enumPaths)
            {
                switch (Dominate(newPath.weights, path.weights))
                {
                    case DominateResult.NON_DOMINATE:
                        //Do nothing
                        break;

                    case DominateResult.BE_DOMINATE:
                        beDominateFlag = true;
                        break;

                    case DominateResult.DOMINATE:
                        removePaths.Add(path);
                        break;
                }               
            }

            if (!beDominateFlag)
            {
                enumPaths.Add(newPath);
                enumPaths.RemoveAll(path => removePaths.Contains(path));
            }
        }

        private bool BeDominateByEnumPaths(float[] weight)
        {
            foreach(Path path in enumPaths)            
                if (Dominate(weight, path.weights) == DominateResult.BE_DOMINATE)
                    return true;
            return false;
        }

        private void Enumerate(string start, string end)
        {
            onPath.Add(start);

            if (start == end)
            {
                DominateAdd(new Path(new List<string>(onPath), (float[])pathWeights.Clone()));
                //enumPaths.Add(new Path(new List<string>(onPath), (float[])pathWeights.Clone()));
                onPath.Remove(start);
                //Console.WriteLine("Enum " + enumPaths.Count + " paths");
                return;
            }
            else if (BeDominateByEnumPaths(pathWeights))
            {
                //Console.WriteLine("return " + new Path(new List<string>(onPath), (float[])pathWeights.Clone()));
                onPath.Remove(start);
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
}
