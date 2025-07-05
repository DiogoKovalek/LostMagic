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
    //================================================

    void Start() {
        uiGameScreen = GameScreen.GetComponent<UIGameScreen>();
        uiInventoryScreen = InventoryScreen.GetComponent<UIInventoryScreen>();
    }

    public void OnTradeScreen(UIType uis){
        switch(uis){
            case UIType.None:
                uiGameScreen.SetScreen();
                uiInventoryScreen.SetScreen();
                break;
            case UIType.Game:
                uiGameScreen.SetScreen(true, true);
                uiInventoryScreen.SetScreen();
                break;
            case UIType.Inventory:
                uiGameScreen.SetScreen();
                uiInventoryScreen.SetScreen(true,true);
                break;
            case UIType.Pause:
                break;
            case UIType.Death:
                break;
            case UIType.Loading:
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
