namespace Root.Scripts.Managers.Tick.Abstractions
{
    public interface IStandardTickable : ITickable
    {
        void Tick();
        void FixedTick();
        void LateTick();
    }
}