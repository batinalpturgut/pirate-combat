namespace Root.Scripts.Managers.UI.Abstractions
{
    public interface IMenuCallbacks
    {
        void OnInitCallback();
        void OnShowCallback();
        void OnHideCallback();
        void OnEscapeCallback();
        void OnUIManagerDestroyCallback();
    }
}