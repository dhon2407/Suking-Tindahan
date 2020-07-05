using System.Collections.Generic;
using UnityEngine;

namespace Player.Data
{
    [CreateAssetMenu(fileName = "ColorInfo", menuName = "Data/Colors", order = 0)]
    public class ColorInfo : ScriptableObject
    {
        [SerializeField] private List<ColorMap> colors = null;

        public static Color Get(SkinColor skinColor)
        {
            foreach (var colorMap in Instance.colors)
            {
                if (colorMap.color != skinColor) continue;

                return colorMap.colorValue;
            }
            
            Debug.LogError($"No skin color for {skinColor}, using white as default.");
            
            return Color.white;
        }
        
        private static ColorInfo _instance;
        private static ColorInfo Instance
        {
            get
            {
                if (_instance == null)
                    Initialize();
                return _instance;
            }
        }

        private static void Initialize()
        {
            _instance = Resources.Load<ColorInfo>("PlayerData/ColorInfo");
        }

        [System.Serializable]
        private struct ColorMap
        {
            public SkinColor color;
            public Color colorValue;
        }
    }
}