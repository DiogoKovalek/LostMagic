using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grimore : MonoBehaviour,IGrimore
{
    private GrimoreBase grimoreBase;
    public float DelayMagic;
    public int ManaExpend;
    private IManaManager mm;
    private IMagic magic;

    private bool enebleForMagic = true;

    void OnDisable() {
        enebleForMagic = true;
    }
    public int getIdItem() {
        return grimoreBase.id;
    }
    public GrimoreBase getGrimoreBase() {
        return grimoreBase;
    }
    public void initGrimore(GrimoreBase grimoreBase) {
        this.grimoreBase = grimoreBase;
        GetComponent<SpriteRenderer>().sprite = this.grimoreBase.Sprite;
        magic = this?.GetComponent<IMagic>();
    }

    public void conjureMagic(Vector2 direction) {
        if(mm == null) mm = this.transform.parent.transform.parent?.GetComponent<IManaManager>();
        if (enebleForMagic && mm.getTotalMana() - ManaExpend >= 0) {
            mm.expendMana(ManaExpend);
            magic.castMagic(direction);
            StartCoroutine(delayMagic());
        }
    }
    private IEnumerator delayMagic() {
        enebleForMagic = false;
        yield return new WaitForSeconds(DelayMagic);
        enebleForMagic = true;
    }

    
}
