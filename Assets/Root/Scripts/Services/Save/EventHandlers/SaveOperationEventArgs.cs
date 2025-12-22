using System;
using Root.Scripts.Services.Save.Abstractions.Interfaces;

namespace Root.Scripts.Services.Save.EventHandlers
{
    public class SaveOperationEventArgs : EventArgs
    {
        public IDataRoot DataRoot { get; }
        public bool IsSuccessful { get; }
        public Exception Exception { get; }
        public string ErrorMessage { get; }

        public SaveOperationEventArgs(IDataRoot dataRoot, bool isSuccessful, Exception exception = null,
            string errorMessage = null)
        {
            DataRoot = dataRoot;
            IsSuccessful = isSuccessful;
            Exception = exception;
            ErrorMessage = errorMessage;
        }
    }
}