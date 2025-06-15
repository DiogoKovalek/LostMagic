using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : MonoBehaviour, IStaff {
    // Behavior ============================
    private StaffBase staffBase;
    //======================================
    public void initStaff(StaffBase staffBase) {
        GetComponent<SpriteRenderer>().sprite = staffBase.Sprite;
    }
    public void attack(GameObject grimore) {
        grimore?.GetComponent<IGrimore>().magic(this.gameObject);
    }
}
