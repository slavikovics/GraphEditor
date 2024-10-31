using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GraphEditor
{
    internal class GraphItemBorder : Border
    {
        public StackPanel _innerStackPanel;

        GraphItemNameLabel _graphItemNameLabel;

        public string _borderType;

        public string _borderString;

        public ControlTemplate _buttonTemplate;

        public Image _buttonAddNodeContent;

        public Image _buttonAddEdgeContent;

        public Image _buttonGraphImageContent;

        public GraphItemBorder(string borderName, string borderString, string borderType, 
            ControlTemplate buttonTemplate, Image buttonAddNodeContent, Image buttonAddEdgeContent, Image buttonGraphImageContent)
        {
            Name = borderName;
            _borderType = borderType;
            _borderString = borderString;
            _buttonTemplate = buttonTemplate;
            _buttonAddNodeContent = buttonAddNodeContent;
            _buttonAddEdgeContent = buttonAddEdgeContent;
            _buttonGraphImageContent = buttonGraphImageContent;

            Height = 30;
            Margin = new Thickness(4);
            Background = new SolidColorBrush(Color.FromArgb(255, 153, 143, 199));
            CornerRadius = new CornerRadius(10);

            _innerStackPanel = new StackPanel();
            SetMargins();
            _innerStackPanel.Orientation = Orientation.Horizontal;

            RenamingButton graphItemRenamingButton = new RenamingButton(buttonTemplate);
            SetContent(graphItemRenamingButton);
            graphItemRenamingButton.Click += RenamingButtonClick;

            _graphItemNameLabel = new GraphItemNameLabel(borderString);

            _innerStackPanel.Children.Add(graphItemRenamingButton);
            _innerStackPanel.Children.Add(_graphItemNameLabel);

            Child = _innerStackPanel;
        }

        public void SetMargins()
        {
            switch (_borderType)
            {
                case "graph": _innerStackPanel.Margin = new Thickness(4, 0, 4, 0); break;
                default:
                    Margin = new Thickness(20, 4, 4, 4);
                    _innerStackPanel.Margin = new Thickness(4, 0, 4, 0);
                    break;
            }
        }

        public void SetContent(RenamingButton graphItemRenamingButton)
        {
            switch (_borderType)
            {
                case "graph": GenerateGraphButtonContent(graphItemRenamingButton); break;
                case "node": GenerateNodeButtonContent(graphItemRenamingButton); break;
                case "edge": GenerateEdgeButtonContent(graphItemRenamingButton); break;
            }
        }

        private void GenerateGraphButtonContent(Button graphButton)
        {
            Image graphButtonContentImage = new Image();
            graphButtonContentImage.Source = _buttonGraphImageContent.Source;
            graphButton.Content = graphButtonContentImage;
        }

        private void GenerateNodeButtonContent(Button nodeButton)
        {
            Image nodeContentImage = new Image();
            nodeContentImage.Source = (_buttonAddNodeContent).Source;
            nodeButton.Content = nodeContentImage;
        }

        private void GenerateEdgeButtonContent(Button edgeButton)
        {
            Image edgeContentImage = new Image();
            edgeContentImage.Source = (_buttonAddEdgeContent).Source;
            edgeButton.Content = edgeContentImage;
        }

        private void RenamingButtonClick(object sender, RoutedEventArgs e)
        {
            RenamingWindow renamingWindow = new RenamingWindow((string) _graphItemNameLabel.Content);
            renamingWindow.Show();
            renamingWindow.RenamingResult += RenamingWindow_RenamingResult;
        }

        private void RenamingWindow_RenamingResult(object sender, RenamingEventArgs e)
        {
            if (e._wasRenamed == true)
            {
                _graphItemNameLabel.Content = e._newName;
            }
        }
    }
}
