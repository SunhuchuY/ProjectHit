using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private int _max;
    [SerializeField] private UnityEvent<float> _curHealthChangedEvent;

    private int p_cur;
    private int _cur
    {
        get
        {
            return p_cur;
        }

        set
        {
            p_cur = Mathf.Clamp(value, 0, _max);

            // in case: dead
            if(p_cur <= 0)
            {
                _locomotion.SwitchToState(Locomotion.State.Dead);
            }

            _curHealthChangedEvent.Invoke((float)p_cur / (float)_max);
        }
    }
    private Locomotion _locomotion;

    private void Awake()
    {
        _cur = _max;
        _locomotion = GetComponent<Locomotion>();    
    }

    public void ApplyDamage(int damage)
    {
        _cur -= damage;
    }

    public void AddHealth(int health)
    {
        _cur += health;
    }
}
