using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Base/Enemy")]
public class EnemyBase : ScriptableObject {
    public string NameEnemy;
    public GameObject EnemyPrefab;
    public int MinLevel = 1;
    public int MaxLevel = 0;
}

public enum ActionEnemyBehavior {
    SlimeBehavior,
    Teleport,
    Atack
}


