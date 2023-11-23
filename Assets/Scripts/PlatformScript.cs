using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class PlatformScript : MonoBehaviour
{
    PlatformEffector2D plat_eff;
    [SerializeField]
    private Collider2D player_col;
    private Collider2D col;
    private bool flipped = false;
    
    private ContactFilter2D cFil = new();
    private List<Collider2D> cRes = new();
    
    // Start is called before the first frame update
    void Start()
    {
        plat_eff = GetComponent<PlatformEffector2D>();
        col = GetComponent<TilemapCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (Input.GetKey("s") || Input.GetKey(KeyCode.DownArrow)){
            plat_eff.rotationalOffset = 180;
            flipped = true;
        } else {
            if (flipped){
                col.OverlapCollider(cFil, cRes);
                if (!cRes.Contains(player_col)){
                    plat_eff.rotationalOffset = 0;
                    flipped = false;
                }
            }
        }

    }
}
