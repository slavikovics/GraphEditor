using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace GraphEditor
{
    public partial class RenamingWindow : Window
    {
        public event EventHandler<RenamingEventArgs> RenamingResult;

        private DispatcherTimer timer;

        public RenamingWindow()
        {
            InitializeComponent();
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

        private void UpdateRenamedName()
        {
            RenamedName.Text = HiddenTextBox.Text + "|";
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            HiddenTextBox.Focus();
            HiddenTextBox.Text = GetTextInput();
            HiddenTextBox.CaretIndex = HiddenTextBox.Text.Length;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(800);
            timer.Tick += TimerTick;
            timer.Start();
        }

        private string GetTextInput()
        {
            return RenamedName.Text.Substring(0, RenamedName.Text.Length - 1);
        }

        private void TimerTick(object sender, EventArgs e)
        {
            string newText = "";
            if (RenamedName.Text[RenamedName.Text.Length - 1] == '|')
            {
                newText = GetTextInput() + " ";
            }
            else
            {
                newText = GetTextInput() + "|";
            }
            RenamedName.Text = newText;
        }

        private void ApplyButtonClick(object sender, RoutedEventArgs e)
        {
            RenamingEventArgs renamingEventArgs = new RenamingEventArgs(true, GetTextInput());
            RenamingResult?.Invoke(this, renamingEventArgs);
        }

        private void HiddenTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateRenamedName();
        }

        private void TextInputClick(object sender, RoutedEventArgs e)
        {
            HiddenTextBox.Focus();
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            RenamingEventArgs renamingEventArgs = new RenamingEventArgs(false, "");
            RenamingResult?.Invoke(this, renamingEventArgs);
        }
    }
}
