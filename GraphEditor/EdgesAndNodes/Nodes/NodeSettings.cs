using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace GraphEditor.EdgesAndNodes.Nodes
{
    internal class NodeSettings
    {
        public static void SetUpTextBlock(TextBlock _textBlock, int _id)
        {
            _textBlock.TextAlignment = TextAlignment.Center;
            _textBlock.Foreground = new SolidColorBrush(Color.FromArgb(255, 174, 163, 216));
            _textBlock.Text = $"Node {_id}";
            _textBlock.FontSize = 14;
            _textBlock.FontWeight = FontWeights.Bold;
            _textBlock.Height = 30;
            _textBlock.Width = 100;
        }

        public static void SetUpEllipse(Button ellipse, MainWindow _window, int EllipseDimensions)
        {
            ellipse.Width = EllipseDimensions;
            ellipse.Height = EllipseDimensions;
            ellipse.Template = _window.ButtonMagicWond.Template;
            ellipse.Content = new Image();
            (ellipse.Content as Image).Source = ((Image)_window.EllipseExample.Content).Source;
        }
    }
}
