using Root.Scripts.Gameplay.TowerDefense.Entities.Hostile;
using Root.Scripts.Grid;
using Root.Scripts.Managers.Grid;
using Root.Scripts.Managers.Hostile;
using Root.Scripts.ScriptableObjects.Abstractions;
using UnityEngine;

namespace Root.Scripts.Gameplay.TowerDefense.Spells.SpellSos
{
    [CreateAssetMenu(fileName = "PortalSpell", menuName = "Spells/PortalSpell")]
    public class PortalSpellSO : ASpellSO
    {
        public override void Apply(NodePosition appliedNode, HostileManager hostileManager, GridManager gridManager) 
        {
            if (gridManager.TryGetHostileWithNodePosition(appliedNode, out HostileBlueprint hostile))
            {
                hostile.ApplySpell.PortalEffect();
            }
        }

        public override string ToString()
        {
            return "Portal Spell";
        }
    }
}