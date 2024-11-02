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
        private List<IEdgeable> edges;
        private Node _controlNode;

        public EdgeAnimationController(Node controlNode)
        {
            edges = new List<IEdgeable>();
            _controlNode = controlNode;
            _controlNode.OnNodesAnimated += OnNodesAnimated;
        }

        private void OnNodesAnimated()
        {
            foreach (IEdgeable edge in edges)
            {
                edge.EdgePositioning(true);
            }
        }

        public void AddEdge(IEdgeable edge)
        {
            edges.Add(edge);
        }

        public void RemoveEdge(IEdgeable edge)
        {
            edges.Remove(edge);
        }

        public void EdgesDragged(double dragDeltaX, double dragDeltaY)
        {
            foreach (IEdgeable edge in edges)
            {
                edge.EdgeDragged(dragDeltaX, dragDeltaY);
            }
        }
    }
}
