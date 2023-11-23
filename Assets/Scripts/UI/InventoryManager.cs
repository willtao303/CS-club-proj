using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    const int cols = 9;
    const int rows = 5;
    Vector2Int start = new(-240, 120);
    int gap = 60;
    InventorySlot[,] inventorySlots = new InventorySlot[rows,cols];

    [SerializeField]
    GameObject TileSlot;
    
    void Start()
    {
        Vector2 currentPosition = start;
        for (int i = 0; i < rows; i++){
            for (int j = 0; j < cols; j++){
                GameObject currTile = GameObject.Instantiate(TileSlot, this.transform);
                currTile.transform.localPosition = currentPosition;
                inventorySlots[i,j] = currTile.GetComponent<InventorySlot>();
                inventorySlots[i,j].Init();
                currentPosition.x += gap;
            }
            currentPosition.y -= gap;
            currentPosition.x = start.x;
        }
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public bool TryAddItem(Item item){
        InventoryItem empty_slot = null;
        for (int i = 0; i < rows; i++){
            for (int j = 0; j < cols; j++){
                if (inventorySlots[i,j].itemSlot.TryMergeItem(item) == 0){
                    return true;
                } else if (empty_slot == null && inventorySlots[i,j].itemSlot.IsEmpty()){
                    empty_slot = inventorySlots[i,j].itemSlot;
                }
            }
        }
        if (empty_slot != null){
            empty_slot.SetItem(item, 1);
            return true;
        }
        return false;
    }
}
