using UnityEngine;

public class WaterShild : MonoBehaviour
{
    private StatusManager statusManager;
    void Start() {
        statusManager = transform.parent.GetComponentInChildren<StatusManager>();
        statusManager.AplicateStatus(Element.Water);
    }
}
