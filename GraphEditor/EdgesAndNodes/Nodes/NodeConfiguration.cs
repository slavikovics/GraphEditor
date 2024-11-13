using System.Windows;
using System.Windows.Media;

namespace GraphEditor.EdgesAndNodes.Nodes
{
    internal static class NodeConfiguration
    {
        public const TextAlignment TextBlockTextAlignment = TextAlignment.Center;
        public static readonly SolidColorBrush TextBlockForeground = new SolidColorBrush(Color.FromArgb(255, 174, 163, 216));
        public const string TextBlockText = "Node ";
        public const int TextBlockFontSize = 14;
        public static FontWeight TextBlockFontWeight = FontWeights.Bold;
        public static readonly int TextBlockHeight = 30;
        public static readonly int TextBlockWidth = 300;

        public static readonly int EllipseDimensions = 40;
        public static int EllipseHeight = 40;
        public static int EllipseWidth = 40;

        public const int UserInterfaceLeftSize = 80;
        public const int UserInterfaceTopSize = 35;
    }
}
