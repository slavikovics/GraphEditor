using System.Windows.Media;

namespace GraphEditor.EdgesAndNodes.Edges.EdgesOriented
{
    internal class OrientedEdgeConfiguration : EdgeConfiguration
    {
        public int PencileHeight;

        public OrientedEdgeConfiguration()
        {
            Margin = 4;
            Height = 4;
            PencileHeight = 12;
            RadiusX = 4;
            RadiusY = 4;
            StrokeColor = Colors.MediumPurple;
            Width = 10;
            StrokeThickness = 3;
            EdgeOffsetLeft = 5;
        }
    }
}
