using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class changeText : MonoBehaviour
{
    public Text posTextX;
    public Text lifeText;
    public Text speedText;
    public Text posTextY;
    private static string positionX = "";
    private static string positionY = "";
    private static string lifePoint = "";
    private static string speed = "";
    private static string alertInfo = "MISSILE APPROACHING";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        posTextX.text = positionX;
        posTextY.text = positionY;
        lifeText.text = lifePoint;
        speedText.text = speed;
    }

    public static void changePosText(Vector3 pos){
        string a = (Mathf.Round(pos.x*10)/10).ToString();
        string b = (Mathf.Round(pos.y*10)/10).ToString();
        positionX = a;
        positionY = b;
    }

    public static void changeLifeText(string q){
        lifePoint = q;
    }

    public static void changeSpeed(float f){
        float s = f/0.16f;
        speed = "";
        for(int i = 0; i < s; i++){
            speed += "[]";
        }
    }
}
