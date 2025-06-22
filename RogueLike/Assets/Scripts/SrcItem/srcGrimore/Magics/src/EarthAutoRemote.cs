using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthAutoRemote : MagicBase
{
    private Magic EMagic = Magic.Earth;
    private ProjectileBase projectile = ProjectileBank.GetProject("Earth Project");
    public override Magic getEMagic() {
        return EMagic;
    }

    public override void magic(GameObject staff) {
        base.magic(staff);
    }
}
