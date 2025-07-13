using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IEnemy {
    /*
    Classe dedicada a todos os inimigos no jogo, o comportamento e afins serao realizados em outra classe para os devidos comportamentos
    Nessa classe sera colocado o padrão de cada inimigo, como gerenciamento de vida e outras coisas
    */
    public int maxLife;
    public int atack;
    public float speed;
    public Element element;
    public LayerMask layerBase;
    public LayerMask layerIgnore;
    public LayerMask layerForTarget;
    public float rayVision;
    private int life;


    [HideInInspector] public Transform target;
    [HideInInspector] public Animator anim;
    void Awake() {
        anim = GetComponent<Animator>();
        life = maxLife;
    }
    void Start() {

    }

    void Update() {

    }
    void FixedUpdate() {
        Collider2D targetPlayer = Physics2D.OverlapCircle(this.transform.position, rayVision, layerForTarget);
        target = targetPlayer?.GetComponent<Transform>();
    }

    public void TakeDamage(int atack) {
        this.life -= atack;
        if (life < 0) {
            Destroy(this.gameObject);
        }
    }
    public Transform GetTarget() {
        return target;
    }

    public Vector2 directionForPlayer() {
        return (target.position - this.transform.position).normalized;
    }
    public float distanceForPlayer() {
        float x1 = target.position.x;
        float x2 = transform.position.x;
        float y1 = target.position.y;
        float y2 = transform.position.y;
        return Mathf.Sqrt(Mathf.Pow(x1-x2, 2) + Mathf.Pow(y1-y2, 2));
    }
}

public interface IEnemy {
    public void TakeDamage(int atack);
    public Transform GetTarget();
}
