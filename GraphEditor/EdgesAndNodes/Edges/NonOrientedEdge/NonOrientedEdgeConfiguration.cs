using System.Windows.Media;

namespace GraphEditor.EdgesAndNodes.Edges
{
    internal class NonOrientedEdgeConfiguration : EdgeConfiguration
    {
        public NonOrientedEdgeConfiguration()
        {
            Margin = 4;
            Height = 10;
            RadiusX = 4;
            RadiusY = 4;
            StrokeColor = Colors.MediumPurple;
            Width = 10;
            StrokeThickness = 3;
            EdgeOffsetLeft = 5;
        }
    }
}
