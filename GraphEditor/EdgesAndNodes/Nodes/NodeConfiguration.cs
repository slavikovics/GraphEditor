using System.Windows;
using System.Windows.Media;

namespace GraphEditor.EdgesAndNodes.Nodes
{
    internal class NodeConfiguration
    {
        public static TextAlignment TextBlockTextAlignment = TextAlignment.Center;
        public static SolidColorBrush TextBlockForeground = new SolidColorBrush(Color.FromArgb(255, 174, 163, 216));
        public static string TextBlockText = "Node ";
        public static int TextBlockFontSize = 14;
        public static FontWeight TextBlockFontWeight = FontWeights.Bold;
        public static int TextBlockHeight = 30;
        public static int TextBlockWidth = 300;

        public static int EllipseDimensions = 40;
        public static int EllipseHeight = 40;
        public static int EllipseWidth = 40;

        public static int UserInterfaceLeftSize = 80;
        public static int UserInterfaceTopSize = 35;
    }
}
