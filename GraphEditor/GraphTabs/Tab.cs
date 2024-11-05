using GraphEditor.GraphTabs;
using System.Windows.Controls;

namespace GraphEditor.GraphTab
{
    internal class Tab
    {
        private TabButton _tabButton;

        public Tab(TextBlock name, ControlTemplate buttonTemplate, int id)
        {
            _tabButton = new TabButton(buttonTemplate, id);
            _tabButton.Content = name;
        }

        public Tab(Image image, ControlTemplate buttonTemplate, int id)
        {
            _tabButton = new TabButton(buttonTemplate, id);
            Image contentImage = new Image();
            contentImage.Source = image.Source;
            _tabButton.Content = contentImage;
        }

        public TabButton GetTabAsTabButton()
        {
            return _tabButton;
        }
    }
}
