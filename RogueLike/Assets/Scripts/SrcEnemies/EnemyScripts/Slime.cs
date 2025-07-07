using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour {
    // this References =========================
    private Rigidbody2D rig;
    private Enemy scrEnemy;
    private Transform targetPlayer;

    //============================================

    private const float raiForAtack = 3.0f;
    private const float timeForAtack = 1.3f;
    private const float delayForAttack = 0.2f;
    private const float delayForBackWalk = 1.0f;
    private const float delayForBackAtack = 3.0f;
    private const float forceJump = 25.0f;
    private const float forceRecoilPlayer = 15.0f;
    private bool nearPlayer = false;
    private bool freeForAttack = true; // trocar para true
    private bool freeForMove = false; // aqui trocar para true
    private bool isAttaking = false;
    void Awake() {
        rig = GetComponent<Rigidbody2D>();
        scrEnemy = GetComponent<Enemy>();
    }

    void Update() {
        if (freeForMove && targetPlayer != null) {
            Vector2 direction = (targetPlayer.position - this.transform.position).normalized;
            rig.velocity = direction * scrEnemy.speed;
        }
        else if (targetPlayer == null) {
            targetPlayer = scrEnemy.GetTarget();
            if (targetPlayer != null) {
                freeForMove = true;
                StartCoroutine(calcDistanceForPlayer());
            }
        }
        if (freeForAttack && nearPlayer) {
            StartCoroutine(attack());
        }
        if (targetPlayer != null) {
            attackColision();
        }
    }
    private IEnumerator calcDistanceForPlayer() {
        if (targetPlayer != null) {
            float distthisForPlayer = Vector2.Distance(this.transform.position, targetPlayer.position);
            if (distthisForPlayer <= raiForAtack && freeForAttack) {
                nearPlayer = true;
            }
        }
        yield return new WaitForFixedUpdate();
        this.StartCoroutine(calcDistanceForPlayer());
    }
    private void attackColision() {
        Collider2D areaAttack = Physics2D.OverlapCircle(this.transform.position, this.GetComponent<CircleCollider2D>().radius, 1 << targetPlayer.gameObject.layer);
        if (areaAttack != null) {
            Player scriptPlayer = areaAttack.GetComponent<Player>();
            // Se estiver atacando
            if (isAttaking) {
                scriptPlayer.CauseDamageInPlayer(scrEnemy.atack);
            }
            else {
                scriptPlayer.RecoilAttack(this.transform.position, forceRecoilPlayer);
                scriptPlayer.CauseDamageInPlayer(scrEnemy.atack); // se auto transforma em int
            }
        }
    }
    private IEnumerator attack() {
        // Prepara for attack
        nearPlayer = false;
        freeForMove = false;
        freeForAttack = false;
        rig.velocity = new Vector2(0, 0); // zera a velocidade
        this.gameObject.layer = (int)Math.Log(scrEnemy.layerIgnore, 2);
        yield return new WaitForSeconds(timeForAtack);
        // Attack
        isAttaking = true;
        Vector2 direction = (targetPlayer.position - this.transform.position).normalized;
        rig.AddForce(direction * forceJump, ForceMode2D.Impulse);
        yield return new WaitForSeconds(delayForAttack);
        this.gameObject.layer = (int)Math.Log(scrEnemy.layerBase, 2);
        rig.velocity = new Vector2(0, 0); // para o pulo
        isAttaking = false;
        yield return new WaitForSeconds(delayForBackWalk);
        // BackWalk
        freeForMove = true;
        yield return new WaitForSeconds(delayForBackAtack);
        // BackAttack
        freeForAttack = true;
    }
}
