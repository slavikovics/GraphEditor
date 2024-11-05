using System.Windows.Controls;

namespace GraphEditor.EdgesAndNodes.Nodes
{
    internal class NodeCalculations
    {
        public static double CalculateEllipsePositionLeft(double canvasLeft, double uiLeftSize, double ellipseDimensions)
        {
            return canvasLeft - uiLeftSize - ellipseDimensions / 2;
        }

        public static double CalculateEllipsePositionTop(double canvasTop, double uiTopSize, double ellipseDimensions)
        {
            return canvasTop - uiTopSize - ellipseDimensions / 2;
        }

        public static double CalculateTextBlockCanvasLeft(Node node, TextBlock textBlock, int ellipseDimensions)
        {
            return node.GetPosLeft() - (textBlock.Width - ellipseDimensions) / 2;
        }

        public static double CalculateTextBlockCanvasTop(Node node, int ellipseDimensions)
        {
            return node.GetPosTop() + ellipseDimensions + 8;
        }
    }
}
