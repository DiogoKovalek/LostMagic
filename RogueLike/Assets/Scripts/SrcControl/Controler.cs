using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controler : MonoBehaviour {
    public GameObject player;
    //Procedural and Rooms ========================
    public bool isGeneretedProceduralInStart = true;
    [SerializeField] GameObject floor;
    private Procedural procedural = new Procedural();
    [SerializeField] GameObject[] listRooms;
    private RoomConfig[,] mapGenerated;
    private byte roomWidth = 26;
    private byte roomHeight = 14;
    private int mapSize2D = 10; // Não pode ser mudado a qualquer momento, tem que mudar na UIGameScreen
    private int lengthBranch;
    private int levelEnemy;
    private int level = 1;
    private int[] posPlayer = new int[2];
    //==============================================

    // EventManager ================================
    [SerializeField] EventManager eventManager;
    private bool eventsDeclarate = false;
    //==============================================
    //Events =======================================
    public delegate void SendMapForUI(RoomConfig[,] map);
    public event SendMapForUI SentMapForUI;
    public delegate void TradeRoomUI(int x, int y);
    public event TradeRoomUI TradedRoomUI;
    //==============================================

    void Awake() {
        ItemBank.IntiItemBank();
        MagicBank.InitMagics();
        EnemyBank.InitEnemyBank();
        setDificult();
    }
    void Start() {
        EventStartPlayer();
        if (isGeneretedProceduralInStart) {
            initiLevel();
        }
    }

    private void initiLevel() {
        createMapProcedural();
        SentMapForUI(mapGenerated);
        posPlayer[0] = mapSize2D / 2;
        posPlayer[1] = mapSize2D / 2;
        TradedRoomUI(posPlayer[0], posPlayer[1]);
    }
    private void createMapProcedural() {
        mapGenerated = procedural.generateMapProcedural(mapSize2D, lengthBranch, listRooms.Length);
        for (int y = 0; y < mapGenerated.GetLength(0); y++) { // Y
            for (int x = 0; x < mapGenerated.GetLength(1); x++) { // X
                if (mapGenerated[x, y] != null) {
                    GameObject room = listRooms[mapGenerated[x, y].IdRoom];
                    Vector3 pos = new Vector3(roomWidth * x, roomHeight * (mapSize2D - 1 - y));
                    room = Instantiate(room, pos, Quaternion.Euler(0, 0, 0));
                    room.transform.SetParent(floor.transform);
                    RoomConfigAplicate rca = room.GetComponent<RoomConfigAplicate>();
                    if (rca != null) {
                        rca.ApplySettings(roomWidth, roomHeight, level, mapGenerated[x, y]);
                    }
                }
            }
        }
        Camera.main.transform.position = new Vector3(roomWidth * mapSize2D / 2, roomHeight * (mapSize2D - 2) / 2, -10); // Teste
    }

    public void OnNextLevel() {
        level += 1;
        foreach (Transform child in floor.transform) {
            Destroy(child.gameObject);
        }
        setDificult();
        initiLevel();
    }
    private void setDificult() {
        switch (level) {
            case 1:
                lengthBranch = 2;
                levelEnemy = 1;
                break;
            case 2:
                lengthBranch = 2;
                levelEnemy = 2;
                break;
            case 3:
                lengthBranch = 3;
                levelEnemy = 3;
                break;
            case 4:
                lengthBranch = 4;
                levelEnemy = 4;
                break;
            case 5:
                lengthBranch = 4;
                levelEnemy = 5;
                break;
            case 6:
                lengthBranch = 5;
                levelEnemy = 6;
                break;
            case 7:
                lengthBranch = 6;
                levelEnemy = 7;
                break;
            case 8:
                lengthBranch = 7;
                levelEnemy = 8;
                break;
            case 9:
                lengthBranch = 8;
                levelEnemy = 9;
                break;
            case 10:
                lengthBranch = 9;
                levelEnemy = 10;
                break;
            default:
                lengthBranch = 10;
                levelEnemy = 11;
                break;
        }
    }

    public void EventStartPlayer() {
        if (!eventsDeclarate) {
            //Teste ===============
            UIManager ui = FindObjectOfType<UIManager>();
            Player playerScr = player.GetComponent<Player>();
            //=====================
            eventManager.addPlayer(playerScr);
            eventManager.addUIManager(ui);
            eventManager.addControler(this);
            eventManager.startConectionPlayerWithUI();
            eventManager.startConectionPlayerWithControler();
            eventManager.startConectionCotrolerWithUI();
            eventsDeclarate = true;
        }
    }

    public void UpdatePosition(short[] direction) {
        posPlayer[0] += direction[0];
        posPlayer[1] -= direction[1];
        TradedRoomUI(posPlayer[0], posPlayer[1]);
    }
}
