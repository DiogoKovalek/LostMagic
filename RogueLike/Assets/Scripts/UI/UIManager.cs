using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    //HP/Mana Bar ====================================
    public GameObject HPBar;
    public GameObject ManaBar;
    public int maxRightHPManaBar = 152;
    //================================================

    public void OnUpdateBar(int value, int maxValue, HPorMana op){
        RectTransform rect;
        switch(op){
            case HPorMana.HP:
                rect = HPBar.GetComponent<RectTransform>();
                break;
            case HPorMana.Mana:
                rect = ManaBar.GetComponent<RectTransform>();
                break;
            default:
                rect = null;
                break;
        }

        float porcent = (float)(value*100/maxValue)/100;
        if(porcent <= 0) porcent = 0.0f; // Garante que nao vai passar de maxRightHPManaBar
        float deslocRight = maxRightHPManaBar - maxRightHPManaBar*porcent;
    
        if(rect != null){
            rect.offsetMax = new Vector2(-deslocRight, rect.offsetMax.y);
        }
    }
}
public enum HPorMana{
    HP,
    Mana
}
