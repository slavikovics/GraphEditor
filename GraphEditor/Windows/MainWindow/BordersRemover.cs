using System.Collections.Generic;
using System.Windows.Controls;

namespace GraphEditor.Windows.MainWindow
{
    internal class BordersRemover
    {
        public static void RemoveAllBordersForNode(int nodeId, StackPanel graphVisualTreeStackPanel)
        {
            List<GraphItemBorder> bordersToRemove = new List<GraphItemBorder>();
            foreach (GraphItemBorder border in graphVisualTreeStackPanel.Children)
            {
                foreach (int node in border.NodesDependencies)
                {
                    if (node == nodeId)
                    {
                        bordersToRemove.Add(border);
                    }
                }
            }

            foreach (GraphItemBorder border in bordersToRemove)
            {
                graphVisualTreeStackPanel.Children.Remove(border);
            }
        }

        public static void RemoveAllBordersForEdge(int firstNodeId, int secondNodeId, StackPanel graphVisualTreeStackPanel)
        {
            GraphItemBorder borderToRemove = null;
            foreach (GraphItemBorder border in graphVisualTreeStackPanel.Children)
            {
                if (border.NodesDependencies.Count == 2)
                {
                    if ((firstNodeId == border.NodesDependencies[0] && secondNodeId == border.NodesDependencies[1]) || (secondNodeId == border.NodesDependencies[0] && firstNodeId == border.NodesDependencies[1]))
                    {
                        borderToRemove = border;
                    }
                }
            }

            graphVisualTreeStackPanel.Children.Remove(borderToRemove);
        }
    }
}
