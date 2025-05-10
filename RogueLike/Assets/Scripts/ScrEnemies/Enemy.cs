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
    public LayerMask layerEnemy;
    public LayerMask layerEnemyIgoreCollision;
    
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
        if(life < 0){
            Destroy(this.gameObject);
        }
    }
}
