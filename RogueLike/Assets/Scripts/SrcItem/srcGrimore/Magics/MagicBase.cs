using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MagicBase : IMagic {
    protected float distance = 1;

    protected Vector2 posPlayer;
    protected Vector2 direction;
    protected GameObject handPlayer;
    public abstract Magic getEMagic();

    public virtual void magic(GameObject staff) {
        if (handPlayer == null) handPlayer = staff.transform.parent.gameObject;
        posPlayer = handPlayer.transform.position;
        direction = ((Vector2) staff.transform.position - posPlayer).normalized;
    }
    protected void conjure(GameObject staff, ProjectileBase project) {       
        GameObject obj = MonoBehaviour.Instantiate(ProjectileBank.GetMoldProject(), posPlayer + (direction*distance), Quaternion.Euler(0, 0, 0));
        obj.transform.parent = staff.transform.parent.parent.Find("Projects");
        obj.GetComponent<IProject>().insertProjectBase(project, staff.GetComponent<IStaff>().GetPlayerLinked());
        obj.GetComponent<IProject>().initMoviment(direction);
    }
}
