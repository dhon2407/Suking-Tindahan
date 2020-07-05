using System;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Ads
{
    public class AdsHandler : MonoBehaviour
    {
        [SerializeField] private GameObject playerObj = null;
        [SerializeField] private Button closeButton = null;

        [SerializeField] private int adsBonusGold = 100;

        public void Open()
        {
            playerObj.SetActive(false);
            gameObject.SetActive(true);
        }

        private void Awake()
        {
            closeButton.onClick.AddListener(CloseAd);
            gameObject.SetActive(false);
        }

        private void CloseAd()
        {
            Coins.Give(adsBonusGold);
            gameObject.SetActive(false);
            playerObj.SetActive(true);
        }
    }
}