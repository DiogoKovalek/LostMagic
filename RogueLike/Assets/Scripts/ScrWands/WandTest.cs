using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class WandTest : MonoBehaviour, IActionsWand
{
    //Player =====================================
    [SerializeField] GameObject prefProjectile;
    private GameObject playerOwner;
    //============================================
    //Projectile =================================
    private List<GameObject> projectilesConjured = new List<GameObject>();
    private int maxProjectile = 5;
    private float speedProjectileInOrbit = 95.0f;
    private float radio = 1.2f;
    private float speedProjectile = 9f;
    //============================================

    public void attack(){
        if(projectilesConjured.Count > 0){ // para textes
            GameObject project = projectilesConjured[0];
            projectilesConjured.Remove(project);
            project.transform.position = transform.position;
            project.GetComponent<CircleCollider2D>().enabled = true;
            project.GetComponent<Rigidbody2D>().AddForce(transform.right * speedProjectile, ForceMode2D.Impulse);
        }
        
    }
    public void defense(){
        Debug.Log("Defense");
    }
    public void conjure(){
        if(projectilesConjured.Count < maxProjectile){
            GameObject project = Instantiate(prefProjectile, transform.position, transform.rotation);
            projectilesConjured.Add(project);
            if(projectilesConjured.Count == 1){ // initi the corrotine
                StartCoroutine(orbitPlayer());
        }
        }
        
    }
    public void grimoreMagic(){
        Debug.Log("Grimore Magic");
    }
    public void getWand(){
        playerOwner = transform.parent.gameObject;
    }
    public void dropWand(){
        if(projectilesConjured.Count == 0) return;
        foreach(var project in projectilesConjured){
            Vector2 direction = (project.transform.position - playerOwner.transform.position).normalized;
            project.GetComponent<CircleCollider2D>().enabled = true;
            project.GetComponent<Rigidbody2D>().AddForce(direction*speedProjectile, ForceMode2D.Impulse);
        }
        projectilesConjured.Clear();
    }

    private IEnumerator orbitPlayer(){
        float angle = 0;
        while(projectilesConjured.Count >= 1){
            angle = (angle + speedProjectileInOrbit * Time.deltaTime) % 360f;
            for(int i = 0; i < projectilesConjured.Count; i++){
                float rad = (angle + 360f/projectilesConjured.Count * i) * Mathf.Deg2Rad;
                
                Vector2 direction = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
                projectilesConjured[i].transform.position = (Vector2) playerOwner.transform.position + (direction.normalized * radio);
            }
            yield return null;
        }
        
    }
}
