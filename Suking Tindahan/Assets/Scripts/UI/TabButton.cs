using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Button))]
    public class TabButton : MonoBehaviour
    {
        [SerializeField] private Color selectedColor = Color.white;
        [SerializeField] private Color unSelectedColor = Color.gray;
        public OnSelectTabEvent OnSelect { get; } = new OnSelectTabEvent();

        public void Select()
        {
            if (_selected) return;
            
            _setColors.normalColor = selectedColor;
            _button.colors = _setColors;
            _selected = true;
            _transform.localScale = Vector3.one;
        }

        public void UnSelect()
        {
            _setColors.normalColor = unSelectedColor;
            _button.colors = _setColors;
            _selected = false;
            var unSelectedVectorScale = Vector3.one;
            unSelectedVectorScale.y *= UnSelectedScale;
            
            _transform.localScale = unSelectedVectorScale;

        }
        public void SetIndex(int index)
        {
            _tabIndex = index;
        }
        
        public class OnSelectTabEvent : UnityEvent<int> { }

        private const float UnSelectedScale = 0.95f;
        private Button _button;
        private int _tabIndex;
        private bool _selected;
        private Transform _transform;
        private ColorBlock _setColors;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(()=> OnSelect.Invoke(_tabIndex));
            _setColors = _button.colors;
            
            _transform = transform;
        }
    }
}