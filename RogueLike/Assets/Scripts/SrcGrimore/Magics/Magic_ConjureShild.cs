using UnityEngine;

public class Magic_ConjureShild : MonoBehaviour, IMagic {
    public GameObject prefShild;
    [SerializeField] Transform posCenter;
    [SerializeField] bool isPlayer = true;
    public void addConfigInMagic(Transform center) {
        if (isPlayer) posCenter = center.parent;
        else posCenter = center;
    }

    public void castMagic(Vector2 direction) {
        Instantiate(prefShild, posCenter.position, posCenter.rotation, posCenter);
    }
}
