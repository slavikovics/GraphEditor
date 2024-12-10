using System;
using System.Linq;
using GraphEditor.GraphLogic;
using GraphEditor.Properties;

namespace GraphEditor.GraphsSavingAndLoading
{
    public class MatrixBuilder
    {
        private Graph _graph;

        public string Result { get; private set; } = String.Empty;

        public MatrixBuilder(Graph graph)
        {
            _graph = graph;
            if (graph.Nodes.Count == 0) return;
            Result = BuildMatrix();
        }

        private string BuildMatrix()
        {
            string result = GenerateStart();
            result += GenerateHead();
            result += GenerateMiddlePart();
            result += GenerateEndPart();

            return result;
        }

        private string GenerateStart()
        {
            return Resources.MatrixBuilder_GenerateStart_;
        }

        private string GenerateTableHeaderItem(string className, string content)
        {
            return $"<th class=\"{className}\">{content}</th>";
        }
        
        private string GenerateTableMiddleItem(string className, string content)
        {
            return $"<td class=\"{className}\">{content}</td>";
        }

        private string GenerateHead()
        {
            string head = Resources.MatrixBuilder_GenerateHead_;
            head += GenerateTableHeaderItem("left-top-corner-element", "");
            foreach (Node node in _graph.Nodes)
            {
                if (node != _graph.Nodes.Last()) head += GenerateTableHeaderItem("matrixTd", node.Name);
            }
            
            head += GenerateTableHeaderItem("right-top-corner-element", _graph.Nodes.Last().Name);

            head += Resources.MatrixBuilder_GenerateHead_End;
            return head;
        }

        private string FindSignForTableItem(Node firstNode, Node secondNode)
        {
           if (_graph.HasEdgeByTwoNodesOrientationSpecific(firstNode, secondNode)) return "+";
           return "-";
        }

        private string GenerateMiddlePart()
        {
            string result = Resources.MatrixBuilder_GenerateMiddlePart;
            string trEnd = Resources.MatrixBuilder_GenerateMiddleParTrEnd;
            
            for (int i = 0; i < _graph.Nodes.Count - 1; i++)
            {
                result += trEnd;
                
                result += GenerateTableMiddleItem("matrixTd", _graph.Nodes[i].Name);
                for (int j = 0; j < _graph.Nodes.Count; j++)
                {
                    result += GenerateTableMiddleItem("matrixTd", FindSignForTableItem(_graph.Nodes[i], _graph.Nodes[j]));
                }
                
                result += trEnd;
            }
            
            return result;
        }

        private string GenerateEndPart()
        {
            string result = Resources.MatrixBuilder_GenerateEndPart;
            result += GenerateTableMiddleItem("left-bottom-corner-element", _graph.Nodes.Last().Name);
            
            for (int j = 0; j < _graph.Nodes.Count - 1; j++)
            {
                result += GenerateTableMiddleItem("matrixTd", FindSignForTableItem(_graph.Nodes.Last(), _graph.Nodes[j]));
            }
            
            result += GenerateTableMiddleItem("right-bottom-corner-element", FindSignForTableItem(_graph.Nodes.Last(), _graph.Nodes.Last()));
            result += Resources.MatrixBuilder_GenerateEndPart_TanleEnd;
            return result;
        }
    }
}