namespace Root.Scripts.Services.Save.Abstractions.Interfaces
{
    public interface IDataRoot
    { 
        string FileName { get; }
        bool IsCloudData { get; }
    }
}