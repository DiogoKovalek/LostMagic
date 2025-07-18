using UnityEngine;

public class RecoilOponent : MonoBehaviour {
    public float RayRecoil = 2f;
    public float forceRecoil = 40;
    private int layersRecoil = (1 << 3) | (1 << 7) | (1 << 10);
    void OnDisable() {
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, RayRecoil, layersRecoil);
        foreach (var c in cols) {
            if (c.tag == "Player") {
                c.GetComponent<IPlayer>().RecoilAttack(transform.position, forceRecoil);
            }
            else {
                c.GetComponent<IEnemy>().RecoilAttack(transform.position, forceRecoil);
            }
        }

    }
    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, RayRecoil);
    }

    private Vector2 getDirection(Transform other) {
        return (other.position - this.transform.position).normalized;
    }
}
