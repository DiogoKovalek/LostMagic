using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BatBehavior : IEnemyBehavior
{
    //Enemy References =======================
    private Rigidbody2D rig;
    private Transform targetPlayer;
    private EnemyBase enemyBase;
    //=========================================
    public void StartBehavior(Enemy enemy)
    {
        enemyBase = enemy.enemyBase;
        enemy.gameObject.layer = (int) Math.Log(enemy.layerEnemyIgoreCollision);
        rig = enemy.GetComponent<Rigidbody2D>();
    }

    public void UpdateBehavior(Enemy enemy)
    {
        
    }
}
