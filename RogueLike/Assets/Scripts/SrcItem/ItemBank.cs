using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public static class ItemBank
{
    private static ItemBase[] listOfItens;

    public static void IntiItemBank(){
        ItemBase[] items = Resources.LoadAll<ItemBase>("Items");//Carrega tudo da pasta Resources/Items
        ItemBase[] aux = new ItemBase[items.Length];
        foreach(var item in items){
            aux[item.id - 1] = item;
        }
        if(aux.Contains(null)){ // Significa que algum scriptable tem o mesmo id
            Debug.LogWarning("Items with same id");
        }
        listOfItens = aux;
    }
    public static ItemBase GetItemFromId(int id){
        return listOfItens[id - 1];
    }
    public static Sprite GetSpriteFromId(int id){
        return GetItemFromId(id).Sprite;
    }
}
