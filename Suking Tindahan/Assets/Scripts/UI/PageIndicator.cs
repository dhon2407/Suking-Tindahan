using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Image))]
    public class PageIndicator : MonoBehaviour
    {
        public void Activate()
        {
            _image.color = Color.blue;
        }

        public void Deactivate()
        {
            _image.color = Color.gray;
        }
        
        public void Hide()
        {
            Deactivate();
            gameObject.SetActive(false);
        }

        private Image _image;

        private void Awake()
        {
            _image = GetComponent<Image>();
            Deactivate();
        }
    }
}