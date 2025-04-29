using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRoomManage : MonoBehaviour
{
    [SerializeField] GameObject enemyPref;
    [SerializeField] EnemyBase[] listEnemyBase;

    void Start(){
        GameObject enemy = enemyPref;
        enemy.GetComponent<Enemy>().enemyBase = listEnemyBase[Random.Range(0,listEnemyBase.Length)];
        Instantiate(enemy, transform.position, transform.rotation);
    }
}
