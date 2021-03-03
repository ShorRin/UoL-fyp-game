using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScheduleList : MonoBehaviour
{
    public MegaStation MS1;
    public MegaStation MS2;
    public MegaStation t2MS1;
    public MegaStation t2MS2;
    public MegaStation t4MS1;
    public MegaStation t4MS2;
    public static Dictionary<string, Schedule> schedueList = new Dictionary<string, Schedule>();

    void Start()
    {
        List<MegaStation> stations = new List<MegaStation>{MS1, MS2, t2MS1, t2MS2, t4MS1, t4MS2};
        foreach(MegaStation thisOne in stations){
            foreach(MegaStation thisTwo in stations){
                if(thisOne != thisTwo){
                    loadSchedule(thisOne, thisTwo);
                }
            }
        }
        /* 
        Schedule thisOne = new Schedule();
        thisOne.start = MS1;
        thisOne.end = MS2;
        List<Vector3> thisL = new List<Vector3>();
        //thisL.Add(new Vector3(-2,-6,0));
        //thisL.Add(new Vector3(0,1.1f,0));
        thisL.Add(new Vector3(1.2f, 1.1f, 0));
        thisOne.midway = thisL;
        schedueList["MS1toMS2"] = thisOne;

        Schedule thisTwo = new Schedule();
        thisTwo.start = MS2;
        thisTwo.end = MS1;
        List<Vector3> thisL2 = new List<Vector3>();
        //thisL2.Add(new Vector3(2.2f,1,0));
        //thisL2.Add(new Vector3(0.5f,-6.2f,0));
        thisL2.Add(new Vector3(-0.6f, -6.2f, 0));
        thisTwo.midway = thisL2;
        schedueList["MS2toMS1"] = thisTwo;

        Schedule thisTre = new Schedule();
        thisTre.start = MS2;
        thisTre.end = MS1;
        List<Vector3> thisL3 = new List<Vector3>();
        //thisL2.Add(new Vector3(2.2f,1,0));
        //thisL2.Add(new Vector3(0.5f,-6.2f,0));
        thisL3.Add(new Vector3(-0.6f, -6.2f, 0));
        thisTre.midway = thisL3;
        schedueList["MS2toMS1"] = thisTwo;
        */
    }

    void loadSchedule(MegaStation start, MegaStation end){
        Schedule thisOne = new Schedule();
        thisOne.start = start;
        thisOne.end = end;
        List<Vector3> thisL = new List<Vector3>();
        thisL.Add(end.transform.position);
        thisOne.midway = thisL;
        string thisID = start.gameObject.name + "to" + end.gameObject.name;
        schedueList[thisID] = thisOne;
    }
}
