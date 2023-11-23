using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryCanvas : MonoBehaviour
{
    GameObject inventory;
    InventoryManager inventoryScript;
    bool invShowing = false;
    // Start is called before the first frame update
    void Start()
    {
        inventory = transform.GetChild(0).gameObject;
        inventoryScript = transform.GetChild(0).GetComponent<InventoryManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)){
            invShowing = !invShowing;
            inventory.SetActive(invShowing);
        } else if (Input.GetKeyDown(KeyCode.Alpha1)){
            inventoryScript.TryAddItem(GlobalItemManager.Clone(0));
        } else if (Input.GetKeyDown(KeyCode.Alpha2)){
            inventoryScript.TryAddItem(GlobalItemManager.Clone(1));
        }
    }
}
