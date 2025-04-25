using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Projectile1 : MonoBehaviour
{
    public int damage = 3; // temporario
    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "Enemy"){
            other.GetComponent<EnemyBase>().CauseDamageInEnemy(damage);
        }
    }
}
