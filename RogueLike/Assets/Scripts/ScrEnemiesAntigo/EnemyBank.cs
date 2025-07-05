using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyBank {

    private static GameObject enemyBasic;
    private static EnemyBase[] listEnemys;

    public static void InitEnemyBank() {
        enemyBasic = Resources.Load<GameObject>("LoadPrefab/Enemy/EnemyBase");
        listEnemys = Resources.LoadAll<EnemyBase>("ScrObj/Enemy");
    }

    public static EnemyBase CreateEnemyById(int id) {
        return listEnemys[id]; ;
    }
    public static EnemyBase CreateRandomEnemy() {
        return listEnemys[Random.Range(0, listEnemys.Length)];
    }

    public static GameObject InstantiateEnemyByBase(EnemyBase enemyBase) {
        GameObject e = MonoBehaviour.Instantiate(enemyBasic);
        e.GetComponent<Enemy>().SetEnemyBase(enemyBase);
        return e;
    }
}
