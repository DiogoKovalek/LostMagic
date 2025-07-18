using UnityEngine;

public class DestroyCreateFire : MonoBehaviour
{
    public GameObject PrefFire;
    void OnDestroy() {
        GameObject fire = Instantiate(PrefFire, transform.position, PrefFire.transform.rotation);
        fire.transform.SetParent(this.transform.parent);
    }
}
