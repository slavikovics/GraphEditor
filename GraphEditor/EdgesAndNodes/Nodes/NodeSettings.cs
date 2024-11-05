using System.Windows.Controls;

namespace GraphEditor.EdgesAndNodes.Nodes
{
    internal class NodeSettings
    {
        public static void SetUpTextBlock(TextBlock textBlock, int id)
        {
            textBlock.TextAlignment = NodeConfiguration.TextBlockTextAlignment;
            textBlock.Foreground = NodeConfiguration.TextBlockForeground;
            textBlock.Text = NodeConfiguration.TextBlockText + id;
            textBlock.FontSize = NodeConfiguration.TextBlockFontSize;
            textBlock.FontWeight = NodeConfiguration.TextBlockFontWeight;
            textBlock.Height = NodeConfiguration.TextBlockHeight;
            textBlock.Width = NodeConfiguration.TextBlockWidth;
        }

        public static void SetUpEllipse(Button ellipse, MainWindow window)
        {
            ellipse.Width = NodeConfiguration.EllipseDimensions;
            ellipse.Height = NodeConfiguration.EllipseDimensions;
            ellipse.Template = window.ButtonMagicWond.Template;
            ellipse.Content = new Image();
            (ellipse.Content as Image).Source = ((Image)window.EllipseExample.Content).Source;
        }
    }
}
