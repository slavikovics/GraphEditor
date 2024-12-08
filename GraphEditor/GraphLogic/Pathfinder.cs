using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using GraphEditor.EdgesAndNodes;
using GraphEditor.EdgesAndNodes.Edges;

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

        public static List<List<Node>> FindFirstEulerCycle(Graph graph)
        {
            foreach (Node node in graph.Nodes)
            {
                List<List<Node>> result = FindPaths(graph, node, node);
                if (result != null) return result;
            }

            return null;
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
                        if (!ShouldNodeBeVisited(onePath, nextNode, startNode, endNode, graph)) continue;
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
                    if (onePath.Last() == endNode && CheckResult(onePath, startNode, endNode, graph))
                    {
                        PrintPaths(paths);
                        return paths;
                    }
                }

                if (!wasAnythingChanged) return null;
            }
        }

        private static bool CheckResult(List<Node> onePath, Node startNode, Node endNode, Graph graph)
        {
            if (onePath.Count - 1 < graph.Edges.Count) return false;
            return true;
        }

        private static bool ShouldNodeBeVisited(List<Node> onePath, Node nextNode, Node startNode, Node endNode, Graph graph)
        {
            if (startNode != endNode) return !onePath.Contains(nextNode);

            if (nextNode == onePath[onePath.Count - 1]) return false;
            
            for (int i = 0; i < onePath.Count - 1; i++)
            {
                if (onePath[i] == onePath[onePath.Count - 1] && onePath[i + 1] == nextNode ||
                    onePath[i] == nextNode && onePath[i + 1] == onePath[onePath.Count - 1]) return false;
            }
            
            /*foreach (IEdge edge in graph.Edges)
            {
                bool hasEdge = false;
                for (int i = 0; i < onePath.Count - 1; i++)
                {
                    if (edge.GetFirstNode() == onePath[i] && edge.GetSecondNode() == onePath[i + 1] ||
                        edge.GetFirstNode() == onePath[i + 1] && edge.GetSecondNode() == onePath[i])
                    {
                        hasEdge = true;
                        break;
                    }

                    if (edge.GetFirstNode() == onePath[onePath.Count - 1] && edge.GetSecondNode() == nextNode ||
                        edge.GetFirstNode() == nextNode && edge.GetSecondNode() == onePath[onePath.Count - 1])
                    {
                        hasEdge = true;
                        break;
                    }
                }

                if (!hasEdge) return false;
            }*/
            
            return true;
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