using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EventManager : MonoBehaviour {
    private Player player;
    private UIManager ui;
    private Controler controler;

    public void addPlayer(Player player) {
        this.player = player;
    }
    public void addUIManager(UIManager ui) {
        this.ui = ui;
    }
    public void addControler(Controler controler) {
        this.controler = controler;
    }
    public void startConectionPlayerWithUI() {
        if (player == null || ui == null || controler == null) Debug.LogWarning("Eventos nao declarados");
        else {
            //Player
            player.TradedScreen += ui.OnTradeScreen;
            player.UpdatedBar += ui.uiGameScreen.OnUpdateBar;
            player.UpdatedBoxStaff += ui.uiGameScreen.OnUpdateBoxStaff;
            player.UpdatedGrimore += ui.uiGameScreen.OnUpdateGrimore;
            player.UpdatedGrimoreSelect += ui.uiGameScreen.OnUpdateGrimoreIndex;

            //Game Screen
            ui.uiGameScreen.SetActivedInput += player.OnSetActiveInput;

            //Inventory Screen
            ui.uiInventoryScreen.TradedScreen += ui.OnTradeScreen;
            ui.uiInventoryScreen.GotItemsFromPlayer += player.OnGetItemsFromPlayer;
            ui.uiInventoryScreen.UpdatedItemsFromPlayer += player.OnUpdateItemsFromPlayer;
            ui.uiInventoryScreen.GotAtributes += player.OnGetAtributes;
            ui.uiInventoryScreen.GotPosition += player.OnGetPosition;
        }
    }

    public void startConectionPlayerWithControler() {
        player.NextedLevel += controler.OnNextLevel;
    }
}
