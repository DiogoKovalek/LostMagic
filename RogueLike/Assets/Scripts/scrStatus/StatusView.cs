using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class StatusView : MonoBehaviour {
    public GameObject iconBurning;
    public GameObject iconStunned;
    public GameObject iconWet;
    private List<GameObject> status = new List<GameObject>();

    private float distElements = 0.5f;
    public void addStatus(Status status) {
        GameObject s = Instantiate(getIcon(status), this.transform.position, this.transform.rotation, this.transform);
        this.status.Add(s);
        updatePos();
    }
    public void removeStatus(Status status) {
        GameObject s = getListByStatus(status);
        this.status?.Remove(s);
        Destroy(s);
        updatePos();
    }
    private void updatePos() {
        int spaces = status.Count - 1;
        float eixoW = spaces * distElements / 2;
        for (int i = 0; i < status.Count; i++) {
            status[i].transform.position = new Vector2(-eixoW + i * distElements, 0) + (Vector2)transform.position;
        }
    }

    private GameObject getIcon(Status status) {
        switch (status) {
            case Status.Burning:
                return iconBurning;
            case Status.Stunned:
                return iconStunned;
            case Status.Wet:
                return iconWet;
            default:
                return null;
        }
    }
    private GameObject getListByStatus(Status status) {
        foreach (var s in this.status) {
            if (s.GetComponent<VariableStatus>().status == status) {
                return s;
            }
        }
        return null;
    }
}
