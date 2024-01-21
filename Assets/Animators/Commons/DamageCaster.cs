using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCaster : MonoBehaviour
{
    [SerializeField] private int _damage = 30;
    [SerializeField] private string targetTag;
    [SerializeField] private Player.VFXManager _playerVFXManager;
    private Collider _casterCollider;    
    private List<Collider> _targetList = new List<Collider>();


    private void Awake()
    {
        _casterCollider = GetComponent<Collider>();
        _casterCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(targetTag) && !_targetList.Contains(other))
        {
            Locomotion cc = other.GetComponent<Locomotion>();
            DamageReceiver receiver = other.GetComponent<DamageReceiver>();

            if(cc != null)
            {
                cc.ApplyDamage(_damage, transform.parent.position);

                if (_playerVFXManager != null)
                {
                    _playerVFXManager.PlaySlash(other.transform.position + new Vector3(0, 0.5f, 0f));
                }

            }
            else if(receiver != null)
            {
                Debug.Log("Testddddddddddddddd");
                receiver.ApplyDamage(_damage);
            }       

            _targetList.Add(other);
        }
    }

        
    public void EnableDamageCaster()
    {
        _targetList.Clear();
        _casterCollider.enabled =true;
    }

    public void DisableDamageCaster()
    {
        _casterCollider.enabled = false;
    }

    private void OnDrawGizmos()
    {
        _casterCollider = GetComponent<Collider>();

        RaycastHit hit;
        Vector3 originalPos = transform.position + (-_casterCollider.bounds.extents.z) * transform.forward;
        bool isHit = Physics.BoxCast(originalPos, _casterCollider.bounds.extents / 2, 
            transform.forward, out hit, transform.rotation, _casterCollider.bounds.extents.z, 1 << 6);

        if (isHit)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(hit.point, 0.4f);
        }
    }
}