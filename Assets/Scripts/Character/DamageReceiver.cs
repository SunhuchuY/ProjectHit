using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageReceiver : MonoBehaviour
{
    [SerializeField] Health health;

    public void ApplyDamage(int damage)
    {
        health.ApplyDamage(damage);
    }
}
