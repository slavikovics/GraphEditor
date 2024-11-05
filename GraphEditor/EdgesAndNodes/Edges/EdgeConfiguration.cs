using System.Windows.Media;

namespace GraphEditor.EdgesAndNodes.Edges
{
    internal class EdgeConfiguration
    {
        public int Margin { get; set; }

        public int Height { get; set; }

        public int RadiusX { get; set; } 

        public int RadiusY { get; set; }

        public double Width { get; set; }

        public Color StrokeColor { get; set; }

        public int StrokeThickness { get; set; }

        public int EdgeOffsetLeft { get; set; }

        public EdgeConfiguration()
        {
            StrokeColor = Colors.MediumPurple;
        }
    }
}
 