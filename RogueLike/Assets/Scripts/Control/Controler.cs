using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controler : MonoBehaviour
{
    //Procedural and Rooms ========================
    [SerializeField] GameObject floor;
    private Procedural procedural = new Procedural();
    [SerializeField] GameObject[] listRooms;
    private RoomConfig[,] mapGenerated;
    private byte roomWidth = 26;
    private byte roomHeight = 14;
    private int mapSize2D = 10;
    private int lengthBranch = 5;
    //==============================================
    void Start()
    {
        createMapProcedural();
    }

    /*
    void Update()
    {
        
    }
    */
    private void createMapProcedural(){
        mapGenerated = procedural.generateMapProcedural(mapSize2D,lengthBranch,listRooms.Length);
        for(int y = 0; y<mapGenerated.GetLength(0); y++){ // Y
            for(int x = 0; x<mapGenerated.GetLength(1); x++){ // X
                if(mapGenerated[x,y] != null){
                    GameObject room = listRooms[mapGenerated[x,y].IdRoom - 1];
                    Vector3 pos = new Vector3(roomWidth*x, roomHeight*(mapSize2D - 1 - y));
                    room = Instantiate(room, pos, Quaternion.Euler(0,0,0));
                    room.transform.SetParent(floor.transform);
                    RoomConfigAplicate rca = FindObjectOfType<RoomConfigAplicate>();
                    if(rca != null){
                        rca.ApplySettings(roomWidth, roomHeight, mapGenerated[x,y]); // Futuramente colocar o Room config como parametro
                    }
                }
            }
        }
        Camera.main.transform.position = new Vector3(roomWidth*mapSize2D/2,roomHeight*(mapSize2D-2)/2,-10); // Teste
    }
}
