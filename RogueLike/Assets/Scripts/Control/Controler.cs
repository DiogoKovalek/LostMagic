using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controler : MonoBehaviour
{
    //Procedural ========================
    private Procedural procedural = new Procedural();
    [SerializeField] GameObject[] listRooms;
    private RoomConfig[,] mapGenerated;
    private int roomWidth = 18;
    private int roomHeight = 10;
    private int mapWidthInRoom = 4;
    private int mapHeghtInRoom = 4;
    private int lengthBranch = 3;
    //Teste
    public GameObject WallTest;
    public GameObject DoorTest;
    //===================================
    void Start()
    {
        createMapProcedural();
        testeMapGenerated();
    }

    
    void Update()
    {
        
    }

    private void createMapProcedural(){
        mapGenerated = procedural.generateMapProcedural(mapWidthInRoom,mapHeghtInRoom,lengthBranch,listRooms.Length);
        
        for(int i = 0; i<mapGenerated.GetLength(0); i++){
            for(int j = 0; j<mapGenerated.GetLength(1); j++){
                if(mapGenerated[i,j] != null){
                    GameObject room = listRooms[mapGenerated[i,j].IdRoom - 1];
                    Vector3 pos = new Vector3(roomWidth*j, roomHeight*(mapHeghtInRoom - 1 - i));
                    Instantiate(room, pos, Quaternion.Euler(0,0,0));
                    
                    // Create Doors -> Refaser codigo
                    String doorsInRoom = Convert.ToString(mapGenerated[i,j].DirectionDoors, 2);
                    if(doorsInRoom.Length < 4){
                        doorsInRoom = new String('0', 4 - doorsInRoom.Length) + doorsInRoom;
                    }
                    
                    Char[] aux = doorsInRoom.ToCharArray();
                    if(aux[0] == '1'){ // Cima
                        Instantiate(DoorTest, pos + new Vector3(0,roomHeight/2 - 1,0), Quaternion.Euler(0,0,0));
                    }else{
                        Instantiate(WallTest, pos + new Vector3(0,roomHeight/2 - 1,0), Quaternion.Euler(0,0,0));
                    }
                    if(aux[1] == '1'){ // Direita
                        Instantiate(DoorTest, pos + new Vector3(roomWidth/2 - 1,0,0), Quaternion.Euler(0,0,0));
                    }else{
                        Instantiate(WallTest, pos + new Vector3(roomWidth/2 - 1,0,0), Quaternion.Euler(0,0,0));
                    }
                    if(aux[2] == '1'){ // Baixo
                        Instantiate(DoorTest, pos + new Vector3(0,-roomHeight/2 + 1,0), Quaternion.Euler(0,0,0));
                    }else{
                        Instantiate(WallTest, pos + new Vector3(0,-roomHeight/2 + 1,0), Quaternion.Euler(0,0,0));
                    }
                    if(aux[3] == '1'){ // Esquerda
                        Instantiate(DoorTest, pos + new Vector3(-roomWidth/2 + 1,0,0), Quaternion.Euler(0,0,0));
                    }else{
                        Instantiate(WallTest, pos + new Vector3(-roomWidth/2 + 1,0,0), Quaternion.Euler(0,0,0));
                    }
                }
            }
        }
    }

    private void testeMapGenerated(){// codigo para ser excluido futuramente
        String mapTest = "";
        String mapBit = "";
        for(int i = 0; i < mapGenerated.GetLength(0); i++){
            mapTest += "[";
            mapBit += "[";
            for(int j = 0; j < mapGenerated.GetLength(1); j++){
                if(mapGenerated[i,j] == null){
                    mapTest += 0;
                    mapBit += "0000";
                }else{
                    mapTest += mapGenerated[i,j].IdRoom;
                    String bits = Convert.ToString(mapGenerated[i,j].DirectionDoors, 2);
                    if(bits.Length < 4){
                        bits = new String('0', 4 - bits.Length) + bits;
                    }
                    mapBit += bits;
                }
                if(j+1 != mapGenerated.GetLength(1)){
                    mapTest +=",";
                    mapBit +=",";
                }
            }
            mapTest += "]";
            mapBit += "]";
        }
        Debug.Log(mapTest);
        Debug.Log(mapBit);
    }
}
