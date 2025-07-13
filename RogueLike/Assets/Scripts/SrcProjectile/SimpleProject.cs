using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SimpleProject : MonoBehaviour {

    public Element element;
    private Rigidbody2D rb;
    private int damage;
    private bool projectPlayer;
    void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }
    public void initMoviment(Vector2 direction, float speed, int damage, bool projectPlayer) {
        rb.velocity = direction * speed;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.Euler(0, 0, angle);
        this.damage = damage;
        this.projectPlayer = projectPlayer;
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (!projectPlayer && collision.gameObject.tag == "Player") {
            collision.GetComponent<IPlayer>().TakeDamage(damage);
            Destroy(this.gameObject);
        }
        else if (projectPlayer && collision.gameObject.tag == "Enemy") {
            collision.GetComponent<IEnemy>().TakeDamage(damage);
            Destroy(this.gameObject);
        }
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Obstacle") {
            Destroy(this.gameObject);
        }
    }
}
