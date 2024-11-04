using GraphEditor.GraphTabs;
using System.Windows.Controls;
using System.Xml.Linq;

namespace GraphEditor.GraphTab
{
    internal class Tab
    {
        TabButton tabButton;

        public Tab(TextBlock name, ControlTemplate buttonTemplate, int id)
        {
            tabButton = new TabButton(buttonTemplate, id);
            tabButton.Content = name;
        }

        public Tab(Image image, ControlTemplate buttonTemplate, int id)
        {
            tabButton = new TabButton(buttonTemplate, id);
            Image contentImage = new Image();
            contentImage.Source = image.Source;
            tabButton.Content = contentImage;
        }

        public TabButton GetTabAsRenamingButton()
        {
            return tabButton;
        }
    }
}
