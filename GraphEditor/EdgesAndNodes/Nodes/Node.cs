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
            NodeSettings.SetUpEllipse(ellipse, _window);
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
            SetPosLeft(NodeCalculations.CalculateEllipsePositionLeft(CanvasLeft, NodeConfiguration.UserInterfaceLeftSize, NodeConfiguration.EllipseDimensions));
            SetPosTop(NodeCalculations.CalculateEllipsePositionTop(CanvasTop, NodeConfiguration.UserInterfaceTopSize, NodeConfiguration.EllipseDimensions));
        }

        public int GetEllipseDimensions()
        {
            return NodeConfiguration.EllipseDimensions;
        }

        private void MoveTextBlock()
        {
            TextBlockSetPosLeft(NodeCalculations.CalculateTextBlockCanvasLeft(this, _textBlock, NodeConfiguration.EllipseDimensions));
            TextBlockSetPosTop(NodeCalculations.CalculateTextBlockCanvasTop(this, NodeConfiguration.EllipseDimensions));
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

                movementDiffLeft = currentMousePosition.X - NodeConfiguration.EllipseDimensions / 2 - NodeConfiguration.UserInterfaceLeftSize - GetPosLeft();
                movementDiffTop = currentMousePosition.Y - NodeConfiguration.EllipseDimensions / 2 - NodeConfiguration.UserInterfaceTopSize - GetPosTop();
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
            SetPosTop(currentMousePosition.Y - NodeConfiguration.EllipseDimensions / 2 - NodeConfiguration.UserInterfaceTopSize - movementDiffTop);
            SetPosLeft(currentMousePosition.X - NodeConfiguration.EllipseDimensions / 2 - NodeConfiguration.UserInterfaceLeftSize - movementDiffLeft);

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
            DoubleAnimation nodeWidthAnimation = NodeAnimator.BuildEllipseArrivalAnimationWidth(NodeConfiguration.EllipseDimensions);
            ellipse.BeginAnimation(Button.WidthProperty, nodeWidthAnimation);

            DoubleAnimation nodeMovingLeftAnimation = NodeAnimator.BuildEllipseArrivalAnimationLeft(this, NodeConfiguration.EllipseDimensions);
            ellipse.BeginAnimation(Canvas.LeftProperty, nodeMovingLeftAnimation);

            DoubleAnimation nodeMovingTopAnimation = NodeAnimator.BuildEllipseArrivalAnimationTop(this, NodeConfiguration.EllipseDimensions);
            ellipse.BeginAnimation(Canvas.TopProperty, nodeMovingTopAnimation);

            DoubleAnimation nodeHeightAnimation = NodeAnimator.BuildEllipseArrivalAnimationHeight(NodeConfiguration.EllipseDimensions);
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
