using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow : MonoBehaviour
{
    public GameObject playerShip;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(playerShip!=null){
            Vector3 thisPos = new Vector3(playerShip.transform.position.x, playerShip.transform.position.y, -3.79f);
            this.transform.position = thisPos;
        }
    }
}
