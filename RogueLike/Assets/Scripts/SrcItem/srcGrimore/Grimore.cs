using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grimore : MonoBehaviour, IGrimore {
    private GrimoreBase grimoreBase;
    private IMagic magicGrimore;
    public void initGrimore(GrimoreBase grimoreBase) {
        GetComponent<SpriteRenderer>().sprite = grimoreBase.Sprite;
        magicGrimore = ItemBank.getMagicFromEnum(grimoreBase.GrimoreMagic);
    }

    public void magic(GameObject staff) {
        magicGrimore.magic(staff);
    }
}

public enum Magic {
    Teste
}
