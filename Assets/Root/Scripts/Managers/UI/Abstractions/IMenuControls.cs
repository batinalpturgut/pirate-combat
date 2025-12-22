namespace Root.Scripts.Managers.UI.Abstractions
{
    public interface IMenuControls
    {
        void InitHide();
        void Show();
        void Hide();
        void InjectDependencies(UIManager uiManagerInstance, AContext context);
    }
}