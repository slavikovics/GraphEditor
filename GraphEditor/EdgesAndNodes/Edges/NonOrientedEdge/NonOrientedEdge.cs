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
    internal class NonOrientedEdge : Edge
    {
        private NonOrientedEdgeConfiguration edgeConfiguration;

        public NonOrientedEdge(Node firstNode, Node secondNode, MainWindow window, Canvas mainCanvas) : base(firstNode, secondNode, window, mainCanvas)
        {
            edgeConfiguration = new NonOrientedEdgeConfiguration();

            SetUpEdgeVisualRepresentation();    
        }

        private void SetUpEdgeVisualRepresentation()
        {
            _mainCanvas.Children.Insert(0, _edgeVisualRepresentation);
            _edgeVisualRepresentation.Height = edgeConfiguration.Height;
            _edgeVisualRepresentation.Width = edgeConfiguration.Width;
            _edgeVisualRepresentation.RadiusX = edgeConfiguration.RadiusX;
            _edgeVisualRepresentation.RadiusY = edgeConfiguration.RadiusY;
            _edgeBrush = new SolidColorBrush(edgeConfiguration.StrokeColor);
            _edgeVisualRepresentation.Stroke = _edgeBrush;
            _edgeVisualRepresentation.StrokeThickness = edgeConfiguration.StrokeThickness;
        }

        protected override void EdgeVisualRepresentationMouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ColorAnimation edgeHoverAnimation = EdgeAnimator.BuildEdgeHoverAnimationLeavePhase(edgeConfiguration);
            _edgeBrush.BeginAnimation(SolidColorBrush.ColorProperty, edgeHoverAnimation);
        }

        protected override void EdgeVisualRepresentationMouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ColorAnimation edgeHoverAnimation = EdgeAnimator.BuildEdgeHoverAnimationEnterPhase();
            _edgeBrush.BeginAnimation(SolidColorBrush.ColorProperty, edgeHoverAnimation);
        }

        protected override void EdgeVisualRepresentationRenderTransformUpdate(object sender, EventArgs e)
        {
            double originLeft = EdgeCalculations.CalculateRenderTransformOriginLeft(edgeConfiguration.Width, _firstNode, _edgeVisualRepresentation);
            _edgeVisualRepresentation.RenderTransformOrigin = new Point(originLeft, 0.5);
        }

        protected override void EdgeVisualRepresentationLoaded(object sender, RoutedEventArgs e)
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
            double width = EdgeCalculations.CalculateFinalWidth(_firstNode, _secondNode, edgeConfiguration.EdgeOffsetLeft);

            if (isInGraph)
            {
                if (_angle == angle) return;
                if (edgeConfiguration.Width == width) return;
            }

            _edgeVisualRepresentation.BeginAnimation(Rectangle.WidthProperty, null);

            if (width >= edgeConfiguration.EdgeOffsetLeft) _edgeVisualRepresentation.Width = width;
            else if (isInGraph)
            {
                _edgeVisualRepresentation.Width = 0;
                return;
            }

            edgeConfiguration.Width = width;
            _angle = angle;
            
            _offsetTop = edgeConfiguration.Height / 2;

            double originLeft = EdgeCalculations.CalculateRenderTransformOriginLeft(edgeConfiguration.Width, _firstNode, _edgeVisualRepresentation);

            _edgeVisualRepresentation.SetValue(Canvas.LeftProperty, EdgeCalculations.CalculateEdgePositionBaseLeft(_firstNode) + edgeConfiguration.EdgeOffsetLeft + _firstNode.GetEllipseDimensions() / 2);
            _edgeVisualRepresentation.SetValue(Canvas.TopProperty, EdgeCalculations.CalculateEdgePositionBaseTop(_firstNode) - _offsetTop);

            _edgeVisualRepresentation.RenderTransformOrigin = new Point(originLeft, 0.5);
            RotateTransform rotateTransform = new RotateTransform(EdgeCalculations.CalculateAngle(_firstNode, _secondNode));

            _edgeVisualRepresentation.RenderTransform = rotateTransform;

        }

        public override void EdgeDragged(double dragDeltaX, double dragDeltaY)
        {
            _edgeVisualRepresentation.SetValue(Canvas.LeftProperty, (double)_edgeVisualRepresentation.GetValue(Canvas.LeftProperty) + dragDeltaX);
            _edgeVisualRepresentation.SetValue(Canvas.TopProperty, (double)_edgeVisualRepresentation.GetValue(Canvas.TopProperty) + dragDeltaY);
        }

        protected override void AnimateEdgeCreation()
        {
            DoubleAnimation edgeWidthAnimation = EdgeAnimator.BuildEdgeCreationAnimation(edgeConfiguration);
            edgeWidthAnimation.Completed += EdgeWidthAnimationCompleted;
            _edgeVisualRepresentation.BeginAnimation(Rectangle.WidthProperty, edgeWidthAnimation);
        }

        public override string ToString()
        {
            return  "Edge " + _firstNode._id + " = " + _secondNode._id;
        }

        public override void Rename(string newName)
        {

        }

        public override void Remove()
        {
            _mainCanvas.Children.Remove(_edgeVisualRepresentation);
        }
    }
}
