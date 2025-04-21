using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTransition : MonoBehaviour
{
    private float deslocPlayer = 3;
    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "Player"){
            short[] direction = getDirectionDoor();
            Camera.main.transform.position += new Vector3(26*direction[0],14*direction[1],-10);
            other.transform.position = transform.position + new Vector3(deslocPlayer*direction[0], deslocPlayer*direction[1], 0);
        }
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
    
}
