using System.Windows;
using System.Windows.Controls;

namespace GraphEditor.EdgesAndNodes.Nodes
{
    /// <summary>
    /// TODO: implement this class to link one edge to the center of another edge 
    /// </summary>
    internal class HiddenNode : Node
    {
        public HiddenNode(double canvasLeft, double canvasTop, Canvas parent, MainWindow window, int id, IEdge edge) : base(canvasLeft, canvasTop, parent, window, id)
        {
            HideNode();
            window.OnMagicWandOrder -= OnMagicWandOrder;
        }

        private void HideNode()
        {
            Ellipse.Visibility = Visibility.Hidden;
            _textBlock.Visibility = Visibility.Hidden;
        }
    }
}
