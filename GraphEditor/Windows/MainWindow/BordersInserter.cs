﻿using GraphEditor.EdgesAndNodes;
using GraphEditor.EdgesAndNodes.Edges;
using GraphEditor.GraphsManager;
using System.Windows;
using System.Windows.Controls;

namespace GraphEditor.Windows.MainWindow
{
    internal class BordersInserter
    {
        public static void InsertEdgeBorder(IEdge edge, GraphManager graphsManager, EdgeTypes selectedEdgeType, StackPanel graphVisualTreeStackPanel)
        {
            Border edgeBorder = graphsManager.AddEdge(edge, edge.GetNodesDependencies());
            edgeBorder.Margin = new Thickness(40, 4, 4, 4);
            string firstNodeId;
            if (selectedEdgeType == EdgeTypes.NonOriented)
            {
                firstNodeId = edge.GetFirstNodeId();
            }
            else
            {
                firstNodeId = edge.GetSecondNodeId();
            }
            int i = 0;
            foreach (UIElement uIElement in graphVisualTreeStackPanel.Children)
            {
                i++;
                if ((uIElement as GraphItemBorder)?.BorderName == "node" + firstNodeId.ToString())
                {
                    break;
                }
            }

            graphVisualTreeStackPanel.Children.Insert(i, edgeBorder);
        }

        public static void InsertImportedEdgeBorder(IEdge edge, GraphManager graphsManager, EdgeTypes selectedEdgeType,
            StackPanel graphVisualTreeStackPanel)
        {
            Border edgeBorder = graphsManager.AddEdge(edge, edge.GetNodesDependencies());
            edgeBorder.Margin = new Thickness(40, 4, 4, 4);
            string firstNodeName;
            if (selectedEdgeType == EdgeTypes.NonOriented)
            {
                firstNodeName = edge.GetFirstNode().Name;
            }
            else
            {
                firstNodeName = edge.GetSecondNode().Name;
            }
            int i = 0;
            foreach (UIElement uIElement in graphVisualTreeStackPanel.Children)
            {
                i++;
                if ((uIElement as GraphItemBorder)?.BorderName == "node" + firstNodeName)
                {
                    break;
                }
            }

            graphVisualTreeStackPanel.Children.Insert(i, edgeBorder);
        }

        public static void InsertNodeBorder(Node node, GraphManager graphsManager, StackPanel graphVisualTreeStackPanel)
        {
            graphVisualTreeStackPanel.Children.Add(graphsManager.AddNode(node, node.GetIdAsList()));
        }
    }
}
