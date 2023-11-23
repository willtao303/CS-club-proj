//using System.Collections;
//using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Transform tf;
    Image img;
    TMP_Text counter;
    
    private Item _item = null; private int _count = 0;

    public Item item{
        get{return _item;} 
        set{_item = value; RefreshSprite();} 
    }
    private int count{
        get{return _count;} 
        set{_count = value; RefreshCounter();} 
    }

    private static Vector3 default_pos = new(0, 0, -1);
    public void Init() {
        tf = this.transform; 
        tf.localPosition = default_pos;
        img = GetComponent<Image>();
        counter = transform.GetChild(0).GetComponent<TMP_Text>();
        counter.gameObject.SetActive(false);
    }

    void Update() {
        if (Vector3.Distance(transform.position, Input.mousePosition) < 30){
            if (Input.GetMouseButtonDown(0)){
                Debug.Log(item + " x" + count.ToString());
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData){
        img.raycastTarget = false;
        ////throw new System.NotImplementedException();
    }

    public void OnDrag(PointerEventData eventData){
        if (item != null){
            tf.position = Input.mousePosition + default_pos*2;
        }
    }

    public void OnEndDrag(PointerEventData eventData){
        img.raycastTarget = true;
        tf.localPosition = default_pos;
    }

    public void SetItem(Item _item, int _count){
        item = _item;
        count = _count;
    }

    public int TryAddItem(Item i, int itemCount = 1){
        if (item == null){
            item = i;
            count = Mathf.Min(itemCount, i.maxStack);
            return Mathf.Max(itemCount - i.maxStack, 0);
        } 
        return TryMergeItem(i, itemCount);
    }
    public int TryMergeItem(Item i, int itemCount = 1){
        if (item != null && i != null && i.id == item.id){
            if (item.maxStack == count) return itemCount;
            int item_overflow = count + itemCount - item.maxStack;
            count = Mathf.Min(item.maxStack, count+itemCount);
            return Mathf.Max(0, item_overflow);
        }
        return itemCount;
    }

    public bool MergeItem(InventoryItem other){
        if (item == null || other.item == null || other.item.id != item.id) return false;
        int item_overflow = TryMergeItem(other.item, other.count);
        if (item_overflow == 0) {
            this.item = null;
        } else {
            this.count = item_overflow;
        }
        return true;
    }

    public void SwapItem(InventoryItem other){
        if (item == null && other.item == null) return;
        (other.item, item) = (item, other.item);
        (other.count, count) = (count, other.count);
    }

    public void RemoveItem(int itemCount){
        count -= itemCount;
        if (count <= 0){
            item = null;
            count = 0;
        }
    }
    public void RemoveAllItem() {
        item = null;
        count = 0;
    }

    public bool IsEmpty(){
        return item == null;
    }

    public void RefreshSprite(){
        if (item != null){
            img.sprite = item.sprite;
        } else {
            img.sprite = GlobalItemManager.item_none;
        }
    }

    public void RefreshCounter(){
        if (count <= 1){
            counter.gameObject.SetActive(false);
        } else {
            counter.gameObject.SetActive(true);
            counter.text = count.ToString();
        }
    }
}
