using System;
using UnityEngine;


[Serializable]
class PlayerMovementDash : PlayerMovement{
    
    [SerializeField]
    private float dashSpeed = 15f; // increase dashSpeed for longer dashes
    private readonly float dashDamp = 1.8f; // increase dashDamp for sharper dashes. increasing this will also shorten dashes
    [SerializeField]
    private float dashCooldown = 0.9f; // dash cooldown in seconds
  

    public PlayerMovementDash(PlayerScript player) : base(player){}

    override public bool Available(){
        return (player.dashNum != 0 && player.dashTimer <= 0);
    }
    override public void Update(ref Vector2 vel){

        // if player is no longer moving fast enough to be considered dashing
        if (vel.magnitude < player.speed.Value()){
            // ensure gravity is still occuring
            if (player.rb.gravityScale != 1.0f){
                player.rb.gravityScale = 3.0f;
            }
            // switch back to default movement
            player.ChangeMvmt(PlayerMovement.Default);
        } else {
            vel = vel - vel.normalized*dashDamp;
        }

        // if the player has dashes remaining and cooldown is available
        if (player.dashNum != 0 && player.dashTimer <= 0){
            Vector2 dir = player.facingDirection;
            // add an impulse force to the player, causing the player to dash
            vel = dir.normalized*(dashSpeed+player.speed.Value());
            // reset the dash timer
            player.dashTimer = dashCooldown;
            player.dashNum--;
            player.rb.gravityScale = PlayerScript.baseGravityScale;
        }
    }

    public void AddDash(int val = 1, bool immediate = true){
        if (immediate) {
            player.baseDashNum += val;
        }
        player.dashNum += val;
    }
}

[Serializable]
class PlayerMovementInput : PlayerMovement{
    public PlayerMovementInput(PlayerScript _player) : base(_player){}
    private int slideAmount = 1; // smaller = more slide

    public override bool Available()
    {
        return true;
    }
    public override void Update(ref Vector2 vel){
        vel = player.rb.velocity;
        // velocity from input
        float input_vel_x = Input.GetAxisRaw("Horizontal") * player.speed.Value();

        if (input_vel_x == 0) {
            // "decreases" velocity by 1, clamps it at 0
            // * since velocity can be negative, decreasing negative velocity means increasing it
            if (vel.x < 0){
                vel.x = Mathf.Min(vel.x + slideAmount, 0);
            } else if (vel.x > 0) {
                vel.x = Mathf.Max(vel.x - slideAmount, 0);
            }
        } else {
            vel.x = input_vel_x;
        }

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey("w")) {
            if (player.OnGround()){
                // in that case, add a vertical velocity, causing player to jump
                vel.y = 10f * player.jumpHeight;
            }
        }

        // caps the falling velocity at 20 units per second, so the player doesnt fall too fast
        if (vel.y < -20) {
            vel.y = -20;
        } 

        if (Input.GetKey(Keybinds.DashKey)){
            player.ChangeMvmt(PlayerMovement.Dash);
        }
    }
}

abstract class PlayerMovement {
    protected PlayerScript player;
    public const String Dash = "Dash";
    public const String Default = "Default";

    public PlayerMovement(PlayerScript _player){
        player = _player;
    }

    public abstract bool Available();

    public abstract void Update(ref Vector2 vel);

    
}