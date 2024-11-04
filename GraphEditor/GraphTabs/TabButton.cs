using System.Windows.Controls;

namespace GraphEditor.GraphTabs
{
    internal class TabButton : RenamingButton
    {
        public int _tabButtonId { get; private set; }

        public TabButton(ControlTemplate buttonTemplate, int id) : base()
        {
            _tabButtonId = id;
        }
    }
}
