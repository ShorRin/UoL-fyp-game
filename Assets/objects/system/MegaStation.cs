using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegaStation : Buildings
{
    private bool defenderSent = false;
    public float defendRange = 20f;
    public defender thisDefenderShip;
    private Dictionary<string, int> timeCounter = new Dictionary<string, int>();
    // Start is called before the first frame update
    public ArrayList enemyList = new ArrayList();
    void Start()
    {
        this.ID = this.gameObject.name;
        this.type = "Mega Station";
        this.pos = this.transform.position;
        List<string> stationsName = new List<string>{"MS1", "MS2", "t2MS1", "t2MS2", "t4MS1", "t4MS2"};
        foreach(string one in stationsName){
            if(one != this.name){
                timeCounter[one] = 0;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        autoSend();
        List<string> keys = new List<string> (timeCounter.Keys);
        foreach(string key in keys){
            if(timeCounter[key] != 0){
                timeCounter[key] -= 1;
            }
        }
        //Debug.Log(userAction.allPirShipsList.Count);
        updateAround();
        defenderSending();

        /* 
        foreach(KeyValuePair<string, int> thisOne in timeCounter){
            if(thisOne.Value != 0){
                timeCounter[thisOne.Key] -= 1;
            }
        }
        */
        
        /* 
        if(sent == false && this.ID == "MS1"){
            sendTransShip();
            sent = true;
        }
        if(sent == false && this.ID == "MS2"){
            userAction.transTList.Add(ScheduleList.schedueList["MS2toMS1"]);
            sent = true;
        }
        */
    }

    void updateAround(){
        enemyList.Clear();
        foreach(Ships s in userAction.allPirShipsList){
            if(s.distanceWith(this.gameObject)<defendRange){
                enemyList.Add(s);
                //Debug.Log("find");
            }
        }
    }

    void autoSend(){
        foreach(KeyValuePair<string, Schedule> thisS in ScheduleList.schedueList){
            if(thisS.Value.start.ID == this.ID){
                if(timeCounter[thisS.Value.end.ID] == 0){
                    userAction.transTList.Add(ScheduleList.schedueList[thisS.Key]);
                    timeCounter[thisS.Value.end.ID] = 2000;
                }
            }
        }
    }

    public void receive(Ships thisOne){
        if((thisOne.transform.position - this.transform.position).magnitude < 0.005f){
            Object.Destroy(thisOne.gameObject);
            //userAction.allShipsList.Remove(thisOne);
            //userAction.allTransShipsList.Remove(thisOne);
            userAction.removeFromList(thisOne);
            if(thisOne.type == "defender"){
                defenderSent = false;
            }
        }
    }

    void defenderSending(){
        if(enemyList.Count!=0){
            foreach(Ships thisS in enemyList){
                userAction.registerEnemy(thisS);
            }
            if(defenderSent == false){
                defender thisD = Instantiate(thisDefenderShip);
                thisD.transform.position = this.transform.position;
                thisD.baseStation = this;
                defenderSent = true;
            }
        }
    }
}
