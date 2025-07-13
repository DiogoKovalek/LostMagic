using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.OnScreen;

public class UIManager : MonoBehaviour {
    
    //UIGame / UIInventory ===========================
    public GameObject GameScreen;
    public GameObject InventoryScreen;
    //================================================

    //References Screens =============================
    public UIGameScreen uiGameScreen;
    public UIInventoryScreen uiInventoryScreen;
    public UIGameOver uiGameOver;
    public UILoad uiLoad;
    //================================================

    void Start() {
        uiGameScreen = GameScreen.GetComponent<UIGameScreen>();
        uiInventoryScreen = InventoryScreen.GetComponent<UIInventoryScreen>();
    }

    public void OnTradeScreen(UIType uis){
        uiGameScreen?.SetScreen();
        uiInventoryScreen?.SetScreen();
        uiLoad?.SetScreen();
        uiGameOver?.SetScreen();
        switch (uis) {
            case UIType.None:         
                break;
            case UIType.Game:
                uiGameScreen.SetScreen(true, true);
                break;
            case UIType.Inventory:
                uiInventoryScreen.SetScreen(true, true);
                break;
            case UIType.Pause:
                break;
            case UIType.Death:
                uiGameOver.SetScreen(true, true);
                break;
            case UIType.Loading:
                uiLoad.SetScreen(true, true);
                break;
            default:
                Debug.LogWarning("UIManager default UIScreen");
                break;
        }
    }
}
public enum UIType {
    None,
    Game,
    Inventory,
    Pause,
    Death,
    Loading,
}

public enum HPorMana {
    HP,
    Mana
}
