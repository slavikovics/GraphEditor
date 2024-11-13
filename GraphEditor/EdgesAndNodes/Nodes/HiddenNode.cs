using GraphEditor.EdgesAndNodes.Edges;
using System.Windows;
using System.Windows.Controls;

namespace GraphEditor.EdgesAndNodes.Nodes
{
    /// <summary>
    /// TODO: implement this class to link one edge to the center of another edge 
    /// </summary>
    public class HiddenNode : Node
    {
        private const int HiddenNodeEllipseDimensions = 40;

        public HiddenNode(double canvasLeft, double canvasTop, Canvas parent, MainWindow window, string id) : base(canvasLeft, canvasTop, parent, window, 0)
        {
            HideNode();
            Id = BuildIdString(id);
            window.OnMagicWandOrder -= OnMagicWandOrder;
            EndMovementAnimations();
        }

        private void HideNode()
        {
            Ellipse.Visibility = Visibility.Hidden;
            TextBlock.Visibility = Visibility.Hidden;
        }

        public override int GetEllipseDimensions()
        {
            return HiddenNodeEllipseDimensions;
        }

        protected override string BuildIdString(string idBase)
        {
            return "hn:" + idBase;
        }
    }
}
