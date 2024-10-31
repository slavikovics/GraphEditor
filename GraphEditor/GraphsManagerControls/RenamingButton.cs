using System.Windows.Controls;

namespace GraphEditor
{
    internal class RenamingButton : Button
    {
        public RenamingButton(ControlTemplate buttonTemplate)
        {
            Height = 20;
            Width = 20;
            VerticalAlignment = System.Windows.VerticalAlignment.Center;
            Template = buttonTemplate;
        }
    }
}
