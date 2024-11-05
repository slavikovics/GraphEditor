using System;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace GraphEditor.EdgesAndNodes.Edges
{
    internal class EdgeCalculations
    {
        public static double CalculateEdgePositionBaseLeft(Node node)
        {
            double basePointLeft = (double)node.GetPosLeft() + node.GetEllipseDimensions() / 2;
            return basePointLeft;
        }

        public static double CalculateEdgePositionBaseLeftWithArrow(Node node, int arrowOffset)
        {
            double basePointLeft = (double)node.GetPosLeft() + node.GetEllipseDimensions() / 2 + arrowOffset;
            return basePointLeft;
        }

        public static double CalculateEdgePositionBaseTop(Node node)
        {
            return (double)node.GetPosTop() + node.GetEllipseDimensions() / 2;
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

        public static double CalculateFinalWidth(Node node1, Node node2, int edgeOffsetLeft)
        {
            return CalculateLengthBetweenNodes(node1, node2) - node1.GetEllipseDimensions() - 2 * edgeOffsetLeft;
        }

        public static double CalculateFinalWidthWithArrow(Node node1, Node node2, int edgeOffsetLeft, int arrowOffset)
        {
            return CalculateLengthBetweenNodes(node1, node2) - node1.GetEllipseDimensions() - 2 * edgeOffsetLeft - arrowOffset;
        }

        public static double CalculateAngle(Node node1, Node node2)
        {
            double angleRadians = Math.Atan(CalculateDeltaY(node1, node2) / CalculateDeltaX(node1, node2));
            double angleDegrees = angleRadians / Math.PI * 180 * -1;

            if (node1.GetPosLeft() <= node2.GetPosLeft())
            {
                if (node1.GetPosTop() <= node2.GetPosTop())
                {
                    return 360 - angleDegrees;
                }
                else
                {
                    return -1 * angleDegrees;
                }
            }
            else
            {
                if (node1.GetPosTop() <= node2.GetPosTop())
                {
                    return 180 + (-1) * angleDegrees;
                }
                else
                {
                    return 180 - angleDegrees;
                }
            }
        }

        public static double CalculateRenderTransformOriginLeft(double width, Node firstNode, Rectangle edgeVisualRepresentation)
        {
            if (width == 0) return 0;

            double originLeft = -1 * (firstNode.GetEllipseDimensions() / 2 + 5) / edgeVisualRepresentation.Width;

            if (originLeft > 100 || originLeft < -100) return 0;

            return originLeft;
        }

        public static double CalculateRenderTransformOriginLeftWithArrow(double width, Node firstNode, Rectangle edgeVisualRepresentation, int arrowOffset)
        {
            if (width == 0) return 0;

            double originLeft = -1 * (firstNode.GetEllipseDimensions() / 2 + 5 + arrowOffset) / edgeVisualRepresentation.Width;

            if (originLeft > 100 || originLeft < -100) return 0;

            return originLeft;
        }

        public  static double CalculateArrowRenderTransformOriginLeft(Image arrow, Node firstNode)
        {
            if (arrow.Width == 0) return 0;

            double originLeft = -1 * (firstNode.GetEllipseDimensions() / 2 + 5) / arrow.Width;

            if (originLeft > 100 || originLeft < -100) return 0;

            return originLeft;
        }
    }
}
