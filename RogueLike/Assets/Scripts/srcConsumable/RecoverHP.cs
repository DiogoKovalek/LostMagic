using Unity.VisualScripting;
using UnityEngine;

public class RecoverHP : MonoBehaviour, IConsumable {
    private ConsumableBase consumableBase;
    private bool exist = true;
    public int HPRecover;


    public int getIdItem() {
        return consumableBase.id;
    }

    public void init(ConsumableBase consumableBase) {
        this.consumableBase = consumableBase;
    }
    public void action() {
        GetComponentInParent<IPlayer>().AddHP(HPRecover);
        Destroy(this.gameObject);
        exist = false;
    }

    public bool isExist() {
        return exist;
    }
}
