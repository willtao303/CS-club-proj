using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWanderStateDefault : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class EnemyWanderStateFlightSharp : EnemyWanderState {
    Vector2 currentNode;
    Vector2 nextNode;
    float delay = 0f;
    float rangeMax = 5f;
    float rangeMin = 2f;
    public EnemyWanderStateFlightSharp(EnemyScript _enemy) : base(_enemy) {}

    public override bool CanEnterState() {
        return true;
    }
    public override void EnterState(){
        currentNode = enemy.transform.position;
        genNextNode();
    }
    public override void ExitState(){
        delay = 0;
        base.ExitState();
    }

    public override void MovementUpdate(ref Vector2 vel) {
        if (Vector2.Distance(enemy.transform.position, currentNode) < 0.2f){
            currentNode = nextNode;
            genNextNode();
            delay = Random.Range(0.6f,0.12f);
        } 
        if (delay < 0) {
            vel = (currentNode - (Vector2)enemy.transform.position).normalized * enemy.speed.Value()/10f;
        } else {
            vel = Vector2.zero;
            delay -= Time.deltaTime;
        }
        if (enemy.col.IsTouchingLayers(enemy.groundLayer)){
            currentNode = enemy.transform.position;
            genNextNode();
            ContactPoint2D[] contacts = new ContactPoint2D[1];
            enemy.col.GetContacts(contacts);
            vel = -(contacts[0].point - (Vector2)enemy.transform.position).normalized;
        }

        // TODO FOVRANGE
        Vector3 displacement = enemy.player.tf.position-enemy.transform.position;
        RaycastHit2D ray = Physics2D.Raycast(enemy.transform.position, displacement, 5f, enemy.groundLayer);
        if (ray.collider == enemy.player.col){
            queuedState = EnemyState.TRACK;
        }

        if (queuedState != null){
            enemy.ChangeState(queuedState);
        }
    }

    private void genNextNode(){
        bool isValid = false;
        for (int i = 0; i < 5 && !isValid; i++){
            Vector2 dirVector = new Vector2(Random.Range(-1f,1f), Random.Range(-0.5f,0.5f)).normalized;
            float dist = Random.value;
            dist = 0.4f + 0.5f*dist*dist;
            RaycastHit2D ray = Physics2D.Raycast(currentNode,dirVector, rangeMax+1, enemy.groundLayer);
            if (ray.collider == null){
                dist *= rangeMax;
            } else {
                if (ray.distance > rangeMin){
                    isValid = true;
                }
                dist *= ray.distance-1;
            }
            nextNode = currentNode + dist*dirVector;
        }
    }

    public override void Update(){}

    public override void DrawDebug()
    {
        Debug.DrawLine(enemy.transform.position, currentNode, Color.red);
        Debug.DrawLine(currentNode, nextNode, Color.blue);
    }
}
