using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grimore : MonoBehaviour, IGrimore {
    private GrimoreBase grimoreBase;

    public void initGrimore(GrimoreBase grimoreBase) {
        GetComponent<SpriteRenderer>().sprite = grimoreBase.Sprite;
    }

    public void magic() {
        Debug.Log("Magia1");
    }
}
