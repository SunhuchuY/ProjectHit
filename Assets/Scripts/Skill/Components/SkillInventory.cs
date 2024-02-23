using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SkillInventory : MonoBehaviour
{
    public SkillBase[] skills { get; private set; } = new SkillBase[4];

    public void InsertSkill(int index, SkillBase skill)
    {
        skills[index] = skill;
    }

    public void RemoveSkill(int index)
    {
        if (skills[index] == null)
        {
            return;
        }
        else
        {
            skills[index].Dispose();
            skills[index] = null;
        }
    }
}



public interface IState
{
    void OnEnter();
    void OnExit();
    void Update();
}

public abstract class BaseEnemyState
{
    protected readonly EnemyController Controller;

    public BaseEnemyState(EnemyController controller)
    {
        Controller = controller;
    }
}

public class EnemyAttackState : BaseEnemyState ,IState
{
    public EnemyAttackState(EnemyController controller) : base(controller) 
    { 
    }  

    public void OnEnter()
    {
        Vector3 dir = Controller._targetPlayer.position - Controller.transform.position; // 타겟 방향
        Quaternion targetRotation = Quaternion.LookRotation(dir); // 목표 회전
        Controller.transform.DORotateQuaternion(targetRotation, 0.3f)
            .OnComplete(() =>
            {
                Controller._animator.SetTrigger("Attack");
                Debug.Log("SetTrigger Attack");
            });
    }

    public void OnExit()
    {
    }

    public void Update()
    {
        Debug.Log("Attack Update");
    }
}

public class EnemyNormalState : BaseEnemyState, IState
{
    EnemyController Controller;
    StateMachine Machine;

    public EnemyNormalState(EnemyController controller, StateMachine machine) : base(controller)
    {
        Controller = controller;
        Machine = machine;
    }

    public void OnEnter()
    {
    }

    public void OnExit()
    {
    }

    public void Update()
    {
        if (Vector3.Distance(Controller._targetPlayer.position, Controller.transform.position) >= Controller._navMeshAgent.stoppingDistance)
        {
            Controller._navMeshAgent.SetDestination(Controller._targetPlayer.position);
            Controller._animator.SetFloat("Speed", 0.2f);
        }
        else
        {
            Controller._navMeshAgent.SetDestination(Controller.transform.position);
            Controller._animator.SetFloat("Speed", 0f);

            Machine.ChangeState<EnemyAttackState>();
        }
    }
}
public class EnemyBeingHitState : BaseEnemyState, IState
{
    public EnemyBeingHitState(EnemyController controller) : base(controller)
    {

    }

    public void OnEnter()
    {
        Controller._animator.SetTrigger("BeingHit");
        Controller._isInvincible = true;
        Controller.DelayFalseInvincible();
    }

    public void OnExit()
    {
    }

    public void Update()
    {
    }
}

public class EnemyDeadState : BaseEnemyState ,IState
{
    public EnemyDeadState(EnemyController controller) : base(controller)
    {
    }

    public void OnEnter()
    {
        Controller._characterController.enabled = false;
        Controller._animator.SetTrigger("Dead");
        Controller._materialController.MaterialDissolve(2);
    }

    public void OnExit()
    {
    }

    public void Update()
    {
    }
}

public class EnemySpawnState : BaseEnemyState ,IState
{
    StateMachine Machine;

    public EnemySpawnState(EnemyController controller, StateMachine machine) : base(controller)
    {
        Machine = machine;  
    }

    public void OnEnter()
    {
        Controller._isInvincible = true;
        Controller._spawnTime = Controller._spawnDuration;
        Controller._materialController.MaterialAppear(2);
    }

    public void OnExit()
    {
        
    }

    public void Update()
    {
        Controller._spawnTime -= Time.deltaTime;
        if (Controller._spawnTime <= 0)
        {
            Machine.ChangeState<EnemyNormalState>();
        }
    }
}


public class StateMachine
{
    public IState _currentState { get; private set; }
    private Dictionary<Type, IState> _states;

    public StateMachine()
    {
        _states = new Dictionary<Type, IState>();
    }

    public void AddState<T>(T state) where T : IState
    {
        var type = typeof(T);
        if (!_states.ContainsKey(type))
        {
            _states[type] = state;
        }
    }

    public void ChangeState<T>() where T : IState
    {
        var type = typeof(T);
        if (_currentState != null)
        {
            _currentState.OnExit();
        }

        if (_states.TryGetValue(type, out var newState))
        {
            _currentState = newState;
            _currentState.OnEnter();
        }
    }

    protected void Update()
    {
        _currentState?.Update();
    }
}