using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class PageIndicatorHandler : MonoBehaviour
    {
        [SerializeField] private PageIndicator pageIndicator = null;

        public void SetIndicator(int index)
        {
            if (index < 0 || index >= _currentMaxCount)
                return;
            
            ClearIndicators();

            for (int i = 0; i < _currentMaxCount; i++)
            {
                if (i != index) continue;
                
                _indicators[i].Activate(); 
                break;
            }
        }

        public void Refresh(int numberOfPages)
        {
            _currentMaxCount = numberOfPages;
            HideIndicators();

            var missingIndicators = _currentMaxCount - _indicators.Count;
            
            for (int i = 0; i < missingIndicators; i++)
                CreateNewIndicator();
        }
        
        private readonly List<PageIndicator> _indicators = new List<PageIndicator>();
        private int _currentMaxCount = 0;

        private void ClearIndicators()
        {
            foreach (var indicator in _indicators)
                indicator.Deactivate();
        }
        
        private void HideIndicators()
        {
            foreach (var indicator in _indicators)
                indicator.Hide();
        }

        private void CreateNewIndicator()
        {
            _indicators.Add(Instantiate(pageIndicator, transform));
        }
    }
}