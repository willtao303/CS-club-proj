using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public InventoryItem itemSlot;
    [SerializeField]
    GameObject itemSlotPrefab;

    public void Init() {
        GameObject temp = GameObject.Instantiate(itemSlotPrefab, this.transform);
        itemSlot = temp.GetComponent<InventoryItem>();
        itemSlot.Init();
    }

    void Update()
    {
    }

    public void OnDrop(PointerEventData eventData) {
        InventoryItem other = eventData.pointerDrag.GetComponent<InventoryItem>();
        if (!itemSlot.MergeItem(other)){ 
            itemSlot.SwapItem(other);
        }
    }

}
