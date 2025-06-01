using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class ItemForColect : MonoBehaviour
{
    public int itemId; 

    void Start() {
        GetComponent<SpriteRenderer>().sprite = ItemBank.GetItemFromId(itemId).Sprite;
    }
    
    public int GetItem(){
        Destroy(this.gameObject);
        return itemId;
    }
}
