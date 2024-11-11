using GraphEditor.EdgesAndNodes.Nodes;
using GraphEditor.GraphsManagerControls;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphEditor.EdgesAndNodes.Edges
{
    public class Edge : IEdge, IRenamable
    {
        protected double _offsetTop;
        protected double _angle;
        public Node _firstNode { get; private set; }
        public Node _secondNode { get; private set; }
        protected MainWindow _mainWindow;
        protected Canvas _mainCanvas;
        public Rectangle EdgeVisualRepresentation;
        protected Brush _edgeBrush;
        public string Name { get; private set; }

        protected HiddenNode _centerNode;

        public enum EdgeTypes
        {
            OrientedSimple,
            OrientedPencil,
            NonOriented
        }

        public Edge(Node firstNode, Node secondNode, MainWindow window, Canvas mainCanvas)
        {
            _firstNode = firstNode;
            _secondNode = secondNode;
            _mainWindow = window;
            _mainCanvas = mainCanvas;
            EdgeVisualRepresentation = new Rectangle();
            Name = ToString();

            _centerNode = new HiddenNode(100, 100, _mainCanvas, _mainWindow, 0);

            SetUpEvents();
        }

        protected void SetUpEvents()
        {
            _firstNode.OnNodeMoved += OnNodePositionChanged;
            _secondNode.OnNodeMoved += OnNodePositionChanged;
            EdgeVisualRepresentation.LayoutUpdated += OnEdgeVisualRepresentationRenderTransformUpdate;
            EdgeVisualRepresentation.MouseEnter += OnEdgeVisualRepresentationMouseEnter;
            EdgeVisualRepresentation.MouseLeave += OnEdgeVisualRepresentationMouseLeave;
            EdgeVisualRepresentation.MouseDown += OnEdgeVisualRepresentationMouseDown;
            EdgeVisualRepresentation.Loaded += OnEdgeVisualRepresentationLoaded;
        }

        protected void OnEdgeVisualRepresentationMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _mainWindow.OnEdgeSelected(this, e);
        }

        protected virtual void OnEdgeVisualRepresentationMouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
        }

        protected virtual void OnEdgeVisualRepresentationMouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
        }

        protected virtual void OnEdgeVisualRepresentationRenderTransformUpdate(object sender, EventArgs e)
        {
        }

        protected virtual void OnEdgeVisualRepresentationLoaded(object sender, RoutedEventArgs e)
        {
        }

        public virtual void OnNodePositionChanged(object sender, EventArgs e)
        {
        }

        public virtual void EdgePositioning(bool isInGraph)
        {
            double middlePointLeft = (_firstNode.GetPosLeft() + _secondNode.GetPosLeft()) / 2;
            double middlePointTop = (_firstNode.GetPosTop() + _secondNode.GetPosTop()) / 2;
            _centerNode.SetPosLeft(middlePointLeft);
            _centerNode.SetPosTop(middlePointTop);
        }

        public virtual void EdgeDragged(double dragDeltaX, double dragDeltaY)
        {
        }

        protected virtual void AnimateEdgeCreation()
        {
        }

        public int GetFirstNodeId()
        {
            return _firstNode.Id;
        }

        public int GetSecondNodeId()
        {
            return _secondNode.Id;
        }

        protected void EdgeWidthAnimationCompleted(object sender, EventArgs e)
        {
            EdgeVisualRepresentation.LayoutUpdated += OnEdgeVisualRepresentationRenderTransformUpdate;
        }

        public override string ToString()
        {
            return "Edge " + _firstNode.Id + " - " + _secondNode.Id;
        }

        public virtual void Rename(string newName)
        {
            Name = newName;
        }

        public virtual void Remove()
        {
            _mainCanvas.Children.Remove(EdgeVisualRepresentation);
        }

        public List<int> GetNodesDependencies()
        {
            List<int> dependencies = new List<int>();
            dependencies.Add(GetFirstNodeId());
            dependencies.Add(GetSecondNodeId());
            return dependencies;
        }

        public Rectangle GetEdgeVisualRepresentation()
        {
            return EdgeVisualRepresentation;
        }

        public HiddenNode GetEdgeCenterNode()
        {
            return _centerNode;
        }
    }
}
