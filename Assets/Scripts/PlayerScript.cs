using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    // Player Constants / Stats

    public Stat playerSpeed = new Stat(7f);
    public Stat playerMaxHP = new Stat(5);
    //public Stat playerMaxMP = new Stat(5);
    private float jumpHeight = 1.1f;
    
    [SerializeField]
    private float dashSpeed = 4f; // increase dashSpeed for longer dashes
    [SerializeField]
    private float dashDamp = 0.8f; // increase dashDamp for sharper dashes. increasing this will also shorten dashes
    [SerializeField]
    private float dashCooldown = 0.9f; // dash cooldown in seconds

    // Keybinds
    private const KeyCode DashKey = KeyCode.LeftControl;

    // Variables
    private int facingDirection = 1;
    private float dashTimer = 0;

    public bool overrideVelociy = false;
    public Vector3 overridedVelocity = Vector3.zero;

    // Components
    [SerializeField]
    private LayerMask groundLayer;
    
    private Rigidbody2D playerRigidbody;
    private Vector2 boxcast_size;
    private float boxcast_dist;
    

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        boxcast_size = transform.lossyScale*0.9f;
        boxcast_dist = transform.lossyScale.y*0.1f + 0.05f;
    }

    void Update()
    {
        if (!overrideVelociy){
            PlayerMovement();
        } else {
            playerRigidbody.velocity = overridedVelocity;
        }

        UpdateStats();
    }

    // function to control all of players movement
    private void PlayerMovement(){
        Vector2 vel_final = playerRigidbody.velocity;
        bool prob_dashing = false;

    // #---------------------------   calculating horizontal component of velocity   ---------------------------#

        float vel_x = Input.GetAxisRaw("Horizontal") * playerSpeed.Value();

        // if the player is moving fast enough, then they are prob dashing
        //   for more accurate results, you would calculate the 
        //   dash time based off of the dash_speed and dash_damp
        //   however, unity's internal caluclations are unknown, 
        //   thus we approximate.
        if (playerSpeed.Value() < playerRigidbody.velocity.x || playerRigidbody.velocity.x < -playerSpeed.Value()) {
                prob_dashing = true;
        }
        if (vel_x < 0) {
            // ensures that the new velocity isnt overriding a greater velocity, 
            // only changes velocity if the old velocity was "less" than new velocity
            if (playerRigidbody.velocity.x >= vel_x) {
                vel_final.x = vel_x;
            // if velocity is greater, just decrement it, dont override. clamp it at the vel_xocity
            } else {
                vel_final.x = Mathf.Min(vel_final.x + dashDamp, vel_x);
            }
        } else if (vel_x > 0){
            if (playerRigidbody.velocity.x <= vel_x) {
                vel_final.x = vel_x;
            } else {
                vel_final.x = Mathf.Max(vel_final.x - dashDamp, vel_x);
            }
        } else if (vel_x == 0) {
            // "decreases" velocity by 1, clamps it at 0
            // * since velocity can be negative, decreasing negative velocity means increasing it
            if (vel_final.x < 0){
                vel_final.x = Mathf.Min(vel_final.x + dashDamp, 0);
            } else if (vel_final.x > 0) {
                vel_final.x = Mathf.Max(vel_final.x - dashDamp, 0);
            }
        }

    // #---------------------------   calculating vertical component of velocity   ---------------------------#
        
        // If a left/right button was pressed, set the direction player is facing to left/right
        if (Input.GetAxisRaw("Horizontal") != 0){
            facingDirection = (int)Input.GetAxisRaw("Horizontal");
        }

        // cast a box straight down, to a distance of boxcast_dist
        RaycastHit2D ray = Physics2D.BoxCast(
            (Vector2)transform.position, // start at the players center
            boxcast_size, // cast a box slightly smaller than the player (calculated in Start())
            0, // you dont want the box to be tilted, angle = 0
            Vector2.down, // the direction of the boxcast
            boxcast_dist, // the distance the boxcast should travel
            groundLayer // only hit things on the ground layer
        );

        // if there is no collider to process, then the boxcast didnt hit anything
        if (ray.collider != null && (Input.GetKey(KeyCode.UpArrow) || Input.GetKey("w"))) {
            // in that case, add a vertical velocity, causing player to jump
            vel_final.y = 10f * jumpHeight;
        }

        // caps the falling velocity at 20 units per second, so the player doesnt fall too fast
        if (vel_final.y < -20) {
            vel_final.y = -20;
        }

        // if player is dashing, dont move
        if (prob_dashing && dashTimer > 0){
            vel_final.y = 0;
        }

        

        // sets the players velocity to new calculated velocity
        playerRigidbody.velocity = vel_final;

        
        // if the player can dash, or the cooldown timer has been reached, 
        if (dashTimer < 0) {
            // and the dash button has been pressed (DashKey defined in constants)
            if (Input.GetKey(DashKey)){
                // add an impulse force to the player, causing the player to dash
                playerRigidbody.AddForce(new Vector2(facingDirection*dashSpeed*playerSpeed.Value(), 0), ForceMode2D.Impulse);
                // reset the dash timer
                dashTimer = dashCooldown;
            }
        } else {
            // otherwise, decrement the dash cooldown timer
            dashTimer -= Time.deltaTime;
        }
    }

    private void UpdateStats(){
        playerSpeed.Update();
        playerMaxHP.Update();
    }
}

