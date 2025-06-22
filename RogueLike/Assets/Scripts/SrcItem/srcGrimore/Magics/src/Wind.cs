using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MagicBase
{
    private Magic EMagic = Magic.Earth;
    private ProjectileBase projectile = ProjectileBank.GetProject("Wind Project");
    public override Magic getEMagic() {
        return EMagic;
    }

    public override void magic(GameObject staff) {
        base.magic(staff);
        conjure(staff, projectile);
    }
}
