using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AutoProject : MonoBehaviour, IProjectBasic {
    public Element element;
    private Rigidbody2D rb;
    private float speedProject;
    private int damage;
    private bool projectPlayer;
    private Vector2 direction;
    public ParticleSystem PrefParticula;
    private ParticleSystem particle;
    public float speedRotation = 0.5f;
    public float rayDetectTarget = 3;
    private Transform target;
    private LayerMask layerEnemys = (1 << 7) | (1 << 10);
    private LayerMask layerPlayer = 1 << 3;
    private LayerMask layerObstacles = (1 << 15) | (1 << 9);
    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        particle = Instantiate(PrefParticula, transform.position, PrefParticula.transform.rotation);
        particle.transform.SetParent(this.transform);
    }
    public void initMoviment(Vector2 direction, float speed, int damage, bool projectPlayer) {
        this.direction = direction;
        this.projectPlayer = projectPlayer;
        speedProject = speed;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.Euler(0, 0, angle);
        rb.linearVelocity = direction * speed;

        particle.Play();

        this.damage = damage;
        this.projectPlayer = projectPlayer;
    }

    void FixedUpdate() {
        if (target == null) {
            Collider2D[] cols = Physics2D.OverlapCircleAll(this.transform.position, rayDetectTarget, projectPlayer ? layerEnemys : layerPlayer);
            if (cols.Length > 0) {
                List<Collider2D> listCol = new List<Collider2D>(cols);
                foreach (var c in cols) {
                    RaycastHit2D ray = Physics2D.Raycast(transform.position, getDirectionFromTarget(c.transform), getDistanceFromTarget(c.transform), layerObstacles);
                    if (ray.collider != null) listCol.Remove(c);
                }
                target = listCol.Count > 0 ? getTargetNear(listCol).transform : null;
            }
        }
        else { // existe target
            if (getDistanceFromTarget(target) > rayDetectTarget + 0.5f) { // 0.5f para dar um exeço
                target = null; // Perde o alvo
            }
            else {
                RotateProject();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (!projectPlayer && collision.gameObject.tag == "Player") {
            collision.GetComponent<IPlayer>().TakeDamage(damage, element);
            Destroy(this.gameObject);
        }
        else if (projectPlayer && collision.gameObject.tag == "Enemy") {
            collision.GetComponent<IEnemy>().TakeDamage(damage, element);
            Destroy(this.gameObject);
        }
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Obstacle") {
            Destroy(this.gameObject);
        }
    }

    Vector2 getDirectionFromTarget(Transform target) {
        return (target.position - transform.position).normalized;
    }

    float getDistanceFromTarget(Transform target) {
        float x1 = target.position.x;
        float x2 = transform.position.x;
        float y1 = target.position.y;
        float y2 = transform.position.y;
        return Mathf.Sqrt(Mathf.Pow(x1 - x2, 2) + Mathf.Pow(y1 - y2, 2));
    }

    private Collider2D getTargetNear(List<Collider2D> cols) {
        var col = cols[0];
        for (int i = 1; i < cols.Count; i++) {
            if (getDistanceFromTarget(col.transform) > getDistanceFromTarget(cols[i].transform)) {
                col = cols[i];
            }
        }
        return col;
    }

    void RotateProject() {
        Vector2 dirTar = getDirectionFromTarget(target);
        
        float angleProject = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float angleTarget = Mathf.Atan2(dirTar.y, dirTar.x) * Mathf.Rad2Deg;

        float angle = Mathf.MoveTowardsAngle(angleProject, angleTarget, speedRotation);

        float rad = angle * Mathf.Deg2Rad;
        direction = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
        transform.rotation = Quaternion.Euler(0, 0, angle);

        rb.linearVelocity = direction * speedProject;
    }
}
