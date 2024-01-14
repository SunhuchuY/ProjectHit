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

        // exit: ���� ����̶�� �����ϴ�.
        if (other.CompareTag("Enemy") || other.CompareTag("Spawner"))
        {
            return;
        }
        // play: ���� ����̶�� �����մϴ�.
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
