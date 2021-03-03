using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyLead : MonoBehaviour
{
    public playerShip thisPlayer;
    public Ships thisPirate;
    private SpriteRenderer thisR;
    // Start is called before the first frame update
    void Start()
    {
        thisR = this.gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(thisPirate.distanceWith(thisPlayer.gameObject)<=thisPlayer.radarRange){
            thisR.color = Color.red;
        } else {
            thisR.color = Color.yellow;
        }
        this.transform.position = thisPlayer.transform.position + 0.7f*(thisPirate.transform.position-thisPlayer.transform.position).normalized;
        float anlge = Vector3.Angle(Vector3.right,thisPirate.transform.position-thisPlayer.transform.position);
        if(thisPirate.transform.position.y>thisPlayer.transform.position.y){
            anlge *= -1;
        }
        anlge = 90 - anlge;
        //Debug.Log(anlge);
        Quaternion rotation = Quaternion.Euler(0,0,anlge);
        this.transform.rotation = rotation;
    }
}
