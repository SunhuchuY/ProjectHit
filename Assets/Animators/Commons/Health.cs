using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int _max;
    
    private int _cur;
    private Locomotion _locomotion;


    private void Awake()
    {
        _cur = _max;
        _locomotion = GetComponent<Locomotion>();    
    }

    public void ApplyDamage(int damage)
    {
        _cur -= damage;
        Debug.Log(gameObject.name + ", current health: " + _cur);

        CheckHealth();
    }

    private void CheckHealth()
    {
        if(_cur <= 0)
        {
            _locomotion.SwitchToState(Locomotion.State.Dead);
        }
    }

    public void AddHealth(int health)
    {
        _cur += health;

        if (_cur > _max)
        {
            _cur = _max;
        }
    }
}
