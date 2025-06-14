using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGameScreen : MonoBehaviour, ISetScreen {
    //HP/Mana Bar ====================================
    public GameObject HPBar;
    public GameObject ManaBar;
    public int maxRightHPManaBar = 500;
    //================================================
    //Grimore Bar ====================================
    public GameObject[] GrimoresBar = new GameObject[3];
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

    public void OnUpdateGrimore(byte idBox, int idItem) {
        Image boxSprite = GrimoresBar[idBox].transform.Find("SpriteItem").GetComponent<Image>();
        if (idItem == 0) {
            boxSprite.sprite = null;
            boxSprite.gameObject.SetActive(false);
        }
        else {
            boxSprite.gameObject.SetActive(true);
            boxSprite.sprite = ItemBank.GetItemFromId(idItem).Sprite;
        }
    }
    public void SetScreen(bool active = false, bool input = false) {
        // Ativar ou destivar a UI
        this.gameObject.SetActive(active);
        // Ativar ou desativar input do Player
        SetActivedInput(input);
    }
}
