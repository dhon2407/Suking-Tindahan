using System.Collections;
using Managers;
using TMPro;
using UnityEngine;

namespace UI
{
    public class CoinIndicator : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI value = null;

        private void Start()
        {
            StartCoroutine(SubscribeToCoinSystem());
        }

        private IEnumerator SubscribeToCoinSystem()
        {
            while (!Coins.Ready)
                yield return null;

            Coins.OnChangeValue.AddListener(UpdateValue);
            UpdateValue(Coins.Amount);
        }

        private void UpdateValue(int gold)
        {
            value.text = gold.ToString("0");
        }
    }
}