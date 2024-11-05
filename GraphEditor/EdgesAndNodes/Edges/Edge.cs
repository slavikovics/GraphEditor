using GraphEditor.GraphsManagerControls;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphEditor.EdgesAndNodes.Edges
{
    internal class Edge : IEdge, IRenamable
    {
        protected double _offsetTop;
        protected double _angle;
        protected Node _firstNode;
        protected Node _secondNode;
        protected MainWindow _mainWindow;
        protected Canvas _mainCanvas;
        protected Rectangle _edgeVisualRepresentation;
        protected Brush _edgeBrush;

        public Edge(Node firstNode, Node secondNode, MainWindow window, Canvas mainCanvas)
        {
            _firstNode = firstNode;
            _secondNode = secondNode;
            _mainWindow = window;
            _mainCanvas = mainCanvas;
            _edgeVisualRepresentation = new Rectangle();

            SetUpEvents();
        }

        protected void SetUpEvents()
        {
            _firstNode.OnNodeMoved += OnNodePositionChanged;
            _secondNode.OnNodeMoved += OnNodePositionChanged;
            _edgeVisualRepresentation.LayoutUpdated += EdgeVisualRepresentationRenderTransformUpdate;
            _edgeVisualRepresentation.MouseEnter += EdgeVisualRepresentationMouseEnter;
            _edgeVisualRepresentation.MouseLeave += EdgeVisualRepresentationMouseLeave;
            _edgeVisualRepresentation.MouseDown += EdgeVisualRepresentationMouseDown;
            _edgeVisualRepresentation.Loaded += EdgeVisualRepresentationLoaded;
        }

        protected void EdgeVisualRepresentationMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _mainWindow.OnEdgeSelected(this, e);
        }

        protected virtual void EdgeVisualRepresentationMouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
        }

        protected virtual void EdgeVisualRepresentationMouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
        }

        protected virtual void EdgeVisualRepresentationRenderTransformUpdate(object sender, EventArgs e)
        {
        }

        protected virtual void EdgeVisualRepresentationLoaded(object sender, RoutedEventArgs e)
        {
        }

        public virtual void OnNodePositionChanged(object sender, EventArgs e)
        {
        }

        public virtual void EdgePositioning(bool isInGraph)
        {
        }

        public virtual void EdgeDragged(double dragDeltaX, double dragDeltaY)
        {
        }

        protected virtual void AnimateEdgeCreation()
        {
        }

        public int GetFirstNodeId()
        {
            return _firstNode._id;
        }

        public int GetSecondNodeId()
        {
            return _secondNode._id;
        }

        protected void EdgeWidthAnimationCompleted(object sender, EventArgs e)
        {
            _edgeVisualRepresentation.LayoutUpdated += EdgeVisualRepresentationRenderTransformUpdate;
        }

        public override string ToString()
        {
            return "Edge " + _firstNode._id + " - " + _secondNode._id;
        }

        public virtual void Rename(string newName)
        {

        }

        public virtual void Remove()
        {
            _mainCanvas.Children.Remove(_edgeVisualRepresentation);
        }

        public List<int> GetNodesDependencies()
        {
            List<int> dependencies = new List<int>();
            dependencies.Add(GetFirstNodeId());
            dependencies.Add(GetSecondNodeId());
            return dependencies;
        }
    }
}
