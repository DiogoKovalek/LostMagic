using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Consumable", menuName = "Base/Item/Consumable")]
public class ConsumableBase : ItemBase {
    public ConsumableScript consumableScript;

    private void OnValidate() {
        TypeItem = TypeItem.Comsumable;
    }
}
public enum ConsumableScript {
    none,
}
