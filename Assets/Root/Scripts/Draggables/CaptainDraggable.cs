using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Abstractions;
using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Ship;
using Root.Scripts.Grid;
using Root.Scripts.Utilities;
using Root.Scripts.Utilities.Abstractions;

namespace Root.Scripts.Draggables
{
    public class CaptainDraggable : ADraggable, IInitializer<ACaptainSO>
    {
        private ACaptainSO _captainSO;
        private ShipBlueprint _ship;

        public void Initialize(ACaptainSO captainSo)
        {
            _captainSO = captainSo;
            imageReference.sprite = captainSo.PreviewSprite;
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
                GridManager.PlaceCaptain(_captainSO, _ship, MousePos.GetPosition());
            }
        }

        public override bool CanPlace(NodeObject nodeObject)
        {
            if (!nodeObject.TryGetDeployable(out ShipBlueprint ship))
            {
                _ship = null;
                return false;
            }

            if (ship.CaptainBlueprint != null)
            {
                return false;
            }

            _ship = ship;
            return true;
        }
    }
}