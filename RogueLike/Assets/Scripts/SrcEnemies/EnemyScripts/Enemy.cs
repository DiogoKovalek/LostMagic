using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IEnemy, IAtributesComunique {
    /*
    Classe dedicada a todos os inimigos no jogo, o comportamento e afins serao realizados em outra classe para os devidos comportamentos
    Nessa classe sera colocado o padrão de cada inimigo, como gerenciamento de vida e outras coisas
    */
    public int maxLife;
    public int atack;
    public float speed;
    private Rigidbody2D rig;
    public Element element;
    public LayerMask layerBase;
    public LayerMask layerIgnore;
    public LayerMask layerForTarget;
    public float rayVision = 30;
    private int life;

    private Vector2 directionTarget; // Variavel para se caso o inimigo perder o target ele dispare na ultima direção

    [HideInInspector] public Transform target;
    [HideInInspector] public Animator anim;

    private float timeInvensibleComponents = 1.5f;
    private float timeRecoil = 0.1f;
    private bool isInvensibleComponents = false;


    public bool freeForMove = true;

    void Awake() {
        rig = GetComponent<Rigidbody2D>();
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

    public void TakeDamage(int atack, Element element) {
        this.life -= atack;
        if (life < 0) {
            Destroy(this.gameObject);
        }
    }
    public void TakeDamageFromComponents(int atack, Element element) {
        if (!isInvensibleComponents) {
            if (element != this.element) {
                this.life -= atack;
                StartCoroutine(DelayInvensible());
            }
            if (life < 0) {
                Destroy(this.gameObject);
            }
        }
    }
    private IEnumerator DelayInvensible() {
        isInvensibleComponents = true;
        yield return new WaitForSeconds(timeInvensibleComponents);
        isInvensibleComponents = false;
    }
    public Transform GetTarget() {
        return target;
    }

    public Vector2 directionForPlayer() {
        directionTarget = (target.position - this.transform.position).normalized;
        return directionTarget;
    }
    public float distanceForPlayer() {
        float x1 = target != null ? target.position.x : 0;
        float x2 = transform.position.x;
        float y1 = target != null ? target.position.y : 0;
        float y2 = transform.position.y;
        return Mathf.Sqrt(Mathf.Pow(x1 - x2, 2) + Mathf.Pow(y1 - y2, 2));
    }

    public float CalculateSpeed() {
        return this.speed;
    }

    public int CalculateAtack(Element project) {
        return atack;
    }

    public void RecoilAttack(Vector2 pos, float force) {
        StartCoroutine(recoilAttack(pos, force));
    }
    private IEnumerator recoilAttack(Vector2 pos, float force) {
        freeForMove = false;
        Vector2 direction = ((Vector2)transform.position - pos).normalized;
        rig.linearVelocity = direction * force;
        yield return new WaitForSeconds(timeRecoil);
        freeForMove = true;
    }
}

public interface IEnemy {
    public void TakeDamage(int atack, Element element);
    public void TakeDamageFromComponents(int atack, Element element);
    public void RecoilAttack(Vector2 pos, float force);
    public Transform GetTarget();
}
