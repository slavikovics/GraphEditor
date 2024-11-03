using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace GraphEditor.EdgesAndNodes.Edges
{
    internal class EdgeConfiguration
    {
        public int Margin;
        public int Height;
        public int RadiusX;
        public int RadiusY;
        public double Width;
        public Color StrokeColor = Colors.MediumPurple;
        public int StrokeThickness;
        public int EdgeOffsetLeft;

        public EdgeConfiguration()
        {

        }
    }
}
 