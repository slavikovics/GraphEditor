using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GraphEditor
{
    internal class GraphItemNameLabel : Label
    {
        public GraphItemNameLabel(string borderString)
        {
            Content = borderString;
            FontWeight = FontWeights.Bold;
            HorizontalAlignment = HorizontalAlignment.Center;
            Foreground = new SolidColorBrush(Color.FromArgb(230, 34, 27, 47));
            FontSize = 15;
        }
    }
}
