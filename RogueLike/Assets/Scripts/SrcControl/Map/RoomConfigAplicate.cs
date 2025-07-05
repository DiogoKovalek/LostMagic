using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomConfigAplicate : MonoBehaviour {
    [SerializeField] GameObject doorOrWall;
    [SerializeField] GameObject enemyRoomManage;
    [SerializeField] GameObject portal;
    private byte mapSizeX; // Deve estar apenas em controler / depois alterar em doorTransition
    private byte mapSizeY;
    private List<DoorTransition> listDoorsInRoom = new List<DoorTransition>();
    private TypeRoom typeRoom;
    private TypeRoom[] roomsWhithPortals = { TypeRoom.initial, TypeRoom.portalFinal };
    private TypeRoom[] roomsWithEnemy = { TypeRoom.basic, TypeRoom.basicWithChest };
    private TypeRoom[] roomsWhithChest = { TypeRoom.basicWithChest, TypeRoom.bonusChest };

    public void ApplySettings(byte roomWidth, byte roomHeight, int level, RoomConfig roomConfig) {
        mapSizeX = roomWidth;
        mapSizeY = roomHeight;
        typeRoom = roomConfig.TypeRoom;

        #region Add Door
        String doorsInRoom = Convert.ToString(roomConfig.DirectionDoors, 2);
        if (doorsInRoom.Length < 4) {
            doorsInRoom = new String('0', 4 - doorsInRoom.Length) + doorsInRoom;
        }
        //Cima / Direita / Baixo / Esquerda 
        char[] aux = doorsInRoom.ToCharArray();
        int[,] pos = { { 0, (mapSizeX - 2) / 2, 0, -(mapSizeX - 2) / 2 }, { (mapSizeY - 2) / 2, 0, -(mapSizeY - 2) / 2, 0 } }; // X Y

        for (int i = 0; i < aux.Length; i++) {
            GameObject obj = Instantiate(doorOrWall, this.transform.position + new Vector3(pos[0, i], pos[1, i], 0), Quaternion.Euler(0, 0, i * -90));
            if (aux[i] == '1') {
                GameObject wall = getChildPerTag("Wall", obj);
                if (wall == null) continue;
                wall.gameObject.SetActive(false);
                GameObject door = getChildPerTag("Door", obj);
                DoorTransition dt = door.GetComponent<DoorTransition>();
                dt.IntiDoor(mapSizeX, mapSizeY);
                listDoorsInRoom.Add(dt);
            }
            else {
                GameObject door = getChildPerTag("Door", obj);
                if (door == null) continue;
                door.gameObject.SetActive(false);

            }
            obj.transform.SetParent(this.transform);
        }
        #endregion

        #region addPortals
        if (roomsWhithPortals.Contains(typeRoom)) {
            if (typeRoom == TypeRoom.initial) {
                GameObject por = Instantiate(portal, this.transform.position, portal.transform.rotation);
                por.transform.SetParent(this.transform);
                por.GetComponent<IPortal>().initiPortal(TypePortal.Spawn);
            }
            else if (typeRoom == TypeRoom.portalFinal) {
                GameObject por = Instantiate(portal, this.transform.position, portal.transform.rotation);
                por.transform.SetParent(this.transform);
                por.GetComponent<IPortal>().initiPortal(TypePortal.NextLevel);
            }
        }

        #endregion

        #region Add Enemy Room Manager
        if (roomsWithEnemy.Contains(typeRoom)) { // if have an enemy
            GameObject enemyRM = Instantiate(enemyRoomManage, this.transform.position, this.transform.rotation);
            EnemyRoomManage srcEnemyRM = enemyRM.GetComponent<EnemyRoomManage>();
            if (srcEnemyRM != null) {
                srcEnemyRM.transform.SetParent(this.transform);
                srcEnemyRM.initEnemyRoomManage(mapSizeX, mapSizeY, level, roomConfig, this);
            }
        }
        #endregion

        #region Add Chest
        if (roomsWhithChest.Contains(typeRoom) && typeRoom != TypeRoom.basicWithChest) {
            GameObject chest = ItemBank.CreateChest(this.transform.position);
            chest.transform.SetParent(this.transform);
        }
        #endregion
    }
    private GameObject getChildPerTag(String tag, GameObject obj) {
        foreach (Transform child in obj.transform) {
            if (child.gameObject.tag == tag) {
                return child.gameObject;
            }
        }
        return null;
    }
    public void OpenOrCloseAllDoors() {
        for (byte i = 0; i < listDoorsInRoom.Count; i++) {
            listDoorsInRoom[i].OpenOrCloseDoor();
        }
    }
    public TypeRoom GetTypeRoom() {
        return typeRoom;
    }
}
