using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTransition : MonoBehaviour
{
    private byte widthRoom;
    private byte heightRoom;

    private float CameraMoveDuration = 0.6f; // Deve sincronizar com EnemyRoomManager
    private float deslocPlayer = 3;

    private bool isTransition = false;

    [SerializeField] GameObject doorOfClose;
    
    void OnTriggerEnter2D(Collider2D other){
        if (isTransition) return;
        if (other.gameObject.tag == "Player") {
            short[] direction = getDirectionDoor();
            Controler controler = GameObject.Find("Controler")?.GetComponent<Controler>();
            controler.UpdatePosition(direction);

            /*
            Camera.main.transform.position += new Vector3(widthRoom*direction[0],heightRoom*direction[1],-10);
            other.transform.position = transform.position + new Vector3(deslocPlayer*direction[0], deslocPlayer*direction[1], 0);
            */

            other.transform.position = transform.position + new Vector3(deslocPlayer*direction[0], deslocPlayer*direction[1], 0);

            Vector3 targetCamera = Camera.main.transform.position + new Vector3(widthRoom * direction[0], heightRoom * direction[1], -10);
            StartCoroutine(AnimateTransition(targetCamera));
        }
    }

    private IEnumerator AnimateTransition(Vector3 targetCamera) {
        isTransition = true;
        Time.timeScale = 0f;

        float elapsed = 0f;
        Vector3 startCameraPos = Camera.main.transform.position;


        while (elapsed < CameraMoveDuration) {
            float t = elapsed / CameraMoveDuration;

            Camera.main.transform.position = Vector3.Lerp(startCameraPos, targetCamera, t);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        Camera.main.transform.position = targetCamera;
        isTransition = false;

        Time.timeScale = 1f;
    }
    
    public void IntiDoor(byte whidthRoom, byte heightRoom){
        this.widthRoom = whidthRoom;
        this.heightRoom = heightRoom;
    }
    private short[] getDirectionDoor(){ // [x,y]
        Transform father = transform.parent;
        switch(father.eulerAngles.z){
            case 0: return new short[2] {0,1};
            case 90: return new short[2] {-1,0};
            case 180: return new short[2] {0,-1};
            case 270: return new short[2] {1,0};
            default: return new short[2] {0,0};
        }
    }
    public void OpenOrCloseDoor(){
        doorOfClose.SetActive(!doorOfClose.activeSelf);
    }

    
    
}
