using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MagicBase
{
    private Magic EMagic = Magic.Earth;
    private ProjectileBase projectile = ProjectileBank.GetProject("Fire Project");
    public override Magic getEMagic() {
        return EMagic;
    }

    public override void magic(GameObject staff) {
        base.magic(staff);
        conjure(staff, projectile);
    }
}
