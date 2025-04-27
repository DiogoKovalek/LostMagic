using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Enemy Base =======================
    [SerializeField] EnemyBase enemyBase;
    private string nameEnemy;
    private int life;
    private float speed;
    //===================================
    private Animator anim;
    void Awake()
    {
        nameEnemy = enemyBase.name;
        anim = GetComponent<Animator>();
        anim.runtimeAnimatorController = enemyBase.rumAnim;
        life = enemyBase.life;
        speed = enemyBase.speed;
    }
    void Start()
    {
        
    }
}
