using GraphEditor.EdgesAndNodes;
using GraphEditor.EdgesAndNodes.Edges;
using GraphEditor.GraphsManagerControls;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace GraphEditor
{
    internal class Edge : IRenamable, IEdgeable
    {
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
        private double desiredWidth;
        private Node _firstNode;
        private Node _secondNode;
        private MainWindow _mainWindow;
        private Canvas _mainCanvas;
        private Rectangle edgeVisualRepresentation;
        private Brush edgeBrush;
        private bool isAnimated = false;

        public Edge(Node firstNode, Node secondNode, MainWindow window, Canvas mainCanvas)
        {
            _firstNode = firstNode;
            _secondNode = secondNode;
            _mainWindow = window;
            _mainCanvas = mainCanvas;
            edgeVisualRepresentation = new Rectangle();

            SetUpEdgeVisualRepresentation();
            SetUpEvents();      
        }

        private void SetUpEdgeVisualRepresentation()
        {
            _mainCanvas.Children.Insert(0, edgeVisualRepresentation);
            edgeVisualRepresentation.Height = Height;
            edgeVisualRepresentation.Width = Width;
            edgeVisualRepresentation.RadiusX = RadiusX;
            edgeVisualRepresentation.RadiusY = RadiusY;
            edgeBrush = new SolidColorBrush(StrokeColor);
            edgeVisualRepresentation.Stroke = edgeBrush;
            edgeVisualRepresentation.StrokeThickness = StrokeThickness;
        }

        private void SetUpEvents()
        {
            _firstNode.OnNodeMoved += OnNodePositionChanged;
            _secondNode.OnNodeMoved += OnNodePositionChanged;
            edgeVisualRepresentation.LayoutUpdated += EdgeVisualRepresentationRenderTransformUpdate;
            edgeVisualRepresentation.MouseEnter += EdgeVisualRepresentationMouseEnter;
            edgeVisualRepresentation.MouseLeave += EdgeVisualRepresentationMouseLeave;
            edgeVisualRepresentation.MouseDown += EdgeVisualRepresentationMouseDown;
            edgeVisualRepresentation.Loaded += EdgeVisualRepresentationLoaded;
        }

        private void EdgeVisualRepresentationMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _mainWindow.OnEdgeSelected(this, e);
        }

        private void EdgeVisualRepresentationMouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ColorAnimation edgeHoverAnimation = new ColorAnimation();
            edgeHoverAnimation.To = StrokeColor;
            edgeHoverAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(200));
            edgeBrush.BeginAnimation(SolidColorBrush.ColorProperty, edgeHoverAnimation);
        }

        private void EdgeVisualRepresentationMouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ColorAnimation edgeHoverAnimation = new ColorAnimation();
            edgeHoverAnimation.To = Color.FromArgb(255, 153, 143, 199);
            edgeHoverAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(200));
            edgeBrush.BeginAnimation(SolidColorBrush.ColorProperty, edgeHoverAnimation);
        }

        private void EdgeVisualRepresentationRenderTransformUpdate(object sender, EventArgs e)
        {
            double originLeft = CalculateRenderTransformOriginLeft();
            
            edgeVisualRepresentation.RenderTransformOrigin = new Point(originLeft, 0.5);
        }

        private void EdgeVisualRepresentationLoaded(object sender, RoutedEventArgs e)
        {
            EdgePositioning(false);
            _mainWindow.UpdateLayout();
           
            AnimateEdgeCreation();
        }

        public void OnNodePositionChanged(object sender, EventArgs e)
        {
            EdgePositioning(true);
        }

        public void EdgePositioning(bool isInGraph)
        {
            double angle = CalculateAngle(_firstNode, _secondNode);
            double width = EdgeCalculations.CalculateFinalWidth(_firstNode, _secondNode, EdgeOffsetLeft);

            if (isInGraph)
            {
                if (Angle == angle) return;
                if (Width == width) return;
            }

            edgeVisualRepresentation.BeginAnimation(Rectangle.WidthProperty, null);

            if (width >= EdgeOffsetLeft) edgeVisualRepresentation.Width = width;
            else if (isInGraph)
            {
                edgeVisualRepresentation.Width = 0;
                return;
            }

            Width = width;
            Angle = angle;
            
            _offsetTop = Height / 2;

            double originLeft = CalculateRenderTransformOriginLeft();

            edgeVisualRepresentation.SetValue(Canvas.LeftProperty, EdgeCalculations.CalculateEdgePositionBaseLeft(_firstNode) + EdgeOffsetLeft + _firstNode.GetEllipseDimensions() / 2);
            edgeVisualRepresentation.SetValue(Canvas.TopProperty, EdgeCalculations.CalculateEdgePositionBaseTop(_firstNode) - _offsetTop);

            edgeVisualRepresentation.RenderTransformOrigin = new Point(originLeft, 0.5);
            RotateTransform rotateTransform = new RotateTransform(CalculateAngle(_firstNode, _secondNode));

            edgeVisualRepresentation.RenderTransform = rotateTransform;

        }

        public void EdgeDragged(double dragDeltaX, double dragDeltaY)
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
            edgeWidthAnimation.Completed += EdgeWidthAnimationCompleted;
            edgeVisualRepresentation.BeginAnimation(Rectangle.WidthProperty, edgeWidthAnimation);

        }

        public int GetFirstNodeId()
        {
            return _firstNode._id;
        }

        public int GetSecondNodeId()
        {
            return _secondNode._id;
        }

        private void EdgeWidthAnimationCompleted(object sender, EventArgs e)
        {
            edgeVisualRepresentation.LayoutUpdated += EdgeVisualRepresentationRenderTransformUpdate;
        }

        //private double CalculateFinalWidth(Node node1, Node node2)
        //{
        //    return EdgeCalculations.CalculateLengthBetweenNodes(node1, node2) - node1.GetEllipseDimensions() - 2 * EdgeOffsetLeft;
        //}

        private double CalculateAngle(Node node1, Node node2)
        {
            double AngleRadians = Math.Atan(EdgeCalculations.CalculateDeltaY(node1, node2) / EdgeCalculations.CalculateDeltaX(node1, node2));
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

        private double CalculateRenderTransformOriginLeft()
        {
            if (Width == 0) return 0;

            double originLeft = -1 * (_firstNode.GetEllipseDimensions() / 2 + 5) / edgeVisualRepresentation.Width;

            if (originLeft > 100 || originLeft < -100) return 0;

            return originLeft;
        }

        public override string ToString()
        {
            return  "Edge " + _firstNode._id + " = " + _secondNode._id;
        }

        public void Rename(string newName)
        {

        }

        public void Remove()
        {
            _mainCanvas.Children.Remove(edgeVisualRepresentation);
        }

        public List<int> GetNodesDependencies()
        {
            List<int> dependencies = new List<int>();
            dependencies.Add(GetFirstNodeId());
            dependencies.Add(GetSecondNodeId());
            return dependencies;
        }
    }
}
