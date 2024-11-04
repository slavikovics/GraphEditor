using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GraphEditor.Windows.MainWindow
{
    internal class BordersRemover
    {
        public static void RemoveAllBordersForNode(int nodeId, StackPanel GraphVisualTreeStackPanel)
        {
            List<GraphItemBorder> bordersToRemove = new List<GraphItemBorder>();
            foreach (GraphItemBorder border in GraphVisualTreeStackPanel.Children)
            {
                foreach (int node in border._nodesDependencies)
                {
                    if (node == nodeId)
                    {
                        bordersToRemove.Add(border);
                    }
                }
            }

            foreach (GraphItemBorder border in bordersToRemove)
            {
                GraphVisualTreeStackPanel.Children.Remove(border);
            }
        }

        public static void RemoveAllBordersForEdge(int firstNodeId, int secondNodeId, StackPanel GraphVisualTreeStackPanel)
        {
            GraphItemBorder borderToRemove = null;
            foreach (GraphItemBorder border in GraphVisualTreeStackPanel.Children)
            {
                if (border._nodesDependencies.Count == 2)
                {
                    if ((firstNodeId == border._nodesDependencies[0] && secondNodeId == border._nodesDependencies[1]) || (secondNodeId == border._nodesDependencies[0] && firstNodeId == border._nodesDependencies[1]))
                    {
                        borderToRemove = border;
                    }
                }
            }

            GraphVisualTreeStackPanel.Children.Remove(borderToRemove);
        }
    }
}
