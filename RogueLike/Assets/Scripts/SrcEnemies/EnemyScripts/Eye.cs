using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eye : MonoBehaviour {
    // this References =========================
    private Rigidbody2D rig;
    private Enemy scrEnemy;
    private Transform targetPlayer;
    //============================================
    private GameObject eye;
    private Vector2 direction;
    public GameObject projectPrefab;
    private float delayForShot = 3.0f;
    private bool isShoting = false;
    private float distSpawn = 0.7f;
    private float speedProject = 6.0f;
    void Start() {
        rig = GetComponent<Rigidbody2D>();
        scrEnemy = GetComponent<Enemy>();
        eye = this.transform.Find("Eye").gameObject;
    }
    void FixedUpdate() {
        if (targetPlayer != null) {
            direction = getDirectionFromPlayer();
            float angleEye = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
            eye.transform.rotation = Quaternion.Euler(0, 0, angleEye);
            if (!isShoting) {
                StartCoroutine(shot());
            }
        }
        else {
            targetPlayer = scrEnemy.GetTarget();
        }
    }
    private IEnumerator shot() {
        isShoting = true;
        yield return new WaitForSeconds(delayForShot);
        GameObject proj = Instantiate(projectPrefab, (Vector2) this.transform.position + (direction * distSpawn), projectPrefab.transform.rotation);
        proj.GetComponent<SimpleProject>().initMoviment(direction, speedProject, scrEnemy.atack);
        proj.transform.SetParent(this.transform);
        isShoting = false;
    }
    private Vector2 getDirectionFromPlayer() {
        return (targetPlayer.position - this.transform.position).normalized;
    }
}
