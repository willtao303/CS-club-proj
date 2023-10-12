using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField]
    private float playerSpeed = 0.2f;
    
    private Rigidbody2D playerRigidbody;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector3 movement = Vector3.zero;
        if (Input.GetKey(KeyCode.LeftArrow)) {
            movement += Vector3.left * playerSpeed;
        } else if (Input.GetKey(KeyCode.RightArrow)) {
            movement += Vector3.right * playerSpeed;
        } else if (Input.GetKey(KeyCode.UpArrow)) {
            movement += Vector3.up * playerSpeed;
        } else if (Input.GetKey(KeyCode.DownArrow)) {
            movement += Vector3.down * playerSpeed;
        }
    }
}
