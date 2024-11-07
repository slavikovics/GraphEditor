using GraphEditor.EdgesAndNodes.Edges;
using System.Windows;
using System.Windows.Controls;

namespace GraphEditor.EdgesAndNodes.Nodes
{
    /// <summary>
    /// TODO: implement this class to link one edge to the center of another edge 
    /// </summary>
    internal class HiddenNode : Node
    {
        private const int HiddenNodeEllipseDimensions = 40;

        public HiddenNode(double canvasLeft, double canvasTop, Canvas parent, MainWindow window, int id) : base(canvasLeft, canvasTop, parent, window, id)
        {
            HideNode();
            window.OnMagicWandOrder -= OnMagicWandOrder;
            EndMovementAnimations();
        }

        private void HideNode()
        {
            Ellipse.Visibility = Visibility.Hidden;
            _textBlock.Visibility = Visibility.Hidden;
        }

        public override int GetEllipseDimensions()
        {
            return HiddenNodeEllipseDimensions;
        }
    }
}
