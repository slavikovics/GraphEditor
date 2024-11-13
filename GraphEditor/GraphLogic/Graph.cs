using System.Collections.Generic;
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

        /*public void RemoveAllEdgesConnectedToNode(string id)
        {
            List<Edge> edgesToRemove = new List<Edge>();
            foreach (Edge edge in Edges)
            {
                if (edge.GetFirstNodeId() == id || edge.GetSecondNodeId() == id)
                {
                    edgesToRemove.Add(edge);
                }
            }

            foreach (Edge edge in edgesToRemove)
            {
                RemoveEdge(edge);
            }
        }*/

        public int GetEdgesCount()
        {
            return Edges.Count;
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

        public void ClearGraph()
        {
            Nodes.Clear();
            Edges.Clear();
        }
    }
}