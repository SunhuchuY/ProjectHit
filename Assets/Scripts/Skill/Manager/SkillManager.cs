using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.TextCore.Text;

public class SkillManager : MonoBehaviour
{
    public SkillInventory inventory { get; private set; } = new SkillInventory();

    private void Awake()
    {
        inventory.InsertSkill(0, new InvincibleSkill());
        StartCoroutine(DealyTest());
    }
    
    private IEnumerator DealyTest()
    {
        yield return new WaitForSeconds(3);
        inventory.RemoveSkill(0);
    }
}

public class SkillInventory
{
    public Skill[] skills { get; private set; } = new Skill[4];

    public void InsertSkill(int index, Skill skill)
    {
        skills[index] = skill;
    }

    public void RemoveSkill(int index)
    {
        if (skills[index] == null)
        {
            Debug.Log("NULL");
            return;
        }
        else
        {
            Debug.Log("NOT NULL");
            skills[index].Dispose();
            skills[index] = null;
        }
    }
}

public interface ISkill
{
    string Name { get; }
    float Cooldown { get; }
    void Activate();
    void ApplyEffects(Character target);
}

public abstract class Skill : ISkill, IDisposable
{
    public string Name { get; protected set; }
    public float Cooldown { get; protected set; }
    private AsyncOperationHandle<GameObject> handle;
    public GameObject effectPrefab { get; protected set; }


    protected Skill(string name, float cooldown, string AddressablePrefabName)
    {
        Name = name;
        Cooldown = cooldown;

        handle = Addressables.LoadAssetAsync<GameObject>(AddressablePrefabName);
        handle.Completed += OnAssetLoaded;
    }

    private void OnAssetLoaded(AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            effectPrefab = UnityEngine.Object.Instantiate(handle.Result);
        }
        else
        {
            Debug.LogError("Failed to load the addressable asset.");
        }
    }


    public abstract void Activate();
    public abstract void ApplyEffects(Character target);

    public void Dispose()
    {
        GC.SuppressFinalize(this);

        //UnityEngine.Object.Destroy(effectPrefab);
        if (handle.IsValid())
        {
            UnityEngine.Object.Destroy(effectPrefab);
            Addressables.Release(handle);
        }

    }
}


public class InvincibleSkill : Skill
{
    public readonly float duration;

    public InvincibleSkill() : base(name: "무적", cooldown: 20f, AddressablePrefabName: "InvincibleEffect")
    {
    }

    public override void Activate()
    {
        // 스킬 활성화 로직1
    }

    public override void ApplyEffects(Character target)
    {
        // 타겟에게 효과 적용
    }

}

