namespace GraphEditor.Windows.MainWindow
{
    public class MainWindowStates
    {
        public bool shouldNodeBeMoved;

        public bool shouldBeDragged;

        public bool shouldNodeBeAdded;

        public bool shouldEdgeBeAdded;

        public bool shouldBeRemoved;

        public bool shouldFileBeLoaded;

        public bool shouldSettingsBeOpened;

        public bool shouldTaskBeDone;

        public bool shouldConsoleBeOpened;

        public bool shouldBeMagicallyAnimated;

        public MainWindowStates() 
        {
            MoveToMovingState();
        }

        public void MoveToMovingState()
        {
            shouldNodeBeMoved = true;
            shouldBeDragged = false;
            shouldNodeBeAdded = false;
            shouldEdgeBeAdded = false;
            shouldBeRemoved = false;
            shouldFileBeLoaded = false;
            shouldSettingsBeOpened = false;
            shouldTaskBeDone = false;
            shouldConsoleBeOpened = false;
            shouldBeMagicallyAnimated = false;
        }

        public void MoveToDraggingState()
        {
            shouldNodeBeMoved = false;
            shouldBeDragged = true;
            shouldNodeBeAdded = false;
            shouldEdgeBeAdded = false;
            shouldBeRemoved = false;
            shouldFileBeLoaded = false;
            shouldSettingsBeOpened = false;
            shouldTaskBeDone = false;
            shouldConsoleBeOpened = false;
            shouldBeMagicallyAnimated = false;
        }

        public void MoveToNodeAddingState()
        {
            shouldNodeBeMoved = false;
            shouldBeDragged = false;
            shouldNodeBeAdded = true;
            shouldEdgeBeAdded = false;
            shouldBeRemoved = false;
            shouldFileBeLoaded = false;
            shouldSettingsBeOpened = false;
            shouldTaskBeDone = false;
            shouldConsoleBeOpened = false;
            shouldBeMagicallyAnimated = false;
        }

        public void MoveToEdgeAddingState()
        {
            shouldNodeBeMoved = false;
            shouldBeDragged = false;
            shouldNodeBeAdded = false;
            shouldEdgeBeAdded = true;
            shouldBeRemoved = false;
            shouldFileBeLoaded = false;
            shouldSettingsBeOpened = false;
            shouldTaskBeDone = false;
            shouldConsoleBeOpened = false;
            shouldBeMagicallyAnimated = false;
        }

        public void MoveToRemovingState()
        {
            shouldNodeBeMoved = false;
            shouldBeDragged = false;
            shouldNodeBeAdded = false;
            shouldEdgeBeAdded = false;
            shouldBeRemoved = true;
            shouldFileBeLoaded = false;
            shouldSettingsBeOpened = false;
            shouldTaskBeDone = false;
            shouldConsoleBeOpened = false;
            shouldBeMagicallyAnimated = false;
        }

        public void MoveToFileLoadingState()
        {
            shouldNodeBeMoved = false;
            shouldBeDragged = false;
            shouldNodeBeAdded = false;
            shouldEdgeBeAdded = false;
            shouldBeRemoved = false;
            shouldFileBeLoaded = true;
            shouldSettingsBeOpened = false;
            shouldTaskBeDone = false;
            shouldConsoleBeOpened = false;
            shouldBeMagicallyAnimated = false;
        }

        public void MoveToSettingsState()
        {
            shouldNodeBeMoved = false;
            shouldBeDragged = false;
            shouldNodeBeAdded = false;
            shouldEdgeBeAdded = false;
            shouldBeRemoved = false;
            shouldFileBeLoaded = false;
            shouldSettingsBeOpened = true;
            shouldTaskBeDone = false;
            shouldConsoleBeOpened = false;
            shouldBeMagicallyAnimated = false;
        }

        public void MoveToTaskState()
        {
            shouldNodeBeMoved = false;
            shouldBeDragged = false;
            shouldNodeBeAdded = false;
            shouldEdgeBeAdded = false;
            shouldBeRemoved = false;
            shouldFileBeLoaded = false;
            shouldSettingsBeOpened = false;
            shouldTaskBeDone = true;
            shouldConsoleBeOpened = false;
            shouldBeMagicallyAnimated = false;
        }

        public void MoveToConsoleState()
        {
            shouldNodeBeMoved = false;
            shouldBeDragged = false;
            shouldNodeBeAdded = false;
            shouldEdgeBeAdded = false;
            shouldBeRemoved = false;
            shouldFileBeLoaded = false;
            shouldSettingsBeOpened = false;
            shouldTaskBeDone = false;
            shouldConsoleBeOpened = true;
            shouldBeMagicallyAnimated = false;
        }

        public void MoveToMagicState()
        {
            shouldNodeBeMoved = false;
            shouldBeDragged = false;
            shouldNodeBeAdded = false;
            shouldEdgeBeAdded = false;
            shouldBeRemoved = false;
            shouldFileBeLoaded = false;
            shouldSettingsBeOpened = false;
            shouldTaskBeDone = false;
            shouldConsoleBeOpened = false;
            shouldBeMagicallyAnimated = true;
        }

        public void MoveToEmptyState()
        {
            shouldNodeBeMoved = false;
            shouldBeDragged = false;
            shouldNodeBeAdded = false;
            shouldEdgeBeAdded = false;
            shouldBeRemoved = false;
            shouldFileBeLoaded = false;
            shouldSettingsBeOpened = false;
            shouldTaskBeDone = false;
            shouldConsoleBeOpened = false;
            shouldBeMagicallyAnimated = false;
        }
    }
}
