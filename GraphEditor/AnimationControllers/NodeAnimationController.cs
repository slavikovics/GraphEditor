using System.Collections.Generic;
using System.Windows.Controls;

namespace GraphEditor
{
    internal class NodeAnimationController
    {
        private List<Node> _nodes;

        private double _dragDeltaX;
        private double _dragDeltaY;

        public NodeAnimationController()
        {
            _nodes = new List<Node>();
        }

        public void SetDragParameters(double dragDeltaX, double dragDeltaY)
        {
            _dragDeltaX = dragDeltaX;
            _dragDeltaY = dragDeltaY;   
        }

        public void Drag()
        {
            foreach (Node node in _nodes)
            {
                node.ellipse.SetValue(Canvas.TopProperty, (double)node.ellipse.GetValue(Canvas.TopProperty) + _dragDeltaY);
                node.ellipse.SetValue(Canvas.LeftProperty, (double)node.ellipse.GetValue(Canvas.LeftProperty) + _dragDeltaX);
            }
        }

        public void EndMovementAnimations()
        {
            foreach (Node node in _nodes)
            {
                node.EndMovementAnimations();
            }
        }

        public void AddNode(Node node)
        {
            _nodes.Add(node);
        }

        public void RemoveNode(Node node)
        {
            _nodes.Remove(node);
        }
    }
}
