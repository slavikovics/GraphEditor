namespace GraphEditor.Windows.MainWindow
{
    public class MainWindowStates
    {
        public bool ShouldNodeBeMoved;

        public bool ShouldBeDragged;

        public bool ShouldNodeBeAdded;

        public bool ShouldEdgeBeAdded;

        public bool ShouldBeRemoved;

        public bool ShouldFileBeLoaded;

        public bool ShouldSettingsBeOpened;

        public bool ShouldTaskBeDone;

        public bool ShouldConsoleBeOpened;

        public bool ShouldBeMagicallyAnimated;

        public MainWindowStates() 
        {
            MoveToMovingState();
        }

        public void MoveToMovingState()
        {
            ShouldNodeBeMoved = true;
            ShouldBeDragged = false;
            ShouldNodeBeAdded = false;
            ShouldEdgeBeAdded = false;
            ShouldBeRemoved = false;
            ShouldFileBeLoaded = false;
            ShouldSettingsBeOpened = false;
            ShouldTaskBeDone = false;
            ShouldConsoleBeOpened = false;
            ShouldBeMagicallyAnimated = false;
        }

        public void MoveToDraggingState()
        {
            ShouldNodeBeMoved = false;
            ShouldBeDragged = true;
            ShouldNodeBeAdded = false;
            ShouldEdgeBeAdded = false;
            ShouldBeRemoved = false;
            ShouldFileBeLoaded = false;
            ShouldSettingsBeOpened = false;
            ShouldTaskBeDone = false;
            ShouldConsoleBeOpened = false;
            ShouldBeMagicallyAnimated = false;
        }

        public void MoveToNodeAddingState()
        {
            ShouldNodeBeMoved = false;
            ShouldBeDragged = false;
            ShouldNodeBeAdded = true;
            ShouldEdgeBeAdded = false;
            ShouldBeRemoved = false;
            ShouldFileBeLoaded = false;
            ShouldSettingsBeOpened = false;
            ShouldTaskBeDone = false;
            ShouldConsoleBeOpened = false;
            ShouldBeMagicallyAnimated = false;
        }

        public void MoveToEdgeAddingState()
        {
            ShouldNodeBeMoved = false;
            ShouldBeDragged = false;
            ShouldNodeBeAdded = false;
            ShouldEdgeBeAdded = true;
            ShouldBeRemoved = false;
            ShouldFileBeLoaded = false;
            ShouldSettingsBeOpened = false;
            ShouldTaskBeDone = false;
            ShouldConsoleBeOpened = false;
            ShouldBeMagicallyAnimated = false;
        }

        public void MoveToRemovingState()
        {
            ShouldNodeBeMoved = false;
            ShouldBeDragged = false;
            ShouldNodeBeAdded = false;
            ShouldEdgeBeAdded = false;
            ShouldBeRemoved = true;
            ShouldFileBeLoaded = false;
            ShouldSettingsBeOpened = false;
            ShouldTaskBeDone = false;
            ShouldConsoleBeOpened = false;
            ShouldBeMagicallyAnimated = false;
        }

        public void MoveToFileLoadingState()
        {
            ShouldNodeBeMoved = false;
            ShouldBeDragged = false;
            ShouldNodeBeAdded = false;
            ShouldEdgeBeAdded = false;
            ShouldBeRemoved = false;
            ShouldFileBeLoaded = true;
            ShouldSettingsBeOpened = false;
            ShouldTaskBeDone = false;
            ShouldConsoleBeOpened = false;
            ShouldBeMagicallyAnimated = false;
        }

        public void MoveToSettingsState()
        {
            ShouldNodeBeMoved = false;
            ShouldBeDragged = false;
            ShouldNodeBeAdded = false;
            ShouldEdgeBeAdded = false;
            ShouldBeRemoved = false;
            ShouldFileBeLoaded = false;
            ShouldSettingsBeOpened = true;
            ShouldTaskBeDone = false;
            ShouldConsoleBeOpened = false;
            ShouldBeMagicallyAnimated = false;
        }

        public void MoveToTaskState()
        {
            ShouldNodeBeMoved = false;
            ShouldBeDragged = false;
            ShouldNodeBeAdded = false;
            ShouldEdgeBeAdded = false;
            ShouldBeRemoved = false;
            ShouldFileBeLoaded = false;
            ShouldSettingsBeOpened = false;
            ShouldTaskBeDone = true;
            ShouldConsoleBeOpened = false;
            ShouldBeMagicallyAnimated = false;
        }

        public void MoveToConsoleState()
        {
            ShouldNodeBeMoved = false;
            ShouldBeDragged = false;
            ShouldNodeBeAdded = false;
            ShouldEdgeBeAdded = false;
            ShouldBeRemoved = false;
            ShouldFileBeLoaded = false;
            ShouldSettingsBeOpened = false;
            ShouldTaskBeDone = false;
            ShouldConsoleBeOpened = true;
            ShouldBeMagicallyAnimated = false;
        }

        public void MoveToMagicState()
        {
            ShouldNodeBeMoved = false;
            ShouldBeDragged = false;
            ShouldNodeBeAdded = false;
            ShouldEdgeBeAdded = false;
            ShouldBeRemoved = false;
            ShouldFileBeLoaded = false;
            ShouldSettingsBeOpened = false;
            ShouldTaskBeDone = false;
            ShouldConsoleBeOpened = false;
            ShouldBeMagicallyAnimated = true;
        }

        public void MoveToEmptyState()
        {
            ShouldNodeBeMoved = false;
            ShouldBeDragged = false;
            ShouldNodeBeAdded = false;
            ShouldEdgeBeAdded = false;
            ShouldBeRemoved = false;
            ShouldFileBeLoaded = false;
            ShouldSettingsBeOpened = false;
            ShouldTaskBeDone = false;
            ShouldConsoleBeOpened = false;
            ShouldBeMagicallyAnimated = false;
        }
    }
}
