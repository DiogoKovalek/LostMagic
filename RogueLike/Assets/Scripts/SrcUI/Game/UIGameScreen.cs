using System;
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
    //Buton E ========================================
    public GameObject PrefabBtE;
    private GameObject BtE;
    private GameObject interact;
    private float distBTfromInteract = 1f;
    //================================================
    //Map ============================================
    public Transform MapCamp;
    public GameObject SquareMap;
    private float opcNotAcess = 0.3f;
    public GameObject CirclePlayerMap;
    private int[] mapPosElement = {16, 44, 72, 100, 128, 156, 184, 212, 240, 268};
    private GameObject[,] mapSquare = new GameObject[10, 10];
    private byte[,] miniMap = new byte[10, 10];
    // tem 4 valores possiveis
    //0 para não existe sala,
    //1 para tem sala porem não foi descoberta
    //2 para sala descoberta porem não acessada
    //3 para sala acessada
    //================================================
    // Consumable Box ================================
    public GameObject ConsumableBox;
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
            box.sprite = ItemBank.GetSpriteFromId(idItem);
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
            boxSprite.sprite = ItemBank.GetSpriteFromId(idItem);
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

    public void OnUpdateConsumable(int idItem) {
        Image boxSprite = ConsumableBox.transform.Find("Sprite").GetComponent<Image>();
        if (idItem == 0) {
            boxSprite.sprite = null;
            boxSprite.gameObject.SetActive(false);
        }
        else {
            boxSprite.gameObject.SetActive(true);
            boxSprite.sprite = ItemBank.GetSpriteFromId(idItem);
        }
    }
    public void OnUIForInterct(GameObject interact) {
        if (interact != null) {
            Vector2 posForm = new Vector2(interact.transform.position.x, interact.transform.position.y + distBTfromInteract);
            if (BtE == null) {
                BtE = Instantiate(PrefabBtE, posForm, PrefabBtE.transform.rotation);
                this.interact = interact;
            }
            else if (this.interact != interact) {
                BtE.transform.position = posForm;
                this.interact = interact;
            }
        }
        else if (BtE != null) {
            Destroy(BtE);
            this.interact = null;
        }
    }
    public void OnSendMapForUI(RoomConfig[,] map) {
        miniMap = new byte[10, 10];
        for (byte i = 0; i < 10; i++) {
            for (byte j = 0; j < 10; j++) {
                // Implementar sala
                if (map[i, j] != null) {
                    miniMap[i, j] = 1;
                }

                //Limpar mapa antigo
                if (mapSquare[i, j] != null) {
                    Destroy(mapSquare[i, j]);
                    mapSquare[i, j] = null;
                }
            }
        }
    }
    public void OnTradeRoomUI(int x, int y) {
        // sala acessada
        miniMap[x, y] = 3;
        // Salas a serem descobertas ao redor
        if (miniMap[x - 1, y] == 1) miniMap[x - 1, y] = 2;
        if (miniMap[x + 1, y] == 1) miniMap[x + 1, y] = 2;
        if (miniMap[x, y - 1] == 1) miniMap[x, y - 1] = 2;
        if (miniMap[x, y + 1] == 1) miniMap[x, y + 1] = 2;

        //Atualizar minimapa
        for (byte i = 0; i < 10; i++) {
            for (byte j = 0; j < 10; j++) {
                if (miniMap[i, j] != 0 && miniMap[i, j] != 1) {
                    if (miniMap[i, j] == 2 && mapSquare[i, j] == null) {
                        mapSquare[i, j] = Instantiate(SquareMap, MapCamp);
                        mapSquare[i, j].GetComponent<RectTransform>().anchoredPosition = new Vector2(mapPosElement[i], -mapPosElement[j]); // Cria sala nao acessda
                        Image img = mapSquare[i, j].GetComponent<Image>();
                        Color corSquare = img.color;
                        corSquare.a = opcNotAcess;
                        img.color = corSquare;
                    }
                    else if (miniMap[i, j] == 3 && mapSquare[i, j]?.GetComponent<Image>().color.a == opcNotAcess) { // Atualiza cor
                        Image img = mapSquare[i, j].GetComponent<Image>();
                        Color corSquare = img.color;
                        corSquare.a = 1;
                        img.color = corSquare;
                    }
                    else if (miniMap[i, j] == 3 && mapSquare[i, j] == null) { // Cria sala ja acessada
                        mapSquare[i, j] = Instantiate(SquareMap, MapCamp);
                        mapSquare[i, j].GetComponent<RectTransform>().anchoredPosition = new Vector2(mapPosElement[i], -mapPosElement[j]);
                    }
                }
            }
        }
        // posicao do player
        CirclePlayerMap.GetComponent<RectTransform>().anchoredPosition = new Vector2(mapPosElement[x], -mapPosElement[y]);
    }
    public void SetScreen(bool active = false, bool input = false) {
        // Ativar ou destivar a UI
        this?.gameObject.SetActive(active);
        // Ativar ou desativar input do Player
        if (SetActivedInput != null) SetActivedInput(input);
    }
}
