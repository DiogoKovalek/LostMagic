using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class ItemForColect : MonoBehaviour
{
    public int itemId = 0; 

    void Start() {
        initItem();
    }
    public void inserId(int id) {
        itemId = id;
        initItem(); // Atualiza
    }
    private void initItem() {
        if (itemId != 0) {
            GetComponent<SpriteRenderer>().sprite = ItemBank.GetItemFromId(itemId).Sprite;
        }
    }
    
    public int GetItem() {
        Destroy(this.gameObject);
        return itemId;
    }
}
