using UnityEngine;

namespace Store
{
    [CreateAssetMenu(fileName = "Slot Data", menuName = "Data/Slot Data", order = 0)]
    public class StoreSlot : ScriptableObject
    {
        public Sprite unrevealedIcon = null;
        public Sprite revealedIcon = null;
        public BaseAppearanceAction action = null;

        public string SlotName => action.Name;
    }
}