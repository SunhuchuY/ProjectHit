using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public abstract class SkillBase : ISkill, IDisposable
{
    public string Name { get; protected set; }
    public float Cooldown { get; protected set; }
    public float Current { get; set; }
    public float CoolProgress { get => Current / Cooldown; }
    public bool isCoolTime { get; set; } = false;

    private AsyncOperationHandle<GameObject> handle;
    public GameObject effectPrefab { get; protected set; }


    protected SkillBase(string name, float cooldown, string AddressablePrefabName = null)
    {
        Name = name;
        Cooldown = cooldown;

        if (AddressablePrefabName == null)
            return;

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


    public abstract void Start();
    public virtual void Update() { }
    public virtual void End() { }

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
