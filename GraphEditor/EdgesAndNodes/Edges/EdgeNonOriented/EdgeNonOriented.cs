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
    internal class EdgeNonOriented : EdgeBase, IRenamable, IEdgeable
    {
        private EdgeNonOrientedConfiguration edgeConfiguration;

        public EdgeNonOriented(Node firstNode, Node secondNode, MainWindow window, Canvas mainCanvas) : base(firstNode, secondNode, window, mainCanvas)
        {
            edgeConfiguration = new EdgeNonOrientedConfiguration();

            SetUpEdgeVisualRepresentation();    
        }

        private void SetUpEdgeVisualRepresentation()
        {
            _mainCanvas.Children.Insert(0, edgeVisualRepresentation);
            edgeVisualRepresentation.Height = edgeConfiguration.Height;
            edgeVisualRepresentation.Width = edgeConfiguration.Width;
            edgeVisualRepresentation.RadiusX = edgeConfiguration.RadiusX;
            edgeVisualRepresentation.RadiusY = edgeConfiguration.RadiusY;
            edgeBrush = new SolidColorBrush(edgeConfiguration.StrokeColor);
            edgeVisualRepresentation.Stroke = edgeBrush;
            edgeVisualRepresentation.StrokeThickness = edgeConfiguration.StrokeThickness;
        }

        protected override void EdgeVisualRepresentationMouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ColorAnimation edgeHoverAnimation = EdgeAnimator.BuildEdgeHoverAnimationLeavePhase(edgeConfiguration);
            edgeBrush.BeginAnimation(SolidColorBrush.ColorProperty, edgeHoverAnimation);
        }

        protected override void EdgeVisualRepresentationMouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ColorAnimation edgeHoverAnimation = EdgeAnimator.BuildEdgeHoverAnimationEnterPhase();
            edgeBrush.BeginAnimation(SolidColorBrush.ColorProperty, edgeHoverAnimation);
        }

        protected override void EdgeVisualRepresentationRenderTransformUpdate(object sender, EventArgs e)
        {
            double originLeft = EdgeCalculations.CalculateRenderTransformOriginLeft(edgeConfiguration.Width, _firstNode, edgeVisualRepresentation);
            edgeVisualRepresentation.RenderTransformOrigin = new Point(originLeft, 0.5);
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
                if (Angle == angle) return;
                if (edgeConfiguration.Width == width) return;
            }

            edgeVisualRepresentation.BeginAnimation(Rectangle.WidthProperty, null);

            if (width >= edgeConfiguration.EdgeOffsetLeft) edgeVisualRepresentation.Width = width;
            else if (isInGraph)
            {
                edgeVisualRepresentation.Width = 0;
                return;
            }

            edgeConfiguration.Width = width;
            Angle = angle;
            
            _offsetTop = edgeConfiguration.Height / 2;

            double originLeft = EdgeCalculations.CalculateRenderTransformOriginLeft(edgeConfiguration.Width, _firstNode, edgeVisualRepresentation);

            edgeVisualRepresentation.SetValue(Canvas.LeftProperty, EdgeCalculations.CalculateEdgePositionBaseLeft(_firstNode) + edgeConfiguration.EdgeOffsetLeft + _firstNode.GetEllipseDimensions() / 2);
            edgeVisualRepresentation.SetValue(Canvas.TopProperty, EdgeCalculations.CalculateEdgePositionBaseTop(_firstNode) - _offsetTop);

            edgeVisualRepresentation.RenderTransformOrigin = new Point(originLeft, 0.5);
            RotateTransform rotateTransform = new RotateTransform(EdgeCalculations.CalculateAngle(_firstNode, _secondNode));

            edgeVisualRepresentation.RenderTransform = rotateTransform;

        }

        public override void EdgeDragged(double dragDeltaX, double dragDeltaY)
        {
            edgeVisualRepresentation.SetValue(Canvas.LeftProperty, (double)edgeVisualRepresentation.GetValue(Canvas.LeftProperty) + dragDeltaX);
            edgeVisualRepresentation.SetValue(Canvas.TopProperty, (double)edgeVisualRepresentation.GetValue(Canvas.TopProperty) + dragDeltaY);
        }

        protected override void AnimateEdgeCreation()
        {
            DoubleAnimation edgeWidthAnimation = EdgeAnimator.BuildEdgeCreationAnimation(edgeConfiguration);
            edgeWidthAnimation.Completed += EdgeWidthAnimationCompleted;
            edgeVisualRepresentation.BeginAnimation(Rectangle.WidthProperty, edgeWidthAnimation);
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
            _mainCanvas.Children.Remove(edgeVisualRepresentation);
        }
    }
}
