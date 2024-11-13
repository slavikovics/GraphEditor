using System;

namespace GraphEditor.GraphsSavingAndLoading
{
    public class ImportEventArgs : EventArgs
    {
        public string GraphName { get; set; }

        public ImportEventArgs(string graphName)
        {
            GraphName = graphName;
        }
    }
}