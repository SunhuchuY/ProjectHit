using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialController : MonoBehaviour
{
    [SerializeField] Character character;

    [SerializeField] SkinnedMeshRenderer _skinnedMeshRenderer;
    MaterialPropertyBlock _materialPropertyBlock;

    public void GetComponents()
    {
        _materialPropertyBlock = new MaterialPropertyBlock();
    }

    public void MaterialDissolve(float duration)
    {
        StartCoroutine(MaterialDissolveC(duration));
    }

    private IEnumerator MaterialDissolveC(float duration)
    {
        yield return new WaitForSeconds(2);

        float currentTime = 0;
        float dissolve_Start = 20;
        float dissolve_Target = -10;
        float dissolve_Current;

        _materialPropertyBlock.SetFloat("_enableDissolve", 1);
        _skinnedMeshRenderer.SetPropertyBlock(_materialPropertyBlock);

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            dissolve_Current = Mathf.Lerp(dissolve_Start, dissolve_Target, currentTime / duration);
            _materialPropertyBlock.SetFloat("_dissolve_height", dissolve_Current);
            _skinnedMeshRenderer.SetPropertyBlock(_materialPropertyBlock);
            yield return null;
        }
    }

    public void MaterialAppear(float duration)
    {
        StartCoroutine(MaterialAppearC(duration));
    }

    private IEnumerator MaterialAppearC(float duration)
    {
        float currentTime = 0;
        float dissolve_Start = -10;
        float dissolve_Target = 20;
        float dissolve_Current;


        _materialPropertyBlock.SetFloat("_enableDissolve", 1);
        _skinnedMeshRenderer.SetPropertyBlock(_materialPropertyBlock);

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            dissolve_Current = Mathf.Lerp(dissolve_Start, dissolve_Target, currentTime / duration);
            _materialPropertyBlock.SetFloat("_dissolve_height", dissolve_Current);
            _skinnedMeshRenderer.SetPropertyBlock(_materialPropertyBlock);
            yield return null;
        }

        _materialPropertyBlock.SetFloat("_enableDissolve", 0);
        _skinnedMeshRenderer.SetPropertyBlock(_materialPropertyBlock);
    }
}