using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicTest : MagicBase {
    private Magic EMagic = Magic.Teste;
    public GameObject projectile;
    public override Magic getEMagic() {
        return EMagic;
    }

    public override void magic(GameObject staff) {
        base.magic(staff);
        Debug.Log("Ataque");
    }
}
