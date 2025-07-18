using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public static class ItemBank {
    private static ItemBase[] listOfItens;
    private static GameObject ItemBasic;
    private static GameObject staffBasic;
    private static GameObject chest;

    // Drop Rate =============================================
    private static Dictionary<int, float> dicStaffRandom = new Dictionary<int, float>();
    private static Dictionary<int, float> dicGrimoreRandom = new Dictionary<int, float>();
    private static Dictionary<int, float> dicEquipmentRandom = new Dictionary<int, float>();
    private static Dictionary<int, float> dicConsumableRandom = new Dictionary<int, float>();



    //Nestes a soma de todos devem dar 100
    private static float chanceToDropStaff = 20; 
    private static float chanceToDropGrimore = 20; 
    private static float chanceToDropEquipment = 30; 
    private static float chanceToDropConsumable = 30; 
    // =======================================================
    public static void IntiItemBank() {
        if (listOfItens?.Length == null) {
            ItemBase[] items = Resources.LoadAll<ItemBase>("ScrObj/Items");//Carrega tudo da pasta Resources/Items
            ItemBase[] aux = new ItemBase[items.Length];
            foreach (var item in items) {
                aux[item.id - 1] = item;
                switch (item.TypeItem) {
                    case TypeItem.Staff:
                        dicStaffRandom.Add(item.id, item.chanceToDrop);
                        break;
                    case TypeItem.Grimore:
                        dicGrimoreRandom.Add(item.id, item.chanceToDrop);
                        break;
                    case TypeItem.Equipment:
                        dicEquipmentRandom.Add(item.id, item.chanceToDrop);
                        break;
                    case TypeItem.Comsumable:
                        dicConsumableRandom.Add(item.id, item.chanceToDrop);
                        break;
                }
            }
            if (aux.Contains(null)) { // Significa que algum scriptable tem o mesmo id
                Debug.LogWarning("Items with same id");
            }
            listOfItens = aux;
            dicStaffRandom = rearrangeChanceInDict(dicStaffRandom);
            dicGrimoreRandom = rearrangeChanceInDict(dicGrimoreRandom);
            dicEquipmentRandom = rearrangeChanceInDict(dicEquipmentRandom);
            dicConsumableRandom = rearrangeChanceInDict(dicConsumableRandom);

            ItemBasic = Resources.Load<GameObject>("LoadPrefab/Items/ItemBasic");
            staffBasic = Resources.Load<GameObject>("LoadPrefab/Items/StaffBasic");
            chest = Resources.Load<GameObject>("LoadPrefab/Chest/Chest");
        }
    }
    public static ItemBase GetItemFromId(int id) {
        if (id == 0) {
            Debug.LogWarning("Retornando Item de id 0, nulo");
            return null;
        }
        return listOfItens[id - 1];
    }
    public static Sprite GetSpriteFromId(int id) {
        return GetItemFromId(id).Sprite;
    }
    public static T GetItemAs<T>(int id) where T : ItemBase {
        ItemBase item = GetItemFromId(id);
        if (item is T t) {
            return t;
        }
        return null;
    }
    public static GameObject CreateItemBasicById(int id) {
        if (id != 0 && id <= listOfItens.Length) {
            GameObject item = GameObject.Instantiate(ItemBasic);
            item.GetComponent<ItemForColect>().inserId(id);
            return item;
        }
        return null;
    }

    public static GameObject CreateStaffBasicById(int id, GameObject playerLinked) {
        if (GetItemFromId(id).TypeItem == TypeItem.Staff) {
            StaffBase staffBase = GetItemAs<StaffBase>(id);
            GameObject staff = GameObject.Instantiate(staffBasic);
            staff.GetComponent<IStaff>().initStaff(staffBase, playerLinked);
            return staff;
        }

        return null;
    }
    public static GameObject CreateGrimoreBasicById(int id) {
        if (GetItemFromId(id).TypeItem == TypeItem.Grimore) {
            GrimoreBase grimoreBase = GetItemAs<GrimoreBase>(id);
            GameObject grimore = GameObject.Instantiate(grimoreBase.PrefGrimore);
            grimore.GetComponent<IGrimore>().initGrimore(grimoreBase);
            return grimore;
        }
        return null;
    }
    public static GameObject CreateConsumableById(int id) {
        if (GetItemFromId(id).TypeItem == TypeItem.Comsumable) {
            ConsumableBase consumableBase = GetItemAs<ConsumableBase>(id);
            GameObject consumable = GameObject.Instantiate(consumableBase.PrefConsumable);
            consumable.GetComponent<IConsumable>().init(consumableBase);
            return consumable;
        }
        return null;
    }
    private static Dictionary<int, float> rearrangeChanceInDict(Dictionary<int, float> dic) {
        Dictionary<int, float> auxDic = new Dictionary<int, float>();
        float sumChance = 0;
        foreach (var d in dic) {
            sumChance += d.Value;
        }
        foreach (var item in dic) {
            auxDic.Add(item.Key, (item.Value * 100) / sumChance);
        }
        auxDic = relocateDict(auxDic);
        return auxDic;
    }
    private static Dictionary<int, float> relocateDict(Dictionary<int, float> dic) {
        Dictionary<int, float> auxDic = new Dictionary<int, float>();
        Dictionary<int, float> dicParts = dic;
        while (dicParts.Count > 0) {
            int chose = Random.Range(0, dicParts.Count);
            var i = dicParts.ElementAt(chose);
            auxDic.Add(i.Key, i.Value);
            dicParts.Remove(i.Key);
        }
        return auxDic;
    }
    public static void relocateAllDict() {
        dicStaffRandom = relocateDict(dicStaffRandom);
        dicGrimoreRandom = relocateDict(dicGrimoreRandom);
        dicEquipmentRandom = relocateDict(dicEquipmentRandom);
        dicConsumableRandom = relocateDict(dicConsumableRandom);
    }
    private static int sortItem() {
        int id = 0;
        Dictionary<int, float> dic = new Dictionary<int, float>();
        float type = Random.Range(0.0f, 100.0f);
        float count = chanceToDropStaff;
        dic = (type <= count) ? dicStaffRandom :
        (type <= count + chanceToDropGrimore) ? dicGrimoreRandom :
        (type <= count + chanceToDropGrimore + chanceToDropEquipment) ? dicEquipmentRandom :
        (type <= count + chanceToDropGrimore + chanceToDropEquipment + chanceToDropConsumable) ? dicConsumableRandom : null;

        if (dic.Count != 0) {
            float numberSorted = Random.Range(0.0f, 100.0f);
            count = 0;
            foreach (var item in dic) {
                count += item.Value;
                if (numberSorted <= count) {
                    id = item.Key;
                    break;
                }
            }
        }
        return id;
    }

    public static GameObject CreateChest(Vector2 position) {
        GameObject aux = GameObject.Instantiate(chest, position, chest.transform.rotation);
        aux?.GetComponent<IChest>().AddChest(sortItem());
        return aux;
    }

}