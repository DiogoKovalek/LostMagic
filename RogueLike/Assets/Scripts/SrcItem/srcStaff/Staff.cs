using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : MonoBehaviour, IStaff {
    // Behavior ============================
    private StaffBase staffBase;
    private GameObject playerLinked;
    private Animator anim;
    //======================================
    public void initStaff(StaffBase staffBase, GameObject playerLinked) {
        this.staffBase = staffBase;
        anim = GetComponent<Animator>();
        anim.runtimeAnimatorController = this.staffBase.runAnim;
        this.playerLinked = playerLinked;
    }
    public void attack(GameObject grimore, Vector2 direction) {
        grimore?.GetComponent<IGrimore>().conjureMagic(direction);
    }

    public GameObject GetPlayerLinked() {
        return this.playerLinked;
    }

    public int getIdItem() {
        return staffBase.id;
    }
}
