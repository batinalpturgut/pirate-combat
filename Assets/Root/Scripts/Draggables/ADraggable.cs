using System;
using Root.Scripts.Grid;
using Root.Scripts.Managers.Grid;
using Root.Scripts.Managers.Inventory;
using Root.Scripts.Managers.UI;
using Root.Scripts.Utilities;
using Root.Scripts.Utilities.Abstractions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Root.Scripts.Draggables
{
    public abstract class ADraggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler,
        IInitializer<GridManager, UIManager, InventoryManager>
    {
        [Header("References:")] [SerializeField]
        protected Image imageReference;

        private Transform _parentAfterDrag;
        private UIManager _uiManager;
        private int _siblingIndexBeforeDrag;
        protected InventoryManager InventoryManager;
        protected GridManager GridManager;

        protected abstract void Place();
        public abstract bool CanPlace(NodeObject nodeObject);
        public static event Action<ADraggable> OnHoldStart;
        public static event Action<ADraggable, NodeObject> OnHold;
        public static event Action OnHoldEnd;

        public void OnBeginDrag(PointerEventData eventData)
        {
            _siblingIndexBeforeDrag = transform.GetSiblingIndex();
            _parentAfterDrag = transform.parent;
            transform.SetParent(_uiManager.FreeArea);
            OnHoldStart?.Invoke(this);
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = Input.mousePosition;

            if (!GridManager.TryGetNodePosition(MousePos.GetPosition(), out NodePosition nodePosition))
            {
                OnHold?.Invoke(this, null);
                return;
            }

            NodeObject nodeObject = GridManager.GetNodeObjectWithNodePosition(nodePosition);
            OnHold?.Invoke(this, nodeObject);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            transform.SetParent(_parentAfterDrag);
            transform.SetSiblingIndex(_siblingIndexBeforeDrag);
            OnHoldEnd?.Invoke();
            Place();
        }

        public void Initialize(GridManager gridManager, UIManager uiManager, InventoryManager inventoryManager)
        {
            GridManager = gridManager;
            _uiManager = uiManager;
            InventoryManager = inventoryManager;
        }
    }
}