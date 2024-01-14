using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_02_Shoot : MonoBehaviour
{
    [SerializeField] private DamageOrb _damageOrb;
    [SerializeField] private Transform _shootTf;

    private Locomotion _locomotion;

    private void Awake()
    {
        _locomotion = GetComponent<Locomotion>();
    }

    public void ShootTheDamageOrb()
    {
        Instantiate(_damageOrb, _shootTf.position, Quaternion.LookRotation(_shootTf.forward));
    }

    private void Update()
    {
        _locomotion.RotateToTarget();
    }
}
