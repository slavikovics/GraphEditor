using System;
using System.Collections.Generic;
using System.Linq;
using GraphEditor.EdgesAndNodes;
using GraphEditor.EdgesAndNodes.Edges;

namespace GraphEditor.GraphLogic
{
    public class Graph
    {
        public List<Node> Nodes { get; set; }
        
        public List<IEdge> Edges { get; set; }
        
        public string Name { get; set; }

        public Graph()
        {
            Nodes = new List<Node>();
            Edges = new List<IEdge>();
            Name = "graph";
        }

        public Graph(string name)
        {
            Nodes = new List<Node>();
            Edges = new List<IEdge>();
            Name = name;
        }

        public void AddEdge(IEdge edge)
        {
            Edges.Add(edge as Edge);
        }

        public void RemoveEdge(IEdge edge)
        {
            Edges.Remove(edge as Edge);
        }

        public int GetEdgesCount()
        {
            return Edges.Count;
        }

        public int GetNodePower(Node node)
        {
            int power = 0;
            
            foreach (IEdge edge in Edges)
            {
                if (edge.GetFirstNode() == node || edge.GetSecondNode() == node)
                {
                    power++;
                }
            }
            
            return power;
        }

        public bool CheckPowerIsEven(Node node)
        {
            return GetNodePower(node) % 2 == 0;
        }

        public void AddNode(Node node)
        {
            Nodes.Add(node);
        }

        public void RemoveNode(Node node)
        {
            Nodes.Remove(node);
        }

        public Node GetNodeById(string id)
        {
            foreach (Node node in Nodes)
            {
                if (node.Id == id) return node;
            }

            return null;
        }

        public void PrintAllNodes()
        {
            foreach (Node node in Nodes)
            {
                Console.WriteLine("Name: " + node.Name + " Id: " + node.Id);
            }
        }

        public bool IsNodeIdUnique(string id)
        {
            if (GetNodeById(id) != null) return false;
            return true;
        }

        public string GetNodeNameById(string id)
        {
            foreach (Node node in Nodes)
            {
                if (node.Id == id) return node.Name;
            }

            return null;
        }

        public IEdge GetEdgeByNodeIds(string firstNodeId, string secondNodeId)
        {
            foreach (Edge edge in Edges)
            {
                if (edge.GetFirstNodeId() == firstNodeId && edge.GetSecondNodeId() == secondNodeId)
                {
                    return edge;
                }
            }

            return null;
        }

        public Edge GetEdgeByTwoNodes(Node firstNode, Node secondNode)
        {
            foreach (Edge edge in Edges)
            {
                if (edge.GetFirstNode() == firstNode && edge.GetSecondNode() == secondNode || edge.GetFirstNode() == secondNode && edge.GetSecondNode() == firstNode)
                {
                    return edge;
                }
            }

            return null;
        }

        public void HighlightEdges(List<List<Node>> nodes, Node selectedNode, HighlightTargetColor targetColor)
        {
            if (nodes == null) return;
            List<Edge> edges = new List<Edge>();
            foreach (List<Node> list in nodes)
            {
                if (list.Last() != selectedNode) continue;
                for (int i = 0; i < list.Count - 1; i++)
                {
                    edges.Add(GetEdgeByTwoNodes(list[i], list[i + 1]));
                }

                break;
            }
            
            foreach (Edge edge in edges)
            { 
                GraphLogicAnimator.AnimateEdgeHighlight(edge, targetColor);
            }
        }

        public void HighlightIEdges(List<IEdge> edges)
        {
            foreach (IEdge edge in edges)
            {
                GraphLogicAnimator.AnimateEdgeHighlight(edge as Edge, HighlightTargetColor.Red);
            }
        }

        public void HighlightEdgesRemoval()
        {
            foreach (Edge edge in Edges)
            {
                GraphLogicAnimator.AnimateEdgeHighlightRemoval(edge);
            }
        }

        public List<Node> GoNextNodes(Node node)
        {
            List<Node> nodes = new List<Node>();
            foreach (Edge edge in Edges)
            {
                if (edge.GetSecondNode() == node)
                {
                    if (!nodes.Contains(edge.GetSecondNode())) nodes.Add(edge.GetFirstNode());
                }
                else if (edge is NonOrientedEdge && edge.GetFirstNode() == node)
                {
                    if (!nodes.Contains(edge.GetFirstNode())) nodes.Add(edge.GetSecondNode());   
                }
            }
            
            return nodes;
        }

        public void ClearGraph()
        {
            Nodes.Clear();
            Edges.Clear();
        }
    }
}