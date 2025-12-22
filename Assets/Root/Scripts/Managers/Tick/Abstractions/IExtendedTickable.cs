namespace Root.Scripts.Managers.Tick.Abstractions
{
    public interface IExtendedTickable : IStandardTickable
    {
        void BeforeTick();
        void AfterTick();
    }
}