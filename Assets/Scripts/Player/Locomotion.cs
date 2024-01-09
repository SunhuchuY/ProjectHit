using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Locomotion : MonoBehaviour
{
    [SerializeField] private float _gravity = -9.8f;
    [SerializeField] private float _moveSpeed = 5f;

    private Vector3 _movementVelocity;
    private Animator _animator;
    private CharacterController _characterController;
    private Player.InputManager _inputManager;

    // Enemy
    [SerializeField] private bool _isPlayer = true;
    private UnityEngine.AI.NavMeshAgent _navMeshAgent;
    private Transform _targetPlayer;

    public enum State
    {
        Normal, Attacking
    }

    private State currentState = State.Normal;


    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();  
        _animator = GetComponent<Animator>();

        if (_isPlayer)
        {
            _inputManager = GetComponent<Player.InputManager>();
        }
        else
        {
            _navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            _navMeshAgent.speed = _moveSpeed;
            _targetPlayer = GameObject.FindWithTag("Player").transform;
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
                break;
        }

        if(_isPlayer)
        {
            CalculateGravityUpdate();
            _characterController.Move(_movementVelocity);

            if (!_inputManager.isMouseButtonDown)
            {
                SwitchToState(State.Normal);
            }
        }
    }

    private void CalculatePlayerMovement()
    {
        if (_inputManager.isMouseButtonDown && _characterController.isGrounded)
        {
            SwitchToState(State.Attacking);
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

    private void SwitchToState(State newState)
    {
        // Clear Cache
        _inputManager.isMouseButtonDown = false;


        // Current State
        switch (currentState)
        {
            case State.Normal:
                break;

            case State.Attacking:
                break;
        }


        // New State
        switch (currentState)
        {
            case State.Normal:
                break;

            case State.Attacking:
                break;
        }


        currentState = newState;

        Debug.Log("Change To State: " + currentState);
    }

}


