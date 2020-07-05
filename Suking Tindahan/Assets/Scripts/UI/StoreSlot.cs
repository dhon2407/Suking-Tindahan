using Managers;
using Player.Data;
using Store;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(Button))]
    public class StoreSlot : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI buttonText = null;
        [SerializeField] private Image icon = null;

        public Button.ButtonClickedEvent onClick => _button.onClick;

        public Store.StoreSlot Data => _data;
        public bool Active { get; private set; }
        
        public void Hide()
        {
            _canvasGroup.alpha = 0;
            _button.interactable = false;
            Active = false;
        }

        public void Show(Store.StoreSlot data)
        {
            _canvasGroup.alpha = 1;
            Active = true;
            
            _data = data;

            var neededImage = (_data.action is ShapeAppearanceAction || _data.action is ColorAppearanceAction);
            
            buttonText.gameObject.SetActive(!neededImage);
            icon.gameObject.SetActive(neededImage);

            icon.sprite = _data.revealedIcon;
            buttonText.text = "???";
        }
        
        public void Unlocked()
        {
            Active = true;
            _button.interactable = true;

            icon.sprite = _data.unrevealedIcon;
            icon.color = (_data.action is ColorAppearanceAction)
                ? ColorInfo.Get(((ColorAppearanceAction) _data.action).color)
                : Color.gray;
            
            buttonText.text = _data.SlotName;
        }

        private CanvasGroup _canvasGroup;
        private Button _button;
        private Store.StoreSlot _data;
        private ColorBlock _defaultColors;
        
        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _button = GetComponent<Button>();
            _defaultColors = _button.colors;
            
            _button.onClick.AddListener(TakeEffect);
        }

        private void TakeEffect()
        {
            _data.action.Execute(PlayerPreview.Player);
        }

        public void Selected(bool isSelected)
        {
            _button.image.color = isSelected ? _button.colors.selectedColor : Color.white;
        }

        public void Roulette(bool active)
        {
            if (!active)
            {
                _button.colors = _defaultColors;
            }
            else
            {
                var rouletteColor = _defaultColors;
                rouletteColor.disabledColor = Color.red;
                
                _button.colors = rouletteColor;
            }
        }
    }
}