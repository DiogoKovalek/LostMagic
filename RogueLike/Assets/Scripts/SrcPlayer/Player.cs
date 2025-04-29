using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    [SerializeField] float speed;
    private Vector3 dirPlayer;
    private Rigidbody2D rig;
    private int life;

    // Varibles for Wand ==================
    private GameObject wand;
    private Vector2 mousePosition;
    private float distanWand = 0.4f;
    private bool allowedForUsingMagic = true;
    private float timeDeleyForUsingMagic = 0.3f;
    [SerializeField] GameObject handForWand;
    //====================================

    // Itens config ======================
    private float rayCollect = 2.0f;
    private int layerForCollect = 1 << 6; // or 0b1000000
    private GameObject itemNear;
    private bool allowedForGetItem = true;
    private float timeDeleyForGetItem = 0.8f;
    //====================================

    // Start is called before the first frame update
    void Awake(){
        rig = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        //Collider2D[] circleCollect = Physics2D.OverlapCircleAll(transform.position, rayCollect, layerForCollect);
        /*circleCollect = selectSortForPhysics2D(circleCollect);
        foreach(var wand in circleCollect){
            Debug.Log(wand.name);
            //Debug.Log(distForPlayer(wand));
        }*/
        /*
        Collider2D wandNear = getColNearFromThePlayer(circleCollect);
        Debug.Log(wandNear.name);
        */
    }

    // Update is called once per frame
    void Update()
    {
        
        #region Movement in Update / Get Input
        //Input.GetAxisRaw() retorna -1 0 ou 1, sem a suavizacao
        dirPlayer = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), transform.position.z).normalized;
    	#endregion
        

        #region Mouse and wand tranform
        if(wand != null){
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 circleInMouseForPlayer = (mousePosition - (Vector2) handForWand.transform.position).normalized;

            wand.transform.position = (Vector2) handForWand.transform.position + circleInMouseForPlayer * distanWand;
            float angleMouse = Mathf.Atan2(circleInMouseForPlayer.y, circleInMouseForPlayer.x) * Mathf.Rad2Deg;
            wand.transform.rotation = Quaternion.Euler(0,0,angleMouse);
        }
        #endregion

        #region GetItem and drop
        if(allowedForGetItem && itemNear != null && Input.GetKey(KeyCode.E)){
            if(itemNear.tag == "wand"){
                if(wand != null){ // dropa a wanda atual
                    wand.transform.rotation = Quaternion.Euler(0,0,0);
                    wand.transform.position = transform.position;
                    wand.GetComponent<IActionsWand>().dropWand();
                    wand.GetComponent<CircleCollider2D>().enabled = true;
                    wand.transform.SetParent(null);
                }
                wand = itemNear;
                wand.GetComponent<CircleCollider2D>().enabled = false;
                wand.transform.SetParent(this.transform);
                wand.GetComponent<IActionsWand>().getWand();
                StartCoroutine(delayForGetItem());
            }
        }
        #endregion

        #region using wand
        usingWand();
        #endregion
    }
    void FixedUpdate()
    {
        #region Movment in FixedUpdate / aplicate velocity
        rig.velocity = dirPlayer * speed;
        #endregion

        #region Itens colision
        Collider2D[] circleCollect = Physics2D.OverlapCircleAll(transform.position, rayCollect, layerForCollect);
        if(circleCollect.Length > 0){
            itemNear = getColNearFromThePlayer(circleCollect).gameObject;
        }else{
            itemNear = null;
        }
        #endregion
    }
    /*
    // Para desenhar o circulo de colisao de item
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rayCollect);
    }
    */
    private void usingWand(){
        if(allowedForUsingMagic && wand != null){
            IActionsWand wandInter = wand.GetComponent<IActionsWand>();
            if(wandInter == null) return;
            if(Input.GetKey(KeyCode.Mouse0)){
                wandInter.attack();
                StartCoroutine(delayForUsingMagic());
            }else if(Input.GetKey(KeyCode.Mouse1)){
                wandInter.conjure();
                StartCoroutine(delayForUsingMagic());
            }else if(Input.GetKey(KeyCode.Space)){
                wandInter.defense();
                StartCoroutine(delayForUsingMagic());
            }else if(Input.GetKey(KeyCode.Q)){
                wandInter.grimoreMagic();
                StartCoroutine(delayForUsingMagic());
            }
        }
    }
    public void CauseDamageInPlayer(int attack){
        this.life -= attack;
    }
    Collider2D getColNearFromThePlayer(Collider2D[] list){
        var near = list[0];
        for(int i = 1; i < list.Length; i++){
            if(distForPlayer(near) > distForPlayer(list[i])){
                near = list[i]; 
            } 
        } 
        return near;
    }
    double distForPlayer(Collider2D col){
        return Math.Sqrt(Math.Pow(col.transform.position.x - this.transform.position.x, 2) + Math.Pow(col.transform.position.y - this.transform.position.y, 2));
    }

    private IEnumerator delayForGetItem(){
        allowedForGetItem = false;
        yield return new WaitForSeconds(timeDeleyForGetItem);
        allowedForGetItem = true;
    }
    private IEnumerator delayForUsingMagic(){
        allowedForUsingMagic = false;
        yield return new WaitForSeconds(timeDeleyForUsingMagic);
        allowedForUsingMagic = true;
    }
}
/* //Fiz para brincar
    Collider2D[] selectSortForPhysics2D(Collider2D[] list){
        for(int i = 0; i < list.Length - 1 ; i++){ // menos 1 pois nao tem necessidade verificar se o ultimo é o menor do que os da direita, pois nao tem mais
            var itemNear = list[i];
            int pos = i;
            for(int j = i + 1; j < list.Length; j++){
                if(distForPlayer(itemNear) > distForPlayer(list[j])){ // verifica se é menor
                    itemNear = list[j];
                    pos = j;
                }
            }
            if(pos != i){ // ouve algem menor
                var itemI = list[i];
                list[i] = itemNear;
                list[pos] = itemI;
            }
        }
        return list;
    }
    */
    
    /*
    void OnTriggerStay2D(Collider2D collider){
        Debug.Log("presione e");

        if(collider.CompareTag("wand")){
            if(Input.GetKey(KeyCode.E)){
                Debug.Log("PRESCIONADO");
                wand = collider.transform.parent?.gameObject;
                wand.GetComponentInChildren<CircleCollider2D>().enabled = false;
            }
        }
    }
    */
