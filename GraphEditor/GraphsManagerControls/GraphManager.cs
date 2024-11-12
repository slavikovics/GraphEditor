using GraphEditor.EdgesAndNodes;
using GraphEditor.GraphsManagerControls;
using GraphEditor.Windows.MainWindow;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace GraphEditor.GraphsManager
{
    internal class GraphManager
    {
        private List<Node> _nodes;

        private List<IEdge> _edges;

        public ControlTemplate ButtonTemplate;

        public Image ButtonAddNodeContent;

        public Image ButtonAddEdgeContent;

        public Image ButtonAddGraphContent;

        public GraphManager(ControlTemplate buttonTemplate, Image buttonAddNodeContent, Image buttonAddEdgeContent, Image buttonAddGraphContent) 
        {
            ButtonTemplate = buttonTemplate;
            ButtonAddNodeContent = buttonAddNodeContent;
            ButtonAddEdgeContent = buttonAddEdgeContent;
            ButtonAddGraphContent = buttonAddGraphContent;
            _nodes = new List<Node>();
            _edges = new List<IEdge>();
        }

        public GraphItemBorder AddNode(Node node, List<string> nodesDependencies)
        {
            GraphItemBorder border = GenerateGraphManagerGraphBorder("node" + node.Id.ToString(), node.ToString(), "node", node, nodesDependencies);
            _nodes.Add(node);
            return border;
        }

        public GraphItemBorder AddGraph(string graphName)
        {
            return GenerateGraphManagerGraphBorder("", graphName, "graph", null, new List<string>());
        }

        public GraphItemBorder AddEdge(IEdge edge, List<string> nodesDependencies)
        {
            GraphItemBorder border = GenerateGraphManagerGraphBorder("", edge.ToString(), "edge", edge, nodesDependencies);
            _edges.Add(edge);
            return border;
        }

        private GraphItemBorder GenerateGraphManagerGraphBorder(string borderName, string borderString, string borderType, IRenamable renamable, List<string> nodesDependencies)
        {
            GraphItemBorder graphItemBorder = new GraphItemBorder(borderName, borderString, borderType,
                ButtonTemplate, ButtonAddNodeContent, ButtonAddEdgeContent, ButtonAddGraphContent, renamable, nodesDependencies);

            return graphItemBorder;
        }

        public static void AnimateGraphsManagerGridExpansion(StackPanel graphVisualTreeStackPanel, Grid graphsManagerGrid) 
        {
            DoubleAnimation gridAnimation = MainWindowAnimator.BuildGraphsManagerGridExpansion(graphVisualTreeStackPanel);
            if (gridAnimation.To >= 600 && graphsManagerGrid.Height < gridAnimation.To) return;
            graphsManagerGrid.BeginAnimation(Grid.HeightProperty, gridAnimation);
        }
    }
}
