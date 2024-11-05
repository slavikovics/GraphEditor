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
        public Button Ellipse { get; private set; }

        private Canvas _canvas;

        protected TextBlock _textBlock;

        private MainWindow _window;

        private Random _random;

        public int Id { get; private set; }

        private bool _isSelected = false;

        private double _movementDiffLeft;

        private double _movementDiffTop;

        private bool _testWasMagicWandClicked = false;

        public event EventHandler OnButtonSelected;

        public event EventHandler OnNodeMoved;

        public event Action OnNodesAnimated;

        public Node(double canvasLeft, double canvasTop, Canvas parent, MainWindow window, int id)
        {            
            Id = id;
            _canvas = parent;
            _window = window;

            _random = new Random();
            Ellipse = new Button();
            NodeSettings.SetUpEllipse(Ellipse, _window);
            PositionEllipse(canvasLeft, canvasTop);
            _canvas.Children.Add(Ellipse);

            SetUpEvents();
            NodeAddingAnimation();

            _textBlock = new TextBlock();
            NodeSettings.SetUpTextBlock(_textBlock, Id);
            MoveTextBlock();
            _canvas.Children.Add(_textBlock);
        }

        private void SetUpEvents()
        {
            (Ellipse.Content as Image).MouseDown += OnMouseDown;
            _window.MouseMove += OnMouseMove;
            _window.OnKillAllSelections += OnUnselect;
            _window.OnMagicWandOrder += OnMagicWondOrder;
            Ellipse.LayoutUpdated += OnEllipseLayoutUpdated;
        }

        protected void PositionEllipse(double canvasLeft, double canvasTop)
        {
            SetPosLeft(NodeCalculations.CalculateEllipsePositionLeft(canvasLeft, NodeConfiguration.UserInterfaceLeftSize, NodeConfiguration.EllipseDimensions));
            SetPosTop(NodeCalculations.CalculateEllipsePositionTop(canvasTop, NodeConfiguration.UserInterfaceTopSize, NodeConfiguration.EllipseDimensions));
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

        private void OnUnselect()
        {
            _isSelected = false;
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (_isSelected)
            {
                _isSelected = false;
            }
            else
            {            
                OnButtonSelected?.Invoke(this, new EventArgs());
                if (!_window.MainWindowStates.shouldNodeBeMoved) return;

                _isSelected = true;
                
                Point currentMousePosition = e.GetPosition(sender as Window);

                _movementDiffLeft = currentMousePosition.X - NodeConfiguration.EllipseDimensions / 2 - NodeConfiguration.UserInterfaceLeftSize - GetPosLeft();
                _movementDiffTop = currentMousePosition.Y - NodeConfiguration.EllipseDimensions / 2 - NodeConfiguration.UserInterfaceTopSize - GetPosTop();
            }
        }

        public void EndMovementAnimations()
        {
            double canvasLeft = GetPosLeft();
            double canvasTop = GetPosTop();
            NodeAnimator.StopEllipseAnimationOnProperty(Ellipse, Canvas.LeftProperty);
            NodeAnimator.StopEllipseAnimationOnProperty(Ellipse, Canvas.TopProperty);
            SetPosLeft(canvasLeft);
            SetPosTop(canvasTop);
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (!_isSelected) return;
            if (!_window.MainWindowStates.shouldNodeBeMoved) return;

            EndMovementAnimations();

            Point currentMousePosition = e.GetPosition(sender as Window);
            SetPosTop(currentMousePosition.Y - NodeConfiguration.EllipseDimensions / 2 - NodeConfiguration.UserInterfaceTopSize - _movementDiffTop);
            SetPosLeft(currentMousePosition.X - NodeConfiguration.EllipseDimensions / 2 - NodeConfiguration.UserInterfaceLeftSize - _movementDiffLeft);

            OnNodeMoved?.Invoke(this, e);
        }
        
        public double GetPosLeft()
        {
            return (double)Ellipse.GetValue(Canvas.LeftProperty);
        }

        public double GetPosTop()
        {
            return (double)Ellipse.GetValue(Canvas.TopProperty);
        }

        private void SetPosLeft(double canvasLeft)
        {
            Ellipse.SetValue(Canvas.LeftProperty, canvasLeft);
        }

        private void SetPosTop(double canvasTop)
        {
            Ellipse.SetValue(Canvas.TopProperty, canvasTop);
        }

        private void TextBlockSetPosLeft(double canvasLeft)
        {
            _textBlock.SetValue(Canvas.LeftProperty, canvasLeft);
        }

        private void TextBlockSetPosTop(double canvasTop)
        {
            _textBlock.SetValue(Canvas.TopProperty, canvasTop);
        }

        protected void OnMagicWondOrder()
        {
            if (!_canvas.Children.Contains(Ellipse)) return;

            double leftTarget = _random.Next(100) - 50;
            double topTarget = _random.Next(100) - 50;

            double toLeft = GetPosLeft() + leftTarget;
            DoubleAnimation ellipseAnimationLeft = NodeAnimator.BuildEllipseMagicAnimationLeft(toLeft);

            double toTop = GetPosTop() + topTarget;
            DoubleAnimation ellipseAnimationTop = NodeAnimator.BuildEllipseMagicAnimationTop(toTop);

            Ellipse.BeginAnimation(Canvas.LeftProperty, ellipseAnimationLeft);
            Ellipse.BeginAnimation(Canvas.TopProperty, ellipseAnimationTop);

            _testWasMagicWandClicked = true;
        }

        private void NodeAddingAnimation()
        {
            DoubleAnimation nodeWidthAnimation = NodeAnimator.BuildEllipseArrivalAnimationWidth(NodeConfiguration.EllipseDimensions);
            Ellipse.BeginAnimation(Button.WidthProperty, nodeWidthAnimation);

            DoubleAnimation nodeMovingLeftAnimation = NodeAnimator.BuildEllipseArrivalAnimationLeft(this, NodeConfiguration.EllipseDimensions);
            Ellipse.BeginAnimation(Canvas.LeftProperty, nodeMovingLeftAnimation);

            DoubleAnimation nodeMovingTopAnimation = NodeAnimator.BuildEllipseArrivalAnimationTop(this, NodeConfiguration.EllipseDimensions);
            Ellipse.BeginAnimation(Canvas.TopProperty, nodeMovingTopAnimation);

            DoubleAnimation nodeHeightAnimation = NodeAnimator.BuildEllipseArrivalAnimationHeight(NodeConfiguration.EllipseDimensions);
            Ellipse.BeginAnimation(Button.HeightProperty, nodeHeightAnimation);
        }

        private void OnEllipseLayoutUpdated(object sender, EventArgs e)
        {
            MoveTextBlock();
            OnNodesAnimated?.Invoke();
        }

        public override string ToString()
        {
            return "Node " + Id;
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
            _window.OnKillAllSelections -= OnUnselect;
            _window.OnMagicWandOrder -= OnMagicWondOrder;
            _canvas.Children.Remove(Ellipse);
            OnButtonSelected -= _window.OnNodeSelected;
        }

        public List<int> GetIdAsList()
        {
            return new List<int> { Id };
        }
    }
}
