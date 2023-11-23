using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemySleepStateDefault : EnemySleepState
{
    public EnemySleepStateDefault(EnemyScript _enemy) : base(_enemy)
    {
    }

    public override void MovementUpdate(ref Vector2 vel) {
        throw new System.NotImplementedException();
    }

    public override void Update()
    {
        throw new System.NotImplementedException();
    }
}