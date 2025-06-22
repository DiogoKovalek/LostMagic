using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grimore : MonoBehaviour, IGrimore {
    private GrimoreBase grimoreBase;
    private IMagic magicGrimore;
    private bool enableForMagic = true;
    private IManaManager mm;
    public int getIdItem() {
        return grimoreBase.id;
    }

    public void initGrimore(GrimoreBase grimoreBase) {
        this.grimoreBase = grimoreBase;
        GetComponent<SpriteRenderer>().sprite = this.grimoreBase.Sprite;
        magicGrimore = MagicBank.GetMagicFromEnum(this.grimoreBase.GrimoreMagic);
    }

    public void magic(GameObject staff) {
        if (mm == null) mm = staff.GetComponent<IStaff>().GetPlayerLinked().GetComponent<IManaManager>();
        if (enableForMagic && mm.getTotalMana() - grimoreBase.ManaExpend >= 0) {
            mm.expendMana(grimoreBase.ManaExpend);
            magicGrimore.magic(staff);
            StartCoroutine(delayMagic());
        }
    }

    private IEnumerator delayMagic() {
        enableForMagic = false;
        yield return new WaitForSeconds(grimoreBase.DelayMagic);
        enableForMagic = true;
    }
}
