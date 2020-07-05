using System;
using Player.Data;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ShapePlayer : MonoBehaviour
    {
        public SkinColor CurrentSkin { get; private set; }
        public Shape CurrentShape { get; private set; }
        public Size CurrentSize { get; private set; }

        public void ChangeShape(Shape newShape)
        {
            CurrentShape = newShape;
            _spriteRenderer.sprite = ShapeInfo.GetSprite(newShape);
        }

        public void ChangeSkinColor(SkinColor newColor)
        {
            CurrentSkin = newColor;
            _spriteRenderer.color = ColorInfo.Get(newColor);
        }

        public void ChangeSize(Size newSize)
        {
            CurrentSize = newSize;
            _transform.localScale = _initialScale * SizeInfo.GetRate(newSize);
        }

        private SpriteRenderer _spriteRenderer;
        private Vector3 _initialScale;
        private Transform _transform;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _transform = transform;
            _initialScale = _transform.localScale;
        }

        private void Start()
        {
            ChangeSkinColor(PlayerUnlocks.GetInitialColor());
            ChangeSize(PlayerUnlocks.GetInitialSize());
            ChangeShape(PlayerUnlocks.GetInitialShape());
        }
    }
    
}