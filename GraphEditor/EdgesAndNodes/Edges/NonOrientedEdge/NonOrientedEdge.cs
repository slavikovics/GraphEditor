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
            MainCanvas.Children.Insert(0, EdgeVisualRepresentation);
            EdgeVisualRepresentation.Height = _edgeConfiguration.Height;
            EdgeVisualRepresentation.Width = _edgeConfiguration.Width;
            EdgeVisualRepresentation.RadiusX = _edgeConfiguration.RadiusX;
            EdgeVisualRepresentation.RadiusY = _edgeConfiguration.RadiusY;
            EdgeBrush = new SolidColorBrush(_edgeConfiguration.StrokeColor);
            EdgeVisualRepresentation.Fill = _edgeConfiguration.Fill;
            EdgeVisualRepresentation.Stroke = EdgeBrush;
            EdgeVisualRepresentation.StrokeThickness = _edgeConfiguration.StrokeThickness;
        }

        protected override void OnEdgeVisualRepresentationMouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ColorAnimation edgeHoverAnimation = EdgeAnimator.BuildEdgeHoverAnimationLeavePhase(_edgeConfiguration);
            EdgeBrush.BeginAnimation(SolidColorBrush.ColorProperty, edgeHoverAnimation);
        }

        protected override void OnEdgeVisualRepresentationMouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ColorAnimation edgeHoverAnimation = EdgeAnimator.BuildEdgeHoverAnimationEnterPhase();
            EdgeBrush.BeginAnimation(SolidColorBrush.ColorProperty, edgeHoverAnimation);
        }

        protected override void OnEdgeVisualRepresentationRenderTransformUpdate(object sender, EventArgs e)
        {
            double originLeft = EdgeCalculations.CalculateRenderTransformOriginLeft(_edgeConfiguration.Width, FirstNode, EdgeVisualRepresentation);
            EdgeVisualRepresentation.RenderTransformOrigin = new Point(originLeft, 0.5);
        }

        protected override void OnEdgeVisualRepresentationLoaded(object sender, RoutedEventArgs e)
        {
            EdgePositioning(false);
            MainWindow.UpdateLayout();      
            AnimateEdgeCreation();
        }

        public override void OnNodePositionChanged(object sender, EventArgs e)
        {
            EdgePositioning(true);
        }

        public override void EdgePositioning(bool isInGraph)
        {
            base.EdgePositioning(isInGraph);

            double angle = EdgeCalculations.CalculateAngle(FirstNode, SecondNode);
            double width = EdgeCalculations.CalculateFinalWidth(FirstNode, SecondNode, _edgeConfiguration.EdgeOffsetLeft);

            if (isInGraph)
            {
                if (Angle == angle) return;
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
            Angle = angle;
            
            OffsetTop = _edgeConfiguration.Height / 2;

            double originLeft = EdgeCalculations.CalculateRenderTransformOriginLeft(_edgeConfiguration.Width, FirstNode, EdgeVisualRepresentation);

            EdgeVisualRepresentation.SetValue(Canvas.LeftProperty, EdgeCalculations.CalculateEdgePositionBaseLeft(FirstNode) + _edgeConfiguration.EdgeOffsetLeft + FirstNode.GetEllipseDimensions() / 2);
            EdgeVisualRepresentation.SetValue(Canvas.TopProperty, EdgeCalculations.CalculateEdgePositionBaseTop(FirstNode) - OffsetTop);

            EdgeVisualRepresentation.RenderTransformOrigin = new Point(originLeft, 0.5);
            RotateTransform rotateTransform = new RotateTransform(EdgeCalculations.CalculateAngle(FirstNode, SecondNode));

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
            return  "Edge " + FirstNode.Id + " = " + SecondNode.Id;
        }

        public override void Rename(string newName)
        {

        }

        public override void Remove()
        {
            MainCanvas.Children.Remove(EdgeVisualRepresentation);
        }
    }
}
