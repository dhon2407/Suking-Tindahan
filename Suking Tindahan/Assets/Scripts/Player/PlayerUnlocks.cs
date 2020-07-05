using System.Collections.Generic;
using Helpers;
using Player.Data;
using Store;
using UnityEngine;

namespace Player
{
    public class PlayerUnlocks : MonoBehaviour
    {
        [SerializeField] private List<StoreSlot> unlocks = new List<StoreSlot>();
        
        public static SkinColor GetInitialColor()
        {
            const SkinColor initialColor = SkinColor.Celadon;

            foreach (var data in _instance.unlocks)
            {
                if (data.action is ColorAppearanceAction actionData)
                    return actionData.color;

            }

            Debug.LogWarning($"No initial color found, returning default color {initialColor}");
            
            return initialColor;
        }
        
        public static Size GetInitialSize()
        {
            const Size initialSize = Size.S100;

            foreach (var data in _instance.unlocks)
            {
                if (data.action is SizeAppearanceAction actionData)
                    return actionData.size;

            }

            Debug.LogWarning($"No initial color found, returning default color {initialSize.ToNiceString()}");
            
            return initialSize;
        }
        
        public static Shape GetInitialShape()
        {
            const Shape initialShape = Shape.Circle;

            foreach (var data in _instance.unlocks)
            {
                if (data.action is ShapeAppearanceAction actionData)
                    return actionData.shape;

            }

            Debug.LogWarning($"No initial shape found, returning default shape {initialShape}");
            
            return initialShape;
        }
        
        public static bool Has(StoreSlot storeSlot)
        {
            return _instance.unlocks.Contains(storeSlot);
        }

        public static void Unlock(StoreSlot unlock)
        {
            _instance.unlocks.Add(unlock);
        }

        private static PlayerUnlocks _instance;

        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }
            
            _instance = this;
        }
    }
}