using NaughtyAttributes;
using UnityEngine;
using PrimeTween;
using Root.Scripts.Extensions;
using Root.Scripts.Utilities;

namespace Root.Scripts.Managers.UI.Abstractions
{
    public abstract class AMenu : MonoBehaviour, IMenuCallbacks, IMenuControls
    {
        [SerializeField, BoxGroup(Consts.InspectorTitles.ANIMATION_SETTINGS)] 
        private bool useCustomTarget;
        
        [SerializeField, ShowIf(nameof(useCustomTarget)), BoxGroup(Consts.InspectorTitles.ANIMATION_SETTINGS)] 
        private Transform customTarget;
        
        [SerializeField, BoxGroup(Consts.InspectorTitles.ANIMATION_SETTINGS)] 
        private bool useEnterAnimation;
        
        [SerializeField, ShowIf(nameof(useEnterAnimation)), BoxGroup(Consts.InspectorTitles.ANIMATION_SETTINGS)] 
        private TweenSettings enterAnimationSettings;
        
        [SerializeField, BoxGroup(Consts.InspectorTitles.ANIMATION_SETTINGS)] 
        private bool useExitAnimation;
        
        [SerializeField, ShowIf(nameof(useExitAnimation)), BoxGroup(Consts.InspectorTitles.ANIMATION_SETTINGS)] 
        private TweenSettings exitAnimationSettings;
        
        public bool IsActive => gameObject.activeInHierarchy;
        protected UIManager UIManager { get; private set; }
        protected AContext Context { get; private set; }
        private bool IsAnimating { get; set; }

        void IMenuControls.InjectDependencies(UIManager uiManager, AContext context)
        {
            UIManager = uiManager;
            Context = context;
        }

        void IMenuControls.InitHide()
        {
            gameObject.SetActive(false);

            if (customTarget == null)
            {
                customTarget = transform;
            }
        }

        void IMenuControls.Show()
        {
            transform.SetAsLastSibling();
            UIManager.FreeArea.SetAsLastSibling();
            gameObject.SetActive(true);

            if (useEnterAnimation)
            {
                PlayEnterAnimation();
            }
        }

        private void PlayEnterAnimation()
        {
            UIManager.SetInteraction(false);
            IsAnimating = true;
            
            Tween.Scale(customTarget, 0.01f, 1f, enterAnimationSettings)
                .OnComplete(this, instance =>
                {
                    instance.UIManager.SetInteraction(true);
                    instance.IsAnimating = false;
                });
        }

        void IMenuControls.Hide()
        {
            if (useExitAnimation)
            {
                PlayExitAnimation();
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        private void PlayExitAnimation()
        {
            Tween.Scale(customTarget, 1f, 0.01f, exitAnimationSettings)
                .OnComplete(this, instance =>
                {
                    instance.gameObject.SetActive(false);
                    instance.transform.localScale = Vector3.one;
                });
        }

        void IMenuCallbacks.OnInitCallback() => OnInit();
        void IMenuCallbacks.OnShowCallback() => OnShow();
        void IMenuCallbacks.OnHideCallback() => 
            UIManager.WaitWhile(this,target => target.IsAnimating, target => target.OnHide());
        void IMenuCallbacks.OnEscapeCallback() => OnEscape();
        void IMenuCallbacks.OnUIManagerDestroyCallback() => OnUIManagerDestroy();

        protected abstract void OnInit();
        protected abstract void OnShow();
        protected abstract void OnHide();
        protected abstract void OnEscape();
        protected abstract void OnUIManagerDestroy();
    }
}