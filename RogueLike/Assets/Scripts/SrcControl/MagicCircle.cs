using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCircle : MonoBehaviour, IPortal {
    private float timeToStay = 2.0f;
    private GameObject player;
    [SerializeField] TypePortal typePortal;
    private byte greenMin = 100;
    private byte greenMax = 200;
    private byte speedTradeColor = 10;
    private Color32 colorInit = new Color32(0x00, 0xC8, 0xFF, 255);//00C8FF
    private Color32 colorDisable = new Color32(0xCB, 0xEE, 0xFF, 255);//CBEEFF
    private SpriteRenderer spRen;
    private bool isSwitch = true;
    public void initiPortal(TypePortal type) {
        spRen = this.GetComponent<SpriteRenderer>();
        StartCoroutine(switchColor());
        player = GameObject.Find("Controler").GetComponent<Controler>().player;
        typePortal = type;
        switch (typePortal) {
            case TypePortal.Spawn:
                StartCoroutine(invokePlayer());
                break;
            case TypePortal.NextLevel:
                StartCoroutine(nextLevel());
                break;
        }
    }
    public bool ableForInteract() {
        if (typePortal == TypePortal.Spawn) return false;
        return true;
    }
    private IEnumerator invokePlayer() {
        yield return new WaitForSeconds(timeToStay);
        isSwitch = false;
        player.SetActive(true);
        player.transform.position = this.transform.position;
    }

    private IEnumerator nextLevel() {
        yield return new WaitForEndOfFrame();
    }

    private IEnumerator switchColor() {
        spRen.color = colorInit;
        byte green = (byte)(spRen.color.g * 255);
        bool isGreenUp = false;
        while (true) {
            if (isGreenUp) {
                green += speedTradeColor;
                if (green > greenMax) {
                    green = greenMax;
                    isGreenUp = !isGreenUp;
                }
            }
            else {
                green -= speedTradeColor;
                if (green < greenMin) {
                    green = greenMin;
                    isGreenUp = !isGreenUp;
                }
            }
            spRen.color = new Color32((byte)(spRen.color.r * 255), green, (byte)(spRen.color.b * 255), (byte)(spRen.color.a * 255));
            yield return new WaitForSeconds(0.08f);
            if (!isSwitch) {
                disablePortal();
                break;
            }
        }
    }

    private void disablePortal() {
        spRen.color = colorDisable;
    }
}
public interface IPortal {
    void initiPortal(TypePortal type);
    bool ableForInteract();
}
public enum TypePortal {
    Spawn,
    NextLevel,
    trap
}
