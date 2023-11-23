using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    // Player Constants / Stats
    public Stat speed = new Stat(7f);
    public Stat maxHP = new Stat(5);
    //public Stat maxMP = new Stat(5);
    public float jumpHeight = 1.13f;
    public const float baseGravityScale = 3.0f;

    // Variables
    public Vector2 facingDirection = Vector2.right;
    public float dashTimer = 0;

    public int baseDashNum = 1; // base number of dashes left
    [ShowOnly]
    public int dashNum = 1; // current number of dashes left

    // Components
    public Transform tf;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Collider2D col;
    public Vector2 groundPos;
    public Vector2 vel = Vector2.zero;

    // ------ movement ------
    private readonly Dictionary<string, PlayerMovement> MovementModules = new();
    private PlayerMovement currentMovementModule;
    
    // grounding
    private Vector2 boxcast_size;
    private float boxcast_dist;
    [SerializeField]
    private LayerMask groundLayer;

    void Start()
    {
        tf = this.transform;// GetComponent<Transform>();
        groundPos = tf.position;
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        boxcast_size = tf.lossyScale*0.9f;
        boxcast_dist = tf.lossyScale.y*0.1f + 0.05f;
        //groundLayer = LayerMask.NameToLayer("Ground");

        MovementModules.Add(PlayerMovement.Dash, new PlayerMovementDash(this));
        MovementModules.Add(PlayerMovement.Default, new PlayerMovementInput(this));
        //MovementModules.Add("Climb", new PlayerMovementInput(this));
        MovementModules.TryGetValue(PlayerMovement.Default, out currentMovementModule);
    }

    public bool debug = false;

    void Update()
    {
        UpdateStats();
    
        RaycastHit2D ray = Physics2D.Raycast(tf.position, Vector2.down, Mathf.Infinity, groundLayer);
        groundPos = ray.point;
        if (debug){
            Debug.DrawLine(tf.position, groundPos, Color.blue);
            Debug.DrawLine(tf.position, (Vector2)tf.position + facingDirection, Color.red);
        }
    }

    void FixedUpdate(){// setting facing direciton
        if (Input.GetAxisRaw("Horizontal") != 0){
            facingDirection.x = (int)Input.GetAxisRaw("Horizontal");
        }
        facingDirection.y = (int)Input.GetAxisRaw("Vertical");
        UpdateMvmt();
        currentMovementModule.Update(ref vel);
        rb.velocity = vel;
    }

    private void UpdateStats(){
        speed.Update();
        maxHP.Update();
    }

    // all updates related to movement conditions
    private void UpdateMvmt(){
        if (dashTimer > 0){
            dashTimer -= Time.deltaTime;
        }
        if (dashNum != baseDashNum){
            if (OnGround()){
                dashNum = baseDashNum;
            }
        }
    }
    public bool ChangeMvmt(String next_module){
        PlayerMovement next_mod ;
        MovementModules.TryGetValue(next_module, out next_mod);
        if (next_mod == null || !next_mod.Available()){
            return false;
        }
        currentMovementModule = next_mod;
        return true;
    }



    public bool OnGround(){
        // cast a box straight down, to a distance of boxcast_dist
        return (
            Physics2D.BoxCast(
                (Vector2)tf.position, // start at the players center
                boxcast_size, // cast a box slightly smaller than the player (calculated in Start())
                0, // you dont want the box to be tilted, angle = 0
                Vector2.down, // the direction of the boxcast
                boxcast_dist, // the distance the boxcast should travel
                groundLayer // only hit things on the ground layer
            ).collider != null // if there is a collider to process, then the boxcast hit something
            && rb.velocity.y <= 0 // and player is falling
        );
    }
}

