using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGameScreen : MonoBehaviour, ISetScreen {
    //HP/Mana Bar ====================================
    public GameObject HPBar;
    public GameObject TextHPBar;
    public GameObject ManaBar;
    public GameObject TextManaBar;
    public int maxRightHPManaBar = 500;
    //================================================
    //Staff Box ======================================
    public GameObject StaffBox;
    //================================================
    //Grimore Bar ====================================
    public GameObject[] GrimoresBar = new GameObject[3];
    private Color32 cNormalIcon = new Color32(0x3C, 0x4F, 0x5B, 255);
    private Color32 cSelectIcon = new Color32(0xFF, 0xFF, 0xFF, 255);
    //================================================
    // event Player ==================================
    public delegate void SetActiveInput(bool isActive);
    public event SetActiveInput SetActivedInput;
    //================================================

    public void OnUpdateBar(int value, int maxValue, HPorMana op) {
        RectTransform rect;
        switch (op) {
            case HPorMana.HP:
                TextHPBar.GetComponent<IText>().UpdateText(value + "/" + maxValue);
                rect = HPBar.GetComponent<RectTransform>();
                break;
            case HPorMana.Mana:
                TextManaBar.GetComponent<IText>().UpdateText(value + "/" + maxValue);
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
    public void OnUpdateBoxStaff(int idItem) {
        Image box = StaffBox.transform.Find("Sprite").GetComponent<Image>();
        if (idItem == 0) {
            box.sprite = null;
            box.gameObject.SetActive(false);
        }
        else {
            box.gameObject.SetActive(true);
            box.sprite = ItemBank.GetItemFromId(idItem).Sprite;
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
    public void OnUpdateGrimoreIndex(byte newInd, byte oldInd = 3) {
        Image contourBox = GrimoresBar[newInd].transform.Find("Contour").GetComponent<Image>();
        contourBox.color = cSelectIcon;
        if (oldInd < 3) {
            Image contourBoxOld = GrimoresBar[oldInd].transform.Find("Contour").GetComponent<Image>();
            contourBoxOld.color = cNormalIcon;
        }
    }
    public void SetScreen(bool active = false, bool input = false) {
        // Ativar ou destivar a UI
        this.gameObject.SetActive(active);
        // Ativar ou desativar input do Player
        SetActivedInput(input);
    }
}
