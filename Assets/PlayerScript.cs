using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField]
    private float playerSpeed = 2f;

    [SerializeField]
    private LayerMask layer;
    
    private Rigidbody2D playerRigidbody;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 final_vel = playerRigidbody.velocity;
        float x_vel = 0f;
        if (Input.GetKey(KeyCode.LeftArrow)) {
            x_vel -= playerSpeed;
        } else if (Input.GetKey(KeyCode.RightArrow)) {
            x_vel += playerSpeed;
        }

        if (playerRigidbody.velocity.x != x_vel){
            final_vel.x = x_vel;
        }
        Debug.DrawLine(((Vector2)transform.position) + (Vector2.down/2.1f), ((Vector2)transform.position) + (Vector2.down/2.1f));
        RaycastHit2D ray = Physics2D.Raycast(((Vector2)transform.position) + (Vector2.down/2.1f), Vector2.down, 0.1f, layer);
        Debug.Log(ray.point);
        // if there is no collider to hit, then the raycast didnt hit anything
        if (ray.collider != null && Input.GetKey(KeyCode.UpArrow)) {
            final_vel.y = 10f;
        }

        playerRigidbody.velocity = final_vel;
    }
}
