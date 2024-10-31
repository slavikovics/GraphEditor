using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
