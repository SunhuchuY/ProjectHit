public interface ISkill
{
    string Name { get; }
    float Cooldown { get; }
    float Current { get; }
    float CoolProgress { get; }
    bool isCoolTime { get; }
    void Start();
    void Update();
}