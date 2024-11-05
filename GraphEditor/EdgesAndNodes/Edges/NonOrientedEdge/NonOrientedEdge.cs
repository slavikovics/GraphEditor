using GraphEditor.EdgesAndNodes.Edges;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace GraphEditor
{
    internal class NonOrientedEdge : Edge
    {
        private NonOrientedEdgeConfiguration _edgeConfiguration;

        public NonOrientedEdge(Node firstNode, Node secondNode, MainWindow window, Canvas mainCanvas) : base(firstNode, secondNode, window, mainCanvas)
        {
            _edgeConfiguration = new NonOrientedEdgeConfiguration();
            SetUpEdgeVisualRepresentation();    
        }

        private void SetUpEdgeVisualRepresentation()
        {
            _mainCanvas.Children.Insert(0, EdgeVisualRepresentation);
            EdgeVisualRepresentation.Height = _edgeConfiguration.Height;
            EdgeVisualRepresentation.Width = _edgeConfiguration.Width;
            EdgeVisualRepresentation.RadiusX = _edgeConfiguration.RadiusX;
            EdgeVisualRepresentation.RadiusY = _edgeConfiguration.RadiusY;
            _edgeBrush = new SolidColorBrush(_edgeConfiguration.StrokeColor);
            EdgeVisualRepresentation.Stroke = _edgeBrush;
            EdgeVisualRepresentation.StrokeThickness = _edgeConfiguration.StrokeThickness;
        }

        protected override void OnEdgeVisualRepresentationMouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ColorAnimation edgeHoverAnimation = EdgeAnimator.BuildEdgeHoverAnimationLeavePhase(_edgeConfiguration);
            _edgeBrush.BeginAnimation(SolidColorBrush.ColorProperty, edgeHoverAnimation);
        }

        protected override void OnEdgeVisualRepresentationMouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ColorAnimation edgeHoverAnimation = EdgeAnimator.BuildEdgeHoverAnimationEnterPhase();
            _edgeBrush.BeginAnimation(SolidColorBrush.ColorProperty, edgeHoverAnimation);
        }

        protected override void OnEdgeVisualRepresentationRenderTransformUpdate(object sender, EventArgs e)
        {
            double originLeft = EdgeCalculations.CalculateRenderTransformOriginLeft(_edgeConfiguration.Width, _firstNode, EdgeVisualRepresentation);
            EdgeVisualRepresentation.RenderTransformOrigin = new Point(originLeft, 0.5);
        }

        protected override void OnEdgeVisualRepresentationLoaded(object sender, RoutedEventArgs e)
        {
            EdgePositioning(false);
            _mainWindow.UpdateLayout();      
            AnimateEdgeCreation();
        }

        public override void OnNodePositionChanged(object sender, EventArgs e)
        {
            EdgePositioning(true);
        }

        public override void EdgePositioning(bool isInGraph)
        {
            double angle = EdgeCalculations.CalculateAngle(_firstNode, _secondNode);
            double width = EdgeCalculations.CalculateFinalWidth(_firstNode, _secondNode, _edgeConfiguration.EdgeOffsetLeft);

            if (isInGraph)
            {
                if (_angle == angle) return;
                if (_edgeConfiguration.Width == width) return;
            }

            EdgeVisualRepresentation.BeginAnimation(Rectangle.WidthProperty, null);

            if (width >= _edgeConfiguration.EdgeOffsetLeft) EdgeVisualRepresentation.Width = width;
            else if (isInGraph)
            {
                EdgeVisualRepresentation.Width = 0;
                return;
            }

            _edgeConfiguration.Width = width;
            _angle = angle;
            
            _offsetTop = _edgeConfiguration.Height / 2;

            double originLeft = EdgeCalculations.CalculateRenderTransformOriginLeft(_edgeConfiguration.Width, _firstNode, EdgeVisualRepresentation);

            EdgeVisualRepresentation.SetValue(Canvas.LeftProperty, EdgeCalculations.CalculateEdgePositionBaseLeft(_firstNode) + _edgeConfiguration.EdgeOffsetLeft + _firstNode.GetEllipseDimensions() / 2);
            EdgeVisualRepresentation.SetValue(Canvas.TopProperty, EdgeCalculations.CalculateEdgePositionBaseTop(_firstNode) - _offsetTop);

            EdgeVisualRepresentation.RenderTransformOrigin = new Point(originLeft, 0.5);
            RotateTransform rotateTransform = new RotateTransform(EdgeCalculations.CalculateAngle(_firstNode, _secondNode));

            EdgeVisualRepresentation.RenderTransform = rotateTransform;

        }

        public override void EdgeDragged(double dragDeltaX, double dragDeltaY)
        {
            EdgeVisualRepresentation.SetValue(Canvas.LeftProperty, (double)EdgeVisualRepresentation.GetValue(Canvas.LeftProperty) + dragDeltaX);
            EdgeVisualRepresentation.SetValue(Canvas.TopProperty, (double)EdgeVisualRepresentation.GetValue(Canvas.TopProperty) + dragDeltaY);
        }

        protected override void AnimateEdgeCreation()
        {
            DoubleAnimation edgeWidthAnimation = EdgeAnimator.BuildEdgeCreationAnimation(_edgeConfiguration);
            edgeWidthAnimation.Completed += EdgeWidthAnimationCompleted;
            EdgeVisualRepresentation.BeginAnimation(Rectangle.WidthProperty, edgeWidthAnimation);
        }

        public override string ToString()
        {
            return  "Edge " + _firstNode.Id + " = " + _secondNode.Id;
        }

        public override void Rename(string newName)
        {

        }

        public override void Remove()
        {
            _mainCanvas.Children.Remove(EdgeVisualRepresentation);
        }
    }
}
