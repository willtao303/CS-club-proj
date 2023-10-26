using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    [SerializeField]
    private Transform target;

    // how much the screen should move based off of mouses position
    [SerializeField]
    private float mouseMagnitude = 2.5f;

    [SerializeField]
    private float s_time = 0.15f;
    private Vector3 vel = Vector3.zero;

    [SerializeField]
    private bool smoothTracking = true;
    [SerializeField]
    private bool mouseTracking = true;

    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (smoothTracking){
            Vector3 target_mod = target.position;
            // sets z to -10 so that calculations are on same plane as camera
            target_mod.z = -10;
            // smooths out camera movement
            transform.position = Vector3.SmoothDamp(transform.position, target_mod, ref vel, s_time);
        } else {
            // sets camera position on player
            transform.position = target.position + Vector3.back*10;
        }

        // shifts camera based off of mouses distance from center
        if (mouseTracking) {
            Vector3 mouse_shift = Input.mousePosition;
            mouse_shift.x -= Screen.width/2;
            mouse_shift.y -= Screen.height/2;
            mouse_shift *= 0.0001f * mouseMagnitude;
            transform.position = transform.position + mouse_shift;
        }

    }


    /* tracking without acceleration
    private void SimpleTracking(){
        Vector2 distToTarget = (trackTsfm.position - this.transform.position);
        if (distToTarget.magnitude > 0.01) {
            this.transform.position += (Vector3)distToTarget/10;
        } else {
            this.transform.position = new Vector3(trackTsfm.transform.position.x, trackTsfm.transform.position.y, transform.position.z);
        }
    }*/
}
