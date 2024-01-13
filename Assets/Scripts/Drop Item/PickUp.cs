using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] private int _value;
    public int value => _value;

    public enum ItemType
    {
        Heal, Coin
    }

    [SerializeField] private ItemType _type;
    public ItemType type => _type;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Locomotion>().PickUpItem(this);
            Destroy(gameObject);
        }
    }
}
