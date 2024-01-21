using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private int _max;
    [SerializeField] private UnityEvent<float> _curHealthChangedEvent;

    private event System.Action _deadChangedEvent;

    private int m_cur;
    private int _cur
    {
        get
        {
            return m_cur;
        }

        set
        {
            m_cur = Mathf.Clamp(value, 0, _max);

            // in case: dead
            if(m_cur <= 0)
            {
                _deadChangedEvent?.Invoke();
            }

            _curHealthChangedEvent.Invoke((float)m_cur / (float)_max);
        }
    }

    private void Awake()
    {
        _cur = _max;

        if(GetComponent<Locomotion>() != null)
        {
            _deadChangedEvent += () => GetComponent<Locomotion>().SwitchToState(Locomotion.State.Dead);
        }
        else
        {
            _deadChangedEvent += () => GetComponent<Character>().ChangeState(Character.CommonState.Dead);
        }
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
