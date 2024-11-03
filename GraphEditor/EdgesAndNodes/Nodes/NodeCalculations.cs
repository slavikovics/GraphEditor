using System.Windows.Controls;

namespace GraphEditor.EdgesAndNodes.Nodes
{
    internal class NodeCalculations
    {
        public static double CalculateEllipsePositionLeft(double CanvasLeft, double UILeftSize, double EllipseDimensions)
        {
            return CanvasLeft - UILeftSize - EllipseDimensions / 2;
        }

        public static double CalculateEllipsePositionTop(double CanvasTop, double UITopSize, double EllipseDimensions)
        {
            return CanvasTop - UITopSize - EllipseDimensions / 2;
        }

        public static double CalculateTextBlockCanvasLeft(Node node, TextBlock _textBlock, int EllipseDimensions)
        {
            return node.GetPosLeft() - (_textBlock.Width - EllipseDimensions) / 2;
        }

        public static double CalculateTextBlockCanvasTop(Node node, int EllipseDimensions)
        {
            return node.GetPosTop() + EllipseDimensions + 8;
        }
    }
}
