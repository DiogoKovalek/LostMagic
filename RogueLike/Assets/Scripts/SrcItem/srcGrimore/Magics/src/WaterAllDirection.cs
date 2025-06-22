using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterAllDirection : MagicBase
{
    private Magic EMagic = Magic.Earth;
    private ProjectileBase projectile = ProjectileBank.GetProject("Water Project");
    public override Magic getEMagic() {
        return EMagic;
    }

    public override void magic(GameObject staff) {
        base.magic(staff);
    }
}
