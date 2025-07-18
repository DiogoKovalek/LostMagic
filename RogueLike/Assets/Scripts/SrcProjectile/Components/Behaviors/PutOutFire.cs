using UnityEngine;

public class putOutFire : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.transform.tag == "Fire") {
            Destroy(collision.gameObject);
        }
    }
}