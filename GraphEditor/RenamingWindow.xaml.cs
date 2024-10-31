using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GraphEditor
{
    /// <summary>
    /// Interaction logic for InformationWindow.xaml
    /// </summary>
    public partial class RenamingWindow : Window
    {
        private DispatcherTimer timer;

        public RenamingWindow()
        {
            InitializeComponent();
        }

        private void MaximizeWindowButton_Click(object sender, RoutedEventArgs e)
        {
            //if (this.WindowState == WindowState.Normal) this.WindowState = WindowState.Maximized;
            //else this.WindowState = WindowState.Normal;
        }

        private void CollapseWindowButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void CloseWindowButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            VisualStateManager.GoToState(sender as System.Windows.Controls.Button, "MouseOver", true);
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            VisualStateManager.GoToState(sender as System.Windows.Controls.Button, "Normal", true);
        }

        private void Button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            VisualStateManager.GoToState(sender as System.Windows.Controls.Button, "Pressed", true);
        }

        private void Button_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            VisualStateManager.GoToState(sender as System.Windows.Controls.Button, "Normal", true);
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        bool renamingButtonIsSelected = false;
        bool wasTextDeleted = false;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            HiddenTextBox.Focus();
            //if (!renamingButtonIsSelected)
            //{
            //    RenamedName.Background = new SolidColorBrush(Colors.LightBlue);
            //    renamingButtonIsSelected = true;
            //}
            //else
            //{
            //    RenamedName.Background = null;
            //    renamingButtonIsSelected = false;
            //}
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (renamingButtonIsSelected && !wasTextDeleted)
            {
                RenamedName.Text = "";
                
                RenamedName.Text += e.ToString();
                wasTextDeleted = true;
            }
        }

        private void UpdateRenamedName()
        {
            RenamedName.Text = HiddenTextBox.Text + "|";
        }

        private void HiddenTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateRenamedName();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            HiddenTextBox.Focus();
            HiddenTextBox.Text = RenamedName.Text.Substring(0, RenamedName.Text.Length - 1);
            HiddenTextBox.CaretIndex = HiddenTextBox.Text.Length;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(800);
            timer.Tick += TimerTick;
            timer.Start();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            string newText = "";
            if (RenamedName.Text[RenamedName.Text.Length - 1] == '|')
            {
                newText = RenamedName.Text.Substring(0, RenamedName.Text.Length - 1) + " ";
            }
            else
            {
                newText = RenamedName.Text.Substring(0, RenamedName.Text.Length - 1) + "|";
            }
            RenamedName.Text = newText;
        }
    }
}
