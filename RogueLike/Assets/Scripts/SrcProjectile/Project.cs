using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Project : MonoBehaviour, IProject {
    private ProjectileBase projectBase;
    private Animator anim;
    private IAtributesComunique comunique;
    public void initMoviment(Vector2 direction) {
        switch (projectBase.typeMoveProject) {
            case TypeMoveProject.forward:
                moveFoeard(direction);
                break;
            case TypeMoveProject.autoRemote:
                moveAutoRemote();
                break;
            default:
                break;
        }
    }

    private void moveFoeard(Vector2 direction) {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        CircleCollider2D cc = GetComponent<CircleCollider2D>();
        cc.radius = projectBase.coliderSize;
        rb.velocity = direction * projectBase.speed;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
    private void moveAutoRemote() {
        
    }

    public void insertProjectBase(ProjectileBase pb, GameObject player) {
        projectBase = pb;
        anim = GetComponent<Animator>();
        anim.runtimeAnimatorController = projectBase.runAnim;
        transform.localScale *= projectBase.size;
        comunique = player.GetComponent<IAtributesComunique>();
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Enemy") {
            collision.GetComponent<Enemy>().CauseDamageInEnemy(comunique.CalculateAtack(projectBase.element, Element.Void)); // void é para quebrar o galho
        }
    }
}
