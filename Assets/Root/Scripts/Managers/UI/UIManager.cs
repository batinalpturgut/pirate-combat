using System;
using System.Collections.Generic;
using Root.Scripts.Extensions;
using Root.Scripts.Managers.Tick;
using Root.Scripts.Managers.Tick.Abstractions;
using Root.Scripts.Managers.UI.Abstractions;
using Root.Scripts.Managers.UI.Extensions;
using Root.Scripts.Utilities.Abstractions;
using Root.Scripts.Utilities.Guards;
using Root.Scripts.Utilities.Logger;
using UnityEngine;
using LogType = Root.Scripts.Utilities.Logger.Enums.LogType;

namespace Root.Scripts.Managers.UI
{
    public class UIManager : MonoBehaviour, IRealtimeTickable, IInitializer<AContext>
    {
        [field: SerializeField]
        public AMenu StartMenu { get; private set; }

        [SerializeField]
        private AMenu[] menuReferences;

        [SerializeField]
        private CanvasGroup[] canvasGroups;

        public int ExecutionOrder => 1;
        public AMenu CurrentMenu { get; private set; }
        public AMenu PreviousMenu { get; private set; }
        public bool IsUILocked { get; private set; }
        public RectTransform FreeArea { get; private set; }

        private AContext _context;
        private TickManager _tickManager;

        public bool IsUIVisible
        {
            get
            {
                foreach (CanvasGroup canvasGroup in canvasGroups)
                {
                    if (canvasGroup.alpha == 0f)
                    {
                        return false;
                    }
                }

                return true;
            }
        }
        
        // TODO: Dictionary serilestiren plug-in kurulabilir.
        private readonly Dictionary<Type, AMenu> _menuCache = new Dictionary<Type, AMenu>();
        private readonly Stack<AMenu> _menuHistory = new Stack<AMenu>();
        private readonly Stack<AMenu> _popupOrder = new Stack<AMenu>();
        private bool _skipAutoPause;

        private void Awake()
        {
            CreateFreeArea();
            foreach (AMenu menu in menuReferences)
            {
                ((IMenuControls)menu).InitHide();
            }
        }

        public void Initialize(AContext context)
        {
            _context = context;
            _tickManager = context.Resolve<TickManager>();
        }

        private void Start()
        {
            _tickManager.Register(this);
            foreach (AMenu menu in menuReferences)
            {
                ((IMenuControls)menu)
                    .InjectDependencies(this, _context);
                try
                {
                    ((IMenuCallbacks)menu).OnInitCallback();
                }
                catch (Exception ex)
                {
                    throw new Exception($"[{nameof(UIManager)}] {menu.GetType()} Error in OnInit callback:\n {ex}");
                }
            }

            Guard.Against.Null(StartMenu, nameof(StartMenu));

            ShowMenu(StartMenu);
        }

        void IRealtimeTickable.RealtimeTick()
        {
            if (IsUILocked)
            {
                return;
            }

            if (_popupOrder.Count > 0 && Input.GetKeyUp(KeyCode.Escape))
            {
                ((IMenuCallbacks)_popupOrder.Peek()).OnEscapeCallback();
                return;
            }

            if (CurrentMenu is IMenuCallbacks menuCallbacks &&
                Input.GetKeyUp(KeyCode.Escape))
            {
                menuCallbacks.OnEscapeCallback();
            }
        }

        private void OnDestroy()
        {
            foreach (AMenu menu in menuReferences)
            {
                if (menu is IMenuCallbacks menuCallbacks)
                {
                    menuCallbacks.OnUIManagerDestroyCallback();
                }
            }

            _tickManager.Unregister(this);
        }

        private void OnApplicationPause(bool paused)
        {
            if (_skipAutoPause)
            {
                _skipAutoPause = false;
                return;
            }

            if (!paused)
            {
                return;
            }

            foreach (AMenu menu in menuReferences)
            {
                if (menu is IPauseCallback pauseCallback && menu.IsActive)
                {
                    pauseCallback.OnPauseClick();
                }
            }
        }

        public void SkipAutoPause() => _skipAutoPause = true;

        public void SetUIVisibility(bool isVisible)
        {
            foreach (CanvasGroup canvasGroup in canvasGroups)
            {
                canvasGroup.alpha = isVisible ? 1f : 0f;
            }
        }

        public T GetMenu<T>() where T : AMenu
        {
            if (_menuCache.TryGetValue(typeof(T), out AMenu cachedMenu))
            {
                if (cachedMenu != null)
                {
                    return (T)cachedMenu;
                }

                _menuCache.Remove(typeof(T));
            }

            foreach (AMenu menu in menuReferences)
            {
                if (menu is not T tMenu)
                {
                    continue;
                }

                _menuCache[typeof(T)] = tMenu;
                return tMenu;
            }

            Log.Console($"[{nameof(UIManager)}] {typeof(T)} menu not found!", LogType.Warning);
            return default;
        }

        public void ShowMenu<T>(bool saveToHistory = true, bool hideAllPopups = true) where T : AMenu
        {
            AMenu menu = GetMenu<T>();
            ShowMenu(menu, saveToHistory);
        }

        public void ShowMenu(AMenu menu, bool saveToHistory = true)
        {
            if (CurrentMenu == menu)
            {
                Log.Console($"[{nameof(UIManager)}] {menu.GetType()} menu was already open!", LogType.Warning);
                return;
            }

            if (saveToHistory && CurrentMenu != null)
            {
                _menuHistory.Push(CurrentMenu);
            }

            HideAllPopups();

            PreviousMenu = CurrentMenu;

            if (PreviousMenu is IMenuControls previousMenuControls)
            {
                previousMenuControls.Hide();
            }

            CurrentMenu = menu;

            ((IMenuControls)CurrentMenu).Show();

            ((IMenuCallbacks)CurrentMenu).OnShowCallback();
            if (PreviousMenu is IMenuCallbacks previousMenuCallbacks)
            {
                previousMenuCallbacks.OnHideCallback();
            }
        }

        public void HideMenu<T>(bool removeFromHistory = true) where T : AMenu
        {
            AMenu menuToHide = GetMenu<T>();

            if (!menuToHide.IsActive)
            {
                Log.Console($"[{nameof(UIManager)}] Attempted to hide an already hidden menu!", LogType.Warning);
            }

            if (CurrentMenu == menuToHide)
            {
                Log.Console($"[{nameof(UIManager)}] Attempted to hide the top menu. Calling GoBack().");
                GoBack();
                return;
            }

            ((IMenuControls)menuToHide).Hide();
            ((IMenuCallbacks)menuToHide).OnHideCallback();

            if (removeFromHistory)
            {
                RemoveFromMenuHistory<T>();
            }
        }

        public void ShowPopup<T>() where T : AMenu
        {
            AMenu menu = GetMenu<T>();
            ShowPopup(menu);
        }

        public void HidePopup<T>() where T : AMenu
        {
            AMenu menu = GetMenu<T>();
            HidePopup(menu);
        }

        public void ShowPopup(AMenu menu)
        {
            if (menu == null)
            {
                Log.Console($"[{nameof(UIManager)}] popup (Menu) is cannot null!", LogType.Warning);
                return;
            }

            if (Array.IndexOf(menuReferences, menu) < 0)
            {
                Log.Console($"[{nameof(UIManager)}] popup (Menu) is not found in current UIController!",
                    LogType.Warning);
                return;
            }

            if (menu.IsActive)
            {
                Log.Console($"[{nameof(UIManager)}] {menu.GetType()} popup (Menu) was already open!", LogType.Warning);
                return;
            }

            _popupOrder.Push(menu);
            ((IMenuControls)menu).Show();
            ((IMenuCallbacks)menu).OnShowCallback();
        }

        public void HidePopup(AMenu menu)
        {
            if (menu == null)
            {
                Log.Console($"[{nameof(UIManager)}] popup (Menu) is cannot null!", LogType.Warning);
                return;
            }

            if (Array.IndexOf(menuReferences, menu) < 0)
            {
                Log.Console($"[{nameof(UIManager)}] popup (Menu) is not found in current UIController!",
                    LogType.Warning);
                return;
            }

            if (!menu.IsActive)
            {
                Log.Console($"[{nameof(UIManager)}] Attempted to hide an already hidden menu!", LogType.Warning);
                return;
            }

            _popupOrder.Remove(popup => popup == menu);
            ((IMenuControls)menu).Hide();
            ((IMenuCallbacks)menu).OnHideCallback();
        }

        public void HideAllPopups()
        {
            while (_popupOrder.Count > 0)
            {
                HidePopup(_popupOrder.Pop());
            }
        }

        public void RemoveFromMenuHistory<T>() where T : AMenu
        {
            if (!_menuHistory.Contains(GetMenu<T>()))
            {
                Log.Console($"[{nameof(UIManager)}] {typeof(T)} menu is not saved in history!", LogType.Warning);
                return;
            }

            _menuHistory.Remove(menu => menu is T);
        }

        public void ClearMenuHistory()
        {
            _menuHistory.Clear();
        }

        public void GoBack()
        {
            if (_menuHistory.Count <= 0)
            {
                Log.Console($"[{nameof(UIManager)}] There is no menu that can be shown in history!", LogType.Warning);
                return;
            }

            ShowMenu(_menuHistory.Pop(), false);
        }

        public void SetInteraction(bool interactionState)
        {
            foreach (CanvasGroup canvasGroup in canvasGroups)
            {
                canvasGroup.blocksRaycasts = interactionState;
            }

            IsUILocked = !interactionState;
        }

        private void CreateFreeArea()
        {
            foreach (CanvasGroup canvasGroup in canvasGroups)
            {
                GameObject freeArea = new GameObject("FreeArea", typeof(RectTransform));
                RectTransform rectTransform = (RectTransform)freeArea.transform;
                rectTransform.SetParent(canvasGroup.transform);
                rectTransform.SetAsLastSibling();
                rectTransform.anchorMin = Vector2.zero;
                rectTransform.anchorMax = Vector2.one;
                rectTransform.SetLeft(0).SetTop(0).SetRight(0).SetBottom(0);
                FreeArea = rectTransform;
            }
        }

        private void OnValidate()
        {
            if (menuReferences == null)
            {
                return;
            }
            
            if (menuReferences.Length <= 0)
            {
                return;
            }
            
            if (Array.IndexOf(menuReferences, StartMenu) < 0)
            {
                Array.Resize(ref menuReferences, menuReferences.Length + 1);
                menuReferences[^1] = StartMenu;
            }
        }
    }
}