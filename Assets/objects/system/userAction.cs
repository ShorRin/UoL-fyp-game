using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class userAction : MonoBehaviour
{
    private Vector2 mousePos;
    public static Vector2 targetPos;
    public Camera thisCamera;
    public GameObject aim;
    public GameObject missile;
    public playerShip playerShip;
    public GameObject trans;
    public GameObject mslLock;
    public GameObject enemyLead;
    public Text condition;
    public static Dictionary<Ships, gunArea> gunAreas = new Dictionary<Ships, gunArea>();
    public static ArrayList missileToLaunch = new ArrayList();
    public static ArrayList allShipsList = new ArrayList();
    public static ArrayList allTransShipsList = new ArrayList();
    public static ArrayList allPirShipsList = new ArrayList();
    public static ArrayList allDefenderList = new ArrayList();
    public static ArrayList allMslList = new ArrayList();
    public static ArrayList transTList = new ArrayList();
    public static ArrayList foundEnemy = new ArrayList();
    public static Dictionary<enemyLead, Ships> enemyFoundDict = new Dictionary<enemyLead, Ships>();
    public GameObject fire;
    public static Dictionary<Ships, GameObject> shotFireList = new Dictionary<Ships, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("action monitor working");
        targetPos = playerShip.transform.position;
        aim.SetActive(false);
        mslLock.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Input.mousePosition;
        listenClick();
        updateTargetIcon();
        /* 
        if(playerShip.transform.position.x == targetPos.x && playerShip.transform.position.y == targetPos.y){
            aim.SetActive(false);
        } else {
            aim.SetActive(true);
        }
        */
        launchMissile();
        launchTransShip();
        updateMslLock();
        updateGunFire();
        if(playerShip!=null){
            updateEnemyLead();
        }
        if(playerShip == null){
            condition.text = "YOU LOSE";
        }
        if(playerShip.destroyNum == 1){
            condition.text = "YOU WIN";
        }
    }

    void updateTargetIcon(){
        if(playerShip.speed!=0){
            aim.SetActive(true);
            aim.transform.position = targetPos;
        } else {
            aim.SetActive(false);
        }
    }

    void updateEnemyLead(){
        if(foundEnemy.Count!=0){
            foreach(Ships thisO in foundEnemy){
                if(!enemyFoundDict.ContainsValue(thisO)){
                    GameObject thisL = Instantiate(enemyLead);
                    enemyLead thisE = thisL.GetComponent<enemyLead>();
                    thisE.thisPirate = thisO;
                    thisE.thisPlayer = playerShip;
                    enemyFoundDict[thisE] = thisO;
                } 
            }
            foundEnemy.Clear();
        }
        List<enemyLead> toBeRemoved = new List<enemyLead>();
        foreach(KeyValuePair<enemyLead,Ships> thisP in enemyFoundDict){
            if(thisP.Value == null){
                toBeRemoved.Add(thisP.Key);
                //enemyFoundDict.Remove(thisP.Key);
            } else {
                if(thisP.Value.distanceWith(playerShip.gameObject)<3f||thisP.Value.shipsAround.Count==0){
                    thisP.Key.gameObject.SetActive(false);
                } else {
                    thisP.Key.gameObject.SetActive(true);
                }
            }
        }
        foreach(enemyLead thisL in toBeRemoved){
            enemyFoundDict.Remove(thisL);
        }
    }

    void updateGunFire(){
        foreach(Ships thisS in allShipsList){
            if(!shotFireList.ContainsKey(thisS)&&thisS.type!="trans"){
                GameObject thisF = Instantiate(fire);
                thisF.SetActive(false);
                shotFireList[thisS] = thisF;
            }
            /* 
            if(thisS.type!="trans"){
                if(thisS.gunStatus==true){
                    shotFireList[thisS].SetActive(true);
                    shotFireList[thisS].transform.position = thisS.transform.position+(new Vector3(thisS.facing.x, thisS.facing.y,0))*0.8f;
                    shotFireList[thisS].transform.rotation = thisS.transform.rotation;
                } else {
                    shotFireList[thisS].SetActive(false);
                }
            }
            */
            
            
        }
    }

    void updateMslLock(){
        if(playerShip.targetMslShip!=null){
            mslLock.SetActive(true);
            mslLock.transform.position = playerShip.targetMslShip.transform.position;
        } else {
            mslLock.SetActive(false);
        }
    }


    void listenClick(){
        if(Input.GetMouseButtonDown(0)){
            Vector3 getOne = thisCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 3.79f));
            targetPos = new Vector2(getOne.x, getOne.y);
        }
    }

    public static Vector2 getTarget(){
        return targetPos;
    }

    public static void registerEnemy(Ships thisP){
        if(!foundEnemy.Contains(thisP)){
            foundEnemy.Add(thisP);
        }
    }

    public static void addToList(Ships thisShip){
        allShipsList.Add(thisShip);
        if(thisShip.type == "trans"){
            allTransShipsList.Add(thisShip);
        }
        if(thisShip.type == "pirate"){
            allPirShipsList.Add(thisShip);
        }
        if(thisShip.type == "defender"){
            allDefenderList.Add(thisShip);
        }
    }

    public static void removeFromList(Ships thisShip){
        allShipsList.Remove(thisShip);
        if(thisShip.type == "trans"){
            allTransShipsList.Remove(thisShip);
        }
        if(thisShip.type == "pirate"){
            allPirShipsList.Remove(thisShip);
        }
        if(thisShip.type == "defender"){
            allDefenderList.Remove(thisShip);
        }
        if(thisShip.type == "missile"){
            allMslList.Remove(thisShip);
        }
    }


    void launchMissile(){
        if(missileToLaunch.Count != 0){
            missileStr info = (missileStr)missileToLaunch[0];
            GameObject thisOne = Instantiate(missile);
            missile thisMsl = thisOne.GetComponent<missile>();
            thisMsl.transform.position = info.startPos.transform.position;
            thisMsl.shooter = info.startPos;

            thisMsl.initialSpeed = info.startPos.speed;
            
            if(info.targetOb != null){
                thisMsl.aimTarget = info.targetOb;
            } else {
                thisMsl.aimTarget = null;
            }
            missileToLaunch.RemoveAt(0);
            launchMissile();
        }
    }

    void launchTransShip(){
        if(transTList.Count != 0){
            Schedule thisS = (Schedule)transTList[0];
            GameObject thisOne = Instantiate(trans);
            transportShip thisT = thisOne.GetComponent<transportShip>();
            thisT.transform.position = thisS.start.transform.position;
            thisT.thisSchedule = thisS;
            transTList.RemoveAt(0);
            launchTransShip();
        }
    }
}


