using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyRoomManage : MonoBehaviour {
    private List<EnemyBase> listOfEnemysInRoom = new List<EnemyBase>();
    //private int layerForCollider = 1 << 9 ; //Wall, Player, Enemy
    [SerializeField] LayerMask layerPlayer;
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
                controlerRoomConfig.OpenOrCloseAllDoors();
                break;
            }
        }
        instantiateEnemys();
        while (transform.childCount > 0) { // espera todos os inimigos morrerem
            yield return new WaitForFixedUpdate();
        }
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
        if (level < 5) {
            maxOfEnemys = 3;
        }
        else if (level < 10) {
            maxOfEnemys = 5;
        }
        else {
            maxOfEnemys = 10;
        }

        byte numOfEnemys = (byte)Random.Range(1, maxOfEnemys + 1);
        for (byte i = 0; i < numOfEnemys; i++) {
            // instanciar os inimigos dentro da lista
            listOfEnemysInRoom.Add(EnemyBank.CreateRandomEnemy());
        }
    }

    private void instantiateEnemys() {
        int widthSpawn = widthRoom / 2 - 2;
        int heightSpawn = heightRoom / 2 - 2;
        while (listOfEnemysInRoom.Count > 0) {
            int count = 0; // Evitar por enquanto não ter espaço na sala
            while (count < 5) {
                EnemyBase ene = listOfEnemysInRoom[listOfEnemysInRoom.Count - 1];
                float eneCircleRadius = ene.raiHitBox;
                float posX = Random.Range(transform.position.x - (widthSpawn - eneCircleRadius), transform.position.x + (widthSpawn - eneCircleRadius));
                float posY = Random.Range(transform.position.y - (heightSpawn - eneCircleRadius), transform.position.y + (heightSpawn - eneCircleRadius));
                Vector2 posSpaw = new Vector2(posX, posY);
                if (Physics2D.OverlapCircle(posSpaw, eneCircleRadius - 0.05f) == null) {
                    GameObject e = EnemyBank.InstantiateEnemyByBase(ene);
                    e.transform.position = posSpaw;
                    e.transform.SetParent(this.transform);
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
    }
}
