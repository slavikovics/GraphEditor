using GraphEditor.EdgesAndNodes;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Xml.Linq;

namespace GraphEditor
{
    internal class EdgeAnimationController
    {
        private List<IEdge> _edges;
        private Node _controlNode;
        private Canvas _canvas;

        public EdgeAnimationController(Node controlNode, Canvas canvas)
        {
            _edges = new List<IEdge>();
            _controlNode = controlNode;
            _controlNode.OnNodesAnimated += OnNodesAnimated;
            _canvas = canvas;
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
                if (!_canvas.Children.Contains(edge.GetEdgeVisualRepresentation())) continue;
                edge.EdgeDragged(dragDeltaX, dragDeltaY);
            }
        }
    }
}
