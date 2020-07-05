using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class ControlButtons : MonoBehaviour
    {
        [SerializeField] private UnlockRandomButton buyRandom = null; 
        
        public UnityEvent OnBuyRandomStoreItem { get; } = new UnityEvent();
        
        public void Locked()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
        }

        public void Unlocked()
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.interactable = true;
        }

        public void SetRequiredGold(int gold)
        {
            buyRandom.SetUnlockValue(gold);
        }

        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            buyRandom.onClick.AddListener(OnBuyRandomStoreItem.Invoke);
        }
        
    }
}