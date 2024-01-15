using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    public class Coin : MonoBehaviour
    {
        [SerializeField] private UnityEvent<int> _valueChangedEvent;

        private int _value;
        public int value 
        {

            get
            {
                return _value;
            }

            set
            {
                _value = Mathf.Clamp(value, 0, int.MaxValue);
                _valueChangedEvent?.Invoke(_value);
            }
        }

        public void AddCoin(int coin)
        {
            value += coin;
        }
    }
}

