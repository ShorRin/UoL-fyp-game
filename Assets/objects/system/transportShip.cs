using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class transportShip : Ships
{
    // Start is called before the first frame update
    List<Vector3> schedule;
    List<Ships> enemyAround = new List<Ships>();
    public Schedule thisSchedule;
    public string status = "default";
    public int avoidCounter = 0;
    public Vector3 recordTarget;
    void Start()
    {
        maxHP = 80;
        this.type = "trans";
        userAction.addToList(this);
        //userAction.allShipsList.Add(this);
        //userAction.allTransShipsList.Add(this);
        base.toStart();
        this.radarRange = 3f;
        this.maxSpeed = 0.9f;
        this.ID = "ts";
        schedule = new List<Vector3>(thisSchedule.midway);
        this.target = thisSchedule.end.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        base.toUpdate();
        updateAround();

        if (status == "default")
        {
            if (enemyAround.Count != 0)
            {
                status = "avoid";
                avoidCounter = 300;
                recordTarget = this.target;
                return;
            }
            if (closeToArrived())
            {
                //Debug.Log(schedule.Count);
                //if(schedule.Count != 0){
                //    this.target = schedule[0];
                //    schedule.RemoveAt(0);
                //} else {
                thisSchedule.end.receive(this);
                //}
            }
        }



        if (status == "avoid")
        {
            if (enemyAround.Count == 0 && avoidCounter == 0)
            {
                status = "default";
                this.target = recordTarget;
                return;
            }
            idealPos();
            if (enemyAround.Count == 0)
            {
                avoidCounter -= 1;
            }
            else
            {
                avoidCounter = 300;
            }
        }
    }

    void idealPos()
    {
        Vector3 temp = new Vector3(0, 0, 0);
        if (enemyAround.Count != 0)
        {
            //Debug.Log("reachhere");
            foreach (Ships thisP in enemyAround)
            {
                temp = temp + this.transform.position - thisP.transform.position;
                //Debug.Log(this.transform.position - thisP.transform.position);
            }
            temp = temp + this.transform.position;
        }
        else
        {
            temp = this.transform.position + new Vector3(this.facing.x, this.facing.y, 0);
        }

        //temp.Normalize();
        this.target = temp;
    }

    void updateAround()
    {
        enemyAround.Clear();
        foreach (Ships thisS in this.shipsAround)
        {
            if (thisS.type == "pirate")
            {
                enemyAround.Add(thisS);
                userAction.registerEnemy(thisS);
            }
        }
    }
}


public struct Schedule
{
    public MegaStation start;
    public MegaStation end;
    public List<Vector3> midway;
}
