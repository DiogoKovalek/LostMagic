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
