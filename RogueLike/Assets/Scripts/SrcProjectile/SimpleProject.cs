using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SimpleProject : MonoBehaviour,IProjectBasic{

    public Element element;
    private Rigidbody2D rb;
    private int damage;
    private float speed;
    private bool projectPlayer;
    public ParticleSystem Prefparticula;
    private ParticleSystem particle;
    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        particle = Instantiate(Prefparticula, transform.position, Prefparticula.transform.rotation);
        particle.transform.SetParent(this.transform);
    }
    public void initMoviment(Vector2 direction, float speed, int damage, bool projectPlayer) {
        rb.linearVelocity = direction * speed;
        particle.Play();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.Euler(0, 0, angle);
        this.damage = damage;
        this.speed = speed;
        this.projectPlayer = projectPlayer;
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (!projectPlayer && collision.gameObject.tag == "Player") {
            collision.GetComponent<IPlayer>().TakeDamage(damage, element);
            collision.GetComponent<StatusManager>().AplicateStatus(element);
            Destroy(this.gameObject);
        }
        else if (projectPlayer && collision.gameObject.tag == "Enemy") {
            collision.GetComponent<IEnemy>().TakeDamage(damage, element);
            collision.GetComponent<StatusManager>().AplicateStatus(element);
            Destroy(this.gameObject);
        }
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Obstacle") {
            Destroy(this.gameObject);
        }
    }

    public void AddSpeed(float speedAdd) {
        speed += speedAdd;
        rb.linearVelocity = rb.linearVelocity.normalized * speed;
    }
}
