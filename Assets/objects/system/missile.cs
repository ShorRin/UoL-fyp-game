using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missile : Ships
{
    public Ships shooter;
    public Ships aimTarget;
    public float initialSpeed;
    public List<Ships> closeShips;
    private bool exploded = false;
    private float timer = 15f;
    // Start is called before the first frame update
    //public static Object missilePrefab = Resources.Load("ojbects/ships/missile");

    void Start()
    {
        maxHP = 1;
        userAction.allMslList.Add(this);
        this.ID = "msl";
        this.type = "missile";
        base.toStart();
        this.speed = initialSpeed;
        this.maxSpeed = 2.4f;
        //Debug.Log(this.transform.rotation);
        //Debug.Log(aimTarget.name);
    }

    // Update is called once per frame
    void Update()
    {
        if(aimTarget == null){
            this.target = this.transform.position + new Vector3(this.facing.x,this.facing.y,0);
        } else {
            this.target = aimTarget.transform.position;
        }

        timer -= Time.deltaTime;
        if(mslHit()||timer <= 0){
            this.currentHP = 0;
            explode();
        }

        base.toUpdate();
    }

    void explode(){
        if(exploded == false){
            foreach(Ships ship in userAction.allShipsList){
                float temp = (ship.transform.position - this.transform.position).magnitude;
                if(temp < 0.5f){
                    ship.dealDmg(100*(0.5f-temp));
                    exploded = true;
                }
            }
        }
    }

    public bool mslForU(Ships thisO){
        return aimTarget==thisO;
    }
}

public struct missileStr
{
    public Ships startPos;
    public Ships targetOb; 
}