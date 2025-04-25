using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<EnemyBase> listEnemy = new List<EnemyBase>();
    void Start(){
        foreach(var enemy in listEnemy){
            enemy.StartEnemy();
        }
    }
    void Update(){
        foreach(var enemy in listEnemy){
            enemy.UpdateEnemy();
        }
    }
}
