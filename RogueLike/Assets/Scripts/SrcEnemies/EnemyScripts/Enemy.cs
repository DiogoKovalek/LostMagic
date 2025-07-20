using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Enemy : MonoBehaviour, IEnemy, IAtributesComunique, IStatusAplicate {
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

    [HideInInspector] public bool isFreeForAction = true;

    private Vector2 directionTarget; // Variavel para se caso o inimigo perder o target ele dispare na ultima direção

    [HideInInspector] public Transform target;
    [HideInInspector] public Animator anim;

    private float timeInvensibleComponents = 1.5f;
    private bool isInvensibleComponents = false;


    // Status =============================
    public StatusView localStatus;
    private List<Status> status = new List<Status>();
    //=====================================
    // Damege Number ======================
    public GameObject prefabDamageNumber;
    //=====================================
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
        GameObject dn = Instantiate(prefabDamageNumber, this.transform.position, this.transform.rotation, transform.parent);
        dn.GetComponent<DamageNumberEditor>().Init(atack.ToString(), element);
        if (life < 0) {
            Destroy(this.gameObject);
        }
    }
    public void TakeDamageFromComponents(int atack, Element element) {
        if (!isInvensibleComponents && element != this.element) {
            this.life -= atack;
            GameObject dn = Instantiate(prefabDamageNumber, this.transform.position, this.transform.rotation, transform.parent);
            dn.GetComponent<DamageNumberEditor>().Init(atack.ToString(), element);
            StartCoroutine(DelayInvensible());

            if (life <= 0) {
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
    #region Status Aplicate
    public void managerStatus(Status status, bool aplicate) {
        bool containStatus = this.status.Contains(status);
        Element eleSta;
        switch (status) {
            case Status.Burning:
                eleSta = Element.Fire;
                break;
            case Status.Stunned:
                eleSta = Element.Earth;
                break;
            default:
                eleSta = Element.Void;
                break;
        }
        if (element != eleSta) {
            if (aplicate && !containStatus) {
                this.status.Add(status);
                localStatus.addStatus(status);
            }
            else if (!aplicate && containStatus) {
                this.status.Remove(status);
                localStatus.removeStatus(status);
            }
        }
    }

    public void damageBurning(int damage) {
        if (element != Element.Fire) TakeDamage(damage, Element.Fire);
    }

    public bool isBurning() {
        return status.Contains(Status.Burning);
    }

    public void Stunned(bool isStuned) {
        if (isStuned && element != Element.Earth) {
            isFreeForAction = false;
        }
        else {
            isFreeForAction = true;
        }
    }

    public void Wet() {
        throw new NotImplementedException();
    }
    #endregion
}

public interface IEnemy {
    public void TakeDamage(int atack, Element element);
    public void TakeDamageFromComponents(int atack, Element element);
    public Transform GetTarget();
}
