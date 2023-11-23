using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyScript : MonoBehaviour
{
    protected Dictionary<string, EnemyState> enemyStates = new();

    protected EnemyState currentState;

    public Rigidbody2D rb;
    public Collider2D col;
    Vector2 vel = Vector2.zero;

    [SerializeField]
    public PlayerScript player;
    [SerializeField]
    public LayerMask groundLayer;

    public Stat speed;

    public bool debug;

    protected abstract void Initialize();

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        Initialize();
        currentState.EnterState();
    }

    void Update()
    {
        currentState.Update();
    }

    void FixedUpdate() {
        vel = rb.velocity;
        currentState.MovementUpdate(ref vel);
        rb.velocity = vel;
        if (debug){
            currentState.DrawDebug();
        }
    }

    public bool ChangeState(string nextState){
        EnemyState state;
        enemyStates.TryGetValue(nextState, out state);
        if(state == null || !state.CanEnterState()){
            return false;
        }
        currentState.ExitState();
        currentState = state;
        currentState.EnterState();
        return true;
    }
    
}
