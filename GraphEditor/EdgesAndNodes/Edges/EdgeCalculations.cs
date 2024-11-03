using System;
using System.Windows.Controls;
using System.Windows.Shapes;

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

        public static double CalculateAngle(Node node1, Node node2)
        {
            double _transformRotateAngleCalculationResult;
            double AngleRadians = Math.Atan(CalculateDeltaY(node1, node2) / CalculateDeltaX(node1, node2));
            double AngleDegrees = AngleRadians / Math.PI * 180 * -1;

            if (node1.GetPosLeft() <= node2.GetPosLeft())
            {
                if (node1.GetPosTop() <= node2.GetPosTop())
                {
                    _transformRotateAngleCalculationResult = 360 - AngleDegrees;
                    return 360 - AngleDegrees;
                }
                else
                {
                    _transformRotateAngleCalculationResult = -1 * AngleDegrees;
                    return -1 * AngleDegrees;
                }
            }
            else
            {
                if (node1.GetPosTop() <= node2.GetPosTop())
                {
                    _transformRotateAngleCalculationResult = 180 + (-1) * AngleDegrees;
                    return 180 + (-1) * AngleDegrees;
                }
                else
                {
                    _transformRotateAngleCalculationResult = 180 - AngleDegrees;
                    return 180 - AngleDegrees;
                }
            }
        }

        public static double CalculateRenderTransformOriginLeft(double Width, Node _firstNode, Rectangle edgeVisualRepresentation)
        {
            if (Width == 0) return 0;

            double originLeft = -1 * (_firstNode.GetEllipseDimensions() / 2 + 5) / edgeVisualRepresentation.Width;

            if (originLeft > 100 || originLeft < -100) return 0;

            return originLeft;
        }
    }
}
