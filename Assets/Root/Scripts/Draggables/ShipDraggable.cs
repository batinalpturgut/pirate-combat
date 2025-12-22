using Root.Scripts.Grid;
using Root.Scripts.Utilities;
using Root.Scripts.Utilities.Abstractions;
using UnityEngine;

namespace Root.Scripts.Draggables
{
    public class ShipDraggable : ADraggable, IInitializer<ShipDTO>
    {
        private ShipDTO _shipDto;

        public void Initialize(ShipDTO shipDto)
        {
            _shipDto = shipDto;
            imageReference.sprite = shipDto.ShipSO.PreviewSprite;
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
                GridManager.PlaceShip(_shipDto, nodeObject);
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