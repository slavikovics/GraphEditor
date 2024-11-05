using GraphEditor.EdgesAndNodes.Edges;
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

        private Image _arrow;

        private bool _isPencil;

        private OrientedEdgeConfiguration _edgeConfiguration;

        private const int ArrowOffset = 5;

        public OrientedEdge(Node firstNode, Node secondNode, MainWindow window, Canvas mainCanvas, Image arrow, bool isPencil) : base (firstNode, secondNode, window, mainCanvas)
        {
            _isPencil = isPencil;
            _edgeConfiguration = new OrientedEdgeConfiguration();
            SetUpEdgeVisualRepresentation();
            SetUpArrowVisualRepresentation(arrow.Source);
        }

        private void SetUpEdgeVisualRepresentation()
        {
            if (_isPencil)
            {
                _edgeVisualRepresentation.RadiusX = _edgeConfiguration.RadiusX;
                _edgeVisualRepresentation.RadiusY = _edgeConfiguration.RadiusY;
                _edgeConfiguration.Height = _edgeConfiguration.PencileHeight;
            }
            else
            {
                _edgeVisualRepresentation.Fill = _edgeBrush;
            }
            _mainCanvas.Children.Insert(0, _edgeVisualRepresentation);
            _edgeVisualRepresentation.Height = _edgeConfiguration.Height;
            _edgeVisualRepresentation.Width = _edgeConfiguration.Width;
            _edgeVisualRepresentation.RadiusX = _edgeConfiguration.RadiusX;
            _edgeVisualRepresentation.RadiusY = _edgeConfiguration.RadiusY;
            _edgeBrush = new SolidColorBrush(_edgeConfiguration.StrokeColor);
            _edgeVisualRepresentation.Stroke = _edgeBrush;
            _edgeVisualRepresentation.StrokeThickness = _edgeConfiguration.StrokeThickness;
        }

        private void SetUpArrowVisualRepresentation(ImageSource arrowSource)
        {
            ArrowConfiguration arrowConfiguration = new ArrowConfiguration();
            _arrow = new Image();
            _arrow.Source = arrowSource;
            _arrow.Width = arrowConfiguration.Width;
            _arrow.Height = arrowConfiguration.Height;
            _mainCanvas.Children.Insert(0, _arrow);
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
            double originLeft = EdgeCalculations.CalculateRenderTransformOriginLeftWithArrow(_edgeConfiguration.Width, _firstNode, _edgeVisualRepresentation, ArrowOffset);       
            _edgeVisualRepresentation.RenderTransformOrigin = new Point(originLeft, 0.5);

            originLeft = EdgeCalculations.CalculateArrowRenderTransformOriginLeft(_arrow, _firstNode);
            _arrow.RenderTransformOrigin = new Point(originLeft, 0.5);
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
            double width = EdgeCalculations.CalculateFinalWidthWithArrow(_firstNode, _secondNode, EdgeOffsetLeft, ArrowOffset);

            if (isInGraph)
            {
                if (_angle == angle) return;
                if (_edgeConfiguration.Width == width) return;
            }

            _edgeVisualRepresentation.BeginAnimation(Rectangle.WidthProperty, null);

            if (width >= EdgeOffsetLeft)
            {
                _edgeVisualRepresentation.Width = width;
                _arrow.Visibility = Visibility.Visible;
            }
            else if (isInGraph)
            {
                _edgeVisualRepresentation.Width = 0;
                _arrow.Visibility = Visibility.Hidden;
                return;
            }

            _edgeConfiguration.Width = width;
            _angle = angle;
            
            _offsetTop = _edgeConfiguration.Height / 2;

            double originLeft = EdgeCalculations.CalculateRenderTransformOriginLeftWithArrow(_edgeConfiguration.Width, _firstNode, _edgeVisualRepresentation, ArrowOffset);

            _edgeVisualRepresentation.SetValue(Canvas.LeftProperty, EdgeCalculations.CalculateEdgePositionBaseLeftWithArrow(_firstNode, ArrowOffset) + EdgeOffsetLeft + _firstNode.GetEllipseDimensions() / 2 );
            _edgeVisualRepresentation.SetValue(Canvas.TopProperty, EdgeCalculations.CalculateEdgePositionBaseTop(_firstNode) - _offsetTop);

            _arrow.SetValue(Canvas.LeftProperty, EdgeCalculations.CalculateEdgePositionBaseLeftWithArrow(_firstNode, ArrowOffset) + EdgeOffsetLeft + _firstNode.GetEllipseDimensions() / 2 - ArrowOffset); //
            _arrow.SetValue(Canvas.TopProperty, EdgeCalculations.CalculateEdgePositionBaseTop(_firstNode) - _arrow.Height / 2);

            _edgeVisualRepresentation.RenderTransformOrigin = new Point(originLeft, 0.5);

            originLeft = EdgeCalculations.CalculateArrowRenderTransformOriginLeft(_arrow, _firstNode);
            _arrow.RenderTransformOrigin = new Point(originLeft, 0.5); 
            RotateTransform rotateTransform = new RotateTransform(EdgeCalculations.CalculateAngle(_firstNode, _secondNode));

            _edgeVisualRepresentation.RenderTransform = rotateTransform;

            RotateTransform rotateTransformArrow = new RotateTransform(EdgeCalculations.CalculateAngle(_firstNode, _secondNode));
            _arrow.RenderTransform = rotateTransformArrow;
        }

        public override void EdgeDragged(double dragDeltaX, double dragDeltaY)
        {
            _edgeVisualRepresentation.SetValue(Canvas.LeftProperty, (double)_edgeVisualRepresentation.GetValue(Canvas.LeftProperty) + dragDeltaX);
            _edgeVisualRepresentation.SetValue(Canvas.TopProperty, (double)_edgeVisualRepresentation.GetValue(Canvas.TopProperty) + dragDeltaY);

            _arrow.SetValue(Canvas.LeftProperty, (double)_arrow.GetValue(Canvas.LeftProperty) + dragDeltaX);
            _arrow.SetValue(Canvas.TopProperty, (double)_arrow.GetValue(Canvas.TopProperty) + dragDeltaY);
        }

        protected override void AnimateEdgeCreation()
        {
            DoubleAnimation edgeWidthAnimation = EdgeAnimator.BuildEdgeCreationAnimation(_edgeConfiguration);
            edgeWidthAnimation.Completed += EdgeWidthAnimationCompleted;
            _edgeVisualRepresentation.BeginAnimation(Rectangle.WidthProperty, edgeWidthAnimation);
        }

        public override string ToString()
        {
            if (_isPencil)
            {
                return "Edge " + _secondNode.Id + " => " + _firstNode.Id;
            }
            return  "Edge " + _secondNode.Id + " -> " + _firstNode.Id;
        }

        public override void Rename(string newName)
        {

        }

        public override void Remove()
        {
            _mainCanvas.Children.Remove(_edgeVisualRepresentation);
            _mainCanvas.Children.Remove(_arrow);
        }
    }
}
