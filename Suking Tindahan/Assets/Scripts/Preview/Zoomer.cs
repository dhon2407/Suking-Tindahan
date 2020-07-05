using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Preview
{
    public class Zoomer : MonoBehaviour
    {
        [SerializeField] private Transform targetPosition = null;
        [SerializeField, MinValue(1)] private float zoomRate = 1.5f;
        [SerializeField] private float fadeBackTime = 3f;
        
        public void Zoom(float duration)
        {
            LeanTween.move(gameObject, targetPosition.position, duration)
                .setEase(LeanTweenType.easeOutQuad).setOnComplete(GoBack);
            
            LeanTween.value(gameObject, 1, zoomRate, duration)
                .setEase(LeanTweenType.easeOutQuad)
                .setOnUpdate(UpdateScale);
        }

        private void UpdateScale(float newScale)
        {
            _transform.localScale = Vector3.one * newScale;
        }

        private Vector3 _initialPosition;
        private Transform _transform;

        private void Start()
        {
            _transform = transform;
            _initialPosition = _transform.position;
        }

        private void GoBack()
        {
            LeanTween.move(gameObject, _initialPosition, fadeBackTime)
                .setEase(LeanTweenType.easeInExpo);

            LeanTween.value(gameObject, zoomRate, 1, fadeBackTime)
                .setEase(LeanTweenType.easeInExpo)
                .setOnUpdate(UpdateScale);
        }
    }
}