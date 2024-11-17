using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace GraphEditor.GraphLogic
{
    public static class Pathfinder
    {
        public static List<Node> FindListWithoutLast(List<Node> nodes)
        {
            List<Node> result = new List<Node>();
            for (int i = 0; i < nodes.Count - 1; i++)
            {
                    result.Add(nodes[i]);
            }

            return result;
        }
        
        public static List<List<Node>> FindPaths(Graph graph, Node startNode, Node endNode)
        {
            if (startNode == null || endNode == null) return null;
            Node selectedNode = startNode;
            List<List<Node>> paths = new List<List<Node>>();
            List<Node> path = new List<Node>();
            paths.Add(path);
            path.Add(selectedNode);
            bool wasFirstModified;
            bool wasAnythingChanged;
            
            while (true)
            {
                List<List<Node>> newPaths = new List<List<Node>>();
                wasAnythingChanged = false;
                foreach (List<Node> onePath in paths)
                {
                    wasFirstModified = false;
                    List<Node> nextNodes = graph.GoNextNodes(onePath.Last());
                    foreach (Node nextNode in nextNodes)
                    {
                        if (onePath.Contains(nextNode)) continue;
                        wasAnythingChanged = true;
                        if (!wasFirstModified)
                        {
                            onePath.Add(nextNode);
                            wasFirstModified = true;
                            continue;
                        }
                        List<Node> newPath = new List<Node>();
                        newPath.AddRange(FindListWithoutLast(onePath));
                        newPath.Add(nextNode);
                        newPaths.Add(newPath);
                    }
                }
                
                paths.AddRange(newPaths);
                newPaths.Clear();
                foreach (List<Node> onePath in paths)
                {
                    if (onePath.Last() == endNode)
                    {
                        PrintPaths(paths);
                        return paths;
                    }
                }
                if (!wasAnythingChanged) return null;
            }
        }

        private static void PrintPaths(List<List<Node>> paths)
        {
            foreach (List<Node> onePath in paths)
            {
                foreach (Node oneNode in onePath)
                {
                    Console.Write(oneNode.ToString() + " -> ");
                }

                Console.WriteLine();
            }
        }
    }
}