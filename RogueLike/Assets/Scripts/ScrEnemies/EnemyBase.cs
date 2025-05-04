using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Base/Enemy")]
public class EnemyBase : ScriptableObject
{
    public string nameEnemy;
    public RuntimeAnimatorController rumAnim;
    public int totalLife;
    public float speed;
    public int attack;
    public ActionEnemyBehavior behavior;
}

public enum ActionEnemyBehavior{
        SlimeBehavior,
        Teleport,
        Atack
}

