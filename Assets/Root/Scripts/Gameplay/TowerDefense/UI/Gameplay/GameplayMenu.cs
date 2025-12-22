using System;
using PrimeTween;
using Root.Scripts.Extensions;
using Root.Scripts.Gameplay.TowerDefense.UI.Gameplay.Inventory;
using Root.Scripts.Managers.Game;
using Root.Scripts.Managers.Grid;
using Root.Scripts.Managers.Hostile;
using Root.Scripts.Managers.Inventory;
using Root.Scripts.Managers.Inventory.Enums;
using Root.Scripts.Managers.Spell;
using Root.Scripts.Managers.UI.Abstractions;
using Root.Scripts.Utilities.Spawner;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Root.Scripts.Gameplay.TowerDefense.UI.Gameplay
{
    public class GameplayMenu : AMenu
    {
        [Header("Resource Bar References:")]
        [SerializeField]
        private TMP_Text soulTMPText;

        [SerializeField]
        private FlyingSoul flyingSoulPrefab;

        [SerializeField]
        private Transform flyingSoulDestination;

        [SerializeField]
        private TMP_Text healthTMPText;

        [Header("\nInventory Grid References:")]
        [SerializeField]
        private InventoryGrid inventoryGrid;

        [SerializeField]
        private Button inventoryPanelToggle;

        [SerializeField]
        private Image borderImage;

        [Header("Inventory Grid Show Animation")]
        [SerializeField]
        private float showAnimationDuration;

        [SerializeField]
        private Ease showAnimationEase;

        [Header("Inventory Grid  Hide Animation")]
        [SerializeField]
        private float hideAnimationDuration;

        [SerializeField]
        private Ease hideAnimationEase;

        private GameManager _gameManager;
        private FlyingSoul _flyingSoul;

        protected override void OnInit()
        {
            inventoryPanelToggle.onClick.AddListener(InventoryPanelToggle);

            inventoryGrid.Initialize(UIManager,
                Context.Resolve<GridManager>(),
                Context.Resolve<InventoryManager>(),
                Context.Resolve<SpellManager>());
            inventoryGrid.SetUIReferences(inventoryPanelToggle, borderImage);
            _gameManager = Context.Resolve<GameManager>();

            SoulSystem.OnSoulChanged += UpdateSoulText;
            HostileManager.OnHealthChanged += UpdateHealthText;
            HostileManager.OnHostileDeath += ActivateFlyingSoul;
            InitCounters();
        }

        private void ActivateFlyingSoul(Vector3 pos, int givenSoul)
        {
            _flyingSoul = Spawner.Spawn<FlyingSoul, Vector2, int>(flyingSoulPrefab.transform,
                _gameManager.TowerDefenceCamera.WorldToScreenPoint(pos), Quaternion.identity,
                flyingSoulDestination.position, givenSoul);

            _flyingSoul.transform.SetParent(transform, true);
            _flyingSoul.PlayFlyAnimation().OnComplete(this,
                self =>
                {
                    self.UpdateSoulText(SoulSystem.TotalSouls);

                    if (!self._flyingSoul.gameObject.IsDestroyed())
                    {
                        Destroy(self._flyingSoul.gameObject);
                    }
                });
        }

        private void UpdateHealthText(int healthValue)
        {
            Tween.CompleteAll(healthTMPText);

            Sequence.Create(
                    Tween.Custom(
                        startValue: int.Parse(healthTMPText.text),
                        endValue: healthValue,
                        duration: 0.5f,
                        ease: Ease.OutQuad,
                        onValueChange: value => { healthTMPText.text = Convert.ToInt32(value).ToString(); }))
                .Group(Tween.PunchScale(
                    target: healthTMPText.transform,
                    strength: Vector3.one * 0.5f,
                    duration: 0.5f,
                    easeBetweenShakes: Ease.OutQuad));
        }

        protected override void OnShow()
        {
            inventoryGrid.SpawnDraggables();
        }

        protected override void OnHide()
        {
            inventoryGrid.DestroyDraggables();
        }

        private void InventoryPanelToggle()
        {
            if (inventoryGrid.gameObject.activeInHierarchy)
            {
                inventoryGrid.PlayHideAnimation(hideAnimationDuration, hideAnimationEase);
            }
            else
            {
                inventoryGrid.PlayShowAnimation(showAnimationDuration, showAnimationEase);
            }
        }

        protected override void OnEscape()
        {
            if (inventoryGrid.gameObject.activeInHierarchy)
            {
                inventoryGrid.PlayHideAnimation(hideAnimationDuration, hideAnimationEase);
            }
        }

        protected override void OnUIManagerDestroy()
        {
            inventoryPanelToggle.onClick.RemoveListener(InventoryPanelToggle);

            SoulSystem.OnSoulChanged -= UpdateSoulText;
        }

        private void InitCounters()
        {
            soulTMPText.text = SoulSystem.TotalSouls.ToString();
        }

        private void UpdateSoulText(int soulValue, SoulChangeReason soulChangeReason)
        {
            if (soulChangeReason == SoulChangeReason.HostileDeath)
            {
                return;
            }
            
            UpdateSoulText(soulValue);
        }
        
        private void UpdateSoulText(int soulValue)
        {
            Tween.CompleteAll(soulTMPText);

            Sequence.Create(
                    Tween.Custom(
                        startValue: int.Parse(soulTMPText.text),
                        endValue: soulValue,
                        duration: 0.5f,
                        ease: Ease.OutQuad,
                        onValueChange: value => { soulTMPText.text = Convert.ToInt32(value).ToString(); }))
                .Group(Tween.PunchScale(
                    target: soulTMPText.transform,
                    strength: Vector3.one * 0.5f,
                    duration: 0.5f,
                    easeBetweenShakes: Ease.OutQuad));
        }
    }
}