using GraphEditor.EdgesAndNodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphEditor
{
    internal class EdgeAnimationController
    {
        private List<IEdge> edges;
        private Node _controlNode;

        public EdgeAnimationController(Node controlNode)
        {
            edges = new List<IEdge>();
            _controlNode = controlNode;
            _controlNode.OnNodesAnimated += OnNodesAnimated;
        }

        private void OnNodesAnimated()
        {
            foreach (IEdge edge in edges)
            {
                edge.EdgePositioning(true);
            }
        }

        public void AddEdge(IEdge edge)
        {
            edges.Add(edge);
        }

        public void RemoveEdge(IEdge edge)
        {
            edges.Remove(edge);
        }

        public void EdgesDragged(double dragDeltaX, double dragDeltaY)
        {
            foreach (IEdge edge in edges)
            {
                edge.EdgeDragged(dragDeltaX, dragDeltaY);
            }
        }
    }
}
