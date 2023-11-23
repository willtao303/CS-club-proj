using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrackStateDefault : EnemyIdleState
{
    public EnemyTrackStateDefault(EnemyScript _enemy) : base(_enemy) {}

    public override bool CanEnterState() {
        return true;
    }

    public override void MovementUpdate(ref Vector2 vel) {}

    public override void Update(){}
}