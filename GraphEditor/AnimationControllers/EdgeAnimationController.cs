using GraphEditor.EdgesAndNodes;
using System.Collections.Generic;

namespace GraphEditor
{
    internal class EdgeAnimationController
    {
        private List<IEdge> _edges;
        private Node _controlNode;

        public EdgeAnimationController(Node controlNode)
        {
            _edges = new List<IEdge>();
            _controlNode = controlNode;
            _controlNode.OnNodesAnimated += OnNodesAnimated;
        }

        private void OnNodesAnimated()
        {
            foreach (IEdge edge in _edges)
            {
                edge.EdgePositioning(true);
            }
        }

        public void AddEdge(IEdge edge)
        {
            _edges.Add(edge);
        }

        public void RemoveEdge(IEdge edge)
        {
            _edges.Remove(edge);
        }

        public void EdgesDragged(double dragDeltaX, double dragDeltaY)
        {
            foreach (IEdge edge in _edges)
            {
                edge.EdgeDragged(dragDeltaX, dragDeltaY);
            }
        }
    }
}
