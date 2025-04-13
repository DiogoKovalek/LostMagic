using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBehavior : MonoBehaviour
{
    private Sprite btE;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.tag = transform.parent.tag;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
