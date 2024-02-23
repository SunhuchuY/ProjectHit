using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField] protected float _moveSpeed = 5f;
    [SerializeField] protected float _gravity = -9.8f;

    public Vector3 _movementVelocity;
    public Animator _animator;
    public CharacterController _characterController;
    public MaterialController _materialController;

    // Invincible
    public bool _isInvincible;
    [SerializeField] protected float _invincibleDuration;

    // Being Hit
    public Vector3 _impactOnCharacter;  

    // Health
    protected Health _health;

    // Damage Caster
    DamageCaster _damageCaster;

    // SM
    StateMachine SM;

    bool isPlayer;

    public enum CommonState
    {
        Dead
    }

    protected virtual void Initilize(StateMachine sm, bool _isPlayer)
    {
        _materialController.GetComponents();
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _health = GetComponent<Health>();
        _damageCaster = GetComponentInChildren<DamageCaster>();

        SM = sm;
        isPlayer = _isPlayer;   
    }

    public void ChangeState<T>() where T : IState
    {
        SM.ChangeState<T>();
    }

    public void ChangeState(CommonState state)
    {
        switch (state)
        {
            case CommonState.Dead:
                if (isPlayer)
                {
                    // TODO: 플레이어 죽음 상태로 바꾸어야합니다.
                    SM.ChangeState<EnemyDeadState>();
                }
                else
                {
                    SM.ChangeState<EnemyDeadState>();
                }
                break;
        }
    }

    public void DelayFalseInvincible()
    {
        StartCoroutine(DelayFalseInvincibleC());
    }

    IEnumerator DelayFalseInvincibleC()
    {
        yield return new WaitForSeconds(_invincibleDuration);
        _isInvincible = false;
    }

    public void EnableDamageCaster()
    {
        _damageCaster.EnableDamageCaster();
    }

    public void DisableDamageCaster()
    {
        _damageCaster.DisableDamageCaster();
    }

    public void AttackAnimationEnds()
    {
        SM.ChangeState<EnemyNormalState>();
    }

    public void BeingHitAnimationEnds()
    {
        SM.ChangeState<EnemyNormalState>();
    }
}