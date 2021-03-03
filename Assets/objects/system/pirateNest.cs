using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pirateNest : Buildings
{
    // Start is called before the first frame update
    public GameObject thisPirate;
    public sendCon thisCondition;
    public Vector3 thisPoint;
    void Start()
    {
        this.ID = this.gameObject.name;
        this.type = "Pirate Nest";
        this.pos = this.transform.position;
        thisCondition.sent = false;
        thisCondition.recoverTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(thisCondition.sent == false && thisCondition.recoverTime == 0){
            sendThisShip();
        }
        if(thisCondition.recoverTime > 0){
            thisCondition.recoverTime -= 1;
        }
    }

    //send the ship in this device
    void sendThisShip(){
        GameObject thisOne = Instantiate(thisPirate);
        pirateShip thisShip = thisOne.GetComponent<pirateShip>();
        thisShip.transform.position = this.transform.position;
        thisShip.wandarPoint = thisPoint;
        thisShip.home = this;
        this.thisCondition.sent = true;
    }

    public void receive(pirateShip thisOne){
        if((thisOne.transform.position - this.transform.position).magnitude < 0.005f){
            Object.Destroy(thisOne.gameObject);
            //userAction.allShipsList.Remove(thisOne);
            //userAction.allTransShipsList.Remove(thisOne);
            userAction.removeFromList(thisOne);
            this.thisCondition.recoverTime = 5000;
        }
    }
}

public struct sendCon
{
    public bool sent;
    public int recoverTime;
}
