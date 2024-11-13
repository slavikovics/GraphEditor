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
    public enum EdgeTypes
    {
        OrientedSimple,
        OrientedPencil,
        NonOriented
    }
    
    public class Edge : IEdge, IRenamable
    {
        protected double OffsetTop;
        protected double Angle;
        public Node FirstNode { get; private set; }
        public Node SecondNode { get; private set; }
        protected MainWindow MainWindow;
        protected Canvas MainCanvas;
        public Rectangle EdgeVisualRepresentation;
        protected Brush EdgeBrush;
        public string Name { get; private set; }

        private readonly HiddenNode _centerNode;

        protected Edge(Node firstNode, Node secondNode, MainWindow window, Canvas mainCanvas)
        {
            FirstNode = firstNode;
            SecondNode = secondNode;
            MainWindow = window;
            MainCanvas = mainCanvas;
            EdgeVisualRepresentation = new Rectangle();
            Name = ToString();

            _centerNode = new HiddenNode(100, 100, MainCanvas, MainWindow, BuildHiddenNodeId());

            SetUpEvents();
        }

        private void SetUpEvents()
        {
            FirstNode.OnNodeMoved += OnNodePositionChanged;
            SecondNode.OnNodeMoved += OnNodePositionChanged;
            EdgeVisualRepresentation.LayoutUpdated += OnEdgeVisualRepresentationRenderTransformUpdate;
            EdgeVisualRepresentation.MouseEnter += OnEdgeVisualRepresentationMouseEnter;
            EdgeVisualRepresentation.MouseLeave += OnEdgeVisualRepresentationMouseLeave;
            EdgeVisualRepresentation.MouseDown += OnEdgeVisualRepresentationMouseDown;
            EdgeVisualRepresentation.Loaded += OnEdgeVisualRepresentationLoaded;
        }

        private void OnEdgeVisualRepresentationMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MainWindow.OnEdgeSelected(this, e);
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
            double middlePointLeft = (FirstNode.GetPosLeft() + SecondNode.GetPosLeft()) / 2;
            double middlePointTop = (FirstNode.GetPosTop() + SecondNode.GetPosTop()) / 2;
            _centerNode.SetPosLeft(middlePointLeft);
            _centerNode.SetPosTop(middlePointTop);
        }

        public virtual void EdgeDragged(double dragDeltaX, double dragDeltaY)
        {
        }

        protected virtual void AnimateEdgeCreation()
        {
        }

        public string GetFirstNodeId()
        {
            return FirstNode.Id;
        }

        public string GetSecondNodeId()
        {
            return SecondNode.Id;
        }

        protected void EdgeWidthAnimationCompleted(object sender, EventArgs e)
        {
            EdgeVisualRepresentation.LayoutUpdated += OnEdgeVisualRepresentationRenderTransformUpdate;
        }

        private string BuildHiddenNodeId()
        {
            return FirstNode.Id + "-" + SecondNode.Id;
        }

        public override string ToString()
        {
            return "Edge " + FirstNode.Id + " - " + SecondNode.Id;
        }

        public virtual void Rename(string newName)
        {
            Name = newName;
        }

        public virtual void Remove()
        {
            MainCanvas.Children.Remove(EdgeVisualRepresentation);
        }

        public List<string> GetNodesDependencies()
        {
            List<string> dependencies = new List<string>();
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
