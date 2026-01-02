using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controler : MonoBehaviour {
    public GameObject player;

    public Transform ProjectsAba;
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
    private int level = 6;
    private int[] posPlayer = new int[2];
    //==============================================
    //Load Varibles=================================
    private IPortal scrPortalSpawn;
    private bool ableForSpawnPlayer = true;
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
    public delegate void TradeScreen(UIType uiType);
    public event TradeScreen TradedScreen;
    public delegate float GetTimeForInitLoad();
    public event GetTimeForInitLoad GotTimeForInitLoad;
    public delegate void AbleForLightenScreen();
    public event AbleForLightenScreen AbledForLightenScreen;
    //==============================================

    // Audio =======================================
    public AudioControlerGame audioControl;
    //==============================================

    void Awake() {
        ItemBank.IntiItemBank(); // FUTURAMENTE COLOCAR PARA DESATIVAR O ITEM BANK
        /*
        MagicBank.InitMagics();
        */
        EnemyBank.InitEnemyBank();
        setDificult();
    }
    void Start() {
        EventStartPlayer();
        if (isGeneretedProceduralInStart) {
            initiLevel();
        }
    }
    #region Trade Level
    public void OnNextLevel() {
        audioControl.Stop();
        TradedScreen(UIType.Loading);
        StartCoroutine(initiAtributes(GotTimeForInitLoad()));
    }
    private IEnumerator initiAtributes(float seconds) {
        yield return new WaitForSeconds(seconds);

        //Update Varibles
        level += 1;
        scrPortalSpawn = null;

        //Clean Level
        foreach (Transform child in floor.transform) {
            Destroy(child.gameObject);
        }
        setDificult();
        initiLevel();
    }
    private void initiLevel() {
        createMapProcedural();

        Camera.main.transform.position = new Vector3(roomWidth * mapSize2D / 2, roomHeight * (mapSize2D - 2) / 2, -10);

        SentMapForUI(mapGenerated);
        posPlayer[0] = mapSize2D / 2;
        posPlayer[1] = mapSize2D / 2;
        TradedRoomUI(posPlayer[0], posPlayer[1]);

        // Finalizado
        AbledForLightenScreen();
        StartCoroutine(StayForInitSpawnPlayer());

        audioControl.Play();
    }
    private IEnumerator StayForInitSpawnPlayer() {
        while (!ableForSpawnPlayer) {
            yield return new WaitForEndOfFrame();
        }
        TradedScreen(UIType.None);
        scrPortalSpawn.initSpawnPlayer();
        ableForSpawnPlayer = false;
    }
    public void OnAbleForSpawnPlayer() {
        ableForSpawnPlayer = true;
    }
    #endregion
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
                        rca.ApplySettings(roomWidth, roomHeight, levelEnemy, mapGenerated[x, y]);
                    }
                    if (scrPortalSpawn == null && mapGenerated[x, y].TypeRoom == TypeRoom.initial) {
                        scrPortalSpawn = room.GetComponentInChildren<IPortal>();
                    }
                }
            }
        }
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
        ControlerDificultEnemy();
    }

    private void ControlerDificultEnemy() {
        EnemyBank.InitListEnemysByLevel(levelEnemy);
    }

    public void EventStartPlayer() {
        if (!eventsDeclarate) {
            //Teste ===============
            UIManager ui = FindAnyObjectByType<UIManager>();
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

    public Transform GetAbaProjects() {
        return ProjectsAba;
    }
}
