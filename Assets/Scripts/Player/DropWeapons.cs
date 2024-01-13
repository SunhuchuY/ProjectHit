using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropWeapons : MonoBehaviour
{
    [SerializeField] private List<GameObject> swords;

    public void DropSwords()
    {
        foreach (GameObject sword in swords)
        {
            sword.AddComponent<Rigidbody>();
            sword.AddComponent<BoxCollider>();
            sword.transform.parent = null;
        }
    }
}
