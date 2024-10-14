using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Linq;

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

        private double _transformRotateAngleCalculationResult;
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

            // TODO call width animation
            double angle = CalculateAngle(firstNode, secondNode);

            edgeVisualRepresentation.Width = CalculateFinalWidth(firstNode, secondNode);
            edgeVisualRepresentation.SetValue(Canvas.LeftProperty, GetEdgePositionLeft(firstNode));
            edgeVisualRepresentation.SetValue(Canvas.TopProperty, GetEdgePositionTop(firstNode));

            RotateTransform rotateTransform = new RotateTransform(CalculateAngle(firstNode, secondNode));

            edgeVisualRepresentation.RenderTransform = rotateTransform;

            mainCanvas.Children.Add(edgeVisualRepresentation);
        }

        private double GetEdgePositionBaseLeft(Node node1, Node node2)
        {
            double basePointLeft = (double)node1.ellipse.GetValue(Canvas.LeftProperty) + node1.GetEllipseDimensions() / 2;
            return basePointLeft;
        }

        private double GetEdgePositionOffsetLeft(Node node1, Node node2)
        {
            // return (double)node.ellipse.GetValue(Canvas.LeftProperty) + node.GetEllipseDimensions() + Margin;

            double basePointLeft = GetEdgePositionBaseLeft(node1, node2);
            double otherAngle = _transformRotateAngleCalculationResult - 270;
            double offsetPointLeft = Math.Cos(otherAngle) * Width / 2;
            
            // with knowing angle and Rectangle height we can find another angle and two carets of triangle and then by deviding them by 2 we can find offset 
            return Math.Abs(offsetPointLeft);
        }

        private double GetSignedOffsetLeft(Node node1, Node node2)
        {
            if (node1.GetEdgePositionBaseLeft(node1, node2) <= node2.)
            if (node1.GetPosLeft())
            {
                if (node1.GetPosLeft() <= node2.GetPosLeft())
                {
                    if (node1.GetPosTop() <= node2.GetPosTop())
                    {
                        return
                    }
                    else
                    {
                        return
                    }
                }
                else
                {
                    if (node1.GetPosTop() <= node2.GetPosTop())
                    {
                        return
                    }
                    else
                    {
                        return
                    }
                }
            }
        }

        private double GetEdgePositionOffsetTop(Node node1, Node node2)
        {
            double basePointTop = GetEdgePositionBaseTop(node1);
            double otherAngle = _transformRotateAngleCalculationResult - 270;
            double offsetPointTop = Math.Cos(otherAngle) * Width / 2;

            return Math.Abs(offsetPointTop);
        }

        private double GetEdgePositionBaseTop(Node node)
        {
            // return (double)node.ellipse.GetValue(Canvas.TopProperty) + node.GetEllipseDimensions() / 2 - Height;
            return (double)node.ellipse.GetValue(Canvas.TopProperty) + node.GetEllipseDimensions() / 2;
        }

        private double CalculateFinalWidth(Node node1, Node node2)
        {
            // return CalculateLengthBetweenNodes(node1, node2) - node1.GetEllipseDimensions() - 2 * Margin;
            return CalculateLengthBetweenNodes(node1, node2);
        }

        private double CalculateAngle(Node node1, Node node2)
        {
            double AngleRadians = Math.Atan(CalculateDeltaY(node1, node2) / CalculateDeltaX(node1, node2));
            double AngleDegrees = AngleRadians / Math.PI * 180 * -1;

            if (node1.GetPosLeft() <= node2.GetPosLeft())
            {
                if (node1.GetPosTop() <= node2.GetPosTop())
                {
                    _transformRotateAngleCalculationResult = 360 - AngleDegrees;
                    return 360 - AngleDegrees;
                }
                else
                {
                    _transformRotateAngleCalculationResult = -1 * AngleDegrees;
                    return -1 * AngleDegrees; 
                }
            }
            else
            {
                if (node1.GetPosTop() <= node2.GetPosTop())
                {
                    _transformRotateAngleCalculationResult = 180 + (-1) * AngleDegrees;
                    return 180 + (-1) * AngleDegrees;
                }
                else
                {
                    _transformRotateAngleCalculationResult = 180 - AngleDegrees;
                    return 180 - AngleDegrees;
                }
            }
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
