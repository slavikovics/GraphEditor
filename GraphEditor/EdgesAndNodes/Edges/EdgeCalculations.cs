using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GraphEditor.EdgesAndNodes.Edges
{
    internal class EdgeCalculations
    {
        public static double CalculateEdgePositionBaseLeft(Node node)
        {
            double basePointLeft = (double)node.ellipse.GetValue(Canvas.LeftProperty) + node.GetEllipseDimensions() / 2;
            return basePointLeft;
        }

        public static double CalculateEdgePositionBaseTop(Node node)
        {
            return (double)node.ellipse.GetValue(Canvas.TopProperty) + node.GetEllipseDimensions() / 2;
        }

        public static double CalculateDeltaX(Node node1, Node node2)
        {
            return (double)node2.GetPosLeft() - (double)node1.GetPosLeft();
        }

        public static double CalculateDeltaY(Node node1, Node node2)
        {
            return (double)node2.GetPosTop() - (double)node1.GetPosTop();
        }

        public static double CalculateLengthBetweenNodes(Node node1, Node node2)
        {
            double deltaX = CalculateDeltaX(node1, node2);
            double deltaY = CalculateDeltaY(node1, node2);

            return Math.Sqrt(Math.Pow(deltaX, 2) + Math.Pow(deltaY, 2));
        }

        public static double CalculateFinalWidth(Node node1, Node node2, int EdgeOffsetLeft)
        {
            return CalculateLengthBetweenNodes(node1, node2) - node1.GetEllipseDimensions() - 2 * EdgeOffsetLeft;
        }
    }
}
