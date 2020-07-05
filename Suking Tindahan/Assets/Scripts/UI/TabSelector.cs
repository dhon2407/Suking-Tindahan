using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TabSelector : MonoBehaviour
    {
        [SerializeField] private List<TabPair> tabs = null;
        
        private int _currentIndex = -1;
        private LayoutGroup _layoutGroup;

        private void Awake()
        {
            _layoutGroup = GetComponent<HorizontalLayoutGroup>();
            
            for (int i = 0; i < tabs.Count; i++)
            {
                tabs[i].contents.Hide();
                tabs[i].button.SetIndex(i);
                tabs[i].button.OnSelect.AddListener(ChangeIndex);
            }
        }

        private void Start()
        {
            ChangeIndex(0);
        }

        private void ChangeIndex(int index)
        {
            if (_currentIndex == index) return;
            
            for (int i = 0; i < tabs.Count; i++)
                tabs[i].Activate(i != index);

            StartCoroutine(RefreshLayoutGroup());
            
            _currentIndex = index;
        }

        private IEnumerator RefreshLayoutGroup()
        {
            _layoutGroup.enabled = false;
            yield return null;
            _layoutGroup.enabled = true;
        }

        [Serializable]
        private class TabPair
        {
            [SerializeField] public TabButton button = null;
            [SerializeField] public ContentsLoader contents = null;

            public void Activate(bool isActive)
            {
                if (isActive)
                {
                    button.UnSelect();
                    contents.Hide();
                }
                else
                {
                    button.Select();
                    contents.Show();
                }
            }
            
        }
    }
}