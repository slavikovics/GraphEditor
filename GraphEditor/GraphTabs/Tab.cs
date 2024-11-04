using GraphEditor.GraphTabs;
using System.Windows.Controls;

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

        public TabButton GetTabAsRenamingButton()
        {
            return tabButton;
        }
    }
}
