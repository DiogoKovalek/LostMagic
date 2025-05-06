using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class SlimeBehavior : IEnemyBehavior
{
    // Enemy References =========================
    private Rigidbody2D rig;
    private Transform targetPlayer;
    private EnemyBase enemyBase; // Para evitar alterar valor do BaseEnemy do enemy
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
    private bool freeForMove = true; // aqui trocar para true
    private bool isAttaking = false;

    public void StartBehavior(Enemy enemy){
        enemyBase = enemy.enemyBase;
        rig = enemy.GetComponent<Rigidbody2D>();
        targetPlayer = enemy.TargetPlayer;

        enemy.StartCoroutine(calcDistanceForPlayer(enemy));
    }

    public void UpdateBehavior(Enemy enemy){
        if(freeForMove){
            Vector2 direction = (targetPlayer.position - enemy.transform.position).normalized;
            rig.velocity = direction * enemyBase.speed;
        }
        if(freeForAttack && nearPlayer){
            enemy.StartCoroutine(attack(enemy));
        }
        attackColision(enemy);
    }

    private void attackColision(Enemy enemy){
        Collider2D areaAttack = Physics2D.OverlapCircle(enemy.transform.position, enemy.GetComponent<CircleCollider2D>().radius, 1 << targetPlayer.gameObject.layer);
        if(areaAttack != null){
            Player scriptPlayer = areaAttack.GetComponent<Player>();
            // Se estiver atacando
            if(isAttaking){
                scriptPlayer.CauseDamageInPlayer(enemyBase.attack);
            }else{
                scriptPlayer.RecoilAttack(enemy.transform.position, forceRecoilPlayer);
                scriptPlayer.CauseDamageInPlayer(enemyBase.attack / 3); // se auto transforma em int
            }
        }
    }

    private IEnumerator calcDistanceForPlayer(Enemy enemy){
        float distEnemyForPlayer = Vector2.Distance(enemy.transform.position, targetPlayer.position);
        if(distEnemyForPlayer <= raiForAtack && freeForAttack){
            nearPlayer = true;
        }
        yield return new WaitForFixedUpdate();
        enemy.StartCoroutine(calcDistanceForPlayer(enemy));
    }

    private IEnumerator attack(Enemy enemy){
        // Prepara for attack
        nearPlayer = false;
        freeForMove = false;
        freeForAttack = false;
        rig.velocity = new Vector2(0,0); // zera a velocidade
        enemy.gameObject.layer = (int) Math.Log(enemy.layerEnemyIgoreCollision, 2);
        yield return new WaitForSeconds(timeForAtack);
        // Attack
        isAttaking = true;
        Vector2 direction = (targetPlayer.position - enemy.transform.position).normalized;
        rig.AddForce(direction*forceJump, ForceMode2D.Impulse);
        yield return new WaitForSeconds(delayForAttack);
        enemy.gameObject.layer = (int) Math.Log(enemy.layerEnemy, 2);
        rig.velocity = new Vector2(0,0); // para o pulo
        isAttaking = false;
        yield return new WaitForSeconds(delayForBackWalk);
        // BackWalk
        freeForMove = true;
        yield return new WaitForSeconds(delayForBackAtack);
        // BackAttack
        freeForAttack = true;
    }

    /*
        if(enemy.isFreeForMove){ // movement
            Vector2 direction = (player.position - transform.position).normalized;
            rig.velocity = direction *speed;
        }
        */
}
