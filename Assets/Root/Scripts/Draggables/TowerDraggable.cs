using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Tower.Abstractions;
using Root.Scripts.Grid;
using Root.Scripts.Utilities;
using Root.Scripts.Utilities.Abstractions;
using UnityEngine;

namespace Root.Scripts.Draggables
{
    public class TowerDraggable : ADraggable, IInitializer<ATowerSO>
    {
        private ATowerSO _towerSO;
        
        public void Initialize(ATowerSO towerSO)
        {
            _towerSO = towerSO;
            imageReference.sprite = towerSO.PreviewSprite;
        }

        protected override void Place()
        {
            if (!GridManager.TryGetNodePosition(MousePos.GetPosition(), out NodePosition nodePosition))
            {
                return;
            }
            
            NodeObject nodeObject = GridManager.GetNodeObjectWithNodePosition(nodePosition);
            
            if (CanPlace(nodeObject))
            {
                GridManager.PlaceTower(_towerSO, nodeObject);
            }
        }

        public override bool CanPlace(NodeObject nodeObject)
        {
            if (nodeObject.IsHostilePath || nodeObject.HasDeployable)
            {
                return false;
            }

            return true;
        }
    }
}