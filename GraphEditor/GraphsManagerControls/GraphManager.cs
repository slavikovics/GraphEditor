using GraphEditor.EdgesAndNodes;
using GraphEditor.GraphsManagerControls;
using System.Collections.Generic;
using System.Windows.Controls;

namespace GraphEditor.GraphsManager
{
    internal class GraphManager
    {
        private List<Node> _nodes;

        private List<IEdgeable> _edges;

        public ControlTemplate _buttonTemplate;

        public Image _buttonAddNodeContent;

        public Image _buttonAddEdgeContent;

        public Image _buttonAddGraphContent;

        public GraphManager(ControlTemplate buttonTemplate, Image buttonAddNodeContent, Image buttonAddEdgeContent, Image buttonAddGraphContent) 
        {
            _buttonTemplate = buttonTemplate;
            _buttonAddNodeContent = buttonAddNodeContent;
            _buttonAddEdgeContent = buttonAddEdgeContent;
            _buttonAddGraphContent = buttonAddGraphContent;
            _nodes = new List<Node>();
            _edges = new List<IEdgeable>();
        }

        public GraphItemBorder AddNode(Node node, List<int> nodesDependencies)
        {
            GraphItemBorder border = GenerateGraphManagerGraphBorder("node" + node._id.ToString(), node.ToString(), "node", node, nodesDependencies);
            _nodes.Add(node);
            return border;
        }

        public GraphItemBorder AddGraph(string graphName)
        {
            return GenerateGraphManagerGraphBorder("", "Graph", "graph", null, new List<int>());
        }

        public GraphItemBorder AddEdge(IEdgeable edge, List<int> nodesDependencies)
        {
            GraphItemBorder border = GenerateGraphManagerGraphBorder("", edge.ToString(), "edge", edge, nodesDependencies);
            _edges.Add(edge);
            return border;
        }

        private GraphItemBorder GenerateGraphManagerGraphBorder(string borderName, string borderString, string borderType, IRenamable renamable, List<int> nodesDependencies)
        {
            GraphItemBorder graphItemBorder = new GraphItemBorder(borderName, borderString, borderType,
                _buttonTemplate, _buttonAddNodeContent, _buttonAddEdgeContent, _buttonAddGraphContent, renamable, nodesDependencies);

            return graphItemBorder;
        }
    }
}
