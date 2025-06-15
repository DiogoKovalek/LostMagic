using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Player : MonoBehaviour {
    [SerializeField] float speed;
    private Rigidbody2D rig;
    private int maxLife = 10;
    private int life;
    private bool isInvunerable = false;
    private bool freeForMove = true;
    private float timeForInvunerable = 1.1f;
    private float timeRecoil = 0.1f;

    // Varibles for Staff ==================
    private GameObject staff;
    private Vector2 mousePosition;
    private float distanStaff = 0.4f;
    private bool allowedForUsingMagic = true;
    [SerializeField] GameObject handForStaff;
    //====================================

    // Variables for Grimore =============
    private GameObject[] grimores = new GameObject[3];
    private byte indexGrimoreActive = 0;
    //====================================

    // Itens config ======================
    private float rayCollect = 2.0f;
    private int layerForCollect = 1 << 6; // or 0b1000000
    private GameObject itemNear;
    private bool allowedForGetItem = true;
    private float timeDeleyForGetItem = 0.5f;
    //====================================

    // UI ================================
    public delegate void TradeScreen(UIType uiType);
    public event TradeScreen TradedScreen;
    public delegate void UpdateBar(int value, int maxValue, HPorMana op);
    public event UpdateBar UpdatedBar;
    public delegate void UpdateGrimote(byte idBox, int idItem);
    public event UpdateGrimote UpdatedGrimore;
    //====================================

    // Inventory =========================
    private int[] inventory = new int[20];
    private int staffEquiped;
    private int[] grimoresEquiped = new int[3];
    private int consumableEquiped;
    private int[] equipaments = new int[5];
    private int[] rings = new int[10];
    //====================================

    // Input Manager =====================
    private InputActions input;
    private Vector2 dirPlayer;
    private bool actAtack;
    private bool actInteract;
    private bool actOpenInventory;
    private bool actGrimore1;
    private bool actGrimore2;
    private bool actGrimore3;
    private bool actConsumable;
    private bool actPause;
    //====================================

    // Start is called before the first frame update
    void Awake() {
        rig = GetComponent<Rigidbody2D>();
        input = new InputActions();
        life = maxLife;
    }
    void Start() {
        // inty Events =========================================
        GameObject controler = GameObject.Find("Controler");
        controler.GetComponent<Controler>().EventStartPlayer();
        //======================================================
        // inty UI =============================================
        TradedScreen(UIType.Game);

        if (UpdatedBar != null) {
            UpdatedBar(life, maxLife, HPorMana.HP);
        }
        //======================================================
    }
    void Update() {

        getInput();

        #region Mouse and wand tranform
        if (staff != null) {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 circleInMouseForPlayer = (mousePosition - (Vector2)handForStaff.transform.position).normalized;

            staff.transform.position = (Vector2)handForStaff.transform.position + circleInMouseForPlayer * distanStaff;
            float angleMouse = Mathf.Atan2(circleInMouseForPlayer.y, circleInMouseForPlayer.x) * Mathf.Rad2Deg;
            staff.transform.rotation = Quaternion.Euler(0, 0, angleMouse);
        }
        #endregion

        #region GetItem and drop
        if (allowedForGetItem && itemNear != null && actInteract) {
            getItem();
        }
        #endregion

        #region using Staff
        usingStaff();
        #endregion

        #region Open and close inventory
        if (actOpenInventory) {
            /*// Para teste de lotamento no inventario
            inventory = new int[] {1,2,3,1,2,3,1,2,3,1,2,3,1,2,3,1,2,3,1,2};
            staffEquiped = 3;
            grimoresEquiped = new int[] {1,2,3};
            consumableEquiped = 1;
            equipaments = new int[] {1,2,3,1,2};
            rings = new int[] {1,2,3,1,2,3,1,2,3,1};
            */
            TradedScreen(UIType.Inventory);
        }
        #endregion
    }
    void FixedUpdate() {
        #region Movment in FixedUpdate / aplicate velocity
        if (freeForMove) {
            rig.velocity = dirPlayer * speed;
        }
        #endregion

        #region Itens colision
        Collider2D[] circleCollect = Physics2D.OverlapCircleAll(transform.position, rayCollect, layerForCollect);
        if (circleCollect.Length > 0) {
            itemNear = getColNearFromThePlayer(circleCollect).gameObject;
        }
        else {
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
    private void getItem() {
        if (inventory.Contains(0) && allowedForGetItem) {
            for (int i = 0; i < inventory.Length; i++) {
                if (inventory[i] == 0) {
                    inventory[i] = itemNear.GetComponent<ItemForColect>().GetItem(); // Guarda item no inventory
                    StartCoroutine(delayForGetItem());
                    break;
                }
            }
        }
    }
    private void usingStaff() {
        if (allowedForUsingMagic && staff != null) {
            IStaff staffInter = staff.GetComponent<IStaff>();
            if (actAtack) {
                staffInter.attack(grimores[indexGrimoreActive]);
            }
        }
        /*
        if (allowedForUsingMagic && staff != null) {
            IActionsWand wandInter = wand.GetComponent<IActionsWand>();
            if (wandInter == null) return;
            if (Input.GetKey(KeyCode.Mouse0)) {
                wandInter.attack();
                StartCoroutine(delayForUsingMagic());
            }
            else if (Input.GetKey(KeyCode.Mouse1)) {
                wandInter.conjure();
                StartCoroutine(delayForUsingMagic());
            }
            else if (Input.GetKey(KeyCode.Space)) {
                wandInter.defense();
                StartCoroutine(delayForUsingMagic());
            }
            else if (Input.GetKey(KeyCode.Q)) {
                wandInter.grimoreMagic();
                StartCoroutine(delayForUsingMagic());
            }
        }
        */
    }

    Collider2D getColNearFromThePlayer(Collider2D[] list) {
        var near = list[0];
        for (int i = 1; i < list.Length; i++) {
            if (distForPlayer(near) > distForPlayer(list[i])) {
                near = list[i];
            }
        }
        return near;
    }
    private double distForPlayer(Collider2D col) {
        return Math.Sqrt(Math.Pow(col.transform.position.x - this.transform.position.x, 2) + Math.Pow(col.transform.position.y - this.transform.position.y, 2));
    }

    private IEnumerator delayForGetItem() {
        allowedForGetItem = false;
        yield return new WaitForSeconds(timeDeleyForGetItem);
        allowedForGetItem = true;
    }
    private IEnumerator delayForInvunerable() {
        isInvunerable = true;
        yield return new WaitForSeconds(timeForInvunerable);
        isInvunerable = false;
    }

    private IEnumerator recoilAttack(Vector2 posE, float force) {
        freeForMove = false;
        Vector2 direction = ((Vector2)transform.position - posE).normalized;
        rig.velocity = direction * force;
        yield return new WaitForSeconds(timeRecoil);
        freeForMove = true;
    }
    public void CauseDamageInPlayer(int attack) {
        if (!isInvunerable) {
            this.life -= attack;
            if (UpdatedBar != null) {
                UpdatedBar(life, maxLife, HPorMana.HP);
            }
            StartCoroutine(delayForInvunerable());
        }
    }
    public void RecoilAttack(Vector2 posE, float force) {
        if (!isInvunerable) {
            StartCoroutine(recoilAttack(posE, force));
        }
    }

    public int[] OnGetItemsFromPlayer() {
        int[] aux = new int[40];
        System.Array.Copy(inventory, 0, aux, 0, inventory.Length); // Copia de array para nao prescisar usar for
        aux[20] = staffEquiped;
        System.Array.Copy(grimoresEquiped, 0, aux, 21, grimoresEquiped.Length);
        aux[24] = consumableEquiped;
        System.Array.Copy(equipaments, 0, aux, 25, equipaments.Length);
        System.Array.Copy(rings, 0, aux, 30, rings.Length);

        return aux;
    }

    public void OnUpdateItemsFromPlayer(int[] itemUpd) {
        try {
            byte count = 0;
            System.Array.Copy(itemUpd, count, inventory, 0, inventory.Length);
            count += (byte)inventory.Length;
            staffEquiped = itemUpd[count];
            count++;
            System.Array.Copy(itemUpd, count, grimoresEquiped, 0, grimoresEquiped.Length);
            count += (byte)grimoresEquiped.Length;
            consumableEquiped = itemUpd[count];
            count++;
            System.Array.Copy(itemUpd, count, equipaments, 0, equipaments.Length);
            count += (byte)equipaments.Length;
            System.Array.Copy(itemUpd, count, rings, 0, rings.Length);
        }
        catch {
            Debug.LogWarning("Erro do tamanho de lista em OnUpdateItemsFromPlayer");
        }
        insertItems();
    }
    private void insertItems() { // depois que inserir todos os items no inventario, adiciona staff, grimore e outros no jogo
        // Staff Prescisa substituir
        if (staffEquiped != 0 && staff == null) { // Adiciona staff
            staff = ItemBank.CreateStaffBasicById(staffEquiped);
        }
        else if (staffEquiped == 0 && staff != null) { // Remove staff
            Destroy(staff);
            staff = null;
        }
        // Grimore
        for (byte i = 0; i < grimoresEquiped.Length; i++) {
            if (grimoresEquiped[i] != 0 && grimores[i] == null) {
                UpdatedGrimore(i, grimoresEquiped[i]);
                grimores[i] = ItemBank.CreateGrimoreBasicById(grimoresEquiped[i]);
            }
            else if (grimoresEquiped[i] == 0 && grimores[i] != null) {
                UpdatedGrimore(i, 0);
                Destroy(grimores[i]);
                grimores[i] = null;
            }
        }
    }


    #region InputManager
    private void getInput() {
        dirPlayer = input.PlayerGame.Movement.ReadValue<Vector2>();
        actAtack = input.PlayerGame.Atack.IsPressed();
        actInteract = input.PlayerGame.Interaction.WasPressedThisFrame();
        actOpenInventory = input.PlayerGame.OpenInventory.WasPressedThisFrame();
        actGrimore1 = input.PlayerGame.Grimore1.WasPressedThisFrame();
        actGrimore2 = input.PlayerGame.Grimore2.WasPressedThisFrame();
        actGrimore3 = input.PlayerGame.Grimore3.WasPressedThisFrame();
        actConsumable = input.PlayerGame.Consumable.WasPressedThisFrame();
        actPause = input.PlayerGame.Pause.WasPressedThisFrame();

    }
    public void OnSetActiveInput(bool isActive) {
        if (isActive) {
            input.PlayerGame.Enable();
        }
        else {
            input.PlayerGame.Disable();
        }
    }
    #endregion
}








/* //Codigo para pegar wand
            if (itemNear.tag == "wand") {
                if (wand != null) { // dropa a wanda atual
                    wand.transform.rotation = Quaternion.Euler(0, 0, 0);
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
            */

