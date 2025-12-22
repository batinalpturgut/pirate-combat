using System.Collections.Generic;
using Root.Scripts.Factories;
using Root.Scripts.Gameplay.TowerDefense.Spells;
using Root.Scripts.Grid;
using Root.Scripts.Managers.Grid;
using Root.Scripts.Managers.Hostile;
using Root.Scripts.Managers.Tick;
using Root.Scripts.Managers.Tick.Abstractions;
using Root.Scripts.ScriptableObjects.Abstractions;
using Root.Scripts.Utilities.Abstractions;
using UnityEngine;
using UnityEngine.Serialization;

namespace Root.Scripts.Managers.Spell
{
    // SpellManager, tamamen InventoryManager'a tasinabilir. 
    public class SpellManager : MonoBehaviour, IInitializer<AContext>, IStandardTickable
    {
        [field: SerializeField] 
        public ASpellSO[] SpellSOList { get; private set; }

        [FormerlySerializedAs("spellBlueprint")]
        [SerializeField]
        private SpellBlueprint spellBlueprintBlueprint;
        public List<SpellBlueprint> InGameSpellList { get; private set; } = new List<SpellBlueprint>();
        public bool IsReady { get; private set; }
        private GridManager _gridManager;
        private HostileManager _hostileManager;
        private TickManager _tickManager;
        public int ExecutionOrder { get; }

        public void Initialize(AContext context)
        {
            _tickManager = context.Resolve<TickManager>();
            _gridManager = context.Resolve<GridManager>();
            _hostileManager = context.Resolve<HostileManager>();
        }

        public void Start()
        {
            for (int i = 0; i < SpellSOList.Length; i++)
            {
                InGameSpellList.Add(SpellFactory.CreateAndInitialize(spellBlueprintBlueprint.transform, SpellSOList[i],
                    _tickManager,
                    _hostileManager, _gridManager));
            }

            IsReady = true;
        }

        public void CastSpell(SpellBlueprint spellBlueprint, NodePosition nodePosition)
        {
            spellBlueprint.TryActivate(nodePosition);
        }

        public void Tick()
        {
        }

        public void FixedTick()
        {
        }

        public void LateTick()
        {
        }
    }
}