using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyRoomManage : MonoBehaviour {
    private List<EnemyBase> listOfEnemysInRoom = new List<EnemyBase>();
    private int layerForCollider = (1 << 3) | (1 << 7) | (1 << 9) | (1 << 15); //Wall, Player, Enemy
    [SerializeField] LayerMask layerPlayer;
    private Transform transformPlayer;
    private float distMinPlayer = 5;
    private byte widthRoom;
    private byte heightRoom;

    private RoomConfigAplicate controlerRoomConfig;

    /*
    void Start()
    {
        //teste
        createListOfEnemysInRoom(11);
        instantiateEnemys();
    }
    */
    private IEnumerator logicRoom() {
        while (true) { // Verifica se o player entrou na sala
            yield return new WaitForFixedUpdate();
            Collider2D detectedPlayer = Physics2D.OverlapBox(this.transform.position, new Vector2(widthRoom, heightRoom), 0f, layerPlayer);
            if (detectedPlayer != null) {
                transformPlayer = detectedPlayer.transform;
                controlerRoomConfig.OpenOrCloseAllDoors();
                break;
            }
        }
        instantiateEnemys();
        while (transform.childCount > 0) { // espera todos os inimigos morrerem
            yield return new WaitForFixedUpdate();
        }
        transformPlayer.GetComponent<IPlayer>().RestoreAllMana();
        spawnChest();
        controlerRoomConfig.OpenOrCloseAllDoors();
    }
    public void initEnemyRoomManage(byte widthRoom, byte heightRoom, int level, RoomConfig roomConfig, RoomConfigAplicate controler) { // Depois tem que colocar as portas
        this.widthRoom = widthRoom;
        this.heightRoom = heightRoom;
        this.controlerRoomConfig = controler;

        createListOfEnemysInRoom(level, roomConfig);
        StartCoroutine(logicRoom());
    }
    private void createListOfEnemysInRoom(int level, RoomConfig roomConfig = null) {
        byte maxOfEnemys;
        byte minOfEnemys;
        if (level < 5) {
            maxOfEnemys = 3;
            minOfEnemys = 1;
        }
        else if (level < 10) {
            maxOfEnemys = 5;
            minOfEnemys = 3;
        }
        else if (level < 15) {
            maxOfEnemys = 10;
            minOfEnemys = 7;
        }
        else if (level < 20) {
            maxOfEnemys = 20;
            minOfEnemys = 13;
        }
        else {
            maxOfEnemys = 25;
            minOfEnemys = 20;
        }

        byte numOfEnemys = (byte)Random.Range(minOfEnemys, maxOfEnemys + 1);
        for (byte i = 0; i < numOfEnemys; i++) {
            // instanciar os inimigos dentro da lista
            listOfEnemysInRoom.Add(EnemyBank.GetRandomEnemyByLevel());
        }
    }

    private void instantiateEnemys() {
        int widthSpawn = widthRoom / 2 - 2;
        int heightSpawn = heightRoom / 2 - 2;

        List<Enemy> enemysForActive = new List<Enemy>();
        while (listOfEnemysInRoom.Count > 0) {
            int count = 0; // Evitar por enquanto não ter espaço na sala
            while (count < 5) {
                GameObject ene = listOfEnemysInRoom[listOfEnemysInRoom.Count - 1].EnemyPrefab;
                float eneCircleRadius = ene.GetComponent<CircleCollider2D>().radius + 0.3f;
                float posX = Random.Range(transform.position.x - (widthSpawn - eneCircleRadius), transform.position.x + (widthSpawn - eneCircleRadius));
                float posY = Random.Range(transform.position.y - (heightSpawn - eneCircleRadius), transform.position.y + (heightSpawn - eneCircleRadius));
                Vector2 posSpaw = new Vector2(posX, posY);
                if (Physics2D.OverlapCircle(posSpaw, eneCircleRadius, layerForCollider) == null && getDistForPlayer(posSpaw) > distMinPlayer) {
                    GameObject e = Instantiate(ene, posSpaw, ene.transform.rotation);
                    e.transform.SetParent(this.transform);
                    Enemy enemyScr = e.GetComponent<Enemy>();
                    enemyScr.isFreeForAction = false;
                    enemysForActive.Add(enemyScr);
                    break;
                }
                else {
                    /*
                    Debug.Log("Colidiu x >>" + posSpaw.x + " y >>" + posSpaw.y);
                    Instantiate(testePos, posSpaw, testePos.transform.rotation);
                    */
                    count++;
                }
            }
            listOfEnemysInRoom.RemoveAt(listOfEnemysInRoom.Count - 1);
        }

        StartCoroutine(ActiveEnemy(enemysForActive));
    }
    private IEnumerator ActiveEnemy(List<Enemy> enemys) {
        yield return new WaitForSeconds(0.5f);
        foreach (var e in enemys) {
            e.isFreeForAction = true;
        }
    }

    private void spawnChest() {
        if (controlerRoomConfig.GetTypeRoom() == TypeRoom.basicWithChest) {
            GameObject chest = ItemBank.CreateChest(this.transform.position);
            chest.transform.SetParent(this.transform.parent);
        }
    }
    private float getDistForPlayer(Vector2 ene) {
        float x1 = transformPlayer != null ? transformPlayer.position.x : ene.x;
        float x2 = ene.x;
        float y1 = transformPlayer != null ? transformPlayer.position.y : ene.y;
        float y2 = ene.y;

        return Mathf.Sqrt(Mathf.Pow(x1 - x2, 2) + Mathf.Pow(y1 - y2, 2));
    }
}
