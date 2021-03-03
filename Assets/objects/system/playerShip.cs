using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerShip : Ships
{
    // Start is called before the first frame update

    public static float thisSpeed;
    List<Ships> enemyAround = new List<Ships>();

    void Start()
    {
        maxHP = 100;
        userAction.addToList(this);
        base.toStart();
        this.ID = "player";
        this.type = "player";
        this.pos = transform.position;
        this.radarRange = 8f;
        this.maxSpeed = 1.5f;
    }

    // Update is called once per frame
    void Update()
    {
        //should put invoke method into ships because every ship can do these actions
        /* 
        if (Input.GetKeyDown(KeyCode.S))
        {
            gunStatus = true;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            gunStatus = false;
        }
        */
        if (Input.GetKeyDown(KeyCode.A))
        {
            shootMissile();
            //Instantiate(missile);
        }
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            if(enemyAround.Count==0){
                targetMslShip = null;
            } else if(targetMslShip == null){
                targetMslShip = enemyAround[0];
            } else {
                int temp = enemyAround.IndexOf(targetMslShip);
                if(temp < enemyAround.Count-1){
                    temp += 1;
                } else {
                    temp = 0;
                } 
                targetMslShip = enemyAround[temp];
            }   
            /* 
            if(shipsAround.Count==0){
                targetMslShip = null;
            } else if(targetMslShip == null){
                targetMslShip = shipsAround[0];
            } else {
                int temp = shipsAround.IndexOf(targetMslShip);
                if(temp < shipsAround.Count-1){
                    temp += 1;
                } else {
                    temp = 0;
                } 
                targetMslShip = shipsAround[temp];
            }    
            */     
        }


        this.target = userAction.getTarget();
        base.toUpdate();
        updateAround();
        updatePanel();
        base.autoFire();
        //Debug.Log(this.speed);
        thisSpeed = this.speed;
        //add user control here, can access position, facing degree, speed, etc...
    }

    void updatePanel(){
        string f = this.currentHP+"/"+this.maxHP;
        changeText.changeLifeText(f);
        changeText.changePosText(this.transform.position);
        changeText.changeSpeed(this.speed);
    }

    void updateAround(){
        enemyAround.Clear();
        foreach(Ships thisS in this.shipsAround){
            if(thisS.type == "pirate"){
                enemyAround.Add(thisS);
            }
        }
    }
}
