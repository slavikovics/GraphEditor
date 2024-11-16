using GraphEditor.GraphsManagerControls;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GraphEditor
{
    internal class GraphItemBorder : Border
    {
        public StackPanel InnerStackPanel;

        private GraphItemNameLabel _graphItemNameLabel;

        public string BorderType;

        public string BorderName;

        public string BorderString;

        public ControlTemplate ButtonTemplate;

        public Image ButtonAddNodeContent;

        public Image ButtonAddEdgeContent;

        public Image ButtonGraphImageContent;

        private IRenamable _renamable;

        public List<string> NodesDependencies;

        public GraphItemBorder(string borderName, string borderString, string borderType, 
            ControlTemplate buttonTemplate, Image buttonAddNodeContent, Image buttonAddEdgeContent, Image buttonGraphImageContent, IRenamable renamable, List<string> nodesDependencies)
        {
            BorderName = borderName;
            BorderType = borderType;
            BorderString = borderString;
            ButtonTemplate = buttonTemplate;
            ButtonAddNodeContent = buttonAddNodeContent;
            ButtonAddEdgeContent = buttonAddEdgeContent;
            ButtonGraphImageContent = buttonGraphImageContent;
            _renamable = renamable;
            NodesDependencies = nodesDependencies;

            Height = 30;
            Margin = new Thickness(4);
            Background = new SolidColorBrush(Color.FromArgb(255, 153, 143, 199));
            CornerRadius = new CornerRadius(10);

            InnerStackPanel = new StackPanel();
            SetMargins();
            InnerStackPanel.Orientation = Orientation.Horizontal;

            RenamingButton graphItemRenamingButton = new RenamingButton(buttonTemplate);
            SetContent(graphItemRenamingButton);
            graphItemRenamingButton.Click += OnRenamingButtonClick;

            _graphItemNameLabel = new GraphItemNameLabel(borderString);

            InnerStackPanel.Children.Add(graphItemRenamingButton);
            InnerStackPanel.Children.Add(_graphItemNameLabel);

            Child = InnerStackPanel;
        }

        public void SetMargins()
        {
            switch (BorderType)
            {
                case "graph": InnerStackPanel.Margin = new Thickness(4, 0, 4, 0); break;
                default:
                    Margin = new Thickness(20, 4, 4, 4);
                    InnerStackPanel.Margin = new Thickness(4, 0, 4, 0);
                    break;
            }
        }

        public void SetContent(RenamingButton graphItemRenamingButton)
        {
            switch (BorderType)
            {
                case "graph": GenerateGraphButtonContent(graphItemRenamingButton); break;
                case "node": GenerateNodeButtonContent(graphItemRenamingButton); break;
                case "edge": GenerateEdgeButtonContent(graphItemRenamingButton); break;
            }
        }

        private void GenerateGraphButtonContent(Button graphButton)
        {
            Image graphButtonContentImage = new Image();
            graphButtonContentImage.Source = ButtonGraphImageContent.Source;
            graphButton.Content = graphButtonContentImage;
        }

        private void GenerateNodeButtonContent(Button nodeButton)
        {
            Image nodeContentImage = new Image();
            nodeContentImage.Source = (ButtonAddNodeContent).Source;
            nodeButton.Content = nodeContentImage;
        }

        private void GenerateEdgeButtonContent(Button edgeButton)
        {
            Image edgeContentImage = new Image();
            edgeContentImage.Source = (ButtonAddEdgeContent).Source;
            edgeButton.Content = edgeContentImage;
        }

        private void OnRenamingButtonClick(object sender, RoutedEventArgs e)
        {
            RenamingWindow renamingWindow = new RenamingWindow((string) _graphItemNameLabel.Content);
            renamingWindow.Show();
            renamingWindow.OnRenamingResult += OnRenamingWindowRenamingResult;
        }

        private void OnRenamingWindowRenamingResult(object sender, RenamingEventArgs e)
        {
            if (e.WasRenamed == true)
            {
                _graphItemNameLabel.Content = e.NewName;
                _renamable?.Rename(e.NewName);
            }
        }
    }
}
