using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace GraphEditor.GraphTabs
{
    internal class TabButton : RenamingButton
    {
        public int _tabButtonId { get; private set; }

        public TabButton(ControlTemplate buttonTemplate, int id) : base(buttonTemplate)
        {
            _tabButtonId = id;
            Height = 35;
            if (id != 0)
            {        
                Width = 90;
            }
            else
            {
                Width = 35;
            }
            Margin = new Thickness(0, 15, 0, 0);
        }

        public void AnimateButtonExpansion()
        {
            DoubleAnimation buttonExpansionAnimation = new DoubleAnimation();
            buttonExpansionAnimation.From = 0;
            buttonExpansionAnimation.To = this.Width;
            buttonExpansionAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(300));
            this.BeginAnimation(WidthProperty, buttonExpansionAnimation);
        }

        public void AnimateButtonMovementRight()
        {
            DoubleAnimation buttonMovementAnimation = new DoubleAnimation();
            buttonMovementAnimation.From = (double)this.GetValue(Canvas.LeftProperty) - 100;
            buttonMovementAnimation.To = (double)this.GetValue(Canvas.LeftProperty);
            buttonMovementAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(300));
            this.BeginAnimation(Canvas.LeftProperty, buttonMovementAnimation);
        }
    }
}
