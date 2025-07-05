using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Chest : MonoBehaviour, IChest {

    private int idItem;
    public void AddChest(int idItem) {
        this.idItem = idItem;
    }

    public void OpenChest() {
        GameObject item = ItemBank.CreateItemBasicById(idItem);
        item.transform.position = this.transform.position;
        item.transform.SetParent(this.transform.parent);
        Destroy(this.gameObject);
    }
}

public interface IChest {
    void AddChest(int idItem);
    void OpenChest();
}
