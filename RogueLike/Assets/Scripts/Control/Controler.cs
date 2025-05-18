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
    private int level = 1;
    //==============================================

    // EventManager ================================
    [SerializeField] EventManager eventManager;
    //==============================================

    //Teste ========================================
    [SerializeField] GameObject wand;
    //==============================================
    void Start()
    {
        createMapProcedural();
        Vector2 posWand = new Vector2(mapSize2D/2*roomWidth, (mapSize2D/2-1)*roomHeight);
        Instantiate(wand, posWand, wand.transform.rotation);
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
                    GameObject room = listRooms[mapGenerated[x,y].IdRoom];
                    Vector3 pos = new Vector3(roomWidth*x, roomHeight*(mapSize2D - 1 - y));
                    room = Instantiate(room, pos, Quaternion.Euler(0,0,0));
                    room.transform.SetParent(floor.transform);
                    RoomConfigAplicate rca = room.GetComponent<RoomConfigAplicate>();
                    if(rca != null){
                        rca.ApplySettings(roomWidth, roomHeight, level, mapGenerated[x,y]);
                    }
                }
            }
        }
        Camera.main.transform.position = new Vector3(roomWidth*mapSize2D/2,roomHeight*(mapSize2D-2)/2,-10); // Teste
    }

    public void EventStartPlayer(){
        //Teste ===============
        UIManager ui = FindObjectOfType<UIManager>();
        Player player = FindObjectOfType<Player>();
        //=====================
        eventManager.startConectionPlayerWithUI(player,ui);
    }
}
