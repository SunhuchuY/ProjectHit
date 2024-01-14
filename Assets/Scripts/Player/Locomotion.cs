using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

public class Locomotion : MonoBehaviour
{
    [SerializeField] private float _gravity = -9.8f;
    [SerializeField] private float _moveSpeed = 5f;

    private Vector3 _movementVelocity;
    private Animator _animator;
    private CharacterController _characterController;
    private Player.InputManager _inputManager;

    // Player
    [SerializeField] private Player.VFXManager _playerVFXManager;

    // Slide
    [Header("Player Slide")]
    [SerializeField] private float _slideSpeed = 4f;

    // Enemy
    [SerializeField] private bool _isPlayer = true;
    [SerializeField] private Enemy.VFXManager _enemyVFXManager;
    private UnityEngine.AI.NavMeshAgent _navMeshAgent;
    private Transform _targetPlayer;

    // Health
    private Health _health;

    // DamageCaster
    private DamageCaster _damageCaster;

    // Attack Slide
    private float attackStartTime;
    [SerializeField] private float attackSlideDuration = 0.4f;
    [SerializeField] private float attackSlideSpeed = 0.06f;

    // Being Hit
    private Vector3 _impactOnCharacter;

    // Material Animation
    private SkinnedMeshRenderer _skinnedMeshRenderer;
    private MaterialPropertyBlock _materialPropertyBlock;

    // Drop Item
    [SerializeField] private GameObject _dropItem;

    // Invincible
    private bool _isInvincible;
    private float _invincibleDuration;

    // Item
    private int _coin;

    // Spawn
    [Header("Spawn")]
    private float _spawnDuration = 2f;
    private float _spawnTime;


    public enum State
    {
        Normal, Attacking, Dead, BeingHit, Slide, Spawn
    }

    public State currentState { get; private set; } = State.Normal;


    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();  
        _animator = GetComponent<Animator>();
        _health = GetComponent<Health>();
        _damageCaster = GetComponentInChildren<DamageCaster>(); 

        _skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        _materialPropertyBlock = new MaterialPropertyBlock();
        _skinnedMeshRenderer.GetPropertyBlock(_materialPropertyBlock);

        if (_isPlayer)
        {
            _inputManager = GetComponent<Player.InputManager>();
        }
        else
        {
            _navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            _navMeshAgent.speed = _moveSpeed;
            _targetPlayer = GameObject.FindWithTag("Player").transform;

            SwitchToState(State.Spawn);
        }
    }

    private void FixedUpdate()
    {
        switch (currentState)
        {
            case State.Normal:

                if(_isPlayer)
                {
                    CalculatePlayerMovement();
                }
                else
                {
                    CalculateEnemyMovement();
                }
                break;
            
            case State.Attacking:

                if (_isPlayer)
                {
                    string currentClipName = _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
                    Debug.Log(currentClipName);

                    if (Time.time < attackStartTime + attackSlideDuration && currentClipName == "LittleAdventurerAndie_ATTACK_01")
                    {
                        float timePassed = Time.time - attackStartTime;
                        float lerpTime = timePassed / attackSlideDuration;
                        _movementVelocity = Vector3.Lerp(transform.forward, Vector3.zero, lerpTime);
                    }

                    if(_inputManager.isMouseButtonDown && _characterController.isGrounded)
                    {
                        float currentClipPassedTime = _animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

                        if (currentClipName != "LittleAdventurerAndie_ATTACK_03" && currentClipPassedTime > 0.5f && currentClipPassedTime < 0.8f)
                        {
                            CalculatePlayerMovement();
                        }
                    }
                }
                break;

            case State.Dead:
                return;

            case State.BeingHit:
                
                if(_impactOnCharacter.magnitude > 0.2f)
                {
                    _movementVelocity = _impactOnCharacter * Time.deltaTime;
                }

                _impactOnCharacter = Vector3.Lerp(_impactOnCharacter, Vector3.zero, Time.deltaTime * 5);

                break;

            case State.Slide:
                _movementVelocity = transform.forward * _slideSpeed * Time.deltaTime;

                break;

            case State.Spawn:
                _spawnTime -= Time.deltaTime;
                if (_spawnTime <= 0)
                {
                    SwitchToState(State.Normal);
                }
                break;
        }

        if(_isPlayer)
        {
            // memo: Move And Gravity
            CalculateGravityUpdate();
            _characterController.Move(_movementVelocity);

            _movementVelocity = Vector3.zero;
        }
    }

    private void CalculatePlayerMovement()
    {
        if (_inputManager.isMouseButtonDown && _characterController.isGrounded)
        {
            SwitchToState(State.Attacking);
            return;
        }
        else if (_inputManager.isSpaceDown && _characterController.isGrounded)
        {
            SwitchToState(State.Slide);
            return;
        }

            _movementVelocity = new Vector3(_inputManager.movementInput.horizontal, 0f, _inputManager.movementInput.vertical);
        _movementVelocity.Normalize();
        _movementVelocity = Quaternion.Euler(0f, -45f, 0f) * _movementVelocity;

        _animator.SetFloat("Speed", _movementVelocity.magnitude);
        
        _movementVelocity *= _moveSpeed * Time.deltaTime;

        if (_movementVelocity != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(_movementVelocity);

        _animator.SetBool("isAirBorne", !_characterController.isGrounded);
    }

    private void CalculateEnemyMovement()
    {
        if(Vector3.Distance(_targetPlayer.position, transform.position) >= _navMeshAgent.stoppingDistance)
        {
            _navMeshAgent.SetDestination(_targetPlayer.position);
            _animator.SetFloat("Speed", 0.2f);
        }
        else
        {
            _navMeshAgent.SetDestination(transform.position);
            _animator.SetFloat("Speed", 0f);

            SwitchToState(State.Attacking);
        }
    }

    private void CalculateGravityUpdate()
    {
        float verticalVelocity;

        if(_characterController.isGrounded)
        {
            verticalVelocity = _gravity * 0.3f;
        }
        else
        {
            verticalVelocity = _gravity;
        }

        _movementVelocity += verticalVelocity * Vector3.up * Time.deltaTime;
    }

    public void SwitchToState(State newState)
    {
        // Clear Cache
        if (_isPlayer)
        {
            _inputManager.ClearCache();
        }

        // Current State
        switch (currentState)
        {
            case State.Normal:
                break;

            case State.Attacking:

                if(_damageCaster != null)
                {
                    _damageCaster.DisableDamageCaster();
                }

                if(_isPlayer)
                {
                    _playerVFXManager.StopBlade();
                }
                break;

            case State.Dead:
                return;

            case State.BeingHit:
                break;

            case State.Slide:
                break;

            case State.Spawn:
                _isInvincible = false;
                break;
        }

        // New State
        switch (newState)
        {
            case State.Normal:
                break;

            case State.Attacking:
                if(!_isPlayer)
                {
                    Vector3 dir = _targetPlayer.position - transform.position; // 타겟 방향
                    Quaternion targetRotation = Quaternion.LookRotation(dir); // 목표 회전
                    transform.DORotateQuaternion(targetRotation, 0.3f)
                        .OnComplete(() => 
                        {
                            _animator.SetTrigger("Attack");
                        });
                }


                if (_isPlayer)
                {
                    attackStartTime = Time.time;    
                    _animator.SetTrigger("Attack");
                }
                break;

            case State.Dead:
                _characterController.enabled = false;
                _animator.SetTrigger("Dead");
                StartCoroutine(MaterialDissolve());
                break;

            case State.BeingHit:
                _animator.SetTrigger("BeingHit");
                _isInvincible = true;
                StartCoroutine(DelayFalseInvincible());
                break;

            case State.Slide:
                _animator.SetTrigger("Slide");
                break;

            case State.Spawn:
                _isInvincible = true;
                _spawnTime = _spawnDuration;
                StartCoroutine(MaterialAppear());
                break;
        }

        currentState = newState;
    }

    public void SlideAnimationEnds()
    {
        SwitchToState(State.Normal);
    }

    public void AttackAnimationEnds()
    {
        SwitchToState(State.Normal);
    }

    public void BeingHitAnimationEnds()
    {
        SwitchToState(State.Normal);
    }

    public void ApplyDamage(int damage, Vector3 attackerPos)
    {
        // Exit
        if(_isInvincible)
        {
            return;
        }

        _health.ApplyDamage(damage);

        if (!_isPlayer)
        {
            _enemyVFXManager.BeingHitVFX(attackerPos);
        }

        StartCoroutine(MaterialBlink());

        if (_isPlayer)
        {
            SwitchToState(State.BeingHit);
            AddImpact(attackerPos, 10f);
        }
    }

    public void EnableDamageCaster()
    {
        _damageCaster.EnableDamageCaster();
    }

    public void DisableDamageCaster()
    {
        _damageCaster.DisableDamageCaster();
    }

    private IEnumerator MaterialBlink()
    {
        _materialPropertyBlock.SetFloat("_blink", 0.4f);
        _skinnedMeshRenderer.SetPropertyBlock(_materialPropertyBlock);

        yield return new WaitForSeconds(0.2f);

        _materialPropertyBlock.SetFloat("_blink", 0);
        _skinnedMeshRenderer.SetPropertyBlock(_materialPropertyBlock);
    }

    private IEnumerator MaterialDissolve()
    {
        yield return new WaitForSeconds(2);

        float currentTime = 0;
        float duration = 2;
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

        DropItem();
    }

    private IEnumerator DelayFalseInvincible()
    {
        yield return new WaitForSeconds(_invincibleDuration);
        _isInvincible = false;

    }

    private void DropItem()
    {
        if(_dropItem != null)
        {
            Instantiate(_dropItem, transform.position, Quaternion.identity);
        }
    }

    private void AddImpact(Vector3 attackPos, float force)
    {
        Vector3 dir = transform.position - attackPos;
        dir.Normalize();
        dir.y = 0;
        _impactOnCharacter = dir * force;
    }

    public void PickUpItem(PickUp item)
    {
        switch (item.type)
        {
            case PickUp.ItemType.Heal:
                AddHealth(item.value);
                break;

            case PickUp.ItemType.Coin:
                AddCoin(item.value);
                break;
        }
    }

    private void AddHealth(int health)
    {
        _health.AddHealth(health);

        if (_isPlayer)
        {
            _playerVFXManager.PlayHeal();
        }
    }
        
    private void AddCoin(int coin)
    {
        _coin += coin;
    }

    public void RotateToTarget()
    {
        if (currentState == State.Attacking)
        {
            Vector3 dir = (_targetPlayer.position - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
        }
    }

    private IEnumerator MaterialAppear()
    {
        float currentTime = 0;
        float duration = _spawnDuration;
        float dissolve_Start = -10;
        float dissolve_Target = 20;
        float dissolve_Current;


        _materialPropertyBlock.SetFloat("_enableDissolve", 1);
        _skinnedMeshRenderer.SetPropertyBlock(_materialPropertyBlock);

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            _materialPropertyBlock.SetFloat("_enableDissolve", 1);
            dissolve_Current = Mathf.Lerp(dissolve_Start, dissolve_Target, currentTime / duration);
            _materialPropertyBlock.SetFloat("_dissolve_height", dissolve_Current);
            _skinnedMeshRenderer.SetPropertyBlock(_materialPropertyBlock);
            yield return null;
        }

        _materialPropertyBlock.SetFloat("_enableDissolve", 0);
        _skinnedMeshRenderer.SetPropertyBlock(_materialPropertyBlock);
    }
}


