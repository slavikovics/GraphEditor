using GraphEditor.EdgesAndNodes;
using GraphEditor.GraphsManagerControls;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace GraphEditor
{
    internal class EdgeOriented : IRenamable, IEdgeable
    {
        public const int Margin = 4;
        public int Height = 4;
        //public const int Height = 4;
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
        private Image _arrow;
        private bool _isPencile;

        private const int ArrowOffset = 5;

        public EdgeOriented(Node firstNode, Node secondNode, MainWindow window, Canvas mainCanvas, Image arrow, bool isPencile)
        {
            //isAnimated = true;
            _firstNode = firstNode;
            _secondNode = secondNode;
            _mainWindow = window;
            _mainCanvas = mainCanvas;
            _isPencile = isPencile;

            edgeVisualRepresentation = new Rectangle();
            _mainCanvas.Children.Insert(0, edgeVisualRepresentation);

            edgeVisualRepresentation.Width = Width;
            edgeBrush = new SolidColorBrush(StrokeColor);

            if (isPencile)
            {
                edgeVisualRepresentation.RadiusX = RadiusX;
                edgeVisualRepresentation.RadiusY = RadiusY;
                Height = 12;
            }
            else
            {
                edgeVisualRepresentation.Fill = edgeBrush;
            }

            edgeVisualRepresentation.Height = Height;
            edgeVisualRepresentation.Stroke = edgeBrush;
            
            edgeVisualRepresentation.StrokeThickness = StrokeThickness;

            _arrow = new Image();
            _arrow.Source = arrow.Source;
            _arrow.Width = 16;
            _arrow.Height = 16;
            _mainCanvas.Children.Insert(0, _arrow);
            //_arrow = arrow;

            // TODO call width animation

            _firstNode.OnNodeMoved += OnNodePositionChanged;
            _secondNode.OnNodeMoved += OnNodePositionChanged;
            edgeVisualRepresentation.LayoutUpdated += EdgeVisualRepresentationRenderTransformUpdate;
            edgeVisualRepresentation.MouseEnter += EdgeVisualRepresentationMouseEnter;
            edgeVisualRepresentation.MouseLeave += EdgeVisualRepresentationMouseLeave;
            //EdgePositioning(true);

            edgeVisualRepresentation.Loaded += EdgeVisualRepresentationLoaded;
            
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

            originLeft = CalculateArrowRenderTransformOriginLeft();
            _arrow.RenderTransformOrigin = new Point(originLeft, 0.5);
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
            double width = CalculateFinalWidth(_firstNode, _secondNode);

            if (isInGraph)
            {
                if (Angle == angle) return;
                if (Width == width) return;
            }

            edgeVisualRepresentation.BeginAnimation(Rectangle.WidthProperty, null);

            if (width >= EdgeOffsetLeft)
            {
                edgeVisualRepresentation.Width = width;
                _arrow.Visibility = Visibility.Visible;
            }
            else if (isInGraph)
            {
                edgeVisualRepresentation.Width = 0;
                _arrow.Visibility = Visibility.Hidden;
                return;
            }

            Width = width;
            Angle = angle;
            
            _offsetTop = Height / 2;

            double originLeft = CalculateRenderTransformOriginLeft();

            edgeVisualRepresentation.SetValue(Canvas.LeftProperty, GetEdgePositionBaseLeft(_firstNode) + EdgeOffsetLeft + _firstNode.GetEllipseDimensions() / 2 );
            edgeVisualRepresentation.SetValue(Canvas.TopProperty, GetEdgePositionBaseTop(_firstNode) - _offsetTop);

            _arrow.SetValue(Canvas.LeftProperty, GetEdgePositionBaseLeft(_firstNode) + EdgeOffsetLeft + _firstNode.GetEllipseDimensions() / 2 - ArrowOffset);
            _arrow.SetValue(Canvas.TopProperty, GetEdgePositionBaseTop(_firstNode) - _arrow.Height / 2);

            Console.WriteLine(CalculateAngle(_firstNode, _secondNode));
            edgeVisualRepresentation.RenderTransformOrigin = new Point(originLeft, 0.5);

            originLeft = CalculateArrowRenderTransformOriginLeft();
            _arrow.RenderTransformOrigin = new Point(originLeft, 0.5);
            RotateTransform rotateTransform = new RotateTransform(CalculateAngle(_firstNode, _secondNode));

            edgeVisualRepresentation.RenderTransform = rotateTransform;

            RotateTransform rotateTransformArrow = new RotateTransform(CalculateAngle(_firstNode, _secondNode));
            _arrow.RenderTransform = rotateTransformArrow;
        }

        public void EdgeDragged(double dragDeltaX, double dragDeltaY)
        {
            edgeVisualRepresentation.SetValue(Canvas.LeftProperty, (double)edgeVisualRepresentation.GetValue(Canvas.LeftProperty) + dragDeltaX);
            edgeVisualRepresentation.SetValue(Canvas.TopProperty, (double)edgeVisualRepresentation.GetValue(Canvas.TopProperty) + dragDeltaY);

            _arrow.SetValue(Canvas.LeftProperty, (double)_arrow.GetValue(Canvas.LeftProperty) + dragDeltaX);
            _arrow.SetValue(Canvas.TopProperty, (double)_arrow.GetValue(Canvas.TopProperty) + dragDeltaY);
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

        private double GetEdgePositionBaseLeft(Node node)
        {
            double basePointLeft = (double)node.ellipse.GetValue(Canvas.LeftProperty) + node.GetEllipseDimensions() / 2 + ArrowOffset;
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
            return CalculateLengthBetweenNodes(node1, node2) - node1.GetEllipseDimensions() - 2 * EdgeOffsetLeft - ArrowOffset;
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

        private double CalculateRenderTransformOriginLeft()
        {
            if (Width == 0) return 0;

            double originLeft = -1 * (_firstNode.GetEllipseDimensions() / 2 + 5 + ArrowOffset) / edgeVisualRepresentation.Width;

            if (originLeft > 100 || originLeft < -100) return 0;

            return originLeft;
        }

        private double CalculateArrowRenderTransformOriginLeft()
        {
            if (_arrow.Width == 0) return 0;

            double originLeft = -1 * (_firstNode.GetEllipseDimensions() / 2 + 5) / _arrow.Width;

            if (originLeft > 100 || originLeft < -100) return 0;

            return originLeft;
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
            if (_isPencile)
            {
                return "Edge " + _secondNode._id + " => " + _firstNode._id;
            }
            return  "Edge " + _secondNode._id + " -> " + _firstNode._id;
        }

        public void Rename(string newName)
        {

        }
    }
}
