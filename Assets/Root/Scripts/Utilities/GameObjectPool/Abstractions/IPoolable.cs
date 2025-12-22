namespace Root.Scripts.Utilities.GameObjectPool.Abstractions
{
    public interface IPoolable
    {
        void OnGet();
        void OnRelease();
    }
}