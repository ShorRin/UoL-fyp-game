using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMslLock : MonoBehaviour
{
    public playerShip thisPlayer;
    // Start is called before the first frame update
    void Start()
    {
        //this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(thisPlayer.targetMslShip != null){
            //this.gameObject.SetActive(true);
            this.transform.position = thisPlayer.targetMslShip.transform.position;
        } else {
            //this.gameObject.SetActive(false);
        }
    }
}
