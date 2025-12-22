using NaughtyAttributes;
using PrimeTween;
using Root.Scripts.Draggables;
using Root.Scripts.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Root.Scripts.Grid
{
    public class GridBottomLayer : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField, BoxGroup(Consts.InspectorTitles.REFERENCES)]
        private TextMeshPro textMeshPro;
#endif

        [SerializeField, BoxGroup(Consts.InspectorTitles.REFERENCES)]
        private Image bottomImage;

        [SerializeField, BoxGroup(Consts.InspectorTitles.SETTINGS)]
        private bool showNodeText;

        [SerializeField, BoxGroup(Consts.InspectorTitles.SETTINGS)]
        private float colorTransitionDuration;

        [SerializeField, BoxGroup(Consts.InspectorTitles.SETTINGS)]
        private Color idleColor;

        [SerializeField, BoxGroup(Consts.InspectorTitles.SETTINGS)]
        private Color enabledColor;

        [SerializeField, BoxGroup(Consts.InspectorTitles.SETTINGS)]
        private Color disabledColor;

        [SerializeField, BoxGroup(Consts.InspectorTitles.SETTINGS)]
        private Color holdColor;

        public NodeObject NodeObject { get; set; }
        private Color _currentColor;
        private bool _holdStarted;

        private void Start()
        {
            bottomImage.color = idleColor;
#if UNITY_EDITOR
            ShowNodeText();
#endif
            ADraggable.OnHoldStart += HoldStart;
            ADraggable.OnHold += Hold;
            ADraggable.OnHoldEnd += HoldEnd;
        }

        private void OnDestroy()
        {
            ADraggable.OnHoldStart -= HoldStart;
            ADraggable.OnHold += Hold;
            ADraggable.OnHoldEnd -= HoldEnd;
        }

        private void HoldStart(ADraggable draggable)
        {
            _holdStarted = true;
            Tween.StopAll(bottomImage);

            if (draggable.CanPlace(NodeObject))
            {
                _currentColor = enabledColor;
                Tween.Color(bottomImage, _currentColor, colorTransitionDuration)
                    .OnComplete(this, self => self._holdStarted = false);
                return;
            }

            _currentColor = disabledColor;
            Tween.Color(bottomImage, _currentColor, colorTransitionDuration)
                .OnComplete(this, self => self._holdStarted = false);
        }

        private void Hold(ADraggable draggable, NodeObject nodeObject)
        {
            if (_holdStarted)
            {
                return;
            }

            if (NodeObject == nodeObject && draggable.CanPlace(NodeObject))
            {
                bottomImage.color = holdColor;
            }
            else
            {
                bottomImage.color = _currentColor;
            }
        }

        private void HoldEnd()
        {
            Tween.StopAll(bottomImage);
            _currentColor = idleColor;
            Tween.Color(bottomImage, _currentColor, colorTransitionDuration);
        }

#if UNITY_EDITOR
        private void ShowNodeText()
        {
            if (!showNodeText)
            {
                return;
            }

            textMeshPro.text = NodeObject.ToString();
            if (NodeObject.IsHostilePath)
            {
                textMeshPro.color = Color.red;
            }
        }
#endif
    }
}