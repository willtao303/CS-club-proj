using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyState {

    // not using struct for debugging purposes
    public const string TRACK = "track";
    public const string IDLE = "idle";

    protected EnemyScript enemy;
    protected string queuedState = null;
    protected EnemyState(EnemyScript _enemy){
        enemy = _enemy;
    }
    public abstract void Update();
    public virtual void EnterState(){}
    public virtual void ExitState(){
        queuedState = null;
    }
    public abstract void MovementUpdate(ref Vector2 vel);
    public virtual bool CanEnterState(){return true;}

    public virtual void DrawDebug(){}
}

// Purely for debugging
public class EnemyNullState : EnemyState {
    public EnemyNullState(EnemyScript _enemy) : base(_enemy){}
    public override bool CanEnterState(){
        return false;}
    public override void MovementUpdate(ref Vector2 vel){}
    public override void Update(){}
}

public abstract class EnemySleepState : EnemyState {
    bool sit = false;
    protected EnemySleepState(EnemyScript _enemy) : base(_enemy) {}
    public override bool CanEnterState(){
        return true;
    }
}

public abstract class EnemySupriseState : EnemyState {
    protected EnemySupriseState(EnemyScript _enemy) : base(_enemy) {}
}

public abstract class EnemyIdleState : EnemyState {
    protected EnemyIdleState(EnemyScript _enemy) : base(_enemy){}

    public override void MovementUpdate(ref Vector2 vel) {
        Vector3 displacement = enemy.player.tf.position-enemy.transform.position;
        RaycastHit2D ray = Physics2D.Raycast(enemy.transform.position, displacement, 5f, enemy.groundLayer);
        if (ray.collider == enemy.player.col){
            queuedState = EnemyState.TRACK;
        }
    }
}

public abstract class EnemyWanderState : EnemyState {
    protected EnemyWanderState(EnemyScript _enemy) : base(_enemy) {
    }
}

public abstract class EnemyTrackState : EnemyState {
    protected EnemyTrackState(EnemyScript _enemy) : base(_enemy) {}
}

public abstract class EnemyAttackState : EnemyState {
    protected EnemyAttackState(EnemyScript _enemy) : base(_enemy) {}
}
