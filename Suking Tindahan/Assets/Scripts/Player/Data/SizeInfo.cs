using System;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Data
{
    [CreateAssetMenu(fileName = "ColorInfo", menuName = "Data/Sizes", order = 0)]
    public class SizeInfo : ScriptableObject
    {
        public static float GetRate(Size size)
        {
            switch (size)
            {
                case Size.S100: return 1f;
                case Size.S90: return 0.9f;
                case Size.S80: return 0.8f;
                case Size.S70: return 0.7f;
                case Size.S60: return 0.6f;
                case Size.S50: return 0.5f;
                case Size.S40: return 0.4f;
                case Size.S30: return 0.3f;
                case Size.S110: return 1.1f;
                case Size.S120: return 1.2f;
                case Size.S130: return 1.3f;
                case Size.S140: return 1.4f;
                case Size.S150: return 1.5f;
                case Size.S160: return 1.6f;
                case Size.S170: return 1.7f;
                
                default:
                {
                    Debug.LogError($"No size data found for {size}.");
                    return 1f;
                }
            }
        }
        
        private static SizeInfo _instance;
        private static SizeInfo Instance
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
            _instance = CreateInstance<SizeInfo>();
        }
    }
}