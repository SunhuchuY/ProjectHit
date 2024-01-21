using UnityEngine;

public class InvincibleSkill : SkillBase
{
    private readonly Transform player;

    public InvincibleSkill() : base(name: "무적", cooldown: 20f, AddressablePrefabName: "InvincibleEffect")
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    public override void Start()
    {
        // 스킬 활성화 로직1
        GameObject prefab = UnityEngine.Object.Instantiate(effectPrefab, player.transform.position, Quaternion.identity, player);
        ParticleSystem effect = prefab.GetComponent<ParticleSystem>();
        float duration = effect.duration;
        effect.Play();
        UnityEngine.Object.Destroy(prefab, duration);
    }
}

public class SpinningSkill : SkillBase
{
    private readonly Transform player;
    private readonly Locomotion locomotion;

    public SpinningSkill() : base(name: "회전 공격", cooldown: 15f)
    {
        player = GameObject.FindWithTag("Player").transform;
        locomotion = player.gameObject.GetComponent<Locomotion>();
    }

    public override void Start()
    {
        // 스킬 활성화 로직1
        GameObject prefab = UnityEngine.Object.Instantiate(effectPrefab, player.transform.position, Quaternion.identity, player);
        ParticleSystem effect = prefab.GetComponent<ParticleSystem>();
        effect.Play();

        //locomotion.SwitchToState(Locomotion.State.Skill);

        UnityEngine.Object.Destroy(prefab, 4f);
    }
}


