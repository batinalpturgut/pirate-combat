namespace Root.Scripts.Managers.Tick.Abstractions
{
    public interface IRealtimeTickable : ITickable
    {
        void RealtimeTick();
    }
}