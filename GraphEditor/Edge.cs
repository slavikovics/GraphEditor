using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphEditor
{
    internal class Edge
    {

        // Height="10"
        // RadiusX="4"
        // RadiusY="4"
        // Canvas.Left="575"
        // Canvas.Top="464"
        // Width="10"
        // Stroke="#FFAEA3D8"
        // FlowDirection="RightToLeft"
        // StrokeThickness="3"
        // RenderTransformOrigin="0.5,0.5"

        public const int Margin = 4;
        public const int Height = 10;
        public const int RadiusX = 4;
        public const int RadiusY = 4;
        public const int Width = 10;
        public Color StrokeColor = Colors.MediumPurple;
        public const int StrokeThickness = 3;

        private Node _firstNode;
        private Node _secondNode;
        private MainWindow _mainWindow;
        private Canvas _mainCanvas;
        private Rectangle edgeVisualRepresentation;

        public Edge(Node firstNode, Node secondNode, MainWindow window, Canvas mainCanvas)
        {
            _firstNode = firstNode;
            _secondNode = secondNode;
            _mainWindow = window;
            _mainCanvas = mainCanvas;

            edgeVisualRepresentation = new Rectangle();
            edgeVisualRepresentation.Height = Height;
            edgeVisualRepresentation.Width = Width;
            edgeVisualRepresentation.RadiusX = RadiusX;
            edgeVisualRepresentation.RadiusY = RadiusY;
            edgeVisualRepresentation.Stroke = new SolidColorBrush(StrokeColor);
            edgeVisualRepresentation.StrokeThickness = StrokeThickness;
        }

        private double GetEdgePositionLeft(Node node)
        {
           return (double)node.ellipse.GetValue(Canvas.LeftProperty) + node.GetEllipseDimensions() + Margin;
        }

        private double GetEdgePositionTop(Node node)
        {
            return (double)node.ellipse.GetValue(Canvas.TopProperty) + node.GetEllipseDimensions() / 2 - Height;
        }

        private double CalculateFinalWidth(Node node1, Node node2)
        {
            return CalculateLengthBetweenNodes(node1, node2) - node1.GetEllipseDimensions() - 2 * Margin;
        }

        private double CalculateAngle(Node node1, Node node2)
        {
            return 360 - Math.Atan(CalculateDeltaX(node1, node2) / CalculateDeltaY(node1, node2));
        }

        private double CalculateLengthBetweenNodes(Node node1, Node node2)
        {
            double deltaX = CalculateDeltaX(node1, node2);
            double deltaY = CalculateDeltaY(node1, node2);

            return Math.Sqrt(Math.Pow(deltaX, 2) + Math.Pow(deltaY, 2));
        }

        private double CalculateDeltaX(Node node1, Node node2)
        {
            return (double)node2.GetPosLeft() - (double)node1.GetPosLeft();
        }

        private double CalculateDeltaY(Node node1, Node node2)
        {
            return (double)node2.GetPosTop() - (double)node1.GetPosTop();
        }
    }
}
