using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Character
{
    public Enemy.VFXManager _enemyVFXManager { get; private set; }
    public UnityEngine.AI.NavMeshAgent _navMeshAgent { get; private set; }
    public Transform _targetPlayer { get; private set; }
    private EnemySM SM;

    // Spawn, monster
    [Header("Spawn")]
    public float _spawnDuration = 2f;
    public float _spawnTime;

    void Awake()
    {
        _enemyVFXManager = GetComponent<Enemy.VFXManager>();

        _navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        _navMeshAgent.speed = _moveSpeed;
        _targetPlayer = GameObject.FindWithTag("Player").transform;

        SM = new EnemySM(this);
        base.Initilize(SM);
    }

    private void Start()
    {
        SM.ChangeState<EnemySpawnState>();
    }

    protected override void Initilize(StateMachine sm)
    {
        base.Initilize(sm);
    }


    void FixedUpdate()
    {
        SM.Update();
    }

}
