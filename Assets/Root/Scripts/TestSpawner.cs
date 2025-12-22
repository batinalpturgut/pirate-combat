using Root.Scripts.Gameplay.TowerDefense.Entities.Hostile;
using Root.Scripts.Grid;
using Root.Scripts.Managers.Grid;
using Root.Scripts.Utilities.Logger;
using Root.Scripts.Utilities.TriggerController;
using UnityEngine;

namespace Root.Scripts
{
    public class TestSpawner : MonoBehaviour
    {
        [SerializeField]
        private GridManager gridManager;

        private GridTriggerController<TestSpawner, HostileBlueprint> _gridTriggerController;

        private Vector3 _offset = new Vector3(8, 0, 2);

        private void Start()
        {
            NodePosition node = new NodePosition(4, 1);
            _gridTriggerController = new GridTriggerController<TestSpawner, HostileBlueprint>(this, node, 2, gridManager,
                (spawner, enemy) => { Log.Console("Enter"); },
                (spawner, enemy) => Log.Console($"Exit {enemy}"),
                (spawner, enemy) =>
                {
                    Debug.DrawRay(spawner._offset, enemy.transform.position - spawner._offset); 
                });
        }

        private void Update()
        {
            _gridTriggerController.Calculate();
        }
    }
}