using GraphEditor.EdgesAndNodes;
using GraphEditor.GraphsManagerControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public Border AddNode(Node node)
        {
            Border border = GenerateGraphManagerGraphBorder("node" + node._id.ToString(), node.ToString(), "node", node);
            _nodes.Add(node);
            return border;
        }

        public Border AddGraph(string graphName)
        {
            return GenerateGraphManagerGraphBorder("", "Graph", "graph", null);
        }

        public Border AddEdge(IEdgeable edge)
        {
            Border border = GenerateGraphManagerGraphBorder("", edge.ToString(), "edge", edge);
            _edges.Add(edge);
            return border;
        }

        private Border GenerateGraphManagerGraphBorder(string borderName, string borderString, string borderType, IRenamable renamable)
        {
            GraphItemBorder graphItemBorder = new GraphItemBorder(borderName, borderString, borderType,
                _buttonTemplate, _buttonAddNodeContent, _buttonAddEdgeContent, _buttonAddGraphContent, renamable);

            return graphItemBorder;
        }
    }
}
