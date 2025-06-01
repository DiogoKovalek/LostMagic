using System.Collections;
using System.Collections.Generic;
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
    //==============================================
    // Event =======================================
    public delegate void TradeScreen(UIType uiType);
    public event TradeScreen TradedScreen;
    public delegate int[] GetItemsFromPlayer();
    public event GetItemsFromPlayer GotItemsFromPlayer;
    //==============================================
    //Itens Box ====================================
    [SerializeField] GameObject[] InventoryBoxes = new GameObject[20];
    [SerializeField] GameObject StaffBox;
    [SerializeField] GameObject[] GrimoreBoxes = new GameObject[3];
    [SerializeField] GameObject ConsumableBox;
    [SerializeField] GameObject[] EquipmentBoxes = new GameObject[5];
    [SerializeField] GameObject[] RingBoxes = new GameObject[10];

    //20 , 1, 3, 1, 10
    private int[] items = new int[40]; // 20 primeiro para inventario, 1 para staff, 3 grimorio, 1 Consumable, 10 rings

    //==============================================
    void Update() {
        getInput();
        if (actCloseInventory) {
            TradedScreen(UIType.Game);
        }
    }
    private void getInput() {
        actCloseInventory = input.Inventory.CloseInventory.WasPressedThisFrame();
        actUP = input.Inventory.UP.WasPressedThisFrame();
        actDOWN = input.Inventory.DOWN.WasPressedThisFrame();
        actLEFT = input.Inventory.LEFT.WasPressedThisFrame();
        actRIGHT = input.Inventory.RIGHT.WasPressedThisFrame();
        actGetTrowItem = input.Inventory.GetTrowItem.WasPressedThisFrame();
    }
    public void SetScreen(bool active = false, bool input = false) {
        this.gameObject.SetActive(active);
        if (active == true) {
            insertItems();
        }

        this.input = new InputActions();
        if (input) {
            this.input.Inventory.Enable();
        }
        else {
            this.input.Inventory.Disable();
        }
    }
    private void insertItems() {
        int[] aux = GotItemsFromPlayer();
        byte point = 0;

        for (int i = 0; i < InventoryBoxes.Length; i++) {
            drawSpriteBox(InventoryBoxes[i], aux[point]);
            point++;
        }

        drawSpriteBox(StaffBox, aux[point]);
        point++;

        for(int i = 0; i < GrimoreBoxes.Length; i++){
            drawSpriteBox(GrimoreBoxes[i], aux[point]);
            point++;
        }

        drawSpriteBox(ConsumableBox, aux[point]);
        point++;
        
        for(int i = 0; i < EquipmentBoxes.Length; i++){
            drawSpriteBox(EquipmentBoxes[i], aux[point]);
            point++;
        }

        for(int i = 0; i < RingBoxes.Length; i++){
            drawSpriteBox(RingBoxes[i], aux[point]);
            point++;
        }
    }

    private void drawSpriteBox(GameObject box, int idSprite) {
        if (idSprite != 0) {
            Image spriteBox = box.transform.Find("Sprite").GetComponent<Image>();
            Sprite spr = ItemBank.GetSpriteFromId(idSprite);
            spriteBox.gameObject.SetActive(true);
            spriteBox.sprite = spr;
        }
    }
}
