using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class EnemyBank {
    private static EnemyBase[] listEnemys;

    private static int level;
    private static List<EnemyBase> listEnemysInLevel = new List<EnemyBase>();

    public static void InitEnemyBank() {
        listEnemys = Resources.LoadAll<EnemyBase>("ScrObj/Enemy");
    }

    public static EnemyBase GetEnemyById(int id) {
        return listEnemys[id]; ;
    }
    public static EnemyBase GetRandomEnemy() {
        return listEnemys[Random.Range(0, listEnemys.Length)];
    }

    public static void InitListEnemysByLevel(int le) {
        level = le;
        listEnemysInLevel.Clear();
        foreach (var e in listEnemys) {
            if (e.MinLevel <= level && (e.MaxLevel >= level || e.MaxLevel == 0)) {
                listEnemysInLevel.Add(e);
            } 
        }
    }

    public static EnemyBase GetRandomEnemyByLevel() {
        return listEnemysInLevel[Random.Range(0, listEnemysInLevel.Count)];
    }
}
