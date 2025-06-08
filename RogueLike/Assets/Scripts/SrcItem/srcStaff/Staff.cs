using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : MonoBehaviour, IStaff
{
    // Behavior ============================
    private StaffBase staffBase;
    //======================================
    public void initStaff(StaffBase staffBase) {
        GetComponent<SpriteRenderer>().sprite = staffBase.Sprite;
    }
    public void attack()
    {
        Debug.Log("Atacando");
    }
}
