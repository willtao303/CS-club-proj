using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleStateDefault : EnemyIdleState
{
    public EnemyIdleStateDefault(EnemyScript _enemy) : base(_enemy) {}

    public override void MovementUpdate(ref Vector2 vel) {
        vel = Vector2.zero;
        base.MovementUpdate(ref vel);
    }

    public override void Update(){}
}

public class EnemyIdleStateHover : EnemyIdleState
{
    Vector2 initialPosition;
    bool goingUp = false;
    public EnemyIdleStateHover(EnemyScript _enemy) : base(_enemy) {}

    public override void EnterState()
    {
        initialPosition = enemy.transform.position;
        base.EnterState();
    }
    public override void MovementUpdate(ref Vector2 vel) {
        
        if (goingUp){
            Vector2.SmoothDamp(enemy.transform.position, initialPosition + Vector2.up*0.5f, ref vel, 1f);
            if (Vector2.Distance(initialPosition + Vector2.up*0.5f, enemy.transform.position) < 0.05f){
                goingUp = false;
            }
        } else {
            Vector2.SmoothDamp(enemy.transform.position, initialPosition + Vector2.down*0.5f, ref vel, 1f);
            if (Vector2.Distance(initialPosition + Vector2.down*0.5f, enemy.transform.position) < 0.05f){
                goingUp = true;
            }
        }
        base.MovementUpdate(ref vel);
    }

    public override void Update(){}
}