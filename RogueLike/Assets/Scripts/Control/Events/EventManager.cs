using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private Player player;
    private UIManager ui;
    public void startConectionPlayerWithUI(Player player, UIManager ui){
        this.player = player;
        this.ui = ui;

        player.UpdatedBar += ui.OnUpdateBar;
    }
}
