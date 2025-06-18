using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grimore : MonoBehaviour, IGrimore {
    private GrimoreBase grimoreBase;
    private IMagic magicGrimore;
    public void initGrimore(GrimoreBase grimoreBase) {
        this.grimoreBase = grimoreBase;
        GetComponent<SpriteRenderer>().sprite = this.grimoreBase.Sprite;
        magicGrimore = ItemBank.getMagicFromEnum(this.grimoreBase.GrimoreMagic);
    }

    public void magic(GameObject staff) {
        magicGrimore.magic(staff);
    }
}

public enum Magic {
    Teste
}
