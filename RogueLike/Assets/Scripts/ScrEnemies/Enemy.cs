using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Enemy Base =======================
    public EnemyBase enemyBase;
    public int life;
    //===================================
    private Animator anim;
    private Dictionary<ActionEnemyBehavior, IEnemyBehavior> behaviors;
    public Transform TargetPlayer; // Provisorio
    void Awake()
    {    
        behaviors = new Dictionary<ActionEnemyBehavior, IEnemyBehavior>
        {
            {ActionEnemyBehavior.SlimeBehavior, new SlimeBehavior()},
        };
        
        gameObject.name = enemyBase.name;
        anim = GetComponent<Animator>();
        anim.runtimeAnimatorController = enemyBase.rumAnim;
        life = enemyBase.totalLife;

        
    }
    void Start()
    {
        TargetPlayer = FindObjectOfType<Player>().transform;
        getEnemyBehavior()?.StartBehavior(this);
    }
    void Update()
    {
        getEnemyBehavior()?.UpdateBehavior(this);
    }
    private IEnemyBehavior getEnemyBehavior(){
        if(behaviors.TryGetValue(enemyBase.behavior , out var enemyBehavior)){
            return enemyBehavior;
        }else{
            Debug.LogError($"Erro ao acessar o Behavior >>> {enemyBase.behavior}");
        }
        return null;

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

    /*
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
    */
}
