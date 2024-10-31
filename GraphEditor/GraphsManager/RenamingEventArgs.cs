using System;

namespace GraphEditor
{
    public class RenamingEventArgs : EventArgs
    {
        public bool _wasRenamed {  get; set; }
        public string _newName { get; set; }

        public RenamingEventArgs(bool wasRenamed, string newName)
        {
            _wasRenamed = wasRenamed;
            _newName = newName;
        }
    }
}
