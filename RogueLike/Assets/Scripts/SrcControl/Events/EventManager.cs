using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {
    private Player player;
    private UIManager ui;
    public void startConectionPlayerWithUI(Player player, UIManager ui) {
        this.player = player;
        this.ui = ui;

        //Player
        player.TradedScreen += ui.OnTradeScreen;
        player.UpdatedBar += ui.uiGameScreen.OnUpdateBar;

        //Game Screen
        ui.uiGameScreen.SetActivedInput += player.OnSetActiveInput;

        //Inventory Screen
        ui.uiInventoryScreen.TradedScreen += ui.OnTradeScreen;
        ui.uiInventoryScreen.GotItemsFromPlayer += player.OnGetItemsFromPlayer;
    }
}
