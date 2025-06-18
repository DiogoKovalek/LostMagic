using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicTest : MagicBase {
    private Magic EMagic = Magic.Teste;
    private GameObject projectile = ProjectileBank.GetProject("Fire Ball");

    public override Magic getEMagic() {
        return EMagic;
    }

    public override void magic(GameObject staff) {
        base.magic(staff);
        MonoBehaviour.Instantiate(projectile, staff.transform.position, staff.transform.rotation);
    }
}
