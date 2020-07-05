using System;
using UnityEngine;

namespace Preview
{
    [RequireComponent(typeof(CanvasGroup))]
    public class BlackCurtain : MonoBehaviour
    {
        [SerializeField] private float fadeOutTime = 3f;
        
        private CanvasGroup _canvasGroup;
        
        public void Show(float duration)
        {
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
            
            LeanTween.value(gameObject, 0, 1, duration)
                .setOnUpdate(UpdateAlpha)
                .setEase(LeanTweenType.easeOutQuad).setOnComplete(FadeOut);
        }

        private void FadeOut()
        {
            LeanTween.value(gameObject, 1, 0, fadeOutTime)
                .setOnUpdate(UpdateAlpha)
                .setEase(LeanTweenType.easeInExpo).setOnComplete(() =>
                {
                    _canvasGroup.alpha = 0;
                    _canvasGroup.interactable = false;
                    _canvasGroup.blocksRaycasts = false;
                });
        }

        private void UpdateAlpha(float alpha)
        {
            _canvasGroup.alpha = alpha;
        }

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();

            DisableCurtain();
        }

        private void DisableCurtain()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }
    }
}