using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ScrollRect))]
    public class ScrollSnap : UIBehaviour, IDragHandler, IEndDragHandler
    {
        [SerializeField]
        public int startingIndex = 0;
        [SerializeField]
        public bool wrapAround = false;
        [SerializeField]
        public float lerpTimeMilliSeconds = 200f;
        [SerializeField]
        public float triggerPercent = 5f;
        [Range(0f, 10f)]
        public float triggerAcceleration = 1f;

        public class OnLerpCompleteEvent : UnityEvent { }
        public OnLerpCompleteEvent onLerpComplete;
        public class OnReleaseEvent : UnityEvent<int> { }
        public OnReleaseEvent onRelease;

        private int _actualIndex;
        [SerializeField]
        private int cellIndex;
        private ScrollRect _scrollRect;
        private CanvasGroup _canvasGroup;
        private RectTransform _content;
        private Vector2 _cellSize;
        private bool _indexChangeTriggered = false;
        private bool _isLerping = false;
        private DateTime _lerpStartedAt;
        private Vector2 _releasedPosition;
        private Vector2 _targetPosition;
        private RectTransform _rectTransform;

        public void PushLayoutElement(LayoutElement element)
        {
            element.transform.SetParent(_content.transform, false);
            SetContentSize(LayoutElementCount());
        }

        public void PopLayoutElement()
        {
            LayoutElement[] elements = _content.GetComponentsInChildren<LayoutElement>();
            Destroy(elements[elements.Length - 1].gameObject);
            SetContentSize(LayoutElementCount() - 1);
            
            if (cellIndex == CalculateMaxIndex())
                cellIndex -= 1;
        }

        public void UnshiftLayoutElement(LayoutElement element)
        {
            cellIndex += 1;
            element.transform.SetParent(_content.transform, false);
            element.transform.SetAsFirstSibling();
            SetContentSize(LayoutElementCount());
            var anchoredPosition = _content.anchoredPosition;
            anchoredPosition = new Vector2(anchoredPosition.x - _cellSize.x, anchoredPosition.y);
            _content.anchoredPosition = anchoredPosition;
        }

        public void ShiftLayoutElement()
        {
            Destroy(GetComponentInChildren<LayoutElement>().gameObject);
            SetContentSize(LayoutElementCount() - 1);
            cellIndex -= 1;
            var anchoredPosition = _content.anchoredPosition;
            anchoredPosition = new Vector2(anchoredPosition.x + _cellSize.x, anchoredPosition.y);
            _content.anchoredPosition = anchoredPosition;
        }

        public int CurrentIndex
        {
            get
            {
                var count = LayoutElementCount();
                var mod = _actualIndex % count;
                return mod >= 0 ? mod : count + mod;
            }
        }
        
        public void ClearElements()
        {
            foreach (Transform child in _content.transform)
                Destroy(child.gameObject);
        }

        public void OnDrag(PointerEventData data)
        {
            var dx = data.delta.x;
            var dt = Time.deltaTime * 1000f;
            var acceleration = Mathf.Abs(dx / dt);
            
            if (acceleration > triggerAcceleration && !float.IsPositiveInfinity(acceleration))
                _indexChangeTriggered = true;
        }

        public void OnEndDrag(PointerEventData data)
        {
            if (IndexShouldChangeFromDrag(data))
            {
                var direction = (data.pressPosition.x - data.position.x) > 0f ? 1 : -1;
                SnapToIndex(cellIndex + direction * CalculateScrollingAmount(data));
            }
            else
            {
                StartLerping();
            }
        }
        
        public void SnapToIndex(int newCellIndex)
        {
            var maxIndex = CalculateMaxIndex();
            if (wrapAround && maxIndex > 0)
            {
                _actualIndex += newCellIndex - cellIndex;
                cellIndex = newCellIndex;
                onLerpComplete.AddListener(WrapElementAround);
            }
            else
            {
                newCellIndex = Mathf.Clamp(newCellIndex, 0, maxIndex);
                _actualIndex += newCellIndex - cellIndex;
                cellIndex = newCellIndex;
            }

            UpdateNavigationButtons();

            onRelease.Invoke(cellIndex);
            StartLerping();
        }
        
        protected override void Awake()
        {
            base.Awake();
            _rectTransform = GetComponent<RectTransform>();
            _actualIndex = startingIndex;
            cellIndex = startingIndex;
            onLerpComplete = new OnLerpCompleteEvent();
            onRelease = new OnReleaseEvent();
            _scrollRect = GetComponent<ScrollRect>();
            _canvasGroup = GetComponent<CanvasGroup>();
            _content = _scrollRect.content;
            _cellSize = _rectTransform.rect.size;
            _content.GetComponent<GridLayoutGroup>().cellSize = _cellSize;
            _content.anchoredPosition = new Vector2(-_cellSize.x * cellIndex, _content.anchoredPosition.y);
            int count = LayoutElementCount();
            SetContentSize(count);

            if (startingIndex < count)
                MoveToIndex(startingIndex);
        }

        private IEnumerator InitializeSizeContentSize()
        {
            yield return null;
            var gridLayout = _content.GetComponent<GridLayoutGroup>();
            gridLayout.cellSize = _content.rect.size;
        }

        private void LateUpdate()
        {
            if (!_isLerping) return;
            
            LerpToElement();
            
            if (!ShouldStopLerping()) return;
            
            _isLerping = false;
            _canvasGroup.blocksRaycasts = true;
            onLerpComplete.Invoke();
            onLerpComplete.RemoveListener(WrapElementAround);
        }

        private int CalculateScrollingAmount(PointerEventData data)
        {
            var offset = _scrollRect.content.anchoredPosition.x + cellIndex * _cellSize.x;
            var normalizedOffset = Mathf.Abs(offset / _cellSize.x);
            var skipping = (int)Mathf.Floor(normalizedOffset);

            if (skipping == 0)
                return 1;
            
            if ((normalizedOffset - skipping) * 100f > triggerPercent)
                return skipping + 1;

            return skipping;
        }
        
        private int LayoutElementCount()
        {
            return _content.GetComponentsInChildren<LayoutElement>(false)
                .Count(e => e.transform.parent == _content);
        }
        
        private void StartLerping()
        {
            _releasedPosition = _content.anchoredPosition;
            _targetPosition = CalculateTargetPosition(cellIndex);
            _lerpStartedAt = DateTime.Now;
            _canvasGroup.blocksRaycasts = false;
            _isLerping = true;
        }

        private void UpdateNavigationButtons()
        {
            StartCoroutine(RefreshNavButtons());
        }

        private void MoveToIndex(int newCellIndex)
        {
            var maxIndex = CalculateMaxIndex();
            if (newCellIndex >= 0 && newCellIndex <= maxIndex)
            {
                _actualIndex += newCellIndex - cellIndex;
                cellIndex = newCellIndex;
            }

            UpdateNavigationButtons();

            onRelease.Invoke(cellIndex);
            _content.anchoredPosition = CalculateTargetPosition(cellIndex);
        }

        private int CalculateMaxIndex()
        {
            var cellPerFrame = Mathf.FloorToInt(_scrollRect.GetComponent<RectTransform>().rect.size.x / _cellSize.x);
            return LayoutElementCount() - cellPerFrame;
        }

        private bool IndexShouldChangeFromDrag(PointerEventData data)
        {
            // acceleration was above threshold
            if (_indexChangeTriggered)
            {
                _indexChangeTriggered = false;
                return true;
            }
            // dragged beyond trigger threshold
            var offset = _scrollRect.content.anchoredPosition.x + cellIndex * _cellSize.x;
            var normalizedOffset = Mathf.Abs(offset / _cellSize.x);
            return normalizedOffset * 100f > triggerPercent;
        }

        private void LerpToElement()
        {
            var t = (float)((DateTime.Now - _lerpStartedAt).TotalMilliseconds / lerpTimeMilliSeconds);
            var newX = Mathf.Lerp(_releasedPosition.x, _targetPosition.x, t);
            _content.anchoredPosition = new Vector2(newX, _content.anchoredPosition.y);
        }

        private void WrapElementAround()
        {
            if (cellIndex <= 0)
            {
                var elements = _content.GetComponentsInChildren<LayoutElement>();
                elements[elements.Length - 1].transform.SetAsFirstSibling();
                cellIndex += 1;
                var anchoredPosition = _content.anchoredPosition;
                anchoredPosition = new Vector2(anchoredPosition.x - _cellSize.x, anchoredPosition.y);
                _content.anchoredPosition = anchoredPosition;
            }
            else if (cellIndex >= CalculateMaxIndex())
            {
                var element = _content.GetComponentInChildren<LayoutElement>();
                element.transform.SetAsLastSibling();
                cellIndex -= 1;
                var anchoredPosition = _content.anchoredPosition;
                anchoredPosition = new Vector2(anchoredPosition.x + _cellSize.x, anchoredPosition.y);
                _content.anchoredPosition = anchoredPosition;
            }
        }

        private void SetContentSize(int elementCount)
        {
            _content.sizeDelta = new Vector2(_cellSize.x * elementCount, _content.rect.height);
        }

        private Vector2 CalculateTargetPosition(int index)
        {
            return new Vector2(-_cellSize.x * index, _content.anchoredPosition.y);
        }

        private bool ShouldStopLerping()
        {
            return Mathf.Abs(_content.anchoredPosition.x - _targetPosition.x) < 0.001;
        }

        private static IEnumerator RefreshNavButtons()
        {
            yield return null;
        }
    }
}
