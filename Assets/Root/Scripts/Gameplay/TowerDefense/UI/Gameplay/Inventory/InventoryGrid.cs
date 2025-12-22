using System.Collections.Generic;
using PrimeTween;
using Root.Scripts.Draggables;
using Root.Scripts.Extensions;
using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Abstractions;
using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Tower.Abstractions;
using Root.Scripts.Gameplay.TowerDefense.Spells;
using Root.Scripts.Managers.Grid;
using Root.Scripts.Managers.Inventory;
using Root.Scripts.Managers.Spell;
using Root.Scripts.Managers.UI;
using Root.Scripts.Utilities.Abstractions;
using Root.Scripts.Utilities.Spawner;
using UnityEngine;
using UnityEngine.UI;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Root.Scripts.Gameplay.TowerDefense.UI.Gameplay.Inventory
{
    public class InventoryGrid : MonoBehaviour, IInitializer<UIManager, GridManager, InventoryManager, SpellManager>
    {
        [SerializeField]
        private ShipDraggable shipDraggablePrototype;

        [SerializeField]
        private TowerDraggable towerDraggablePrototype;

        [SerializeField]
        private CaptainDraggable captainDraggablePrototype;

        [SerializeField]
        private SpellDraggable spellDraggablePrototype;
        
        private UIManager _uiManager;
        private GridManager _gridManager;
        private InventoryManager _inventoryManager;
        private SpellManager _spellManager;

        private Button _inventoryPanelToggle;
        private Vector2 _inventoryPanelToggleInitialPosition;
        private Image _gridBackground;
        private Image _borderImage;
        
        private readonly List<ADraggable> _activeDraggables = new List<ADraggable>();

        public void Initialize(UIManager uiManager, GridManager gridManager, InventoryManager inventoryManager, 
            SpellManager spellManager)
        {
            _uiManager = uiManager;
            _gridManager = gridManager;
            _inventoryManager = inventoryManager;
            _spellManager = spellManager;
        }

        private void Start()
        {
            _gridBackground = GetComponent<Image>();
        }

        public void SetUIReferences(Button inventoryPanelToggle, Image borderImage)
        {
            _inventoryPanelToggle = inventoryPanelToggle;
            _borderImage = borderImage;
            _inventoryPanelToggleInitialPosition = ((RectTransform)_inventoryPanelToggle.transform).anchoredPosition;
        }

        public void SpawnDraggables()
        {
            foreach (ShipDTO shipDTO in _inventoryManager.ShipDTOs)
            {
                ShipDraggable shipDraggable = Spawner.Spawn<ShipDraggable, GridManager, UIManager, InventoryManager>(
                    shipDraggablePrototype.transform,
                    Vector3.zero,
                    Quaternion.identity,
                    _gridManager,
                    _uiManager, _inventoryManager
                );

                _activeDraggables.Add(shipDraggable);
                shipDraggable.Initialize(shipDTO);
                shipDraggable.transform.SetParent(transform, false);
            }

            foreach (ATowerSO towerSO in _inventoryManager.towerSOReferences)
            {
                TowerDraggable towerDraggable = Spawner.Spawn<TowerDraggable, GridManager, UIManager, InventoryManager>(
                    towerDraggablePrototype.transform,
                    Vector3.zero,
                    Quaternion.identity,
                    _gridManager,
                    _uiManager, _inventoryManager 
                );

                _activeDraggables.Add(towerDraggable);
                towerDraggable.Initialize(towerSO);
                towerDraggable.transform.SetParent(transform, false);
            }

            foreach (ACaptainSO captainSO in _inventoryManager.captainSOReferences)
            {
                CaptainDraggable captainDraggable =
                    Spawner.Spawn<CaptainDraggable, GridManager, UIManager, InventoryManager>(
                        captainDraggablePrototype.transform, Vector3.zero, Quaternion.identity, _gridManager, _uiManager,
                        _inventoryManager 
                    );

                _activeDraggables.Add(captainDraggable);
                captainDraggable.Initialize(captainSO);
                captainDraggable.transform.SetParent(transform, false);
            }

            this.WaitUntil(this, self => self._spellManager.IsReady, self =>
            {
                foreach (SpellBlueprint spell in self._spellManager.InGameSpellList)
                {
                    SpellDraggable spellDraggable = Spawner.Spawn<SpellDraggable, GridManager, UIManager, InventoryManager>(
                        self.spellDraggablePrototype.transform, Vector3.zero, Quaternion.identity, self._gridManager,
                        self._uiManager, self._inventoryManager
                    );

                    self._activeDraggables.Add(spellDraggable);
                    spellDraggable.Initialize(spell);
                    spellDraggable.transform.SetParent(transform, false);
                }
            });
        }
        
        public void PlayShowAnimation(float showAnimationDuration, Ease showAnimationEase)
        {
            _uiManager.SetInteraction(false);
            transform.parent.gameObject.SetActive(true);
            Sequence
                .Create(Tween.UIAnchoredPositionY((RectTransform)transform.parent,
                    startValue: -((RectTransform)transform.parent).rect.height,
                    endValue: 0f,
                    duration: showAnimationDuration,
                    ease: showAnimationEase))
                .Group(Tween.UIAnchoredPositionY((RectTransform)_inventoryPanelToggle.transform,
                    startValue: 5f,
                    endValue: _inventoryPanelToggleInitialPosition.y,
                    duration: showAnimationDuration,
                    ease: showAnimationEase))
                .Group(Tween.Alpha(_gridBackground,
                    endValue: 1f,
                    duration: showAnimationDuration,
                    ease: showAnimationEase))
                .Group(Tween.Alpha(_borderImage,
                    endValue: 1f,
                    duration: showAnimationDuration,
                    ease: showAnimationEase))
                .ChainCallback(this,
                    target => target._uiManager.SetInteraction(true));
        }

        public void PlayHideAnimation(float hideAnimationDuration, Ease hideAnimationEase)
        {
            _uiManager.SetInteraction(false);
            Sequence
                .Create(Tween.UIAnchoredPositionY((RectTransform)transform.parent,
                    startValue: 0f,
                    endValue: -((RectTransform)transform.parent).rect.height,
                    duration: hideAnimationDuration,
                    ease: hideAnimationEase))
                .Group(Tween.UIAnchoredPositionY((RectTransform)_inventoryPanelToggle.transform,
                    startValue: _inventoryPanelToggleInitialPosition.y,
                    endValue: 5f,
                    duration: hideAnimationDuration,
                    ease: hideAnimationEase))
                .Group(Tween.Alpha(_gridBackground,
                    endValue: 0f,
                    duration: hideAnimationDuration,
                    ease: hideAnimationEase))
                .Group(Tween.Alpha(_borderImage,
                    endValue: 0f,
                    duration: hideAnimationDuration,
                    ease: hideAnimationEase))
                .ChainCallback(this, target =>
                {
                    target.transform.parent.gameObject.SetActive(false);
                    target._uiManager.SetInteraction(true);
                });
        }

        public void DestroyDraggables()
        {
            foreach (ADraggable draggable in _activeDraggables)
            {
                Destroy(draggable.gameObject);
            }

            _activeDraggables.Clear();
        }
    }
}