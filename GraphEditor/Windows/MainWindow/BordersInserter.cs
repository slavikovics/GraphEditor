using GraphEditor.EdgesAndNodes;
using GraphEditor.GraphsManager;
using System.Windows.Controls;
using System.Windows;

namespace GraphEditor.Windows.MainWindow
{
    internal class BordersInserter
    {
        public static void InsertEdgeBorder(IEdgeable edge, GraphManager graphsManager, string selectedEdgeType, StackPanel GraphVisualTreeStackPanel,
            string NonOriented)
        {
            Border edgeBorder = graphsManager.AddEdge(edge, edge.GetNodesDependencies());
            edgeBorder.Margin = new Thickness(40, 4, 4, 4);
            int firstNodeId;
            if (selectedEdgeType == NonOriented)
            {
                firstNodeId = edge.GetFirstNodeId();
            }
            else
            {
                firstNodeId = edge.GetSecondNodeId();
            }
            int i = 0;
            foreach (UIElement uIElement in GraphVisualTreeStackPanel.Children)
            {
                i++;
                if ((uIElement as Border)?.Name == "node" + firstNodeId.ToString())
                {
                    break;
                }
            }

            GraphVisualTreeStackPanel.Children.Insert(i, edgeBorder);
        }

        public static void InsertNodeBorder(Node node, GraphManager graphsManager, StackPanel GraphVisualTreeStackPanel)
        {
            GraphVisualTreeStackPanel.Children.Add(graphsManager.AddNode(node, node.GetIdAsList()));
        }
    }
}
