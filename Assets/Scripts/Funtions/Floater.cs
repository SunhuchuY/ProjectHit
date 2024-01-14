using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    [SerializeField] private float _speed = 80f;
    [SerializeField] private float _startHeight = 1;
    [SerializeField] private float _height = 1;

    void Update()
    {
        transform.position = new Vector3(transform.position.x, Mathf.Sin(_speed * Time.time) * _height + _startHeight, 0);
    }
}
