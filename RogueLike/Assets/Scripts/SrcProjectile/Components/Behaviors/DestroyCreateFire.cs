using UnityEngine;

public class DestroyCreateFire : MonoBehaviour {
    public GameObject PrefFire;

    private bool isQuitting = false;
    void OnDestroy() {
        if (isQuitting) return;
        GameObject fire = Instantiate(PrefFire, transform.position, PrefFire.transform.rotation);
        fire.transform.SetParent(this.transform.parent);
    }
    
    void OnApplicationQuit() {
        isQuitting = true;
    }
}
