using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public static class ItemBank
{
    private static ItemBase[] listOfItens;
    private static GameObject ItemBasic;
    private static GameObject staffBasic;
    public static void IntiItemBank(){
        ItemBase[] items = Resources.LoadAll<ItemBase>("ScrObj/Items");//Carrega tudo da pasta Resources/Items
        ItemBase[] aux = new ItemBase[items.Length];
        foreach(var item in items){
            aux[item.id - 1] = item;
        }
        if(aux.Contains(null)){ // Significa que algum scriptable tem o mesmo id
            Debug.LogWarning("Items with same id");
        }
        listOfItens = aux;
        ItemBasic = Resources.Load<GameObject>("LoadPrefab/Items/ItemBasic");
        staffBasic = Resources.Load<GameObject>("LoadPrefab/Items/StaffBasic");
    }
    public static ItemBase GetItemFromId(int id){
        if(id == 0){
            Debug.LogWarning("Retornando Item de id 0, nulo");
            return null;
        }
        return listOfItens[id - 1];
    }
    public static Sprite GetSpriteFromId(int id){
        return GetItemFromId(id).Sprite;
    }
    public static T GetItemAs<T>(int id)where T : ItemBase{
        ItemBase item = GetItemFromId(id);
        if(item is T t){
            return t;
        }
        return null;
    }

    public static GameObject CreateStaffBasicById(int id){
        Debug.Log(staffBasic == null);
        if(GetItemFromId(id).TypeItem == TypeItem.Staff){
            StaffBase staffBase = GetItemAs<StaffBase>(id);
            GameObject staff = GameObject.Instantiate(staffBasic);
            staff.GetComponent<IStaff>().initStaff(staffBase);
            return staff;
        }
        
        return null;
    }
}