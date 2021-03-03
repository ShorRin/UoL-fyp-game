using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class defender : Ships
{
    // Start is called before the first frame update
    private string status = "defend";
    public Ships targetOne;
    public MegaStation baseStation;
    public float disToTarget;
    public ArrayList friendAround = new ArrayList();
    public ArrayList enemyAround = new ArrayList();
    public float stayCounter = 0;
    private bool mslShot = false;
    void Start()
    {
        maxHP = 100;
        this.type = "defender";
        userAction.addToList(this);
        base.toStart();
        this.maxSpeed = 1.1f;
        this.ID = "def";
        this.radarRange = 6f;
    }

    // Update is called once per frame
    void Update()
    {
        base.toUpdate();
        base.autoFire();
        updateFriends();
        updateEnemy();

        if(targetOne!=null){
            disToTarget = (targetOne.transform.position-this.transform.position).magnitude;
        }

        if(status == "defend"){
            if((baseStation.enemyList.Count==0||this.distanceWith(baseStation.gameObject)>19f)&&this.enemyAround.Count==0){
                //if(!this.shipsAround.Contains(targetOne)){
                    stayCounter -= Time.deltaTime;
                    if(stayCounter <= 0){
                        status = "gohome";
                        return;
                    }
                //} 
            } else {
                stayCounter = 1.5f;
                if(mslShot==false){
                    targetMslShip = (Ships)baseStation.enemyList[0];
                    shootMissile();
                    mslShot = true;
                }
                targetOne = closerOne(baseStation.enemyList, this);
                if(targetOne == null){
                    //targetOne = closerOne(enemyAround, this);
                    status = "gohome";
                    return;
                }
                if(friendAround.Count==0){
                    if(this.distanceWith(targetOne.gameObject)>=3){
                        this.target = targetOne.transform.position;
                    } 
                    if(this.distanceWith(targetOne.gameObject)<3){
                        this.target = this.transform.position+
                        (this.transform.position-targetOne.transform.position)*1+
                        (baseStation.transform.position-this.transform.position).normalized;
                    }
                } else {
                    Vector3 tempV = new Vector3();
                    Vector3 tempFS = new Vector3();
                    float l = 999;
                    float q = 0;
                    foreach(Ships thisS in friendAround){
                        //find a closest friend
                        q = thisS.distanceWith(this.gameObject);
                        if(q < l){
                            tempFS = thisS.transform.position;
                            l = q;
                        }
                        //if(thisS.GetComponent<defender>().targetOne == this.targetOne){

                        //}
                        //has a friendly ship closer to enemy, its safe to approach enemy
                        if(thisS.distanceWith(targetOne.gameObject)<=disToTarget){
                            tempV = targetOne.transform.position-this.transform.position;
                            break;
                        } 
                    }
                    //no friendly ship closer to enemy, should go back a bit to keep together with friend
                    if(tempV == Vector3.zero){
                        if(disToTarget>=5){
                            tempV = targetOne.transform.position-this.transform.position;
                        } else {
                            tempV = this.transform.position-targetOne.transform.position;
                        }
                    }
                    if(this.disToTarget>this.gunRange){
                        tempV += (this.transform.position - tempFS).normalized*1;
                    }
                    this.target = this.transform.position + tempV;
                }
                /* 
                if(thisArea.withinThis(targetOne.gameObject)){
                    gunStatus = true;
                } else {
                    gunStatus = false;
                }
                */
            }
        }
        if(status == "gohome"){
            if(baseStation.enemyList.Count!=0&&mslShot==false){
                status = "defend";
                return;
            }
            this.target = baseStation.transform.position;
            if(arrived()){
                baseStation.receive(this);
            }
        } 
    }

    void updateEnemy(){
        enemyAround.Clear();
        foreach(Ships thisS in this.shipsAround){
            if(thisS.type == "pirate"){
                enemyAround.Add(thisS);
            }
        }
    }

    void updateFriends(){
        friendAround.Clear();
        foreach(Ships thisS in shipsAround){
            if(thisS.type=="defender"||thisS.type=="player"){
                friendAround.Add(thisS);
            }
        }
    }

    Ships closerOne(ArrayList k, Ships m){
        float length = 999;
        Ships chosen = null;
        foreach(Ships l in k){
            float temp = (l.transform.position-m.transform.position).magnitude;
            if(temp < length){
                length = temp;
                chosen = l;
            }
        }
        return chosen;
    }
}
