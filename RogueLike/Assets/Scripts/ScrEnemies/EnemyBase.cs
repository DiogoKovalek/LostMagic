using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Base/Enemy")]
public class EnemyBase : ScriptableObject
{
    public string nameEnemy;
    public RuntimeAnimatorController rumAnim;
    public int life;
    public float speed;
    public ActionEnemyBehavior action;
}

