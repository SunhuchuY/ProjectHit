using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    [SerializeField] private SkillInventory skillInventory;
    [SerializeField] private SKillInputManager sKillInputManager;
    [SerializeField] private SkillUI_Manager skillUI_Manager;

    private void Awake()
    {
        skillInventory.InsertSkill(0, new InvincibleSkill());
        skillInventory.InsertSkill(1, new InvincibleSkill());
        skillInventory.InsertSkill(2, new InvincibleSkill());
        skillInventory.InsertSkill(3, new InvincibleSkill());
    }

    private void Update()
    {
        for (int i = 0; i < sKillInputManager.isInput.Length; i++)
        {
            // condition: skill can use
            if (sKillInputManager.isInput[i] && !skillInventory.skills[i].isCoolTime)
            {
                sKillInputManager.isInput[i] = false;
                skillInventory.skills[i].isCoolTime = true;
                skillInventory.skills[i].Current = 0;
                skillInventory.skills[i].Activate();
                skillInventory.skills[i].ApplyEffects();
                skillUI_Manager.OnSkillCoolTIme(i);
            }

            if (skillInventory.skills[i].isCoolTime)
            {
                // update: cool time
                skillInventory.skills[i].Current += Time.deltaTime;
                skillUI_Manager.SkillCoolTimeUpdate(i, 1 - skillInventory.skills[i].CoolProgress);

                // condition: ending coolTime
                if (skillInventory.skills[i].Current > skillInventory.skills[i].Cooldown)
                {
                    sKillInputManager.isInput[i] = false;
                    skillInventory.skills[i].isCoolTime = false;
                    skillInventory.skills[i].Current = 0;
                    skillUI_Manager.OffSkillCoolTIme(i);
                }
            }
        }
    }

    private IEnumerator DealyTest()
    {
        yield return new WaitForSeconds(3);
        skillInventory.RemoveSkill(0);
    }
}

public interface ISkill
{
    string Name { get; }
    float Cooldown { get; }
    float Current { get; }
    float CoolProgress { get; }
    bool isCoolTime { get; }
    void Activate();
    void ApplyEffects();
}

public abstract class Skill : ISkill, IDisposable
{
    public string Name { get; protected set; }
    public float Cooldown { get; protected set; }
    public float Current { get; set; }
    public float CoolProgress { get => Current / Cooldown; }
    public bool isCoolTime { get; set; } = false;

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
            effectPrefab = handle.Result;
        }
        else
        {
            Debug.LogError("Failed to load the addressable asset.");
        }
    }


    public abstract void Activate();
    public abstract void ApplyEffects();

    public void Dispose()
    {
        GC.SuppressFinalize(this);

        if (handle.IsValid())
        {
            UnityEngine.Object.Destroy(effectPrefab);
            Addressables.Release(handle);
            Addressables.ReleaseInstance(effectPrefab);
            effectPrefab = null;
        }

    }
}


public class InvincibleSkill : Skill
{
    private readonly Transform player;

    public InvincibleSkill() : base(name: "무적", cooldown: 20f, AddressablePrefabName: "InvincibleEffect")
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    public override void Activate()
    {
        // 스킬 활성화 로직1
        GameObject prefab = UnityEngine.Object.Instantiate(effectPrefab, player.transform.position, Quaternion.identity, player);
        float duration = prefab.GetComponent<ParticleSystem>().duration;
        UnityEngine.Object.Destroy(prefab, duration);
    }

    public override void ApplyEffects()
    {
        // 타겟에게 효과 적용
    }

}

