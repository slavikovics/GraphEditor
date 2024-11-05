using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace GraphEditor.GraphTabs
{
    internal class TabButton : RenamingButton
    {
        public int TabButtonId { get; private set; }

        public TabButton(ControlTemplate buttonTemplate, int id) : base(buttonTemplate)
        {
            TabButtonId = id;
            Height = 35;
            SetTabButtonWidth();
            Margin = new Thickness(0, 15, 0, 0);
        }

        private void SetTabButtonWidth()
        {
            if (TabButtonId != 0)
            {
                Width = 90;
            }
            else
            {
                Width = 35;
            }
        }

        public void AnimateButtonExpansion()
        {
            DoubleAnimation buttonExpansionAnimation = new DoubleAnimation();
            buttonExpansionAnimation.From = 0;
            buttonExpansionAnimation.To = this.Width;
            buttonExpansionAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(300));
            this.BeginAnimation(WidthProperty, buttonExpansionAnimation);
        }

        public void AnimateButtonMovementRight(double canvasLeft)
        {
            DoubleAnimation buttonMovementAnimation = new DoubleAnimation();
            buttonMovementAnimation.From = canvasLeft - 100;
            buttonMovementAnimation.To = canvasLeft;
            buttonMovementAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(300));
            this.BeginAnimation(Canvas.LeftProperty, buttonMovementAnimation);
        }

        public void AnimationHeightExpansion()
        {
            DoubleAnimation heightAnimation = new DoubleAnimation();
            heightAnimation.From = Height;
            heightAnimation.To = 45;
            heightAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(150));
            this.BeginAnimation(HeightProperty, heightAnimation);
        }

        public void AnimationHeightShrinking()
        {
            DoubleAnimation heightAnimation = new DoubleAnimation();
            heightAnimation.From = Height;
            heightAnimation.To = 35;
            heightAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(150));
            this.BeginAnimation(HeightProperty, heightAnimation);
        }
    }
}
