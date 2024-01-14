using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] private GameObject _gateVisual;
    [SerializeField] private float _openGateDuration = 2;
    [SerializeField] private float _gateTargetY = -1.5f;
    private BoxCollider _gateCollider;


    private void Awake()
    {
         _gateCollider = GetComponent<BoxCollider>();   
    }

    private IEnumerator GateOpenCoroutine()
    {
        float _openGateCurrentTime = 0;
        Vector3 startPos = _gateVisual.transform.position;
        Vector3 targetPos = startPos + Vector3.up * _gateTargetY;

        while (_openGateCurrentTime < _openGateDuration)
        {
            _openGateCurrentTime += Time.deltaTime;
            _gateVisual.transform.position = Vector3.Lerp(startPos, targetPos, _openGateCurrentTime / _openGateDuration);

            yield return null;
        }

        _gateCollider.enabled = false;
    }


    public void GateOpen()
    {
        StartCoroutine(GateOpenCoroutine());
    }
}
