using GraphEditor.EdgesAndNodes;
using GraphEditor.GraphsManagerControls;
using GraphEditor.Windows.MainWindow;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using GraphEditor.EdgesAndNodes.Edges;
using GraphEditor.GraphsSavingAndLoading;

namespace GraphEditor.GraphsManager
{
    internal class GraphManager
    {
        private List<Node> _nodes;

        private List<IEdge> _edges;
        
        private MainWindow _mainWindow;

        private readonly ControlTemplate _buttonTemplate;

        public GraphItemBorder GraphName;

        private readonly Image _buttonAddNodeContent;

        private readonly Image _buttonAddEdgeContent;

        private readonly Image _buttonAddGraphContent;

        public GraphManager(ControlTemplate buttonTemplate, Image buttonAddNodeContent, Image buttonAddEdgeContent, Image buttonAddGraphContent, MainWindow mainWindow) 
        {
            _buttonTemplate = buttonTemplate;
            _buttonAddNodeContent = buttonAddNodeContent;
            _buttonAddEdgeContent = buttonAddEdgeContent;
            _buttonAddGraphContent = buttonAddGraphContent;
            _nodes = new List<Node>();
            _edges = new List<IEdge>();
            _mainWindow = mainWindow;
        }

        public void OnImportStarting(object sender, ImportEventArgs e)
        {
            _nodes.Clear();
            _edges.Clear();
            AddGraph(e.GraphName);
            _mainWindow.UpdateGraphVisualTreeStackPanelAfterImport();
        }

        public GraphItemBorder AddNode(Node node, List<string> nodesDependencies)
        {
            GraphItemBorder border = GenerateGraphManagerGraphBorder("node" + node.Id.ToString(), node.Name, "node", node, nodesDependencies);
            _nodes.Add(node);
            return border;
        }

        public GraphItemBorder AddGraph(string graphName)
        {
            GraphName = GenerateGraphManagerGraphBorder("", graphName, "graph", null, new List<string>());
            return GraphName;
        }

        public GraphItemBorder AddEdge(IEdge edge, List<string> nodesDependencies)
        {
            GraphItemBorder border = GenerateGraphManagerGraphBorder("", (edge as Edge).Name, "edge", edge, nodesDependencies);
            _edges.Add(edge);
            return border;
        }

        private GraphItemBorder GenerateGraphManagerGraphBorder(string borderName, string borderString, string borderType, IRenamable renamable, List<string> nodesDependencies)
        {
            GraphItemBorder graphItemBorder = new GraphItemBorder(borderName, borderString, borderType,
                _buttonTemplate, _buttonAddNodeContent, _buttonAddEdgeContent, _buttonAddGraphContent, renamable, nodesDependencies);

            return graphItemBorder;
        }

        public void AnimateGraphsManagerGridExpansion(StackPanel graphVisualTreeStackPanel, Grid graphsManagerGrid) 
        {
            DoubleAnimation gridAnimation = MainWindowAnimator.BuildGraphsManagerGridExpansion(graphVisualTreeStackPanel, _mainWindow.Height);
            //if (gridAnimation.To >= 600 && graphsManagerGrid.Height < gridAnimation.To) return;
            graphsManagerGrid.BeginAnimation(Grid.HeightProperty, gridAnimation);
        }
    }
}
