using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using Player;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace UI
{
    public class ContentsLoader : MonoBehaviour
    {
        [SerializeField] private int goldRequirement = 250;
        [SerializeField] private int incrementalRequirement = 100;
        [SerializeField] private ShopPage pageItemObj = null;
        [SerializeField] private SlotValues[] slotPage = null;
        [SerializeField] private ControlButtons controlButtons = null;
        
        public void Show()
        {
            gameObject.SetActive(true);
            ResetPage();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
        
        public void RefreshSelectedSlot()
        {
            foreach (var page in _shopPages)
                page.SetCurrentSelectedSlot();
        }
        
        private ScrollSnap _pageItem;
        private PageIndicatorHandler _pageIndicator;
        private readonly List<ShopPage> _shopPages= new List<ShopPage>();

        private void Awake()
        {
            _pageItem = GetComponentInChildren<ScrollSnap>();
            _pageIndicator = GetComponentInChildren<PageIndicatorHandler>();
            
            controlButtons.OnBuyRandomStoreItem.AddListener(BuyRandomItem);
        }

        private void Start()
        {
            LoadItems();
        }

        private void LoadItems()
        {
            _shopPages.Clear();
            
            for (int i = 0; i < slotPage.Length; i++)
                CreatePageAtIndex(i);
            
            InitializePageItems();
        }

        private void InitializePageItems()
        {
            _pageIndicator.Refresh(slotPage.Length);
            _pageItem.onRelease.AddListener(_pageIndicator.SetIndicator);
            _pageItem.onRelease.AddListener(UpdateControlButtons);
            ResetPage();
        }

        private void CreatePageAtIndex(int pageIndex)
        {
            var shopPage = Instantiate(pageItemObj);
            shopPage.InitializeItems(slotPage[pageIndex].data, slotPage[pageIndex].progressLock, this);
            
            _shopPages.Add(shopPage);
            _pageItem.PushLayoutElement(shopPage.GetComponent<LayoutElement>());
        }
        
        private void ResetPage()
        {
            _pageItem.SnapToIndex(0);
            UpdateControlButtons(0);

            RefreshSelectedSlot();
        }

        private void UpdateControlButtons(int pageIndex)
        {
            if (slotPage[pageIndex].progressLock)
                controlButtons.Locked();
            else
                controlButtons.Unlocked();
            
            controlButtons.SetRequiredGold(goldRequirement);
        }
        
        private void BuyRandomItem()
        {
            if (!gameObject.activeSelf) return;

            if (Coins.Amount < goldRequirement)
            {
                //TODO What to do?
                Debug.Log("Not Enough Money.");
            }
            else
            {
                var lockSlots = _shopPages[_pageItem.CurrentIndex].GetLockedSlots();
                var itemSlot = _shopPages[_pageItem.CurrentIndex].GetRandomItemSlot();
                
                var roulettetin =
                    StartCoroutine(RandomSlotHighlight(lockSlots));

                if (!itemSlot) return;

                RefreshSelectedSlot();
                
                PlayerPreview.UnlockedNewItem(() =>
                {
                    StopCoroutine(roulettetin);

                    foreach (var lockSlot in lockSlots)
                        lockSlot.Roulette(false);
                    
                    itemSlot.Unlocked();
                    itemSlot.Selected(true);
                    
                    PlayerUnlocks.Unlock(itemSlot.Data);
                    Coins.Take(goldRequirement);
                    goldRequirement += incrementalRequirement;
                    itemSlot.Data.action.Execute(PlayerPreview.Player);
                    controlButtons.SetRequiredGold(goldRequirement);
                    
                    RefreshSelectedSlot();
                });
            }
        }

        private IEnumerator RandomSlotHighlight(List<StoreSlot> slots)
        {
            while (true)
            {
                slots[Random.Range(0, slots.Count)].Roulette(true);

                yield return new WaitForSeconds(Random.Range(0.05f, 0.1f));

                foreach (var slot in slots)
                    slot.Roulette(false);
            }
        }

        [Serializable]
        private class SlotValues
        {
            public bool progressLock = false;
            public Store.StoreSlot[] data = null;
        }
    }
}