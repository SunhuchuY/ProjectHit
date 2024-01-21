using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySM : StateMachine
{
    EnemyController Controller;

    public EnemySM(EnemyController controller)
    {

        Controller = controller;

        this.AddState(new EnemyAttackState(Controller));
        this.AddState(new EnemyDeadState(Controller));
        this.AddState(new EnemyBeingHitState(Controller));
        this.AddState(new EnemyNormalState(Controller, this));
        this.AddState(new EnemySpawnState(Controller, this));
    }
    
    public new void Update()
    {
        base.Update();

        // execute: impact
        if (Controller._impactOnCharacter.magnitude > 0.2f)
        {
            Controller._movementVelocity = Controller._impactOnCharacter * Time.deltaTime;
        }
        Controller._impactOnCharacter = Vector3.Lerp(Controller._impactOnCharacter, Vector3.zero, Time.deltaTime * 5);

        if (this._currentState.GetType() != typeof(EnemyNormalState))
        {
            Controller._characterController.Move(Controller._movementVelocity);
            Controller._movementVelocity = Vector3.zero;
        }
    }
}