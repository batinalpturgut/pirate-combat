using Root.Scripts.Managers.Tick.Abstractions;

namespace Root.Scripts.Utilities.TriggerController.Abstractions
{
    public interface ITriggerController 
    {
        void UpdateTriggerRange(float range);
        void Calculate();
    }
}