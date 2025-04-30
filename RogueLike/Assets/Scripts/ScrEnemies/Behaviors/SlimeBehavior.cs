using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBehavior : IEnemyBehavior
{
    // Enemy References =========================
    private Rigidbody2D rig;
    private Transform targetPlayer;
    private EnemyBase enemyBase; // Para evitar alterar valor do BaseEnemy do enemy
    //============================================

    private const float raiForAtack = 3.0f;
    private const float timeForAtack = 3.0f;
    private const float delayForBackWalk = 1.5f;
    private const float delayForBackAtack = 1.0f;
    private const float forceJump = 3.0f;
    private bool nearPlayer = false;
    private bool prepareForAtack = false;
    private bool freeForAtack = true;
    public void StartBehavior(Enemy enemy){
        enemyBase = enemy.enemyBase;
        rig = enemy.GetComponent<Rigidbody2D>();
        targetPlayer = enemy.TargetPlayer;

        enemy.StartCoroutine(calcDistanceForPlayer(enemy));
    }

    public void UpdateBehavior(Enemy enemy){
        if(!nearPlayer || !prepareForAtack){
            Vector2 direction = (targetPlayer.position - enemy.transform.position).normalized;
            rig.velocity = direction * enemyBase.speed;
        }else if(!prepareForAtack){
            enemy.StartCoroutine(atack(enemy));
        }

        /*
        ?Proximo
        se(!Proximo)
            andar()
        senao(!Proximo)
            PrepararParaAtacar()

        PrepararParaAtacar()
            espera x segundos
            addForce(direcaoDoPlayer)
        */
    }

    private IEnumerator calcDistanceForPlayer(Enemy enemy){
        float distEnemyForPlayer = Vector2.Distance(enemy.transform.position, targetPlayer.position);
        Debug.Log(distEnemyForPlayer);
        if(distEnemyForPlayer <= raiForAtack){
            nearPlayer = true;
        }
        yield return new WaitForFixedUpdate();
        enemy.StartCoroutine(calcDistanceForPlayer(enemy));
    }

    private IEnumerator atack(Enemy enemy){
        prepareForAtack = true;
        yield return new WaitForSeconds(timeForAtack);
        Vector2 direction = (targetPlayer.position - enemy.transform.position).normalized;
        rig.AddForce(direction*forceJump, ForceMode2D.Impulse);
        yield return new WaitForSeconds(delayForBackWalk);

        yield return new WaitForSeconds(delayForBackAtack);
    }

    /*
        if(enemy.isFreeForMove){ // movement
            Vector2 direction = (player.position - transform.position).normalized;
            rig.velocity = direction *speed;
        }
        */
}
