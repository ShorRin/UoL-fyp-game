using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pirateShip : Ships
{
    public Vector3 wandarPoint;
    public pirateNest home;
    private Ships enemyTarget = null;
    bool mslShot = false;

    List<Ships> transAround = new List<Ships>();
    List<Ships> enemyAround = new List<Ships>();
    public string status = "default";
    // Start is called before the first frame update
    void Start()
    {
        this.maxHP = 130;
        this.type = "pirate";
        userAction.addToList(this);
        base.toStart();
        this.maxSpeed = 1f;
        this.ID = "pir";
        this.target = this.wandarPoint;
        this.radarRange = 8f;
        this.turningValue = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        base.toUpdate();
        updateAround();
        base.autoFire();

        if(status == "default"){
            this.target = this.wandarPoint;
            if(currentHP < 70){
                status = "gohome";
                return;
            }
            if(shipsAround.Count!=0||mslAround.Count!=0){
                status = "fight";
                enemyTarget = shipsAround[0];
                return;
            }
            if(arrived()){
                status = "gohome";
                return;
            }
        }
        if(status == "fight"){
            //shot everything in range
            /* 
            gunStatus = false;
            foreach(Ships thisS in shipsAround){
                if(thisS.type=="trans"||thisS.type=="defender"||thisS.type=="player"){
                    if(thisArea.withinThis(thisS.gameObject)){
                        gunStatus = true;
                        break;
                    }
                }
            }
            foreach(Ships thisM in mslAround){
                if(thisArea.withinThis(thisM.gameObject)&&thisM.GetComponent<missile>().shooter!=this){
                    gunStatus = true;
                    break;
                }
            }
            */

            if(destroyNum > 3&&enemyAround.Count == 0){
                status = "gohome";
                return;
            }
            if(currentHP < 60&&enemyAround.Count == 0){
                status = "gohome";
                return;
            }
            if(transAround.Count!=0){
                int temp = 0;
                float def = 9999;
                for(int i=0;i<transAround.Count;i++){
                    if((this.transform.position-transAround[i].transform.position).magnitude<def){
                        def = (this.transform.position-transAround[i].transform.position).magnitude;
                        temp = i;
                    }
                }
                enemyTarget = transAround[temp];
            } else {
                if(enemyAround.Count!=0){
                    this.target = idealPos(wandarPoint);
                } else {
                    status = "default";
                    return;
                }
            }
            /* 
            if(enemyTarget == null){

                enemyTarget = transAround[0];
            }
            */
            
            /* 
            if(thisArea.withinThis(enemyTarget.gameObject)){
                gunStatus = true;
            } else {
                gunStatus = false;
            }
            */
            if(enemyAround.Count!=0){
                //Debug.Log("herefind");
                this.targetMslShip = enemyAround[0];
                if(mslShot==false){
                    shootMissile();
                    mslShot = true;
                }
            }
            this.target = idealPos(wandarPoint);
            aviodMsl();
        }
        if(status == "gohome"){
            this.target = home.transform.position;
            if(this.closeTo(home.gameObject, 0.005f)){
                home.receive(this);
            }
        }
    }

    void aviodMsl(){
        if(mslAround.Count!=0){
            this.target = mslAround[0].transform.position;
        }
    }

    Vector3 idealPos(Vector3 idealPoint){
        Vector3 temp = new Vector3();
        if(enemyAround.Count == 1){
            temp = enemyAround[0].transform.position;
        }
        if(enemyAround.Count > 1){
            float lowest = 99;
            Ships lowestOne = null;
            float secondLowest = 99;
            Ships secondLowestOne = null;
            foreach(Ships q in enemyAround){
                float ds = this.distanceWith(q.gameObject);
                if(ds<lowest){
                    secondLowest = lowest;
                    secondLowestOne = lowestOne;
                    lowest = ds;
                    lowestOne = q;
                }
                if(ds>=lowest&&ds<secondLowest){
                    secondLowest = ds;
                    secondLowestOne = q;
                }
            }
            float vari = secondLowest - lowest;
            if(vari > 4){
                //one of the enemy ships is too close than others so the pirate decide to destory it frist
                temp = lowestOne.transform.position;
            } else {
                //get close to target while try to keep away from enemies
                if(enemyTarget!=null){
                    temp = enemyTarget.transform.position;
                } else {
                    temp = idealPoint;
                }
                foreach(Ships q in enemyAround){
                    temp = temp + directionTo(q.gameObject);
                }
            }
        }
        if(enemyAround.Count == 0){
            temp = enemyTarget.transform.position;
        }
        return temp;
    }

    Vector3 directionTo(GameObject s){
        //more distant an enemy is, less effect will it take
        return (this.transform.position - s.transform.position).normalized*(10/(this.transform.position - s.transform.position).magnitude);
    }

    void updateAround(){
        transAround.Clear();
        enemyAround.Clear();
        foreach(Ships thisS in this.shipsAround){
            if(thisS.type == "trans"){
                transAround.Add(thisS);
            } else {
                if(thisS.type != "pirate"){
                    enemyAround.Add(thisS);
                }
            }
        }
    }
}
