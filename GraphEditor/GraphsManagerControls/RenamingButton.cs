using GraphEditor.GraphsManagerControls;
using System.Windows.Controls;

namespace GraphEditor
{
    internal class RenamingButton : Button
    {
        public RenamingButton(ControlTemplate buttonTemplate)
        {
            Height = RenamingButtonConfiguration.ButtonHeight;
            Width = RenamingButtonConfiguration.ButtonWidth;
            VerticalAlignment = RenamingButtonConfiguration.ButtonVerticalAlignment;
            Template = buttonTemplate;
        }

        public RenamingButton()
        {
            Height = RenamingButtonConfiguration.ButtonHeight;
            Width = RenamingButtonConfiguration.ButtonWidth;
            VerticalAlignment = RenamingButtonConfiguration.ButtonVerticalAlignment;
        }
    }
}
