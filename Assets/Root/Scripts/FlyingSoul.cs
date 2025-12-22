using System;
using PrimeTween;
using Root.Scripts.Utilities.Abstractions;
using Root.Scripts.Utilities.Logger;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Root.Scripts
{
    public class FlyingSoul : MonoBehaviour, IInitializer<Vector2, int>
    {
        [SerializeField]
        private Image image;

        [SerializeField]
        private TMP_Text soulText;

        private Vector2 _targetPosition;


        public void Initialize(Vector2 targetPosition, int givenSoul)
        {
            _targetPosition = targetPosition;
            soulText.text = givenSoul.ToString();
        }

        public Sequence PlayFlyAnimation()
        {
            return Sequence.Create(
                    Tween.Alpha(target: image,
                        startValue: 0f,
                        endValue: 1f,
                        duration: 0.1f))
                .Group(
                    Tween.Alpha(
                        target: soulText,
                        startValue: 0f,
                        endValue: 1f,
                        duration: 0.1f))
                .Group(
                    Tween.PositionY(
                        target: transform,
                        endValue: transform.position.y + 50f,
                        duration: 0.3f))
                .Chain(
                    Tween.Position(
                        target: transform,
                        endValue: _targetPosition,
                        duration: 0.5f,
                        ease: Ease.InQuint)
                )
                .Group(
                    Tween.Alpha(
                        target: image,
                        startValue: 1f,
                        endValue: 0f,
                        duration: 0.5f,
                        startDelay: 0.3f))
                .Group(
                    Tween.Alpha(
                        target: soulText,
                        startValue: 1f,
                        endValue: 0f,
                        duration: 0.5f,
                        startDelay: 0.3f));
        }
    }
}