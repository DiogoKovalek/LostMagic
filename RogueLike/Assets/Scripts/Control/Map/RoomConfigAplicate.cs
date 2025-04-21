using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomConfigAplicate : MonoBehaviour
{
    [SerializeField] GameObject doorOrWall;
    
    private byte mapSizeX; // Deve estar apenas em controler / depois alterar em doorTransition
    private byte mapSizeY;

    public void ApplySettings(byte roomWidth, byte roomHeight, RoomConfig roomConfig){
        mapSizeX = roomWidth;
        mapSizeY = roomHeight;
        String doorsInRoom = Convert.ToString(roomConfig.DirectionDoors,2);
        if(doorsInRoom.Length < 4){
            doorsInRoom = new String('0', 4 - doorsInRoom.Length) + doorsInRoom;
        }
        //Cima / Direita / Baixo / Esquerda
        char[] aux = doorsInRoom.ToCharArray();
        int[,] pos = {{0, (mapSizeX - 2)/2, 0, -(mapSizeX - 2)/2}, {(mapSizeY - 2)/2, 0, -(mapSizeY - 2)/2, 0}}; // X Y
        for(int i = 0; i < aux.Length; i++){
            GameObject obj = Instantiate(doorOrWall, this.transform.position + new Vector3(pos[0,i], pos[1,i],0), Quaternion.Euler(0,0,i*-90));

            if(aux[i] == '1'){
                GameObject wall = getChildPerTag("Wall", obj);
                if(wall == null) continue;
                wall.gameObject.SetActive(false);
            }else{
                GameObject door = getChildPerTag("Door", obj);
                if(door == null) continue;
                door.gameObject.SetActive(false);
            }
            obj.transform.SetParent(this.transform);
        }
    }
    private GameObject getChildPerTag(String tag, GameObject obj){
        foreach(Transform child in obj.transform){
            if(child.gameObject.tag == tag){
                return child.gameObject;
            }
        }
        return null;
    }
}
