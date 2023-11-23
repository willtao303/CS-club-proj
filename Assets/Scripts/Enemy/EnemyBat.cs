using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBat : EnemyScript
{
    protected override void Initialize()
    {
        enemyStates.Add(EnemyState.IDLE, new EnemyIdleStateHover(this));
        enemyStates.Add(EnemyState.TRACK, new EnemyNullState(this));
        enemyStates.TryGetValue(EnemyState.IDLE, out currentState);

        speed = new Stat(60.0f);
    }
}
