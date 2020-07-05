using Player;
using Preview;
using Store;
using UnityEngine;
using UnityEngine.Events;

namespace Managers
{
    public class PreviewManager : MonoBehaviour
    {
        [SerializeField] private ShapePlayer player = null;
        [SerializeField] private Rotator rotator = null;
        [SerializeField] private BlackCurtain curtain = null;
        [SerializeField] private Zoomer zoomer = null;
        public ShapePlayer Player => player;

        private void Awake()
        {
            PlayerPreview.Setup(this);
        }

        public void ShowUnlockEffects(UnityAction callback)
        {
            //Magical Numbers!
            curtain.Show(1f);
            zoomer.Zoom(1f);
            rotator.SpeedUp(10f, 2f, callback);
        }
    }

    public static class PlayerPreview
    {
        public static ShapePlayer Player => _manager.Player;
        public static void Setup(PreviewManager manager)
        {
            _manager = manager;
        }

        private static PreviewManager _manager;

        public static void UnlockedNewItem(UnityAction callback)
        {
            _manager.ShowUnlockEffects(callback);
        }

        public static bool CurrentItem(BaseAppearanceAction dataAction)
        {
            switch (dataAction)
            {
                case ColorAppearanceAction colorAction:
                    return (_manager.Player.CurrentSkin == colorAction.color);
                case ShapeAppearanceAction shapeAction:
                    return (_manager.Player.CurrentShape == shapeAction.shape);
                case SizeAppearanceAction sizeAction:
                    return (_manager.Player.CurrentSize == sizeAction.size);
                default:
                    return false;
            }
        }
    }
}