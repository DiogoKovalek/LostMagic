using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    protected int life = 3;
    protected float speed = 2;

    public virtual void StartEnemy(){}
    public virtual void UpdateEnemy(){}
    
    public void CauseDamageInEnemy(int damage){
        life -= damage;
    }
}
