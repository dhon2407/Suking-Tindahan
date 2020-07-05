using System.Collections.Generic;
using Managers;
using Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UI
{
    public class ShopPage : MonoBehaviour
    {
        [SerializeField] private int maxSlot = 9;
        
        public void InitializeItems(Store.StoreSlot[] slots, bool progressLock, ContentsLoader loader)
        {
            _contentsLoader = loader;
            _progressLock = progressLock;
            HideButtons();
            
            for (int i = 0; i < maxSlot && i < slots.Length; i++)
            {
                if (!slots[i]) continue;

                _storeSlots[i].Show(slots[i]);

                if (PlayerUnlocks.Has(_storeSlots[i].Data))
                    _storeSlots[i].Unlocked();
            }
        }
        
        public StoreSlot GetRandomItemSlot()
        {
            var lockedSlots = GetLockedSlots();

            if (lockedSlots.Count <= 0)
                return null; 
            
            var rewardSlot = lockedSlots[Random.Range(0, lockedSlots.Count)];

            return rewardSlot;
        }
        
        public void SetCurrentSelectedSlot()
        {
            foreach (var slot in _storeSlots)
            {
                if (slot.Active)
                    slot.Selected(PlayerPreview.CurrentItem(slot.Data.action));
            }
        }

        public void ClearCurrentSelectedSlot()
        {
            foreach (var slot in _storeSlots)
            {
                if (slot.Active)
                    slot.Selected(false);
            }
        }
        
        public List<StoreSlot> GetLockedSlots()
        {
            var lockedSlots = new List<StoreSlot>();

            foreach (var slot in _storeSlots)
            {
                if (!slot.Active || PlayerUnlocks.Has(slot.Data))
                    continue;

                lockedSlots.Add(slot);
            }

            return lockedSlots;
        }

        private List<StoreSlot> _storeSlots;
        private ContentsLoader _contentsLoader;
        private bool _progressLock;

        private void Awake()
        {
            _storeSlots = new List<StoreSlot>(GetComponentsInChildren<StoreSlot>());
        }

        private void Start()
        {
            foreach (var slot in _storeSlots)
                slot.onClick.AddListener(_contentsLoader.RefreshSelectedSlot);
        }

        private void HideButtons()
        {
            foreach (var slot in _storeSlots)
                slot.Hide();
        }
    }
}