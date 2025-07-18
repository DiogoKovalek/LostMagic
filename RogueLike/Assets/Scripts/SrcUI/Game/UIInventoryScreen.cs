using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryScreen : MonoBehaviour, ISetScreen {
    // Input Manager ===============================
    private InputActions input;
    private bool actCloseInventory;
    private bool actUP;
    private bool actDOWN;
    private bool actLEFT;
    private bool actRIGHT;
    private bool actGetTrowItem;
    private bool actDropItem;
    //==============================================
    // Event =======================================
    public delegate void TradeScreen(UIType uiType);
    public event TradeScreen TradedScreen;
    public delegate int[] GetItemsFromPlayer();
    public event GetItemsFromPlayer GotItemsFromPlayer;
    public delegate void UpdateItemsFromPlayer(int[] itemUpd);
    public event UpdateItemsFromPlayer UpdatedItemsFromPlayer;
    public delegate String[] GetAtributes();
    public event GetAtributes GotAtributes;
    public delegate Vector3 GetPosition();
    public event GetPosition GotPosition;
    //==============================================
    //Itens Box ====================================
    [SerializeField] GameObject[] InventoryBoxes = new GameObject[20];
    [SerializeField] GameObject StaffBox;
    [SerializeField] GameObject[] GrimoreBoxes = new GameObject[3];
    [SerializeField] GameObject ConsumableBox;
    [SerializeField] GameObject[] EquipmentBoxes = new GameObject[5];
    [SerializeField] GameObject[] RingBoxes = new GameObject[10];
    [SerializeField] GameObject HeldBox;
    private int[] listItems = new int[40]; // 20 inventario, 1, staff, 3 grimorio, 1 consumivel, 5 equipamento 10 aneis

    //posicao do seletor do jogador
    //Cima para baixo esquerda para direita
    private byte indexItemX = 0;
    private byte indexItemY = 0;

    //0x3C4F5B
    private Color32 cNormalIcon = new Color32(0x3C, 0x4F, 0x5B, 255);
    private Color32 cSelectIcon = new Color32(0xFF, 0xFF, 0xFF, 255);

    //Item Trow For Move
    private int itemHeld = 0;

    //==============================================

    // TextBox =====================================
    public GameObject BoxName;
    public GameObject BoxDescripition;
    public GameObject[] BoxesAtribute;
    //==============================================

    void Start() {
        drawIconSelectByIndex();
    }
    void Update() {
        getInput();

        if (actCloseInventory) {
            exitScreen();
        }

        if (actUP) {
            moveIndexDirection(DirectionM.up);
            updatePosBoxHeld();
            writeDescripitionByIndex();
        }
        else if (actDOWN) {
            moveIndexDirection(DirectionM.down);
            updatePosBoxHeld();
            writeDescripitionByIndex();
        }
        if (actLEFT) {
            moveIndexDirection(DirectionM.left);
            updatePosBoxHeld();
            writeDescripitionByIndex();
        }
        else if (actRIGHT) {
            moveIndexDirection(DirectionM.right);
            updatePosBoxHeld();
            writeDescripitionByIndex();
        }

        if (actGetTrowItem) {
            GetOrTrowItem();
        }
        if (actDropItem) {
            dropItem();
        }
    }
    private void exitScreen() {
        if (itemHeld != 0) {
            dropItem(true);
        }
        UpdatedItemsFromPlayer(listItems);
        eraseIconSelectByIndex();
        indexItemX = 0;
        indexItemY = 0;
        drawIconSelectByIndex();
        TradedScreen(UIType.Game);
    }
    private void getInput() {
        actCloseInventory = input.Inventory.CloseInventory.WasPressedThisFrame();
        actUP = input.Inventory.UP.WasPressedThisFrame();
        actDOWN = input.Inventory.DOWN.WasPressedThisFrame();
        actLEFT = input.Inventory.LEFT.WasPressedThisFrame();
        actRIGHT = input.Inventory.RIGHT.WasPressedThisFrame();
        actGetTrowItem = input.Inventory.GetTrowItem.WasPressedThisFrame();
        actDropItem = input.Inventory.DropItem.WasPressedThisFrame();
    }
    public void SetScreen(bool active = false, bool input = false) {
        this.gameObject.SetActive(active);
        if (active == true) {
            insertItems();
            writeDescripitionByIndex();
            writeAtributes();
        }

        if (this.input == null) this.input = new InputActions();
        if (input) {
            this.input.Inventory.Enable();
        }
        else {
            this.input.Inventory.Disable();
        }
    }

    private void insertItems() {
        int[] aux = GotItemsFromPlayer();
        listItems = aux; // atribui para a variavel de controle dos item
        byte point = 0;

        for (int i = 0; i < InventoryBoxes.Length; i++) {
            drawSpriteBox(InventoryBoxes[i], aux[point], point);
            point++;
        }

        drawSpriteBox(StaffBox, aux[point], point);
        point++;

        for (int i = 0; i < GrimoreBoxes.Length; i++) {
            drawSpriteBox(GrimoreBoxes[i], aux[point], point);
            point++;
        }

        drawSpriteBox(ConsumableBox, aux[point], point);
        point++;

        for (int i = 0; i < EquipmentBoxes.Length; i++) {
            drawSpriteBox(EquipmentBoxes[i], aux[point], point);
            point++;
        }

        for (int i = 0; i < RingBoxes.Length; i++) {
            drawSpriteBox(RingBoxes[i], aux[point], point);
            point++;
        }
    }

    private void drawSpriteBox(GameObject box, int idSprite, int idPointBox) {
        Transform BoxSprite = box.transform.Find("Sprite");
        if (idSprite != 0) {
            Image sprite = BoxSprite.GetComponent<Image>();
            Sprite spr = ItemBank.GetSpriteFromId(idSprite);
            sprite.gameObject.SetActive(true);
            sprite.sprite = spr;
            activeSpriteTypeItem(idPointBox, false);
        }
        else if (BoxSprite.gameObject.activeSelf && idSprite == 0) {
            Image sprite = BoxSprite.GetComponent<Image>();
            sprite.sprite = null;
            BoxSprite.gameObject.SetActive(false);
            activeSpriteTypeItem(idPointBox, true);
        }
    }
    private void updateSpriteInBoxByIndex(byte x, byte y, int idItem) {
        Image spriteBox = getBoxByIndex(x, y).transform.Find("Sprite").GetComponent<Image>();
        if (idItem == 0) { // desativa
            spriteBox.sprite = null;
            spriteBox.gameObject.SetActive(false);
        }
        else { // ativa
            spriteBox.gameObject.SetActive(true);
            spriteBox.sprite = ItemBank.GetSpriteFromId(idItem);
        }
        UpdatedItemsFromPlayer(listItems); // Atualiza inventario tambem
        writeDescripitionByIndex();
        writeAtributes();
    }
    private void moveIndexDirection(DirectionM dir) {
        eraseIconSelectByIndex();

        switch (dir) {
            case DirectionM.up:
                if (indexItemY == 0) {
                    if (indexItemX < 5) { // Esta no inventario
                        indexItemY = 3;
                    }
                    else { // Esta nos equipamentos
                        indexItemY = 4;
                    }
                }
                else {
                    indexItemY -= 1;
                }
                break;
            case DirectionM.down:
                if (indexItemY == 4 || (indexItemY == 3 && indexItemX < 5)) {
                    indexItemY = 0;
                }
                else {
                    indexItemY += 1;
                }
                break;
            case DirectionM.left:
                if (indexItemX == 0 || (indexItemY == 4 && indexItemX == 5)) {
                    indexItemX = 8;
                }
                else {
                    indexItemX -= 1;
                }
                break;
            case DirectionM.right:
                if (indexItemX == 8) {
                    if (indexItemY <= 3) {
                        indexItemX = 0;
                    }
                    else {
                        indexItemX = 5;
                    }
                }
                else {
                    indexItemX += 1;
                }
                break;
        }
        drawIconSelectByIndex();
    }
    private void updateBoxHeld(bool active) {
        HeldBox.gameObject.SetActive(active);
        HeldBox.transform.Find("Sprite").GetComponent<Image>().sprite = itemHeld != 0 ? ItemBank.GetSpriteFromId(itemHeld) : null;
        updatePosBoxHeld();
    }
    private void updatePosBoxHeld() {
        if (HeldBox.activeSelf == true) {
            float x = indexItemX <= 4 ? 180 + indexItemX * 170 : 1070 + (indexItemX - 5) * 140;
            float y = 900 - indexItemY * 140;
            HeldBox.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
        }
    }
    private void GetOrTrowItem() {
        int itemBoxId = getPosItemByIndex(indexItemX, indexItemY); // Id da lista de item
        if (itemHeld == 0 && listItems[itemBoxId] != 0) { // pegar item
            itemHeld = listItems[itemBoxId];
            listItems[itemBoxId] = 0;
            updateBoxHeld(true);

            activeSpriteTypeItem(itemBoxId, true);
            updateSpriteInBoxByIndex(indexItemX, indexItemY, listItems[itemBoxId]);
        }
        else if (itemHeld != 0 && listItems[itemBoxId] == 0) {// alocar item
            if (checkIfBoxTypeItem(itemHeld, itemBoxId)) {
                listItems[itemBoxId] = itemHeld;
                itemHeld = 0;
                updateBoxHeld(false);

                activeSpriteTypeItem(itemBoxId, false);
                updateSpriteInBoxByIndex(indexItemX, indexItemY, listItems[itemBoxId]);
            }
        }
        else if (itemHeld != 0 && listItems[itemBoxId] != 0) {// subistituir item, incluindo as posicoes
            if (checkIfBoxTypeItem(itemHeld, itemBoxId)) {
                int aux = listItems[itemBoxId];
                listItems[itemBoxId] = itemHeld;
                itemHeld = aux;
                updateBoxHeld(true);

                updateSpriteInBoxByIndex(indexItemX, indexItemY, listItems[itemBoxId]);
            }
        }
    }
    private void dropItem(bool dropItemHeld = false) {
        if (dropItemHeld) {
            GameObject itemDrop = ItemBank.CreateItemBasicById(itemHeld);
            itemDrop.transform.position = GotPosition();
            itemDrop.transform.SetParent(GameObject.Find("Controler").transform.Find("Floor"));
            itemHeld = 0;
            updateBoxHeld(false);
        }
        else {
            int idItem = listItems[getPosItemByIndex(indexItemX, indexItemY)];
            if (idItem != 0) {
                GameObject itemDrop = ItemBank.CreateItemBasicById(idItem);
                itemDrop.transform.position = GotPosition();
                itemDrop.transform.SetParent(GameObject.Find("Controler").transform.Find("Floor"));
                listItems[getPosItemByIndex(indexItemX, indexItemY)] = 0;
                updateSpriteInBoxByIndex(indexItemX, indexItemY, 0);
                activeSpriteTypeItem(getPosItemByIndex(indexItemX, indexItemY), true);
            }
        }
    }
    private void activeSpriteTypeItem(int id, bool active) {
        if (id >= 20) {
            getBoxById(id).transform.Find("SpriteTypeItem").gameObject.SetActive(active);
        }
    }
    /*
    private void activeSpriteTypeItem(int id, bool active) {
        if (id >= 20) {
            getBoxByIndex(indexItemX, indexItemY).transform.Find("SpriteTypeItem").gameObject.SetActive(active);
        }
    }
    */
    private bool checkIfBoxTypeItem(int idItem, int itemBoxId) {
        //Verifica o tipo da box
        //Em inventario e Equipamento é diferente o modo de tratar
        TypeItem tpItem = ItemBank.GetItemFromId(idItem).TypeItem;
        if (itemBoxId <= 19) { // Inventario
            return true;
        }
        else if (itemBoxId == 20 && tpItem == TypeItem.Staff) { // Cajado
            return true;
        }
        else if (21 <= itemBoxId && itemBoxId <= 23 && tpItem == TypeItem.Grimore) { // Grimorios
            return true;
        }
        else if (itemBoxId == 24 && tpItem == TypeItem.Comsumable) { // Consumivel
            return true;
        }
        else if (itemBoxId >= 25 && tpItem == TypeItem.Equipment) { // Equipamento
            // tipo de equipamento
            CategoryEquipment catItem = ItemBank.GetItemAs<EquipmentBase>(idItem).CategoryEquipment;
            if (itemBoxId == 25 && catItem == CategoryEquipment.Hat) {
                return true;
            }
            else if (itemBoxId == 26 && catItem == CategoryEquipment.Necklace) {
                return true;
            }
            else if (itemBoxId == 27 && catItem == CategoryEquipment.Cloak) {
                return true;
            }
            else if (itemBoxId == 28 && catItem == CategoryEquipment.Pants) {
                return true;
            }
            else if (itemBoxId == 29 && catItem == CategoryEquipment.Boot) {
                return true;
            }
            else if (itemBoxId >= 30 && idItem <= 39 && catItem == CategoryEquipment.Ring) {
                return true;
            }
        }
        return false;
    }
    private void drawIconSelectByIndex() {
        getContourBoxByIndex(indexItemX, indexItemY).GetComponent<Image>().color = cSelectIcon;
    }
    private void eraseIconSelectByIndex() {
        getContourBoxByIndex(indexItemX, indexItemY).GetComponent<Image>().color = cNormalIcon;
    }
    private GameObject getBoxByIndex(byte x, byte y) {
        GameObject obj = null;
        if (x < 5) { // inventario
            int ind = x + y * 5;
            obj = InventoryBoxes[ind];
        }
        else if (x >= 7) { // anel
            int ind = (x - 7) + y * 2;
            obj = RingBoxes[ind];
        }
        else if (x == 6) { // equipamento
            int ind = y;
            obj = EquipmentBoxes[ind];
        }
        else if (x == 5) { // cajado, grimorio e consumivel
            if (y == 0) { // cajado
                obj = StaffBox;
            }
            else if (y >= 1 && y <= 3) { // grimorio
                int ind = y - 1;
                obj = GrimoreBoxes[ind];
            }
            else if (y == 4) { // consumivel
                obj = ConsumableBox;
            }
        }
        return obj;
    }
    private GameObject getContourBoxByIndex(byte x, byte y) {
        return getBoxByIndex(x, y).transform.Find("Contour").gameObject;
    }
    private int getPosItemByIndex(byte x, byte y) {
        int id = 0;
        if (x < 5) { // inventario
            id = x + y * 5;
        }
        else if (x >= 7) { // anel
            id = 30 + (x - 7) + y * 2;
        }
        else if (x == 6) { // equipamento
            id = 25 + y;
        }
        else if (x == 5) { // cajado, grimorio e consumivel
            if (y == 0) { // cajado
                id = 20;
            }
            else if (y >= 1 && y <= 3) { // grimorio
                id = 20 + y;
            }
            else if (y == 4) { // consumivel
                id = 24;
            }
        }
        return id;
    }
    private GameObject getBoxById(int id) {
        if (id < 20) {
            return InventoryBoxes[id];
        }
        else if (id == 20) {
            return StaffBox;
        }
        else if (id < 24) {
            return GrimoreBoxes[id - 21];
        }
        else if (id == 24) {
            return ConsumableBox;
        }
        else if (id < 30) {
            return EquipmentBoxes[id - 25];
        } else {
            return RingBoxes[id - 30];
        }
    }
    private void writeDescripitionByIndex() {
        String strN = "";
        String strD = "";
        int item = listItems[getPosItemByIndex(indexItemX, indexItemY)];
        if (item != 0) {
            strN = ItemBank.GetItemFromId(item).NameItem;
            strD = ItemBank.GetItemFromId(item).Description;
        }
        BoxName.GetComponent<IText>().UpdateText(strN);
        BoxDescripition.GetComponent<IText>().UpdateText(strD);
    }
    private void writeAtributes() {
        if (GotAtributes != null) {
            String[] str = GotAtributes();
            for (byte i = 0; i < BoxesAtribute.Length; i++) {
                BoxesAtribute[i].GetComponent<IText>().UpdateText(str[i]);
            }
        }
    }
    private enum DirectionM {
        up,
        down,
        left,
        right,
    }
}

