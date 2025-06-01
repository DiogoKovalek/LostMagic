using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameScreen : MonoBehaviour, ISetScreen
{
    //HP/Mana Bar ====================================
    public GameObject HPBar;
    public GameObject ManaBar;
    public int maxRightHPManaBar = 500;
    //================================================

    // event Player ==================================
    public delegate void SetActiveInput(bool isActive);
    public event SetActiveInput SetActivedInput;
    //================================================

    public void OnUpdateBar(int value, int maxValue, HPorMana op) {
        RectTransform rect;
        switch (op) {
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

        float porcent = (float)(value * 100 / maxValue) / 100;
        if (porcent <= 0) porcent = 0.0f; // Garante que nao vai passar de maxRightHPManaBar
        float deslocRight = maxRightHPManaBar - maxRightHPManaBar * porcent;

        if (rect != null) {
            rect.offsetMax = new Vector2(-deslocRight, rect.offsetMax.y);
        }
    }

    public void SetScreen(bool active = false, bool input = false) {
        // Ativar ou destivar a UI
        this.gameObject.SetActive(active);
        // Ativar ou desativar input do Player
        SetActivedInput(input);
    }
}
