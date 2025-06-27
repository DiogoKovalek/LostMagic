using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class ItemForColect : MonoBehaviour {
    public int itemId = 0;
    private float distAnim = 0.1f;
    private float speedAnim = 0.004f;
    private float quantMoved = 0;
    private bool isUp = true;
    void Start() {
        initItem();
    }
    void FixedUpdate() {
        if (isUp) {
            if (quantMoved < distAnim) {
                quantMoved += speedAnim;
                this.transform.Translate(new Vector3(0, speedAnim, 0), Space.World);
            }
            else {
                isUp = false;
            }
        }
        else {
            if (quantMoved > -distAnim) {
                quantMoved -= speedAnim;
                this.transform.Translate(new Vector3(0, -speedAnim, 0), Space.World);
            }
            else {
                isUp = true;
            }
        }
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
