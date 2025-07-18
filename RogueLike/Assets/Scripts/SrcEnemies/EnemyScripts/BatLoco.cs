using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatLoco : MonoBehaviour {
    // this References =========================
    private Rigidbody2D rig;
    private Enemy scrEnemy;
    private Transform targetPlayer;
    //============================================
    private float forceRecoilPlayer = 15.0f;
    void Start() {
        rig = GetComponent<Rigidbody2D>();
        scrEnemy = GetComponent<Enemy>();
    }

    void FixedUpdate() {

        if (targetPlayer != null) {
            // Move
            rig.AddForce(getDirectionFromPlayer() * scrEnemy.speed);

            Collider2D areaAttack = Physics2D.OverlapCircle(this.transform.position, this.GetComponent<CircleCollider2D>().radius, 1 << targetPlayer.gameObject.layer);
            if (areaAttack != null) {
                IPlayer scriptPlayer = areaAttack.GetComponent<IPlayer>();

                scriptPlayer.RecoilAttack(this.transform.position, forceRecoilPlayer);
                scriptPlayer.TakeDamage(scrEnemy.atack, scrEnemy.element); // se auto transforma em int
            }
        }
        else {
            targetPlayer = scrEnemy.GetTarget();
        }


    }

    private Vector2 getDirectionFromPlayer() {
        return (targetPlayer.position - this.transform.position).normalized;
    }
}
