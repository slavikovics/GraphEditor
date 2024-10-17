using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
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
        public double Width = 10;
        public Color StrokeColor = Colors.MediumPurple;
        public const int StrokeThickness = 3;
        public const int EdgeOffsetLeft = 5;

        private double _offsetTop;
        private double Angle;
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

            EdgePositioning(false);


            _firstNode.OnNodeMoved += OnNodePositionChanged;
            _secondNode.OnNodeMoved += OnNodePositionChanged;

            mainCanvas.Children.Add(edgeVisualRepresentation);
            AnimateEdgeCreation();
        }

        public void OnNodePositionChanged(object sender, EventArgs e)
        {
            EdgePositioning(true);
        }

        public void EdgePositioning(bool isInGraph)
        {
            
            double angle = CalculateAngle(_firstNode, _secondNode);
            double width = CalculateFinalWidth(_firstNode, _secondNode);

            if (isInGraph)
            {
                if (Angle == angle) return;
                if (Width == width) return;
                edgeVisualRepresentation.BeginAnimation(Rectangle.WidthProperty, null);
                if (width >= EdgeOffsetLeft) edgeVisualRepresentation.Width = width;
                else
                {
                    edgeVisualRepresentation.Width = 0;
                    return;
                }
            }
            Width = width;
            Angle = angle;
            
            _offsetTop = Height / 2;

            double originLeft = 0;
            if (Width != 0)
            {
                originLeft = -1 * (_firstNode.GetEllipseDimensions() / 2 + 5) / Width;
                if (!isInGraph)
                {
                    Console.WriteLine("FirstNode dimensions: " + _firstNode.GetEllipseDimensions());
                    Console.WriteLine("OriginLeft: " + originLeft);
                }
            }

            edgeVisualRepresentation.SetValue(Canvas.LeftProperty, GetEdgePositionBaseLeft(_firstNode) + EdgeOffsetLeft + _firstNode.GetEllipseDimensions() / 2);
            edgeVisualRepresentation.SetValue(Canvas.TopProperty, GetEdgePositionBaseTop(_firstNode) - _offsetTop);
            edgeVisualRepresentation.RenderTransformOrigin = new System.Windows.Point(originLeft, 0.5);

            RotateTransform rotateTransform = new RotateTransform(CalculateAngle(_firstNode, _secondNode));

            edgeVisualRepresentation.RenderTransform = rotateTransform;
        }

        public void EdgeDraged(double dragDeltaX, double dragDeltaY)
        {
            edgeVisualRepresentation.SetValue(Canvas.LeftProperty, (double)edgeVisualRepresentation.GetValue(Canvas.LeftProperty) + dragDeltaX);
            edgeVisualRepresentation.SetValue(Canvas.TopProperty, (double)edgeVisualRepresentation.GetValue(Canvas.TopProperty) + dragDeltaY);
        }

        private void AnimateEdgeCreation()
        {
            DoubleAnimation edgeWidthAnimation = new DoubleAnimation();
            edgeWidthAnimation.From = Height;
            edgeWidthAnimation.To = Width;
            edgeWidthAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(300));
            edgeWidthAnimation.AccelerationRatio = 0.5;
            edgeWidthAnimation.DecelerationRatio = 0.5;
            edgeVisualRepresentation.BeginAnimation(Rectangle.WidthProperty, edgeWidthAnimation);
        }

        private double GetEdgePositionBaseLeft(Node node)
        {
            double basePointLeft = (double)node.ellipse.GetValue(Canvas.LeftProperty) + node.GetEllipseDimensions() / 2;
            return basePointLeft;
        }

        private double GetEdgePositionBaseTop(Node node)
        {
            // return (double)node.ellipse.GetValue(Canvas.TopProperty) + node.GetEllipseDimensions() / 2 - Height;
            return (double)node.ellipse.GetValue(Canvas.TopProperty) + node.GetEllipseDimensions() / 2;
        }

        private double CalculateFinalWidth(Node node1, Node node2)
        {
            // return CalculateLengthBetweenNodes(node1, node2) - node1.GetEllipseDimensions() - 2 * Margin;
            return CalculateLengthBetweenNodes(node1, node2) - node1.GetEllipseDimensions() - 2 * EdgeOffsetLeft;
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

        public override string ToString()
        {
            return  _firstNode._id + " - " + _secondNode._id;
        }
    }
}
