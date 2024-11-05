using System;

namespace GraphEditor
{
    public class RenamingEventArgs : EventArgs
    {
        public bool WasRenamed {  get; set; }
        public string NewName { get; set; }

        public RenamingEventArgs(bool wasRenamed, string newName)
        {
            WasRenamed = wasRenamed;
            NewName = newName;
        }
    }
}
