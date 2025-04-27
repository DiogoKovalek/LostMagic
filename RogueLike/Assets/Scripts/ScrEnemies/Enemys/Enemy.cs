using System;
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
    private Rigidbody2D rig;
    private Dictionary<ActionEnemyBehavior, Action> actions;

    private Transform player;
    void Awake()
    {
        
        actions = new Dictionary<ActionEnemyBehavior, Action>
        {
            {ActionEnemyBehavior.Move, Move},
            {ActionEnemyBehavior.Teleport, Teleport},
            {ActionEnemyBehavior.Atack, Atack}
        };
        

        nameEnemy = enemyBase.name;
        anim = GetComponent<Animator>();
        anim.runtimeAnimatorController = enemyBase.rumAnim;
        life = enemyBase.life;
        speed = enemyBase.speed;

        rig = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        getPlayer();      
    }
    void Update(){
        ExecuteActionBehavior();
    }
    public void CauseDamageInEnemy(int damage){
        this.life -= damage;
    }

    public void getPlayer(){ // Melhorar depois
        player = FindObjectOfType<Player>().transform;
    }
    #region  Action Behavior Enemy
    
    private void ExecuteActionBehavior(){
        if(actions.TryGetValue(enemyBase.action, out var action)){ // Paga o Action dentro do dicionario
            action.Invoke();
        }
        else{
            Debug.LogWarning($"Erro of The Action Behavior: not found{enemyBase.action}");
        }
    }
    
    private void Move(){
        Vector2 direction = (player.position - transform.position).normalized;
        rig.velocity = direction *speed;
    }

    private void Teleport(){
        Debug.Log("Teleport");
    }
    private void Atack(){
        Debug.Log("Atack");
    }
}
public enum ActionEnemyBehavior{
        Move,
        Teleport,
        Atack
}
#endregion