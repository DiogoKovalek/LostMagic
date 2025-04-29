using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Enemy Base =======================
    public EnemyBase enemyBase;
    private int life;
    private float speed;
    //===================================
    private Animator anim;
    private Rigidbody2D rig;
    private Dictionary<ActionEnemyBehavior, Action> actions;

    private Transform player;
    private bool isFreeForMove = true;
    void Awake()
    {    
        actions = new Dictionary<ActionEnemyBehavior, Action>
        {
            {ActionEnemyBehavior.SlimeBehavior, SlimeBehavior},
            {ActionEnemyBehavior.Teleport, Teleport},
            {ActionEnemyBehavior.Atack, Atack}
        };
        
        gameObject.name = enemyBase.name;
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

    void OnCollisionEnter2D(Collision2D other){
        if(other.gameObject.tag == "Player"){
            Debug.Log("colidiu");
            isFreeForMove = false;
        }
    }
    void OnCollisionExit2D(Collision2D other){
        if(other.gameObject.tag == "Player"){
            Debug.Log("Saiu");
            isFreeForMove = true;
        }
    }

    public void CauseDamageInEnemy(int damage){
        this.life -= damage;
        if(life < 0){ // Criar outro metodo para destruir o inimigo para ficar mais dianmico
            Destroy(this.gameObject);
        }
    }
    /*
    private void CauseDamageInPlayerPerAPRO(int damage){
        Collider2D cirecleDamage = Physics2D.OverlapCircle(transform.position, GetComponent<CircleCollider2D>().radius, playerLayer);
        if(cirecleDamage != null){ // player colidiu
            Debug.Log("Colidiu");
        }
    }*/

    private void getPlayer(){ // Melhorar depois
        player = FindObjectOfType<Player>().transform;
    }
    
    private void ExecuteActionBehavior(){
        if(actions.TryGetValue(enemyBase.action, out var action)){ // Paga o Action dentro do dicionario
            action.Invoke();
        }
        else{
            Debug.LogWarning($"Erro of The Action Behavior: not found{enemyBase.action}");
        }
    }
    
    #region Slime Behavior
    private void SlimeBehavior(){
        if(isFreeForMove){ // movement
            Vector2 direction = (player.position - transform.position).normalized;
            rig.velocity = direction *speed;
        }
    }
    #endregion
    private void Teleport(){
        Debug.Log("Teleport");
    }
    private void Atack(){
        Debug.Log("Atack");
    }
}
