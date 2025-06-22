using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicTest : MagicBase {
    private Magic EMagic = Magic.Teste;
    private ProjectileBase projectile = ProjectileBank.GetProject("Fire Project");

    public override Magic getEMagic() {
        return EMagic;
    }

    public override void magic(GameObject staff) {
        base.magic(staff);
        conjure(staff, projectile);
    }
}
