using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Abstractions;
using Root.Scripts.Grid;
using Root.Scripts.Managers.Grid;
using Root.Scripts.Utilities;
using UnityEngine;

namespace Root.Scripts.Managers
{
    public class Selected : MonoBehaviour
    {
        [SerializeField]
        private GridManager gridManager;
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 worldPos = MousePos.GetPosition();
                
                if (gridManager.TryGetNodePosition(worldPos, out var nodePosition))
                {
                    NodeObject nodeObject = gridManager.GetNodeObjectWithNodePosition(nodePosition);
                    if (nodeObject.TryGetDeployable(out IUpgradable upgradable))
                    {
                        upgradable.Upgrade();
                    }
                }
            }
        }
    }
}