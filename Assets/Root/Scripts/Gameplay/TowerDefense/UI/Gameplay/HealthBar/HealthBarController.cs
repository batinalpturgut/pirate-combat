using PrimeTween;
using Root.Scripts.Extensions;
using Root.Scripts.Gameplay.TowerDefense.Entities.Hostile;
using Root.Scripts.Managers.Tick;
using Root.Scripts.Managers.Tick.Abstractions;
using Root.Scripts.Utilities.Abstractions;
using UnityEngine;
using UnityEngine.UI;

namespace Root.Scripts.Gameplay.TowerDefense.UI.Gameplay.HealthBar
{
    public class HealthBarController : MonoBehaviour, IInitializer<HostileBlueprint, TickManager>, IStandardTickable
    {
        [Header("References"), SerializeField] 
        private Canvas healthBarCanvas;

        [SerializeField] 
        private Slider healthBarSlider;

        [SerializeField] 
        private Image healthBarColor;

        private HostileBlueprint _hostileBlueprint;
        private TickManager _tickManager;
        private Camera _mainCamera;

        [Header("Settings"), SerializeField]
        private Color[] healthBarLevelColors;
        
        public int ExecutionOrder => 0;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void OnEnable()
        {
            this.WaitWhile(this, controller => controller._tickManager == null,
                controller =>
                {
                    controller._tickManager.Register(this);
                });
        }

        void IInitializer<HostileBlueprint, TickManager>.Initialize(HostileBlueprint hostileBlueprint,
            TickManager tickManager)
        {
            _hostileBlueprint = hostileBlueprint;
            _tickManager = tickManager;
        }

        private void Start()
        {
            transform.rotation = Quaternion.LookRotation(_mainCamera.transform.forward);
            healthBarCanvas.worldCamera = _mainCamera;
            _hostileBlueprint.OnHealthChanged += UpdateHealthBar;
        }

        private void UpdateHealthBar(float value)
        {
            int selectedColor = Mathf.FloorToInt(
                value.Remap(0, 100, 0, healthBarLevelColors.Length));
            healthBarColor.color = healthBarLevelColors[selectedColor];
            Tween.UISliderValue(healthBarSlider, 
                value.Remap(0, 100, 0, 1), 0.5f, Ease.OutBack);
        }

        private void OnDisable()
        {
            _tickManager.Unregister(this);
        }

        private void OnDestroy()
        {
            _hostileBlueprint.OnHealthChanged -= UpdateHealthBar;
        }
        
        void IStandardTickable.LateTick()
        {
            transform.rotation = Quaternion.LookRotation(_mainCamera.transform.forward);
        }
        
        void IStandardTickable.Tick() { }

        void IStandardTickable.FixedTick() { }
    }
}