using Root.Scripts.Gameplay.TowerDefense.Spells;
using Root.Scripts.Grid;
using Root.Scripts.ScriptableObjects.Abstractions;
using Root.Scripts.Utilities;
using Root.Scripts.Utilities.Abstractions;
using UnityEngine;
using UnityEngine.UIElements;

namespace Root.Scripts.Draggables
{
    public class SpellDraggable : ADraggable, IInitializer<SpellBlueprint>
    {
        private ASpellSO _spellSO;

        private SpellBlueprint _spellBlueprint;

        protected override void Place()
        {
            if (!GridManager.TryGetNodePosition(MousePos.GetPosition(), out NodePosition nodePosition))
            {
                return;
            }

            NodeObject nodeObject = GridManager.GetNodeObjectWithNodePosition(nodePosition);

            if (CanPlace(nodeObject))
            {
                GridManager.PlaceSpell(_spellBlueprint, nodePosition);
            }
        }

        public override bool CanPlace(NodeObject nodeObject)
        {
            if (nodeObject.IsEntryPoint)
            {
                return false;
            }

            return true;
        }


        public void Initialize(SpellBlueprint spellBlueprint)
        {
            _spellBlueprint = spellBlueprint;
            _spellSO = spellBlueprint.SpellSO;
            imageReference.sprite = _spellSO.PreviewSprite;
        }
    }
}