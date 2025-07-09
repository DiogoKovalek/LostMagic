using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SimpleProject : MonoBehaviour {
    private Vector2 direction;
    private Rigidbody2D rb;
    private int damage;
    public void initMoviment(Vector2 direction, float speed, int damage, bool autoDirection = false) {
        if (!autoDirection) {
            if (rb == null) rb = GetComponent<Rigidbody2D>();
            rb.velocity = direction * speed;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            this.transform.rotation = Quaternion.Euler(0, 0, angle);
            this.damage = damage;
        }
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            collision.GetComponent<Player>().CauseDamageInPlayer(damage);
            Destroy(this.gameObject);
        }
        if (collision.gameObject.tag == "Wall") {
            Destroy(this.gameObject);
        }
    }
}
