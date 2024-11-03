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
        //public const int Margin = 4;
        //public const int Height = 10;
        //public const int RadiusX = 4;
        //public const int RadiusY = 4;
        //public double Width = 10;
        //public Color StrokeColor = Colors.MediumPurple;
        //public const int StrokeThickness = 3;
        //public const int EdgeOffsetLeft = 5;

        private double _offsetTop;
        private double Angle;
        private Node _firstNode;
        private Node _secondNode;
        private MainWindow _mainWindow;
        private Canvas _mainCanvas;
        private Rectangle edgeVisualRepresentation;
        private Brush edgeBrush;

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
            edgeVisualRepresentation.Height = EdgeConfiguration.Height;
            edgeVisualRepresentation.Width = EdgeConfiguration.Width;
            edgeVisualRepresentation.RadiusX = EdgeConfiguration.RadiusX;
            edgeVisualRepresentation.RadiusY = EdgeConfiguration.RadiusY;
            edgeBrush = new SolidColorBrush(EdgeConfiguration.StrokeColor);
            edgeVisualRepresentation.Stroke = edgeBrush;
            edgeVisualRepresentation.StrokeThickness = EdgeConfiguration.StrokeThickness;
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
            edgeHoverAnimation.To = EdgeConfiguration.StrokeColor;
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
            double originLeft = EdgeCalculations.CalculateRenderTransformOriginLeft(EdgeConfiguration.Width, _firstNode, edgeVisualRepresentation);
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
            double angle = EdgeCalculations.CalculateAngle(_firstNode, _secondNode);
            double width = EdgeCalculations.CalculateFinalWidth(_firstNode, _secondNode, EdgeConfiguration.EdgeOffsetLeft);

            if (isInGraph)
            {
                if (Angle == angle) return;
                if (EdgeConfiguration.Width == width) return;
            }

            edgeVisualRepresentation.BeginAnimation(Rectangle.WidthProperty, null);

            if (width >= EdgeConfiguration.EdgeOffsetLeft) edgeVisualRepresentation.Width = width;
            else if (isInGraph)
            {
                edgeVisualRepresentation.Width = 0;
                return;
            }

            EdgeConfiguration.Width = width;
            Angle = angle;
            
            _offsetTop = EdgeConfiguration.Height / 2;

            double originLeft = EdgeCalculations.CalculateRenderTransformOriginLeft(EdgeConfiguration.Width, _firstNode, edgeVisualRepresentation);

            edgeVisualRepresentation.SetValue(Canvas.LeftProperty, EdgeCalculations.CalculateEdgePositionBaseLeft(_firstNode) + EdgeConfiguration.EdgeOffsetLeft + _firstNode.GetEllipseDimensions() / 2);
            edgeVisualRepresentation.SetValue(Canvas.TopProperty, EdgeCalculations.CalculateEdgePositionBaseTop(_firstNode) - _offsetTop);

            edgeVisualRepresentation.RenderTransformOrigin = new Point(originLeft, 0.5);
            RotateTransform rotateTransform = new RotateTransform(EdgeCalculations.CalculateAngle(_firstNode, _secondNode));

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
            edgeWidthAnimation.From = EdgeConfiguration.Height;
            edgeWidthAnimation.To = EdgeConfiguration.Width;
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
