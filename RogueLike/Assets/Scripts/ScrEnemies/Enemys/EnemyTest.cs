using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyTest : EnemyBase
{
    
    private Rigidbody2D rig;
    [SerializeField] Transform player;
    public override void StartEnemy(){
        rig = GetComponent<Rigidbody2D>();
    }
    public override void UpdateEnemy(){
        Vector2 direction = ((Vector2)(player.position - transform.position)).normalized;
        rig.velocity = direction*speed;
    }
}
