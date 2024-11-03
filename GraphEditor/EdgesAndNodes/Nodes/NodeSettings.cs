using System.Windows.Controls;

namespace GraphEditor.EdgesAndNodes.Nodes
{
    internal class NodeSettings
    {
        public static void SetUpTextBlock(TextBlock _textBlock, int _id)
        {
            _textBlock.TextAlignment = NodeConfiguration.TextBlockTextAlignment;
            _textBlock.Foreground = NodeConfiguration.TextBlockForeground;
            _textBlock.Text = NodeConfiguration.TextBlockText + _id;
            _textBlock.FontSize = NodeConfiguration.TextBlockFontSize;
            _textBlock.FontWeight = NodeConfiguration.TextBlockFontWeight;
            _textBlock.Height = NodeConfiguration.TextBlockHeight;
            _textBlock.Width = NodeConfiguration.TextBlockWidth;
        }

        public static void SetUpEllipse(Button ellipse, MainWindow _window)
        {
            ellipse.Width = NodeConfiguration.EllipseDimensions;
            ellipse.Height = NodeConfiguration.EllipseDimensions;
            ellipse.Template = _window.ButtonMagicWond.Template;
            ellipse.Content = new Image();
            (ellipse.Content as Image).Source = ((Image)_window.EllipseExample.Content).Source;
        }
    }
}
