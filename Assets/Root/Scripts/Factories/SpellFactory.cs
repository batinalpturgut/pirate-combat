using System;
using Root.Scripts.Gameplay.TowerDefense.Spells;
using Root.Scripts.Managers.Grid;
using Root.Scripts.Managers.Hostile;
using Root.Scripts.Managers.Tick;
using Root.Scripts.ScriptableObjects.Abstractions;
using Root.Scripts.Utilities;
using Root.Scripts.Utilities.Spawner;
using UnityEngine;

namespace Root.Scripts.Factories
{
    public static class SpellFactory
    {
        public static SpellBlueprint CreateAndInitialize(Transform blueprint, ASpellSO spellSO, TickManager tickManager,
            HostileManager hostileManager, GridManager gridManager)
        {
            SpellBlueprint spawnedSpellBlueprint =
                Spawner.Spawn<SpellBlueprint, TickManager, HostileManager, GridManager>(blueprint, Vector3.zero, Quaternion.identity,
                    tickManager, hostileManager, gridManager);
            spawnedSpellBlueprint.Initialize(spellSO);
            return spawnedSpellBlueprint;
        }
    }
}