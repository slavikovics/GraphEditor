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

        string _textToEdit;

        public RenamingWindow(string textToEdit)
        {
            InitializeComponent();
            _textToEdit = textToEdit;
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
        string renameNamePureText = "";
        int renameNamePureCaretIndex = -1;
        bool tick = true;

        private void UpdateRenamedName()
        {
            int carretIndex = HiddenTextBox.CaretIndex;

            if (renameNamePureText != HiddenTextBox.Text)
            {
                Console.WriteLine(carretIndex);
                RenamedName.Text = HiddenTextBox.Text;
                renameNamePureText = HiddenTextBox.Text;
                MoveCarret();
            }
            if (carretIndex != renameNamePureCaretIndex)
            {
                MoveCarret();
                renameNamePureCaretIndex = carretIndex;
            }
        }

        private void MoveCarret()
        {
            int carretIndex = HiddenTextBox.CaretIndex;
            RenamedName.Text = HiddenTextBox.Text.Insert(carretIndex, "|");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            HiddenTextBox.Focus();
            RenamedName.Text = _textToEdit;
            HiddenTextBox.Text = _textToEdit;
            HiddenTextBox.CaretIndex = HiddenTextBox.Text.Length;
            MoveCarret();
            UpdateRenamedName();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += TimerTick;
            timer.Start();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            if (tick == true)
            {
                RenamedName.Text = RenamedName.Text.Remove(HiddenTextBox.CaretIndex, 1);
                RenamedName.Text = RenamedName.Text.Insert(HiddenTextBox.CaretIndex, "|");
                tick = false;
            }
            else
            {
                RenamedName.Text = RenamedName.Text.Remove(HiddenTextBox.CaretIndex, 1);
                RenamedName.Text = RenamedName.Text.Insert(HiddenTextBox.CaretIndex, " ");
                tick = true;
            }
        }

        private void ApplyButtonClick(object sender, RoutedEventArgs e)
        {
            RenamingEventArgs renamingEventArgs = new RenamingEventArgs(true, HiddenTextBox.Text);
            RenamingResult?.Invoke(this, renamingEventArgs);
            this.Close();
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            RenamingEventArgs renamingEventArgs = new RenamingEventArgs(false, "");
            RenamingResult?.Invoke(this, renamingEventArgs);
            this.Close();
        }

        private void HiddenTextBox_LayoutUpdated(object sender, EventArgs e)
        {
            UpdateRenamedName();
        }
    }
}
