using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ships : Objects
{
    public float speed;
    public string type;
    public float facingDegree;
    public Vector2 facing;
    public float maxSpeed;
    protected float turningValue;
    public Vector2 target;
    protected float targetDegree;
    public bool gunStatus = false;
    protected List<Vector2> gunArea = new List<Vector2>(); 
    protected gunArea thisArea;
    public List<Ships> shipsAround = new List<Ships>();
    protected List<Ships> mslAround = new List<Ships>();

    public float radarRange;
    protected float maxHP;
    public float currentHP;
    public Ships targetMslShip;
    protected float gunRange;
    protected float gunSecondDamage;
    protected float gunWidth;
    public Animation explode;

    public int destroyNum = 0;




    // Start is called before the first frame update

    public void dealDmg(float dmg){
        currentHP = currentHP - dmg;
        //Debug.Log(currentHP);
    }

    public void recover(){
        currentHP = maxHP;
    }

    public float distanceWith(GameObject s){
        return (this.transform.position - s.transform.position).magnitude;
    }

    public bool closeTo(GameObject that, float num){
        float temp = (that.transform.position - this.transform.position).magnitude;
        if(temp <= num){
            return true;
        } else {
            return false;
        }
    }

    protected void toStart()
    {
        speed = 0;
        facingDegree = this.transform.rotation.z;
        //facing = new Vector2(0,1);
        maxSpeed = 0.005f;
        turningValue = 2;

        currentHP = maxHP;
        this.pos = this.transform.position;
        //target = thisPos;
        //added properties here for testing, should put them into lower class later
        gunRange = 2f;
        gunWidth = 0.08f;
        gunSecondDamage = 50;
    }

    // Update is called once per frame
    protected void toUpdate()
    {
        //facingDegree = Mathf.Atan2(facing.y,facing.x)*Mathf.Rad2Deg;
        targetDegree = Mathf.Atan2(target.y-pos.y, target.x-pos.x)*Mathf.Rad2Deg;
        //speedLimitChange();
        speedChange();  
        if(speed !=0){
            facingChange();
        }
        posUpdate();

        updateShipsAround(radarRange);
        updateMslAround(2f);
        //targetMslShip = shipsAround[targetedIndex];
        updateMslLock();

        //update gun area
        //gunArea.Clear();
        thisArea.clear();
        thisArea.startPoint = this.pos;
        thisArea.endPoint = this.pos + facing*gunRange;
        thisArea.width = gunWidth;
        if(gunStatus == true){
            //Vector2 startPoint = this.pos;
            //Vector2 endPoint = this.pos + facing * gunRange;   //here 1 represents the range of gun is 1 in game world
            //Vector2 width = new Vector2(0.01f, 0); //here 0.01f represents the width of range of gun. 
            
            userAction.gunAreas[this] = thisArea;
            

            userAction.shotFireList[this].SetActive(true);
            userAction.shotFireList[this].transform.position = this.transform.position+(new Vector3(facing.x, facing.y,0))*0.8f;
            userAction.shotFireList[this].transform.rotation = this.transform.rotation;
                

            //gunArea.Add(startPoint);
            //gunArea.Add(endPoint);
            //gunArea.Add(width);
            //userAction.gunAreas[this] = gunArea;
        } else {
            if(userAction.shotFireList.ContainsKey(this)){
                userAction.shotFireList[this].SetActive(false);
            }
            userAction.gunAreas.Remove(this);
        }

        //damage
        foreach (var item in userAction.gunAreas)
        {
            if(item.Key != this){
                if(item.Value.withinThis(this.gameObject)){
                    //Debug.Log("yes");
                    dealDmg(gunSecondDamage*Time.deltaTime);
                    if(currentHP <= 0&&this.type!="missile"){
                        item.Key.destroyedByU();
                    }
                }

                /* 
                var temp = item.Value.startPoint - this.pos;
                if(temp.magnitude < 1){ //not necessarily 1, use a number which is larger than any range of guns in game.
                    var temp2 = item.Value.endPoint - this.pos;
                    //var width = item.Value[2].x;
                    if(Vector2.Angle(temp, temp2) > 150){ //use this angle value to control the width of fireareas
                        //Debug.Log("yes");
                        dealDmg(1);
                    }
                }
                */
            }   
        }
        if(currentHP <= 0){
            Object.Destroy(this.gameObject);
            if(userAction.shotFireList.ContainsKey(this)){
                userAction.shotFireList[this].SetActive(false);
            }
            userAction.removeFromList(this);
        }
    }

    protected void autoFire(){
        gunStatus = false;
        foreach(Ships thisS in shipsAround){
            if(this.type=="pirate"){
                if(thisS.type=="trans"||thisS.type=="defender"||thisS.type=="player"){
                    if(thisArea.withinThis(thisS.gameObject)){                        
                        gunStatus = true;
                        break;
                    }
                }
            } else {
                if(thisS.type=="pirate"){
                    if(thisArea.withinThis(thisS.gameObject)){                        
                        gunStatus = true;
                        break;
                    }
                }
            }
        }
        foreach(Ships thisM in mslAround){
            if(thisArea.withinThis(thisM.gameObject)&&thisM.GetComponent<missile>().mslForU(this)){
                gunStatus = true;
                break;
            }
        }
    }

    protected bool withinGunArea(gunArea thisA, Ships theShip){
        return thisA.withinThis(theShip.gameObject);
        /* 
        var temp = thisA.startPoint - theShip.pos;
        if(temp.magnitude < gunRange){ 
            var temp2 = thisA.endPoint - theShip.pos;
            if(Vector2.Angle(temp, temp2) > 150){ //use this angle value to control the width of fireareas
                        //Debug.Log("yes");
                        dealDmg(1);
                    }
                }
        return true;
        */
    }

    void speedLimitChange(){
        //change max speed according to engine...
        if(currentHP < maxHP*0.6){
            maxSpeed = maxSpeed/2;
        }
    }

    
    void updateMslLock(){
        if(!shipsAround.Contains(targetMslShip)){
            targetMslShip = null;
            /* 
            if(shipsAround.Count != 0){
                targetMslShip = shipsAround[0];
            } else {
                targetMslShip = null;
            }  
            */
        }
    }

    public void destroyedByU(){
        destroyNum++;
    }
    

    protected bool arrived(){
        var temp = this.pos-this.target;
        if(temp.magnitude <= 0.005){
            return true;
        } else {
            return false;
        }
    }

    protected bool mslHit(){
        var temp = this.pos-this.target;
        if(temp.magnitude <= 0.15f){
            return true;
        } else {
            return false;
        }
    }

    protected bool closeToArrived(){
        var temp = this.pos-this.target;
        if(temp.magnitude <= 0.008){
            return true;
        } else {
            return false;
        }
    }

    void speedChange(){
        if(arrived()){
            speed = 0;
            return;
        }
        if(speed < maxSpeed){
            float acc = Time.deltaTime*maxSpeed/5;
            if(acc + speed >= maxSpeed){
                speed = maxSpeed;
            } else {
                speed = speed + acc;
            }
        } else if(speed > maxSpeed){
            float dacc = Time.deltaTime*(speed-maxSpeed)/2;
            if(dacc + maxSpeed <= speed){
                speed = speed - dacc;
            } else {
                speed = maxSpeed;
            }
        }
    }

    float facingDegreeSafeAdd(float k){
        float result = k + facingDegree;
        if(result > 180){
            result = result - 360;
        }
        if(result < -180){
            result = result + 360;
        }
        return result;
    }
    void facingChange(){
        if(facingDegree != targetDegree){
            if(facingDegree - targetDegree > 0 && facingDegree - targetDegree <= 180 || facingDegree - targetDegree <= -180){
                if(Mathf.Abs(facingDegree - targetDegree) < turningValue){
                    facingDegree = targetDegree;
                } else {
                    facingDegree = facingDegreeSafeAdd(-turningValue);
                }
            } else {
                if(Mathf.Abs(facingDegree - targetDegree) < turningValue){
                    facingDegree = targetDegree;
                } else {
                    facingDegree = facingDegreeSafeAdd(turningValue);
                }
            }
        }
        Vector3 temp = transform.rotation.eulerAngles;
        temp.z = facingDegree;
        transform.rotation = Quaternion.Euler(temp);
    }

    void posUpdate(){
        var vForce = Quaternion.AngleAxis(facingDegree, Vector3.forward)*Vector3.right;
        facing = vForce.normalized;
        pos = pos + speed * Time.deltaTime * facing;
        this.transform.localPosition = new Vector3(this.pos.x, this.pos.y, 0);
    }

    protected void shootMissile(){
        if(this.targetMslShip!=null){
            missileStr thisMsl = new missileStr();
            thisMsl.startPos = this;
            //thisMsl.targetOb = targetBot;
            //if(shipsAround.Count > 0){
                thisMsl.targetOb = targetMslShip;
            //}
            userAction.missileToLaunch.Add(thisMsl);
        } else {
            Debug.Log("no target");
        }
    }

    void updateShipsAround(float range){
        shipsAround.Clear();
        foreach (Ships ship in userAction.allShipsList){
            float temp = (ship.transform.position - this.transform.position).magnitude;
            if(temp < range && ship != this){
                shipsAround.Add(ship);
            }
        }
    }

    void updateMslAround(float range){
        mslAround.Clear();
        foreach (Ships ship in userAction.allMslList){
            float temp = (ship.transform.position - this.transform.position).magnitude;
            if(temp < range && ship != this){
                mslAround.Add(ship);
            }
        }
    }
}

public struct gunArea
{
    public Vector3 startPoint;
    public Vector3 endPoint;
    public float width;
    public void clear(){
        startPoint = Vector3.zero;
        endPoint = Vector3.zero;
        width = 0;
    }
    public bool withinThis(GameObject thisO){
        float a = (thisO.transform.position - startPoint).magnitude;
        if(a<=(startPoint-endPoint).magnitude){
            Vector3 temp = thisO.transform.position - startPoint;
            Vector3 temp2 = endPoint - startPoint;
            if(Vector3.Dot(temp, temp2)>0){
                float q = Vector3.Angle(temp, temp2);
                float ln = temp.magnitude * Mathf.Sin(q*Mathf.PI/180);
                if(ln<width){
                    return true;
                }
            }
        }
        return false;
    }
}
