using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Grimore", menuName = "Base/Item/Grimore")]
public class GrimoreBase : ItemBase {
    public Element Element;
    public GameObject PrefGrimore;

    private void OValidate() {
        TypeItem = TypeItem.Grimore;
    }
}


