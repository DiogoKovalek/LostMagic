using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTransition : MonoBehaviour
{
    private float deslocPlayer = 3;
    private byte widthRoom;
    private byte heightRoom;
    [SerializeField] GameObject doorOfClose;
    
    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "Player"){
            short[] direction = getDirectionDoor();
            Controler controler = GameObject.Find("Controler")?.GetComponent<Controler>();
            controler.UpdatePosition(direction);
            Camera.main.transform.position += new Vector3(widthRoom*direction[0],heightRoom*direction[1],-10);
            other.transform.position = transform.position + new Vector3(deslocPlayer*direction[0], deslocPlayer*direction[1], 0);
        }
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
