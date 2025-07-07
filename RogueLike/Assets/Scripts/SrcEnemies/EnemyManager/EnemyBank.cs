using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyBank {
    private static EnemyBase[] listEnemys;

    public static void InitEnemyBank() {
        listEnemys = Resources.LoadAll<EnemyBase>("ScrObj/Enemy");
    }

    public static EnemyBase GetEnemyById(int id) {
        return listEnemys[id]; ;
    }
    public static EnemyBase GetRandomEnemy() {
        return listEnemys[Random.Range(0, listEnemys.Length)];
    }
}
