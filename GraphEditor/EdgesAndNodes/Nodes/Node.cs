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
    public class Node : IRenamable
    {
        public Button Ellipse { get; private set; }

        private Canvas _canvas;

        protected TextBlock TextBlock;

        private MainWindow _window;

        private Random _random;

        public string Id { get; set; }

        public string Name { get; private set; }

        private bool _isSelected = false;

        private double _movementDiffLeft;

        private double _movementDiffTop;

        private bool _testWasMagicWandClicked = false;

        public event EventHandler OnButtonSelected;

        public event EventHandler OnNodeMoved;

        public event Action OnNodesAnimated;

        public Node(double canvasLeft, double canvasTop, Canvas parent, MainWindow window, int id): 
            this(canvasLeft, canvasTop, parent, window)
        {            
            Id = "n" + id;
            Name = Id;
            TextBlock = new TextBlock();
            NodeSettings.SetUpTextBlock(TextBlock, Id);
            MoveTextBlock();
            _canvas.Children.Add(TextBlock);
        }

        protected Node(double canvasLeft, double canvasTop, Canvas parent, MainWindow window)
        {
            _canvas = parent;
            _window = window;
            _random = new Random();
            Ellipse = new Button();
            NodeSettings.SetUpEllipse(Ellipse, _window);
            PositionEllipse(canvasLeft, canvasTop);
            _canvas.Children.Add(Ellipse);

            SetUpEvents();
            NodeAddingAnimation();
        }

        public Node(double canvasLeft, double canvasTop, Canvas parent, MainWindow window, string id) :
            this(canvasLeft, canvasTop, parent, window)
        {
            Id = id;
            Name = id;
            TextBlock = new TextBlock();
            NodeSettings.SetUpTextBlock(TextBlock, Name);
            MoveTextBlock();
            _canvas.Children.Add(TextBlock);
        }

        private void SetUpEvents()
        {
            (Ellipse.Content as Image).MouseDown += OnMouseDown;
            _window.MouseMove += OnMouseMove;
            _window.OnKillAllSelections += OnUnselect;
            _window.OnMagicWandOrder += OnMagicWandOrder;
            Ellipse.LayoutUpdated += OnEllipseLayoutUpdated;
        }

        protected void PositionEllipse(double canvasLeft, double canvasTop)
        {
            SetPosLeft(NodeCalculations.CalculateEllipsePositionLeft(canvasLeft, NodeConfiguration.UserInterfaceLeftSize, NodeConfiguration.EllipseDimensions));
            SetPosTop(NodeCalculations.CalculateEllipsePositionTop(canvasTop, NodeConfiguration.UserInterfaceTopSize, NodeConfiguration.EllipseDimensions));
        }

        public virtual int GetEllipseDimensions()
        {
            return NodeConfiguration.EllipseDimensions;
        }

        private void MoveTextBlock()
        {
            TextBlockSetPosLeft(NodeCalculations.CalculateTextBlockCanvasLeft(this, TextBlock, NodeConfiguration.EllipseDimensions));
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
                if (!_window.MainWindowStates.ShouldNodeBeMoved) return;

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
            if (!_window.MainWindowStates.ShouldNodeBeMoved) return;

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

        public void SetPosLeft(double canvasLeft)
        {
            Ellipse.SetValue(Canvas.LeftProperty, canvasLeft);
        }

        public void SetPosTop(double canvasTop)
        {
            Ellipse.SetValue(Canvas.TopProperty, canvasTop);
        }

        private void TextBlockSetPosLeft(double canvasLeft)
        {
            TextBlock.SetValue(Canvas.LeftProperty, canvasLeft);
        }

        private void TextBlockSetPosTop(double canvasTop)
        {
            TextBlock.SetValue(Canvas.TopProperty, canvasTop);
        }

        protected void OnMagicWandOrder()
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
            return Id;
        }

        protected virtual string BuildIdString(string id)
        {
            return Id;
        }

        public void Rename(string newName)
        {
            TextBlock.Text = newName;
            MoveTextBlock();
            Name = newName;
        }

        public void Remove()
        {
            _canvas.Children.Remove(TextBlock);
            _window.MouseMove -= OnMouseMove;
            _window.OnKillAllSelections -= OnUnselect;
            _window.OnMagicWandOrder -= OnMagicWandOrder;
            _canvas.Children.Remove(Ellipse);
            OnButtonSelected -= _window.OnNodeSelected;
        }

        public List<string> GetIdAsList()
        {
            return new List<string> { Id };
        }
    }
}
