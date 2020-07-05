using System.Collections.Generic;
using UnityEngine;

namespace Player.Data
{
    [CreateAssetMenu(fileName = "ShapeInfo", menuName = "Data/Shapes", order = 0)]
    public class ShapeInfo : ScriptableObject
    {
        [SerializeField] private List<ShapeMap> shapes = null;

        public static Sprite GetSprite(Shape shape)
        {
            foreach (var shapeMap in Instance.shapes)
            {
                if (shapeMap.shape != shape) continue;

                return shapeMap.sprite;
            }
            
            Debug.LogError($"No shape sprite for {shape}, using empty/null as default.");
            
            return null;
        }
        
        private static ShapeInfo _instance;
        private static ShapeInfo Instance
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
            _instance = Resources.Load<ShapeInfo>("PlayerData/ShapeInfo");
        }

        [System.Serializable]
        private struct ShapeMap
        {
            public Shape shape;
            public Sprite sprite;
        }
    }
}