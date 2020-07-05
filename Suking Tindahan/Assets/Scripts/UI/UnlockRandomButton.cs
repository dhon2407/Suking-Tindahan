using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Button))]
    public class UnlockRandomButton : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI goldValue = null;
        
        public UnityEvent onClick = new UnityEvent();

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(onClick.Invoke);
        }

        public void SetUnlockValue(int value)
        {
            goldValue.text = value.ToString("0");
        }

        private Button _button;
    }
}