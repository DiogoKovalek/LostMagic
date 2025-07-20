using UnityEngine;

public class DestoyCreateQuicksand : MonoBehaviour
{
    public GameObject PrefQuicksand;

    private bool isQuitting = false;
    void OnDestroy() {
        if (isQuitting) return;
        GameObject fire = Instantiate(PrefQuicksand, transform.position, PrefQuicksand.transform.rotation);
        fire.transform.SetParent(this.transform.parent);
    }
    
    void OnApplicationQuit() {
        isQuitting = true;
    }
}
