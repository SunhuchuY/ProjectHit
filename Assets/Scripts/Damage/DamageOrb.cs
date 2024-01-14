using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOrb : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private int _damage = 10;
    [SerializeField] private ParticleSystem _hitEffect;

    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();    
    }

    private void FixedUpdate()
    {
        _rb.MovePosition(transform.position + transform.forward * _speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {

        // exit: 예외 대상이라면 나갑니다.
        if (other.CompareTag("Enemy") || other.CompareTag("Spawner"))
        {
            return;
        }
        // play: 실행 대상이라면 실행합니다.
        else if (other.CompareTag("Player"))
        {
            Locomotion locomotion = other.GetComponent<Locomotion>();

            if(locomotion != null)
            {
                locomotion.ApplyDamage(_damage, transform.position);
            }
        }

        ParticleSystem hitEffect =  Instantiate(_hitEffect, transform.position, Quaternion.identity);
        hitEffect.Play();
        Destroy(gameObject);
    }
}
