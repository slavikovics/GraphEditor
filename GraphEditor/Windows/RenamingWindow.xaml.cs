using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace GraphEditor
{
    public partial class RenamingWindow : Window
    {
        public event EventHandler<RenamingEventArgs> OnRenamingResult;

        private DispatcherTimer _timer;

        private string _textToEdit;

        public RenamingWindow(string textToEdit)
        {
            InitializeComponent();
            _textToEdit = textToEdit;
        }

        private void OnCollapseWindowButtonClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void OnCloseWindowButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OnButtonMouseEnter(object sender, MouseEventArgs e)
        {
            VisualStateManager.GoToState(sender as System.Windows.Controls.Button, "MouseOver", true);
        }

        private void OnButtonMouseLeave(object sender, MouseEventArgs e)
        {
            VisualStateManager.GoToState(sender as System.Windows.Controls.Button, "Normal", true);
        }

        private void OnButtonMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            VisualStateManager.GoToState(sender as System.Windows.Controls.Button, "Pressed", true);
        }

        private void OnButtonMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            VisualStateManager.GoToState(sender as System.Windows.Controls.Button, "Normal", true);
        }

        private void OnGridMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private bool _renamingButtonIsSelected = false;
        private bool _wasTextDeleted = false;
        private string _renameNamePureText = "";
        private int _renameNamePureCaretIndex = -1;
        private bool _tick = true;

        private void UpdateRenamedName()
        {
            int carretIndex = HiddenTextBox.CaretIndex;

            if (_renameNamePureText != HiddenTextBox.Text)
            {
                RenamedName.Text = HiddenTextBox.Text;
                _renameNamePureText = HiddenTextBox.Text;
                MoveCarret();
            }
            if (carretIndex != _renameNamePureCaretIndex)
            {
                MoveCarret();
                _renameNamePureCaretIndex = carretIndex;
            }
        }

        private void MoveCarret()
        {
            int carretIndex = HiddenTextBox.CaretIndex;
            RenamedName.Text = HiddenTextBox.Text.Insert(carretIndex, "|");
        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            HiddenTextBox.Focus();
            RenamedName.Text = _textToEdit;
            HiddenTextBox.Text = _textToEdit;
            HiddenTextBox.CaretIndex = HiddenTextBox.Text.Length;
            MoveCarret();
            UpdateRenamedName();
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(500);
            _timer.Tick += OnTimerTick;
            _timer.Start();
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            if (_tick == true)
            {
                RenamedName.Text = RenamedName.Text.Remove(HiddenTextBox.CaretIndex, 1);
                RenamedName.Text = RenamedName.Text.Insert(HiddenTextBox.CaretIndex, "|");
                _tick = false;
            }
            else
            {
                RenamedName.Text = RenamedName.Text.Remove(HiddenTextBox.CaretIndex, 1);
                RenamedName.Text = RenamedName.Text.Insert(HiddenTextBox.CaretIndex, " ");
                _tick = true;
            }
        }

        private void OnApplyButtonClick(object sender, RoutedEventArgs e)
        {
            RenamingEventArgs renamingEventArgs = new RenamingEventArgs(true, HiddenTextBox.Text);
            OnRenamingResult?.Invoke(this, renamingEventArgs);
            this.Close();
        }

        private void OnCancelButtonClick(object sender, RoutedEventArgs e)
        {
            RenamingEventArgs renamingEventArgs = new RenamingEventArgs(false, "");
            OnRenamingResult?.Invoke(this, renamingEventArgs);
            this.Close();
        }

        private void OnHiddenTextBoxLayoutUpdated(object sender, EventArgs e)
        {
            UpdateRenamedName();
        }
    }
}
