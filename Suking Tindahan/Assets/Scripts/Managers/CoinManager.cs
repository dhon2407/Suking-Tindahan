using System;
using UnityEngine;
using UnityEngine.Events;

namespace Managers
{
    public class CoinManager : MonoBehaviour
    {
        [SerializeField] private int currentCoins = 500;
        
        public int CurrentCoins => currentCoins;
        public OnChangeValueEvent OnChangeValue { get; } = new OnChangeValueEvent();

        public void ReduceCoins(int value)
        {
            currentCoins = Mathf.Clamp(currentCoins - value, 0, int.MaxValue);
            OnChangeValue.Invoke(currentCoins);
        }
        
        public void GiveCoins(int amount)
        {
            currentCoins = Mathf.Clamp(currentCoins + amount, currentCoins, int.MaxValue);
            OnChangeValue.Invoke(currentCoins);
        }
        
        private void Awake()
        {
            Coins.SetManager(this);
        }

        public class OnChangeValueEvent : UnityEvent<int> {}
    }

    public static class Coins
    {
        private static CoinManager _manager;
        public static int Amount => _manager.CurrentCoins;
        public static bool Ready { get; private set; }
        public static CoinManager.OnChangeValueEvent OnChangeValue => _manager.OnChangeValue;
        
        public static void SetManager(CoinManager coinManager)
        {
            _manager = coinManager;
            Ready = true;
        }

        public static void Take(int value)
        {
            _manager.ReduceCoins(value);
        }

        public static void Give(int amount)
        {
            _manager.GiveCoins(amount);
        }
    }
}