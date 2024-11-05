using System.Collections.Generic;
using System.Windows.Controls;

namespace GraphEditor
{
    internal class NodeAnimationController
    {
        private List<Node> _nodes;

        private double _dragDeltaX;
        private double _dragDeltaY;
        private Canvas _canvas;

        public NodeAnimationController(Canvas canvas)
        {
            _nodes = new List<Node>();
            _canvas = canvas;
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
                if (!_canvas.Children.Contains(node.Ellipse)) continue;
                node.Ellipse.SetValue(Canvas.TopProperty, (double)node.GetPosTop() + _dragDeltaY);
                node.Ellipse.SetValue(Canvas.LeftProperty, (double)node.GetPosLeft() + _dragDeltaX);
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
