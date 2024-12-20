﻿using GraphEditor.EdgesAndNodes.Edges;
using GraphEditor.EdgesAndNodes.Edges.EdgesOriented;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace GraphEditor
{
    internal class OrientedEdge : Edge
    {
        public const int EdgeOffsetLeft = 5;

        public Image Arrow;
        
        private Image _normalArrow;

        public bool _isPencil;

        private OrientedEdgeConfiguration _edgeConfiguration;

        private const int ArrowOffset = 5;

        public OrientedEdge(Node firstNode, Node secondNode, MainWindow window, Canvas mainCanvas, Image arrow, bool isPencil) : base (firstNode, secondNode, window, mainCanvas)
        {
            _isPencil = isPencil;
            _edgeConfiguration = new OrientedEdgeConfiguration();
            _normalArrow = arrow;
            SetUpEdgeVisualRepresentation();
            SetUpArrowVisualRepresentation(arrow.Source);
        }

        private void SetUpEdgeVisualRepresentation()
        {
            if (_isPencil)
            {
                EdgeVisualRepresentation.RadiusX = _edgeConfiguration.RadiusX;
                EdgeVisualRepresentation.RadiusY = _edgeConfiguration.RadiusY;
                _edgeConfiguration.Height = _edgeConfiguration.PencileHeight;
                EdgeVisualRepresentation.Fill = _edgeConfiguration.Fill;
            }
            else
            {
                EdgeVisualRepresentation.Fill = EdgeBrush;
            }
            MainCanvas.Children.Insert(0, EdgeVisualRepresentation);
            EdgeVisualRepresentation.Height = _edgeConfiguration.Height;
            EdgeVisualRepresentation.Width = _edgeConfiguration.Width;
            EdgeVisualRepresentation.RadiusX = _edgeConfiguration.RadiusX;
            EdgeVisualRepresentation.RadiusY = _edgeConfiguration.RadiusY;
            EdgeBrush = new SolidColorBrush(_edgeConfiguration.StrokeColor);
            EdgeVisualRepresentation.Stroke = EdgeBrush;
            EdgeVisualRepresentation.StrokeThickness = _edgeConfiguration.StrokeThickness;
        }

        private void SetUpArrowVisualRepresentation(ImageSource arrowSource)
        {
            ArrowConfiguration arrowConfiguration = new ArrowConfiguration();
            Arrow = new Image();
            Arrow.Source = arrowSource;
            Arrow.Width = arrowConfiguration.Width;
            Arrow.Height = arrowConfiguration.Height;
            MainCanvas.Children.Insert(0, Arrow);
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
            Arrow.Source = _normalArrow.Source;
        }

        protected override void OnEdgeVisualRepresentationRenderTransformUpdate(object sender, EventArgs e)
        {
            double originLeft = EdgeCalculations.CalculateRenderTransformOriginLeftWithArrow(_edgeConfiguration.Width, FirstNode, EdgeVisualRepresentation, ArrowOffset);       
            EdgeVisualRepresentation.RenderTransformOrigin = new Point(originLeft, 0.5);

            originLeft = EdgeCalculations.CalculateArrowRenderTransformOriginLeft(Arrow, FirstNode);
            Arrow.RenderTransformOrigin = new Point(originLeft, 0.5);
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
            double width = EdgeCalculations.CalculateFinalWidthWithArrow(FirstNode, SecondNode, EdgeOffsetLeft, ArrowOffset);

            if (isInGraph)
            {
                if (Angle == angle) return;
                if (_edgeConfiguration.Width == width) return;
            }

            EdgeVisualRepresentation.BeginAnimation(Rectangle.WidthProperty, null);

            if (width >= EdgeOffsetLeft)
            {
                EdgeVisualRepresentation.Width = width;
                Arrow.Visibility = Visibility.Visible;
            }
            else if (isInGraph)
            {
                EdgeVisualRepresentation.Width = 0;
                Arrow.Visibility = Visibility.Hidden;
                return;
            }

            _edgeConfiguration.Width = width;
            Angle = angle;
            
            OffsetTop = _edgeConfiguration.Height / 2;

            double originLeft = EdgeCalculations.CalculateRenderTransformOriginLeftWithArrow(_edgeConfiguration.Width, FirstNode, EdgeVisualRepresentation, ArrowOffset);

            EdgeVisualRepresentation.SetValue(Canvas.LeftProperty, EdgeCalculations.CalculateEdgePositionBaseLeftWithArrow(FirstNode, ArrowOffset) + EdgeOffsetLeft + FirstNode.GetEllipseDimensions() / 2 );
            EdgeVisualRepresentation.SetValue(Canvas.TopProperty, EdgeCalculations.CalculateEdgePositionBaseTop(FirstNode) - OffsetTop);

            Arrow.SetValue(Canvas.LeftProperty, EdgeCalculations.CalculateEdgePositionBaseLeftWithArrow(FirstNode, ArrowOffset) + EdgeOffsetLeft + FirstNode.GetEllipseDimensions() / 2 - ArrowOffset); //
            Arrow.SetValue(Canvas.TopProperty, EdgeCalculations.CalculateEdgePositionBaseTop(FirstNode) - Arrow.Height / 2);

            EdgeVisualRepresentation.RenderTransformOrigin = new Point(originLeft, 0.5);

            originLeft = EdgeCalculations.CalculateArrowRenderTransformOriginLeft(Arrow, FirstNode);
            Arrow.RenderTransformOrigin = new Point(originLeft, 0.5); 
            RotateTransform rotateTransform = new RotateTransform(EdgeCalculations.CalculateAngle(FirstNode, SecondNode));

            EdgeVisualRepresentation.RenderTransform = rotateTransform;

            RotateTransform rotateTransformArrow = new RotateTransform(EdgeCalculations.CalculateAngle(FirstNode, SecondNode));
            Arrow.RenderTransform = rotateTransformArrow;
        }

        public override void EdgeDragged(double dragDeltaX, double dragDeltaY)
        {
            EdgeVisualRepresentation.SetValue(Canvas.LeftProperty, (double)EdgeVisualRepresentation.GetValue(Canvas.LeftProperty) + dragDeltaX);
            EdgeVisualRepresentation.SetValue(Canvas.TopProperty, (double)EdgeVisualRepresentation.GetValue(Canvas.TopProperty) + dragDeltaY);

            Arrow.SetValue(Canvas.LeftProperty, (double)Arrow.GetValue(Canvas.LeftProperty) + dragDeltaX);
            Arrow.SetValue(Canvas.TopProperty, (double)Arrow.GetValue(Canvas.TopProperty) + dragDeltaY);
        }

        protected override void AnimateEdgeCreation()
        {
            DoubleAnimation edgeWidthAnimation = EdgeAnimator.BuildEdgeCreationAnimation(_edgeConfiguration);
            edgeWidthAnimation.Completed += EdgeWidthAnimationCompleted;
            EdgeVisualRepresentation.BeginAnimation(Rectangle.WidthProperty, edgeWidthAnimation);
        }

        // public override string ToString()
        // {
        //     if (_isPencil)
        //     {
        //         return "Edge " + SecondNode.Id + " => " + FirstNode.Id;
        //     }
        //     return  "Edge " + SecondNode.Id + " -> " + FirstNode.Id;
        // }
        
        public override string ToString()
        {
            if (_isPencil)
            {
                return "Edge " + SecondNode.Name + " => " + FirstNode.Name;
            }
            return  "Edge " + SecondNode.Name + " -> " + FirstNode.Name;
        }

        public override void Rename(string newName)
        {

        }

        public override void Remove()
        {
            MainCanvas.Children.Remove(EdgeVisualRepresentation);
            MainCanvas.Children.Remove(Arrow);
        }
    }
}
