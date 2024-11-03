using GraphEditor.EdgesAndNodes.Nodes;
using GraphEditor.GraphsManagerControls;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace GraphEditor
{
    internal class Node : IRenamable
    {
        public Button ellipse { get; private set; }

        private Canvas _canvas;

        private TextBlock _textBlock;

        private MainWindow _window;

        private Random random;

        public int _id { get; private set; }

        private const int EllipseDimensions = 40;

        private const int UILeftSize = 80;

        private const int UITopSize = 35;

        private bool isSelected = false;

        private double movementDiffLeft;

        private double movementDiffTop;

        private bool testWasMagicWondClicked = false;

        public event EventHandler buttonSelected;

        public event EventHandler OnNodeMoved;

        public event Action OnNodesAnimated;

        public Node(double CanvasLeft, double CanvasTop, Canvas parent, MainWindow window, int id)
        {            
            _id = id;
            _canvas = parent;
            _window = window;

            random = new Random();
            ellipse = new Button();
            NodeSettings.SetUpEllipse(ellipse, _window, EllipseDimensions);
            PositionEllipse(CanvasLeft, CanvasTop);
            _canvas.Children.Add(ellipse);

            SetUpEvents();
            NodeAddingAnimation();

            _textBlock = new TextBlock();
            NodeSettings.SetUpTextBlock(_textBlock, _id);
            MoveTextBlock();
            _canvas.Children.Add(_textBlock);
        }

        protected void SetUpEvents()
        {
            (ellipse.Content as Image).MouseDown += OnMouseDown;
            _window.MouseMove += OnMouseMove;
            _window.KillAllSelections += Unselect;
            _window.MagicWandOrder += OnMagicWondOrder;
            ellipse.LayoutUpdated += Ellipse_LayoutUpdated;
        }

        protected void PositionEllipse(double CanvasLeft, double CanvasTop)
        {
            SetPosLeft(NodeCalculations.CalculateEllipsePositionLeft(CanvasLeft, UILeftSize, EllipseDimensions));
            SetPosTop(NodeCalculations.CalculateEllipsePositionTop(CanvasTop, UITopSize, EllipseDimensions));
        }

        public int GetEllipseDimensions()
        {
            return EllipseDimensions;
        }

        private void MoveTextBlock()
        {
            TextBlockSetPosLeft(NodeCalculations.CalculateTextBlockCanvasLeft(this, _textBlock, EllipseDimensions));
            TextBlockSetPosTop(NodeCalculations.CalculateTextBlockCanvasTop(this, EllipseDimensions));
        }

        private void Unselect()
        {
            isSelected = false;
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (isSelected)
            {
                isSelected = false;
            }
            else
            {            
                buttonSelected?.Invoke(this, new EventArgs());
                if (!_window.shouldNodeBeMoved) return;

                isSelected = true;
                _window.shouldNodeBeAdded = false;
                Point currentMousePosition = e.GetPosition(sender as Window);

                movementDiffLeft = currentMousePosition.X - EllipseDimensions / 2 - UILeftSize - GetPosLeft();
                movementDiffTop = currentMousePosition.Y - EllipseDimensions / 2 - UITopSize - GetPosTop();
            }
        }

        public void EndMovementAnimations()
        {
            double canvasLeft = GetPosLeft();
            double canvasTop = GetPosTop();
            NodeAnimator.StopEllipseAnimationOnProperty(ellipse, Canvas.LeftProperty);
            NodeAnimator.StopEllipseAnimationOnProperty(ellipse, Canvas.TopProperty);
            SetPosLeft(canvasLeft);
            SetPosTop(canvasTop);
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (!isSelected) return;
            if (!_window.shouldNodeBeMoved) return;

            EndMovementAnimations();

            Point currentMousePosition = e.GetPosition(sender as Window);
            SetPosTop(currentMousePosition.Y - EllipseDimensions / 2 - UITopSize - movementDiffTop);
            SetPosLeft(currentMousePosition.X - EllipseDimensions / 2 - UILeftSize - movementDiffLeft);

            OnNodeMoved?.Invoke(this, e);
        }
        
        public double GetPosLeft()
        {
            return (double)ellipse.GetValue(Canvas.LeftProperty);
        }

        public double GetPosTop()
        {
            return (double)ellipse.GetValue(Canvas.TopProperty);
        }

        private void SetPosLeft(double CanvasLeft)
        {
            ellipse.SetValue(Canvas.LeftProperty, CanvasLeft);
        }

        private void SetPosTop(double CanvasTop)
        {
            ellipse.SetValue(Canvas.TopProperty, CanvasTop);
        }

        private void TextBlockSetPosLeft(double CanvasLeft)
        {
            _textBlock.SetValue(Canvas.LeftProperty, CanvasLeft);
        }

        private void TextBlockSetPosTop(double CanvasTop)
        {
            _textBlock.SetValue(Canvas.TopProperty, CanvasTop);
        }

        private void OnMagicWondOrder()
        {
            double leftTarget = random.Next(100) - 50;
            double topTarget = random.Next(100) - 50;

            double toLeft = GetPosLeft() + leftTarget;
            DoubleAnimation ellipseAnimationLeft = NodeAnimator.BuildEllipseMagicAnimationLeft(toLeft);

            double toTop = GetPosTop() + topTarget;
            DoubleAnimation ellipseAnimationTop = NodeAnimator.BuildEllipseMagicAnimationTop(toTop);

            ellipse.BeginAnimation(Canvas.LeftProperty, ellipseAnimationLeft);
            ellipse.BeginAnimation(Canvas.TopProperty, ellipseAnimationTop);

            testWasMagicWondClicked = true;
        }

        private void NodeAddingAnimation()
        {
            DoubleAnimation nodeWidthAnimation = NodeAnimator.BuildEllipseArrivalAnimationWidth(EllipseDimensions);
            ellipse.BeginAnimation(Button.WidthProperty, nodeWidthAnimation);

            DoubleAnimation nodeMovingLeftAnimation = NodeAnimator.BuildEllipseArrivalAnimationLeft(this, EllipseDimensions);
            ellipse.BeginAnimation(Canvas.LeftProperty, nodeMovingLeftAnimation);

            DoubleAnimation nodeMovingTopAnimation = NodeAnimator.BuildEllipseArrivalAnimationTop(this, EllipseDimensions);
            ellipse.BeginAnimation(Canvas.TopProperty, nodeMovingTopAnimation);

            DoubleAnimation nodeHeightAnimation = NodeAnimator.BuildEllipseArrivalAnimationHeight(EllipseDimensions);
            ellipse.BeginAnimation(Button.HeightProperty, nodeHeightAnimation);
        }

        private void Ellipse_LayoutUpdated(object sender, EventArgs e)
        {
            MoveTextBlock();
            OnNodesAnimated?.Invoke();
        }

        public override string ToString()
        {
            return "Node " + _id;
        }

        public void Rename(string newName)
        {
            _textBlock.Text = newName;
            MoveTextBlock();
        }

        public void Remove()
        {
            _canvas.Children.Remove(_textBlock);
            _window.MouseMove -= OnMouseMove;
            _window.KillAllSelections -= Unselect;
            _window.MagicWandOrder -= OnMagicWondOrder;
            _canvas.Children.Remove(ellipse);
            buttonSelected -= _window.OnNodeSelected;
        }

        public List<int> GetIdAsList()
        {
            return new List<int> { _id };
        }
    }
}
