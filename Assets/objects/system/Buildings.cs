using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Buildings : Objects
{
    protected string type;

    public string getBuildingInfo(){
        string thisOne = "this is a " + type + " at " + pos + " : " + ID;
        return thisOne;
    }
}
