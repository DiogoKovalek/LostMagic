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

public class Player : MonoBehaviour, IAtributesComunique, IManaManager {
    private Rigidbody2D rig;
    private bool isInvunerable = false;
    private bool freeForMove = true;
    private float timeForInvunerable = 1.1f;
    private float timeRecoil = 0.1f;

    private Controler controler;

    // Status ==============================
    public PlayerBase playerBase;
    private int maxLife;
    private int life;
    private int maxMana;
    private int mana;
    private int manaPerSecond;
    private int atack;
    private float speed;
    private int luck;
    private int MFire;
    private int MWater;
    private int MWind;
    private int MEarth;
    private int mVoid;
    //======================================

    //Mana recharge ========================
    private bool isRechargeMana = false;
    //======================================

    // Varibles for Staff ==================
    private GameObject staff;
    private Vector2 mousePosition;
    private float distanStaff = 0.4f;
    private bool allowedForUsingMagic = true;
    private GameObject handForStaff;
    //====================================

    // Variables for Grimore =============
    private GameObject[] grimores = new GameObject[3];
    private byte indexGrimoreActive = 0;
    private float speedGrimoreInOrbit = 95.0f;
    private float rayOrbit = 1.2f;
    private bool inOrbit = false;
    private GameObject handForGrimores;
    //====================================

    // Itens config ======================
    private float rayCollect = 2.0f;
    private int layerItem = 1 << 6; // or 0b1000000
    private GameObject interactionNear;
    private bool allowedForInteraction = true;
    private float timeDeleyForInteraction = 0.3f;
    //====================================

    // UI ================================
    public delegate void TradeScreen(UIType uiType);
    public event TradeScreen TradedScreen;
    public delegate void UpdateBar(int value, int maxValue, HPorMana op);
    public event UpdateBar UpdatedBar;
    public delegate void UpdateBoxStaff(int idItem);
    public event UpdateBoxStaff UpdatedBoxStaff;
    public delegate void UpdateGrimote(byte idBox, int idItem);
    public event UpdateGrimote UpdatedGrimore;
    public delegate void UpdateGrimoreSelect(byte newInd, byte oldInd = 3);
    public event UpdateGrimoreSelect UpdatedGrimoreSelect;
    public delegate void NextLevel();
    public event NextLevel NextedLevel;
    public delegate void UIForInteract(GameObject interact);
    public event UIForInteract UIForInteracted;
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
        handForStaff = transform.Find("HandForStaff").gameObject;
        handForGrimores = transform.Find("Grimores").gameObject;
        awakeStats();
    }
    private void awakeStats() {
        updateAtributes(true);

        //Para teste
        for (int i = 0; i < 8; i++) {
            inventory[i] = i + 1;
        }
    }

    #region OnDisable
    void OnDisable() {
        isRechargeMana = false;
        allowedForUsingMagic = true;
        inOrbit = false;
        allowedForInteraction = true;

        //Recarrega mana
        mana = maxMana;

        try {
            TradedScreen(UIType.None);
        }
        catch { }

        Transform projects = transform.Find("Projects");
        foreach (Transform proj in projects.transform) {
            Destroy(proj.gameObject);
        }
    }
    void OnEnable() {
        // inty Events =========================================
        if (controler == null) {
            controler = GameObject.Find("Controler").GetComponent<Controler>();
            controler.EventStartPlayer();
        }
        //======================================================
        // inty UI =============================================
        TradedScreen(UIType.Game);

        UpdatedBar(life, maxLife, HPorMana.HP);
        UpdatedBar(mana, maxMana, HPorMana.Mana);

        UpdatedGrimoreSelect(0);
        //======================================================

        foreach (int num in grimoresEquiped) {
            if (num != 0) {
                StartCoroutine(orbitPlayer());
                break;
            }
        }

    }
    #endregion



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
        if (allowedForInteraction && interactionNear != null && actInteract) {
            interaction();
        }
        #endregion

        #region using Staff
        usingStaff();
        #endregion

        #region Open and close inventory
        if (actOpenInventory) {
            TradedScreen(UIType.Inventory);
        }
        #endregion
        #region Grimore Menu
        switchGrimore();
        #endregion
    }
    void FixedUpdate() {
        #region Movment in FixedUpdate / aplicate velocity
        if (freeForMove) {
            rig.velocity = dirPlayer * speed;
        }
        #endregion

        #region Interaction colision
        Collider2D[] circleCollect = Physics2D.OverlapCircleAll(transform.position, rayCollect, layerItem);
        if (circleCollect.Length > 0) {
            interactionNear = getColNearFromThePlayer(circleCollect).gameObject;
            UIForInteracted(interactionNear);
        }
        else {
            interactionNear = null;
            UIForInteracted(null);
        }
        #endregion
    }

    void OnTriggerStay2D(Collider2D collision) {
        if (collision.tag == "Spike" && !isInvunerable) {
            CauseDamageInPlayer(3);
        }
    }

    // Para desenhar o circulo de colisao de item
    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rayCollect);
    }
    private void interaction() {
        if (allowedForInteraction && interactionNear.CompareTag("Item")) { // tirei o contains
            for (int i = 0; i < inventory.Length; i++) {
                if (inventory[i] == 0) {
                    inventory[i] = interactionNear.GetComponent<ItemForColect>().GetItem(); // Guarda item no inventory
                    StartCoroutine(delayForInteraction());
                    break;
                }
            }
        }
        else if (allowedForInteraction && interactionNear.CompareTag("Chest")) {
            interactionNear?.GetComponent<IChest>().OpenChest();
        }
        else if (allowedForInteraction && interactionNear.CompareTag("Portal")) {
            IPortal port = interactionNear?.GetComponent<IPortal>();
            if (port != null && port.ableForInteract()) {
                NextedLevel();
                this.gameObject.SetActive(false);
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
    }
    public void expendMana(int value) {
        this.mana -= value;
        UpdatedBar(mana, maxMana, HPorMana.Mana);
        if (!isRechargeMana) StartCoroutine(rechargeMana());
    }
    public int getTotalMana() {
        return mana;
    }
    public IEnumerator rechargeMana() {
        isRechargeMana = true;
        while (mana < maxMana) {
            yield return new WaitForSeconds(1.0f);
            if (mana + manaPerSecond > maxMana) mana = maxMana;
            else mana += manaPerSecond;
            UpdatedBar(mana, maxMana, HPorMana.Mana);
        }
        isRechargeMana = false;
    }
    private void switchGrimore() {
        if (actGrimore1 && indexGrimoreActive != 0) {
            UpdatedGrimoreSelect(0, indexGrimoreActive);
            indexGrimoreActive = 0;
        }
        else if (actGrimore2 && indexGrimoreActive != 1) {
            UpdatedGrimoreSelect(1, indexGrimoreActive);
            indexGrimoreActive = 1;
        }
        else if (actGrimore3 && indexGrimoreActive != 2) {
            UpdatedGrimoreSelect(2, indexGrimoreActive);
            indexGrimoreActive = 2;
        }
    }

    private IEnumerator orbitPlayer() {
        inOrbit = true;
        float angle = 0;
        byte cont = howManyGrimoresHave();
        while (cont >= 1) {
            angle = (angle + speedGrimoreInOrbit * Time.deltaTime) % 360f;
            for (int i = 0; i < grimores.Length; i++) {
                if (grimores[i] != null) {
                    float rad = (angle + 360f / cont * i) * Mathf.Deg2Rad;
                    Vector2 direction = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
                    grimores[i].transform.position = (Vector2)this.transform.position + (direction.normalized * rayOrbit);
                }
            }
            yield return null;
            cont = howManyGrimoresHave();
        }
        inOrbit = false;
    }
    private byte howManyGrimoresHave() {
        byte cont = 0;
        foreach (var g in grimores) {
            if (g != null) {
                cont++;
            }
        }
        return cont;
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

    private IEnumerator delayForInteraction() {
        allowedForInteraction = false;
        yield return new WaitForSeconds(timeDeleyForInteraction);
        allowedForInteraction = true;
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
            if (life <= 0) {
                TradedScreen(UIType.Death);
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
        updateAtributes();
    }

    public String[] OnGetAtributes() {
        String[] str = {maxLife.ToString(),
        maxMana.ToString(),
        manaPerSecond.ToString(),
        atack.ToString(),
        speed.ToString(),
        luck.ToString(),
        MFire.ToString(),
        MWater.ToString(),
        MWind.ToString(),
        MEarth.ToString(),
        mVoid.ToString()};
        return str;
    }
    public Vector3 OnGetPosition() {
        return this.gameObject.transform.position;
    }
    private void insertItems() { // depois que inserir todos os items no inventario, adiciona staff, grimore e outros no jogo
        // Staff Prescisa substituir
        if (staffEquiped != 0 && staff == null) { // Adiciona staff
            UpdatedBoxStaff(staffEquiped);
            staff = ItemBank.CreateStaffBasicById(staffEquiped, this.gameObject);
            staff.transform.SetParent(handForStaff.transform);
        }
        else if (staffEquiped == 0 && staff != null) { // Remove staff
            UpdatedBoxStaff(0);
            Destroy(staff);
            staff = null;
        }
        else if (staffEquiped != 0 && staff?.GetComponent<IStaff>().getIdItem() != staffEquiped) { // Atualizar staff se o novo for diferente
            Destroy(staff);
            UpdatedBoxStaff(staffEquiped);
            staff = ItemBank.CreateStaffBasicById(staffEquiped, this.gameObject);
            staff.transform.SetParent(handForStaff.transform);
        }
        // Grimore
        for (byte i = 0; i < grimoresEquiped.Length; i++) {
            if (grimoresEquiped[i] != 0 && grimores[i] == null) {
                UpdatedGrimore(i, grimoresEquiped[i]);
                grimores[i] = ItemBank.CreateGrimoreBasicById(grimoresEquiped[i]);
                grimores[i].transform.SetParent(handForGrimores.transform);
                if (!inOrbit) {
                    StartCoroutine(orbitPlayer());
                }
            }
            else if (grimoresEquiped[i] == 0 && grimores[i] != null) {
                UpdatedGrimore(i, 0);
                Destroy(grimores[i]);
                grimores[i] = null;
            }
            else if (grimoresEquiped[i] != 0 && grimores[i]?.GetComponent<IGrimore>().getIdItem() != grimoresEquiped[i]) {
                Destroy(grimores[i]);
                UpdatedGrimore(i, grimoresEquiped[i]);
                grimores[i] = ItemBank.CreateGrimoreBasicById(grimoresEquiped[i]);
                grimores[i].transform.SetParent(handForGrimores.transform);
                if (!inOrbit) { // so para garantir
                    StartCoroutine(orbitPlayer());
                }
            }
        }
    }
    private void updateAtributes(bool awaking = false) {
        // Primeiro o base
        maxLife = playerBase.maxLife;
        maxMana = playerBase.maxMana;
        manaPerSecond = playerBase.manaPerSecond;
        atack = playerBase.atack;
        speed = playerBase.speed;
        luck = playerBase.luck;
        MFire = playerBase.MFire;
        MWater = playerBase.MWater;
        MWind = playerBase.MWind;
        MEarth = playerBase.MEarth;
        mVoid = playerBase.MVoid;

        if (staffEquiped != 0) {
            StaffBase o = ItemBank.GetItemAs<StaffBase>(staffEquiped);
            MFire += o.MFire;
            MWater += o.MWater;
            MWind += o.MWind;
            MEarth += o.MEarth;
            mVoid += o.MVoid;
        }
        foreach (var equipament in equipaments) {
            if (equipament != 0) {
                EquipmentBase o = ItemBank.GetItemAs<EquipmentBase>(equipament);
                maxLife += o.HP;
                maxMana += o.Mana;
                manaPerSecond += o.ManaPerSecond;
                atack += o.Atack;
                speed += o.Speed;
                luck += o.Luck;
                MFire += o.MFire;
                MWater += o.MWater;
                MWind += o.MWind;
                MEarth += o.MEarth;
                mVoid += o.MVoid;
            }
        }
        foreach (var ring in rings) {
            if (ring != 0) {
                EquipmentBase o = ItemBank.GetItemAs<EquipmentBase>(ring);
                maxLife += o.HP;
                maxMana += o.Mana;
                manaPerSecond += o.ManaPerSecond;
                atack += o.Atack;
                speed += o.Speed;
                luck += o.Luck;
                MFire += o.MFire;
                MWater += o.MWater;
                MWind += o.MWind;
                MEarth += o.MEarth;
                mVoid += o.MVoid;
            }
        }
        if (!awaking) {
            if (maxLife < life) {
                life = maxLife;
            }
            if (maxMana < mana) {
                mana = maxMana;
            }

            UpdatedBar(life, maxLife, HPorMana.HP);
            UpdatedBar(mana, maxMana, HPorMana.Mana);
        }
        else {
            life = maxLife;
            mana = maxMana;
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

    #region Atributes Comunique
    public float CalculateSpeed() {
        return this.speed;
    }

    public int CalculateAtack(Element project, Element enemy) {
        int extra = 0;
        /*
        switch (project) {
            case Element.Void:
                extra = mVoid;
                break;
            case Element.Earth:
                switch (enemy) {
                    case Element.Earth:
                        extra = MEarth / 2;
                        break;
                    case Element.Wind:
                        extra = MEarth * 2;
                        break;
                    default:
                        extra = MEarth;
                        break;
                }
                break;
            case Element.Fire:
                switch (enemy) {
                    case Element.Earth:
                        extra = MEarth / 2;
                        break;
                    case Element.Wind:
                        extra = MEarth * 2;
                        break;
                    default:
                        extra = MEarth;
                        break;
                }
                break;
            case Element.Wind:
                switch (enemy) {
                    case Element.Earth:
                        extra = 0;
                        break;
                    case Element.Wind:
                        extra = MEarth / 2;
                        break;
                    default:
                        extra = MEarth;
                        break;
                }
                break;
            case Element.Water:
                switch (enemy) {
                    case Element.Earth:
                        extra = MEarth / 2;
                        break;
                    case Element.Wind:
                        extra = MEarth * 2;
                        break;
                    default:
                        extra = MEarth;
                        break;
                }
                break;
            case Element.Magma:
                break;
            case Element.Electric:
                break;
            case Element.Ice:
                break;
            case Element.Leaf:
                break;
            case Element.Blood:
                break;
            case Element.Iron:
                break;
            case Element.Soul:
                break;
            case Element.Poison:
                break;
        }
        */
        return atack + extra;
    }
    #endregion
}

