using System.Collections;
using UnityEngine;

public class FireOnWall : MonoBehaviour {
    public float timeToDesapear = 8.0f;
    public int damage = 3;
    private float ray;
    IEnumerator Start() {
        ray = (transform.localScale.x / 2) - 0.30f;
        yield return new WaitForSeconds(timeToDesapear);
        Destroy(this.gameObject);
    }

    void FixedUpdate() {
        Collider2D[] col = Physics2D.OverlapCircleAll(this.transform.position, ray);
        foreach (var c in col) {
            Transform ent = c.transform;
            if (ent.tag == "Player") {
                ent.GetComponent<IPlayer>().TakeDamage(damage, Element.Fire);
            }
            else if (ent.tag == "Enemy") {
                ent.GetComponent<IEnemy>().TakeDamageFromComponents(damage, Element.Fire);
            }
        }
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, ray);
    }
}
