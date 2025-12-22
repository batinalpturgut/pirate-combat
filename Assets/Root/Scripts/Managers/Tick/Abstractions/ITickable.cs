namespace Root.Scripts.Managers.Tick.Abstractions
{
    public interface ITickable
    {
        int ExecutionOrder { get;  }
    }
}